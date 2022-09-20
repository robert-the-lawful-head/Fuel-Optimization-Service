using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.Core.Utilities.Geography;
using FBOLinx.DB;
using FBOLinx.DB.Models;
using FBOLinx.DB.Projections.AirportWatch;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.DB.Specifications.AircraftHexTailMapping;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
using FBOLinx.ServiceLayer.Extensions.Aircraft;
using FBOLinx.ServiceLayer.Logging;
using Geolocation;
using Mapster;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public class SWIMService : ISWIMService
    {
        //private const int MessageThresholdSec = 30;
        private const int FlightLegsFetchingThresholdMins = -30;
        private const int PlaceholderRecordsFetchingThresholdMins = -60;

        private readonly SWIMFlightLegEntityService _FlightLegEntityService;
        private readonly SWIMFlightLegDataEntityService _FlightLegDataEntityService;
        private readonly AirportWatchLiveDataEntityService _AirportWatchLiveDataEntityService;
        private readonly AircraftHexTailMappingEntityService _AircraftHexTailMappingEntityService;
        private readonly AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;
        private readonly ICustomerAircraftEntityService _CustomerAircraftEntityService;
        private readonly AircraftEntityService _AircraftEntityService;
        private readonly ILoggingService _LoggingService;
        private readonly AirportWatchService _AirportWatchService;
        private readonly IFuelReqService _FuelReqService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private readonly AFSAircraftEntityService _AFSAircraftEntityService;
        private readonly FAAAircraftMakeModelEntityService _FAAAircraftMakeModelEntityService;
        private IAirportWatchFlightLegStatusService _AirportWatchFlightLegStatusService;

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService,
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService,
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService, AircraftEntityService aircraftEntityService, ILoggingService loggingService, 
            AirportWatchService airportWatchService, IFuelReqService fuelReqService, IPricingTemplateService pricingTemplateService,
            AFSAircraftEntityService afsAircraftEntityService,
            FAAAircraftMakeModelEntityService faaAircraftMakeModelEntityService,
            IAirportWatchFlightLegStatusService airportWatchFlightLegStatusService)
        {
            _AirportWatchFlightLegStatusService = airportWatchFlightLegStatusService;
            _FlightLegEntityService = flightLegEntityService;
            _FlightLegDataEntityService = flightLegDataEntityService;
            _AirportWatchLiveDataEntityService = airportWatchLiveDataEntityService;
            _AircraftHexTailMappingEntityService = aircraftHexTailMappingEntityService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _CustomerAircraftEntityService = customerAircraftEntityService;
            _AircraftEntityService = aircraftEntityService;
            _LoggingService = loggingService;
            _AirportWatchService = airportWatchService;
            _FuelReqService = fuelReqService;
            _pricingTemplateService = pricingTemplateService;
            _AFSAircraftEntityService = afsAircraftEntityService;
            _FAAAircraftMakeModelEntityService = faaAircraftMakeModelEntityService;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(int groupId, int fboId, string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins), 
                    new List<FlightLegStatus>(){ FlightLegStatus.TaxiingOrigin, FlightLegStatus.Departing }));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, true, groupId, fboId);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(int groupId, int fboId, string icao)
        {
            List<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins), 
                    new List<FlightLegStatus>() { FlightLegStatus.Landing, FlightLegStatus.TaxiingDestination, FlightLegStatus.Arrived }));
            List<SWIMFlightLeg> placeholderRecords = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(icao, DateTime.UtcNow.AddMinutes(PlaceholderRecordsFetchingThresholdMins), true));
            if (placeholderRecords != null && placeholderRecords.Any())
            {
                swimFlightLegs.AddRange(placeholderRecords);
            }
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, false, groupId, fboId);

            return result;
        }
        
        public async Task<IEnumerable<FlightLegDTO>> GetHistoricalFlightLegs(string icao, bool isArrivals, DateTime historicalETD, DateTime historicalETA, DateTime historicalAircraftPositionDateTime)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs;
            if (isArrivals)
            {
                swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, historicalETD.AddMinutes(FlightLegsFetchingThresholdMins), historicalETA.AddMinutes(30)));
            }
            else
            {
                swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, historicalETD.AddMinutes(FlightLegsFetchingThresholdMins), historicalETA.AddMinutes(30)));
            }
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, isArrivals, 0, 0, true, historicalETD, historicalETA, historicalAircraftPositionDateTime);

            return result;
        }

        public async Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> swimFlightLegs)
        {
            if (swimFlightLegs == null || !swimFlightLegs.Any())
            {
                return;
            }

            IList<SWIMFlightLegDTO> swimFlightLegDTOs = new List<SWIMFlightLegDTO>();
            foreach (IGrouping<string, SWIMFlightLegDTO> flightLegGrouping in swimFlightLegs.GroupBy(x => x.AircraftIdentification))
            {
                SWIMFlightLegDTO swimFlightLegDto = flightLegGrouping.First();
                swimFlightLegDto.SWIMFlightLegDataMessages = new List<SWIMFlightLegDataDTO>() { flightLegGrouping.SelectMany(x => x.SWIMFlightLegDataMessages).ToList().First() };
                swimFlightLegDTOs.Add(swimFlightLegDto);
            }
            
            List<string> atcFlightNumbers = swimFlightLegDTOs.Where(x => !x.AircraftIdentification.ToUpperInvariant().StartsWith('N')).Select(x => x.AircraftIdentification).ToList();

            List<AirportWatchLiveHexTailMapping> antennaLiveData = (await _AirportWatchLiveDataEntityService.GetListBySpec<AirportWatchLiveHexTailMapping>(
                new AirportWatchLiveHexTailMappingSpecification(atcFlightNumbers, DateTime.UtcNow.AddHours(-1)))).OrderByDescending(x => x.AircraftPositionDateTimeUtc).ToList();
            
            List<AirportWatchHistoricalData> antennaHistoricalData = (await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                new AirportWatchHistoricalDataSpecification(atcFlightNumbers, DateTime.UtcNow.AddDays(-1)))).OrderByDescending(x => x.AircraftPositionDateTimeUtc).ToList();

            List<string> airportICAOs = swimFlightLegDTOs.Where(x => !string.IsNullOrEmpty(x.DepartureICAO)).Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegDTOs.Where(x => !string.IsNullOrEmpty(x.ArrivalICAO)).Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));

            DateTime atdMin = swimFlightLegDTOs.Min(x => x.ATD);
            DateTime atdMax = swimFlightLegDTOs.Max(x => x.ATD);
            List<string> departureICAOs = swimFlightLegDTOs.Select(x => x.DepartureICAO).Distinct().ToList();
            List<string> arrivalICAOs = swimFlightLegDTOs.Select(x => x.ArrivalICAO).Distinct().ToList();
            List<SWIMFlightLeg> existingFlightLegs = (await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(departureICAOs, arrivalICAOs, atdMin, atdMax))).OrderByDescending(x => x.ATD).ToList();
            
            List<SWIMFlightLegData> flightLegDataMessagesToInsert = new List<SWIMFlightLegData>();
            List<SWIMFlightLeg> flightLegsToUpdate = new List<SWIMFlightLeg>();
            List<SWIMFlightLeg> flightLegsToInsert = new List<SWIMFlightLeg>();
            
            foreach (SWIMFlightLegDTO swimFlightLegDto in swimFlightLegDTOs)
            {
                var existingLeg = existingFlightLegs.FirstOrDefault(
                    x => x.DepartureICAO == swimFlightLegDto.DepartureICAO && x.ArrivalICAO == swimFlightLegDto.ArrivalICAO && x.ATD == swimFlightLegDto.ATD);
                if (existingLeg != null)
                {
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        flightLegDataMessageDto.SWIMFlightLegId = existingLeg.Oid;
                        flightLegDataMessageDto.MessageTimestamp = DateTime.UtcNow;
                        flightLegDataMessagesToInsert.Add(flightLegDataMessageDto.Adapt<SWIMFlightLegData>());
                    }

                    if (existingLeg.ETA != swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA)
                    {
                        existingLeg.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                        AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == existingLeg.ArrivalICAO);
                        if (arrivalAirport != null)
                        {
                            existingLeg.ETALocal = DateTimeHelper.GetLocalTime(existingLeg.ETA.Value, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                        }
                        flightLegsToUpdate.Add(existingLeg);
                    }
                }
                else
                {
                    await SetTailNumber(swimFlightLegDto, antennaLiveData, antennaHistoricalData);
                    swimFlightLegDto.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                    var flightLegToInsert = swimFlightLegDto.Adapt<SWIMFlightLeg>();
                    flightLegToInsert.Status = FlightLegStatus.Departing;

                    AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == flightLegToInsert.DepartureICAO);
                    if (departureAirport != null)
                    {
                        flightLegToInsert.DepartureCity = departureAirport.AirportCity;
                        flightLegToInsert.ATDLocal = DateTimeHelper.GetLocalTime(flightLegToInsert.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                    }
                    AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == flightLegToInsert.ArrivalICAO);
                    if (arrivalAirport != null)
                    {
                        flightLegToInsert.ArrivalCity = arrivalAirport.AirportCity;
                        if (flightLegToInsert.ETA != null)
                        {
                            flightLegToInsert.ETALocal = DateTimeHelper.GetLocalTime(flightLegToInsert.ETA.Value, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                        }
                    }

                    if (flightLegToInsert.SWIMFlightLegDataMessages != null && flightLegToInsert.SWIMFlightLegDataMessages.Any())
                    {
                        foreach (SWIMFlightLegData swimFlightLegDataMessage in flightLegToInsert.SWIMFlightLegDataMessages)
                        {
                            swimFlightLegDataMessage.MessageTimestamp = DateTime.UtcNow;
                        }
                    }

                    flightLegsToInsert.Add(flightLegToInsert);
                }
            }

            List<SWIMFlightLeg> existingPlaceholderRecordsToDelete = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(flightLegsToInsert.Select(x => x.AircraftIdentification).ToList(), DateTime.UtcNow.AddHours(-3), true));

            if (existingPlaceholderRecordsToDelete.Count > 0)
            {
                await _FlightLegEntityService.BulkDeleteEntities(existingPlaceholderRecordsToDelete);
            }

            if (flightLegsToInsert.Count > 0)
            {
                LogMissedTailNumbers(flightLegsToInsert);
                await _FlightLegEntityService.BulkInsert(flightLegsToInsert, new BulkConfig()
                {
                    SetOutputIdentity = true,
                    IncludeGraph = true
                });
            }
            
            if (flightLegDataMessagesToInsert.Count > 0)
            {
                await _FlightLegDataEntityService.BulkInsert(flightLegDataMessagesToInsert);
            }

            if (flightLegsToUpdate.Count > 0)
            {
                await _FlightLegEntityService.BulkUpdate(flightLegsToUpdate);
            }
        }

        public async Task CreatePlaceholderRecords()
        {
            var tailNumbersByLiveAndParkingCoordinates =
                await _AirportWatchFlightLegStatusService.GetAirportWatchLiveDataWithFlightLegStatuses();

            List<SWIMFlightLeg> existingFlightLegsToUpdate = tailNumbersByLiveAndParkingCoordinates.Where(x =>
                    x.SWIMFlightLeg != null && x.FlightLegStatus.GetValueOrDefault() !=
                    x.SWIMFlightLeg.Status.GetValueOrDefault())
                .Select(x => x.SWIMFlightLeg).ToList();
            List<SWIMFlightLeg> placeholderRecordsToInsert = tailNumbersByLiveAndParkingCoordinates.Where(x =>
                    x.SWIMFlightLeg == null && x.AirportPosition != null && (x.FlightLegStatus == FlightLegStatus.TaxiingOrigin ||
                                                x.FlightLegStatus == FlightLegStatus.Departing))
                .Select(x => new SWIMFlightLeg()
                {
                    DepartureICAO = x.AirportPosition.Icao,
                    AircraftIdentification = x.TailNumber,
                    ATD = DateTime.UtcNow,
                    ATDLocal = DateTimeHelper.GetLocalTime(DateTime.UtcNow, x.AirportPosition.IntlTimeZone, x.AirportPosition.DaylightSavingsYn?.ToLower() == "y"),
                    IsPlaceholder = true,
                    Status = x.FlightLegStatus,
                    IsAircraftOnGround = true
                }).ToList();

            //List<SWIMFlightLeg> existingFlightLegs = await _FlightLegEntityService.GetListBySpec(
            //    new SWIMFlightLegSpecification(tailNumbersByLiveAndParkingCoordinates.Select(x => x.TailNumber).ToList(), tailNumbersByLiveAndParkingCoordinates.Select(x => x.AtcFlightNumber).ToList(), DateTime.UtcNow.AddHours(-1)));
            //List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification());

            //List<SWIMFlightLeg> placeholderRecordsToInsert = new List<SWIMFlightLeg>();
            //List<SWIMFlightLeg> existingFlightLegsToUpdate = new List<SWIMFlightLeg>();

            //foreach (var tailNumberByCoordinates in tailNumbersByLiveAndParkingCoordinates)
            //{
            //    SWIMFlightLeg existingFlightLeg = existingFlightLegs.FirstOrDefault(
            //        x => x.AircraftIdentification == tailNumberByCoordinates.TailNumber || x.AircraftIdentification == tailNumberByCoordinates.AtcFlightNumber);
                
            //    var distanceFromParkingOccurrence = new Coordinates(tailNumberByCoordinates.Latitude, tailNumberByCoordinates.Longitude)
            //        .DistanceTo(
            //            new Coordinates(tailNumberByCoordinates.HistoricalLatitude, tailNumberByCoordinates.HistoricalLongitude),
            //            UnitOfLength.Miles
            //        );
            //    if (distanceFromParkingOccurrence > 0.2)
            //    {
            //        if (existingFlightLeg != null)
            //        {
            //            existingFlightLeg.Status = FlightLegStatus.TaxiingOrigin;
            //            existingFlightLegsToUpdate.Add(existingFlightLeg);
            //        }
            //        else
            //        {
            //            foreach (AcukwikAirport airport in airports.Where(x => x.Latitude != null && x.Longitude != null)) // && x.Icao != null
            //            {
            //                double airportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(airport.Latitude);
            //                double airportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(airport.Longitude);
            //                var distanceFromAirport = new Coordinates(airportLatitude, airportLongitude)
            //                    .DistanceTo(
            //                        new Coordinates(tailNumberByCoordinates.HistoricalLatitude, tailNumberByCoordinates.HistoricalLongitude),
            //                        UnitOfLength.Miles
            //                    );
            //                if (distanceFromAirport < 5)
            //                {
            //                    SWIMFlightLeg placeholderRecord = new SWIMFlightLeg();
            //                    placeholderRecord.DepartureICAO = airport.Icao;
            //                    placeholderRecord.AircraftIdentification = tailNumberByCoordinates.TailNumber;
            //                    placeholderRecord.ATD = DateTime.UtcNow;
            //                    placeholderRecord.ATDLocal = DateTimeHelper.GetLocalTime(placeholderRecord.ATD, airport.IntlTimeZone, airport.DaylightSavingsYn?.ToLower() == "y");
            //                    placeholderRecord.IsPlaceholder = true;
            //                    placeholderRecord.Status = FlightLegStatus.TaxiingOrigin;
            //                    placeholderRecord.IsAircraftOnGround = true;
                                
            //                    placeholderRecordsToInsert.Add(placeholderRecord);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            if (existingFlightLegsToUpdate.Count > 0)
            {
                await _FlightLegEntityService.BulkUpdate(existingFlightLegsToUpdate);
            }

            if (placeholderRecordsToInsert.Count > 0)
            {
                await _FlightLegEntityService.BulkInsert(placeholderRecordsToInsert);
            }
        }

        public async Task UpdateFlightStatusAndOtherDynamicData()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(DateTime.UtcNow.AddMinutes(-10)));
            stopwatch.Stop();
            stopwatch.Restart();
            List<SWIMFlightLegData> swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList(), DateTime.UtcNow.AddHours(-1)));
            stopwatch.Stop();
            stopwatch.Restart();
            List<string> aircraftIdentifications = swimFlightLegs.Select(x => x.AircraftIdentification).Distinct().ToList();
            List<AirportWatchHistoricalData> antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(aircraftIdentifications, aircraftIdentifications, DateTime.UtcNow.AddHours(-1)));
            stopwatch.Stop();
            stopwatch.Restart();
            Parallel.ForEach(swimFlightLegs, swimFlightLeg =>
            {
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification || x.TailNumber == swimFlightLeg.AircraftIdentification)
                    .OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                if (antennaHistoricalDataRecord != null)
                {
                    SetFlightStatus(swimFlightLeg, antennaHistoricalDataRecord.IsAircraftOnGround, antennaHistoricalDataRecord);
                }

                SWIMFlightLegData latestSwimMessage =
                    swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid && x.Latitude != null && x.Longitude != null).OrderByDescending(x => x.Oid).FirstOrDefault();

                if (latestSwimMessage != null)
                {
                    //swimFlightLeg.ETA = latestSwimMessage.ETA; // Updated on SaveFlightLegData
                    swimFlightLeg.ActualSpeed = latestSwimMessage.ActualSpeed;
                    swimFlightLeg.Altitude = latestSwimMessage.Altitude;
                    swimFlightLeg.Latitude = latestSwimMessage.Latitude;
                    swimFlightLeg.Longitude = latestSwimMessage.Longitude;
                }
            });

            await _FlightLegEntityService.BulkUpdate(swimFlightLegs);
            stopwatch.Stop();
        }

        public async Task SetFlightLegsStaticData()
        {
            List<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins), true));

            //List<AirportWatchLiveData> antennaLiveData = new List<AirportWatchLiveData>();
            List<string> tailNumbers = swimFlightLegs.Where(x => x.AircraftIdentification.ToUpperInvariant().StartsWith('N')).Select(x => x.AircraftIdentification).Distinct().ToList();
            IList<Tuple<int, string, string, string>> flightDepartmentsByTailNumbers = await _CustomerAircraftEntityService.GetAircraftsByFlightDepartments(tailNumbers);
            var aircraftIds = flightDepartmentsByTailNumbers.Select(x => x.Item1).Distinct().ToList();
            List<AirCrafts> aircrafts = await _AircraftEntityService.GetListBySpec(new AircraftSpecification(aircraftIds));
            var afsAircrafts = await _AFSAircraftEntityService.GetListBySpec(new AFSAircraftSpecification(aircraftIds));

            List<AircraftHexTailMapping> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(tailNumbers));
            var aircraftMakeModels = await _FAAAircraftMakeModelEntityService.GetListBySpec(new AircraftMakeModelSpecification(hexTailMappings.Select(x => x.FAAAircraftMakeModelCode).Distinct().ToList()));

            Parallel.ForEach(swimFlightLegs, swimFlightLeg =>
            {
                //AirportWatchLiveData latestAntennaLiveDataRecord = antennaLiveData.Where(
                //    x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification || x.TailNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                //if (latestAntennaLiveDataRecord != null)
                //{
                //    swimFlightLeg.IsAircraftOnGround = latestAntennaLiveDataRecord.IsAircraftOnGround;
                //}

                //List<string> atcFlightNumbers = swimFlightLegDTOs.Where(x => !x.AircraftIdentification.ToUpperInvariant().StartsWith('N')).Select(x => x.AircraftIdentification).ToList();

                var aircraftByFlightDepartment = flightDepartmentsByTailNumbers.FirstOrDefault(x => x.Item2 == swimFlightLeg.AircraftIdentification);
                if (aircraftByFlightDepartment != null)
                {
                    swimFlightLeg.FlightDepartment = aircraftByFlightDepartment.Item3;
                    swimFlightLeg.Phone = aircraftByFlightDepartment.Item4;
                    var aircraft = aircrafts.FirstOrDefault(x => x.AircraftId == aircraftByFlightDepartment.Item1);
                    if (aircraft != null)
                    {
                        swimFlightLeg.Make = aircraft.Make;
                        swimFlightLeg.Model = aircraft.Model;
                        swimFlightLeg.FuelCapacityGal = aircraft.FuelCapacityGal;
                    }

                    var afsAircraft = afsAircrafts.FirstOrDefault(x => x.DegaAircraftID == aircraftByFlightDepartment.Item1);
                    if (afsAircraft != null)
                    {
                        swimFlightLeg.ICAOAircraftCode = afsAircraft.Icao;
                    }
                }

                if (swimFlightLeg.AircraftIdentification.ToUpper().StartsWith('N'))
                {
                    var tailMapping = hexTailMappings.FirstOrDefault(x => x.TailNumber == swimFlightLeg.AircraftIdentification);
                    if (tailMapping != null)
                    {
                        FAAAircraftMakeModelReference aircraftMakeModelReference = aircraftMakeModels.FirstOrDefault(x => x.CODE == tailMapping.FAAAircraftMakeModelCode);
                        if (aircraftMakeModelReference != null)
                        {
                            swimFlightLeg.FAAMake = aircraftMakeModelReference.MFR;
                            swimFlightLeg.FAAModel = aircraftMakeModelReference.MODEL;
                        }
                    }
                }
                
                if (string.IsNullOrWhiteSpace(swimFlightLeg.Make))
                {
                    swimFlightLeg.Make = swimFlightLeg.FAAMake;
                }

                if (string.IsNullOrWhiteSpace(swimFlightLeg.Model))
                {
                    swimFlightLeg.Model = swimFlightLeg.FAAModel;
                }

                swimFlightLeg.IsProcessed = true;
            });
            
            await _FlightLegEntityService.BulkUpdate(swimFlightLegs);
        }
        
        //TODO: Refactor this.  Get a lot of this code into it's own set of services.
        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs, bool isArrivals, int groupId, int fboId,
            bool useHistoricalData = false, DateTime? historicalETD = null, DateTime? historicalETA = null, DateTime? historicalAircraftPositionDateTime = null)
        {
            List<string> aircraftIdentifications = swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.AircraftIdentification)).Select(x => x.AircraftIdentification).Distinct().ToList();
            IEnumerable<Tuple<int, string, string>> pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(aircraftIdentifications);
            var customerAircrafts = await _CustomerAircraftEntityService.GetListBySpec(new CustomerAircraftsByGroupSpecification(groupId, aircraftIdentifications));

            List<AirportWatchHistoricalData> antennaHistoricalData = null;
            List<AcukwikAirport> airports = null;
            List<SWIMFlightLegData> swimFlightLegMessages = null;
            if (useHistoricalData)
            {
                List<string> airportICAOs = swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.DepartureICAO)).Select(x => x.DepartureICAO).ToList();
                airportICAOs.AddRange(swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.ArrivalICAO)).Select(x => x.ArrivalICAO).ToList());
                airportICAOs = airportICAOs.Distinct().ToList();

                antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                    new AirportWatchHistoricalDataSpecification(aircraftIdentifications, historicalETD.Value, historicalETA.Value));
                airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
                swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList(), historicalAircraftPositionDateTime.Value.AddHours(-12)));
            }

            PricingTemplateDto defaultCompanyPricingTemplate = await _pricingTemplateService.GetDefaultTemplate(fboId);
            
            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs)
            {
                FlightLegDTO flightLegDto = new FlightLegDTO();
                flightLegDto.TailNumber = swimFlightLeg.AircraftIdentification;
                flightLegDto.DepartureICAO = swimFlightLeg.DepartureICAO;
                flightLegDto.DepartureCity = swimFlightLeg.DepartureCity;
                flightLegDto.ArrivalICAO = swimFlightLeg.ArrivalICAO;
                flightLegDto.ArrivalCity = swimFlightLeg.ArrivalCity;
                flightLegDto.Status = swimFlightLeg.Status ?? FlightLegStatus.Arrived;
                flightLegDto.IsAircraftOnGround = swimFlightLeg.IsAircraftOnGround;
                flightLegDto.Latitude = swimFlightLeg.Latitude;
                flightLegDto.Longitude = swimFlightLeg.Longitude;
                flightLegDto.Altitude = swimFlightLeg.Altitude;
                flightLegDto.ActualSpeed = swimFlightLeg.ActualSpeed;
                flightLegDto.FlightDepartment = swimFlightLeg.FlightDepartment;
                flightLegDto.Make = swimFlightLeg.Make;
                flightLegDto.FAAMake = swimFlightLeg.FAAMake;
                flightLegDto.Model = swimFlightLeg.Model;
                flightLegDto.FAAModel = swimFlightLeg.FAAModel;
                flightLegDto.FuelCapacityGal = swimFlightLeg.FuelCapacityGal;
                flightLegDto.Phone = swimFlightLeg.Phone;
                flightLegDto.ICAOAircraftCode = swimFlightLeg.ICAOAircraftCode;

                flightLegDto.IsInNetwork = GetSwimLegsCustomerAircraft(customerAircrafts, swimFlightLeg.AircraftIdentification).IsInNetwork();
                flightLegDto.IsOutOfNetwork = GetSwimLegsCustomerAircraft(customerAircrafts, swimFlightLeg.AircraftIdentification).IsOutOfNetwork();
                flightLegDto.IsFuelerLinxClient = GetSwimLegsCustomerAircraft(customerAircrafts, swimFlightLeg.AircraftIdentification).IsFuelerLinxClient();

                if (!swimFlightLeg.IsPlaceholder)
                {
                    flightLegDto.ATDZulu = swimFlightLeg.ATD;
                    flightLegDto.ATDLocal = swimFlightLeg.ATDLocal ?? swimFlightLeg.ATD;
                }
                
                if (swimFlightLeg.ETA != null)
                {
                    flightLegDto.ETAZulu = swimFlightLeg.ETA;
                    flightLegDto.ETALocal = swimFlightLeg.ETALocal ?? swimFlightLeg.ETA;

                    flightLegDto.ETE = (flightLegDto.ETAZulu.Value - DateTime.UtcNow).Duration();
                }
                
                if (isArrivals)
                {
                    flightLegDto.Origin = flightLegDto.DepartureICAO;
                    flightLegDto.City = flightLegDto.DepartureCity;
                }
                else
                {
                    flightLegDto.Origin = flightLegDto.ArrivalICAO;
                    flightLegDto.City = flightLegDto.ArrivalCity;
                }

                SetPricingTemplate(flightLegDto, defaultCompanyPricingTemplate, swimFlightLeg.AircraftIdentification, pricingTemplates);
                
                if (useHistoricalData)
                {
                    ApplyHistoricalFlightData(swimFlightLeg, flightLegDto, antennaHistoricalData, historicalAircraftPositionDateTime.Value, swimFlightLegMessages, airports);
                }

                if (!result.Any(x => x.DepartureICAO == flightLegDto.DepartureICAO && x.ArrivalICAO == flightLegDto.ArrivalICAO && x.ATDZulu == flightLegDto.ATDZulu))
                {
                    result.Add(flightLegDto);
                }
            }

            //[#2xrec66] Problem with the speed of checking past visits.
            await SetVisitsToMyFBO(groupId, fboId, result);

            await SetOrderInfo(groupId, fboId, result);

            return result;
        }

        // private FuelReqDto? GetSwimLegsFuelOrder(List<FuelReqDto> fo, string tailNumber)
        // {
        //     return fo.Where(x => x.CustomerAircraft.TailNumber == tailNumber).FirstOrDefault();
        // }
        private CustomerAircrafts? GetSwimLegsCustomerAircraft(List<CustomerAircrafts> ca, string tailNumber)
        {
            return ca.Where(x => x.TailNumber == tailNumber).FirstOrDefault();
        }

        private void ApplyHistoricalFlightData(
            SWIMFlightLeg swimFlightLeg, FlightLegDTO flightLegDto, List<AirportWatchHistoricalData> antennaHistoricalData,
            DateTime historicalAircraftPositionDateTime, List<SWIMFlightLegData> swimFlightLegMessages, List<AcukwikAirport> airports)
        {
            AirportWatchHistoricalData airportWatchHistoricalData = antennaHistoricalData.FirstOrDefault(x =>
                        (x.AtcFlightNumber == swimFlightLeg.AircraftIdentification || x.TailNumber == swimFlightLeg.AircraftIdentification) &&
                         x.AircraftPositionDateTimeUtc >= historicalAircraftPositionDateTime.AddMinutes(-5) &&
                         x.AircraftPositionDateTimeUtc <= historicalAircraftPositionDateTime.AddMinutes(5));
            if (airportWatchHistoricalData != null)
            {
                flightLegDto.IsAircraftOnGround = airportWatchHistoricalData.IsAircraftOnGround;
            }
            
            SWIMFlightLegData latestSWIMMessage =
                swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid && x.Latitude != null && x.Longitude != null &&
                                                 x.MessageTimestamp >= historicalAircraftPositionDateTime.AddMinutes(-5) && // PST Time
                                                 x.MessageTimestamp <= historicalAircraftPositionDateTime.AddMinutes(5)).OrderByDescending(x => x.Oid).FirstOrDefault();
            if (latestSWIMMessage != null)
            {
                flightLegDto.ActualSpeed = latestSWIMMessage.ActualSpeed;
                flightLegDto.Altitude = latestSWIMMessage.Altitude;
                flightLegDto.Latitude = latestSWIMMessage.Latitude;
                flightLegDto.Longitude = latestSWIMMessage.Longitude;

                //flightLegDto.Status = GetFlightStatus(swimFlightLeg, latestSWIMMessage, airports, flightLegDto.IsAircraftOnGround, antennaHistoricalData);
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                SetFlightStatus(swimFlightLeg, antennaHistoricalDataRecord.IsAircraftOnGround, antennaHistoricalDataRecord);
            }
        }

        private async Task SetTailNumber(SWIMFlightLegDTO swimFlightLegDto, List<AirportWatchLiveHexTailMapping> antennaLiveData, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            if (IsCorrectTailNumber(swimFlightLegDto.AircraftIdentification))
            {
                return;
            }

            AirportWatchLiveHexTailMapping antennaLiveDataRecord = antennaLiveData.FirstOrDefault(x => x.AtcFlightNumber == swimFlightLegDto.AircraftIdentification);
            if (antennaLiveDataRecord != null)
            {
                if (!string.IsNullOrEmpty(antennaLiveDataRecord.TailNumber))
                {
                    swimFlightLegDto.AircraftIdentification = antennaLiveDataRecord.TailNumber;
                }
                else if (!string.IsNullOrEmpty(antennaLiveDataRecord.AircraftHexCode))
                {
                    List<AircraftHexTailMapping> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaLiveDataRecord.AircraftHexCode));
                    if (hexTailMappings != null && hexTailMappings.Any())
                    {
                        swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                    }
                }
            }
            else
            {
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.FirstOrDefault(x => x.AtcFlightNumber == swimFlightLegDto.AircraftIdentification);
                if (antennaHistoricalDataRecord != null)
                {
                    if (!string.IsNullOrEmpty(antennaHistoricalDataRecord.TailNumber))
                    {
                        swimFlightLegDto.AircraftIdentification = antennaHistoricalDataRecord.TailNumber;
                    }
                    else if (!string.IsNullOrEmpty(antennaHistoricalDataRecord.AircraftHexCode))
                    {
                        List<AircraftHexTailMapping> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(antennaHistoricalDataRecord.AircraftHexCode));
                        if (hexTailMappings != null && hexTailMappings.Any())
                        {
                            swimFlightLegDto.AircraftIdentification = hexTailMappings.First().TailNumber;
                        }
                    }
                }
            }
        }

        private void SetPricingTemplate(FlightLegDTO flightLegDto, PricingTemplateDto defaultCompanyPricingTemplate, string tailNumber, IEnumerable<Tuple<int, string, string>> pricingTemplates)
        {
            // no flight department ( not a fuelerlinx aircraft) - leave blank/empty
            // if assigned flight department, but no aircraft specific - then show the company's itp margin template
            if (string.IsNullOrWhiteSpace(flightLegDto.FlightDepartment))
            {
                return;
            }

            //flightLegDto.ITPMarginTemplate = "Company Pricing";
            var pricingTemplate = pricingTemplates.FirstOrDefault(x => x.Item2 == tailNumber);
            if (pricingTemplate != null && !string.IsNullOrWhiteSpace(pricingTemplate.Item3))
            {
                flightLegDto.ITPMarginTemplate = pricingTemplate.Item3;
            }
            else if(defaultCompanyPricingTemplate != null)
            {
                flightLegDto.ITPMarginTemplate = defaultCompanyPricingTemplate.Name;
            }
        }

        private async Task SetOrderInfo(int groupId, int fboId, IList<FlightLegDTO> flightLegs)
        {
            try
            {
                //DateTime atdMin = flightLegs.Min(x => x.ATDZulu);
                //DateTime etaMax = flightLegs.Where(x => x.ETAZulu != null).Max(x => x.ETAZulu.Value);
                var fuelOrders = await _FuelReqService.GetUpcomingDirectAndContractOrders(groupId, fboId, true);
                //var fuelOrders = (await _FuelReqService.GetDirectAndContractOrdersByGroupAndFbo(groupId, fboId, atdMin.AddHours(-2), etaMax.AddHours(2))).OrderByDescending(x => x.DateCreated).ToList();
                foreach (FlightLegDTO flightLeg in flightLegs)
                {
                    if (flightLeg.ETAZulu == null)
                    {
                        continue;
                    }

                    var existingFuelOrder = fuelOrders.FirstOrDefault(x => x.TailNumber == flightLeg.TailNumber && x.Etd >= flightLeg.ATDZulu.AddHours(-2) && x.Etd <= flightLeg.ATDZulu.AddHours(2) && x.Eta >= flightLeg.ETAZulu.Value.AddHours(-2) && x.Eta <= flightLeg.ETAZulu.Value.AddHours(2));
                    if (existingFuelOrder != null)
                    {
                        flightLeg.ID = existingFuelOrder.Oid;
                        flightLeg.FuelerlinxID = existingFuelOrder.SourceId;
                        flightLeg.Vendor = existingFuelOrder.Source;
                        flightLeg.TransactionStatus = existingFuelOrder.Cancelled != null && existingFuelOrder.Cancelled.Value ? "Cancelled" : "Live";
                        flightLeg.IsActiveFuelRelease = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _LoggingService.LogError(ex.Message, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
            }
        }

        private async Task SetVisitsToMyFBO(int groupId, int fboId, IList<FlightLegDTO> flightLegs)
        {
            try
            {
                List<string> distinctTailNumbers = flightLegs.Select(x => x.TailNumber).Distinct().ToList();
                List<AirportWatchHistoricalDataResponse> historicalData = await _AirportWatchService.GetArrivalsDeparturesRefactored(
                    groupId, fboId, new AirportWatchHistoricalDataRequest() { StartDateTime = DateTime.UtcNow.AddMonths(-1), EndDateTime = DateTime.UtcNow, FlightOrTailNumbers = distinctTailNumbers });
                foreach (FlightLegDTO flightLeg in flightLegs)
                {
                    flightLeg.VisitsToMyFBO = historicalData.Where(x => x.TailNumber == flightLeg.TailNumber && x.VisitsToMyFbo != null).Sum(x => x.VisitsToMyFbo.Value);
                    flightLeg.Arrivals = historicalData.Count(x => x.TailNumber == flightLeg.TailNumber && x.Status == "Arrival");
                    flightLeg.Departures = historicalData.Count(x => x.TailNumber == flightLeg.TailNumber && x.Status == "Departure");
                }
            }
            catch (Exception ex)
            {
                _LoggingService.LogError(ex.Message, ex.StackTrace, LogLevel.Error, LogColorCode.Red);
            }
            
        }

        private bool IsCorrectTailNumber(string aircraftIdentification)
        {
            if (string.IsNullOrEmpty(aircraftIdentification))
            {
                return false;
            }

            return aircraftIdentification.ToUpperInvariant().StartsWith('N');
        }

        private void LogMissedTailNumbers(IEnumerable<SWIMFlightLeg> flightLegs)
        {
            var missedTailNumbers = flightLegs.Where(x => !IsCorrectTailNumber(x.AircraftIdentification)).Select(x => x.AircraftIdentification).ToList();
            if (missedTailNumbers.Any())
            {
                _LoggingService.LogError("Missed Tail Numbers", string.Join(",", missedTailNumbers), LogLevel.Warning);
            }
        }

        //private FlightLegStatus GetFlightStatus(SWIMFlightLeg swimFlightLeg, SWIMFlightLegData latestSWIMFlightLegMessage, List<AcukwikAirport> airports, bool isAircraftOnGround, List<AirportWatchHistoricalData> antennaHistoricalData)
        private void SetFlightStatus(SWIMFlightLeg swimFlightLeg, bool isAircraftOnGround, AirportWatchHistoricalData latestAntennaHistoricalData)
        {
            FlightLegStatus flightLegStatus = FlightLegStatus.Arrived;

            // AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == swimFlightLeg.DepartureICAO);
            // AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == swimFlightLeg.ArrivalICAO);
            // if (arrivalAirport == null || string.IsNullOrEmpty(arrivalAirport.Latitude) || string.IsNullOrEmpty(arrivalAirport.Longitude) || 
            //     departureAirport == null || string.IsNullOrEmpty(departureAirport.Latitude) || string.IsNullOrEmpty(departureAirport.Longitude) ||
            //     latestSWIMFlightLegMessage == null)
            // {
            //     return flightLegStatus;
            // }
            //
            // double arrivalAirportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(arrivalAirport.Latitude);
            // double arrivalAirportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(arrivalAirport.Longitude);
            // var distanceFromArrivalAirport = new Coordinates(arrivalAirportLatitude, arrivalAirportLongitude)
            //     .DistanceTo(
            //         new Coordinates(latestSWIMFlightLegMessage.Latitude.Value, latestSWIMFlightLegMessage.Longitude.Value),
            //         UnitOfLength.Miles
            //     );
            // double departureAirportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(departureAirport.Latitude);
            // double departureAirportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(departureAirport.Longitude);
            // var distanceFromDepartureAirport = new Coordinates(departureAirportLatitude, departureAirportLongitude)
            //     .DistanceTo(
            //         new Coordinates(latestSWIMFlightLegMessage.Latitude.Value, latestSWIMFlightLegMessage.Longitude.Value),
            //         UnitOfLength.Miles
            //     );
            
            if (!isAircraftOnGround)
            {
                //if (distanceFromArrivalAirport < 5)
                if (swimFlightLeg.ETA != null && swimFlightLeg.ETA.Value.Subtract(DateTime.UtcNow).TotalMinutes < 5)
                {
                    flightLegStatus = FlightLegStatus.Landing;
                }
                //else if (distanceFromDepartureAirport < 5)
                else if (DateTime.UtcNow.Subtract(swimFlightLeg.ATD).TotalMinutes < 5)
                {
                    flightLegStatus = FlightLegStatus.Departing;
                }
                else
                {
                    flightLegStatus = FlightLegStatus.EnRoute;
                }
            }
            else if (latestAntennaHistoricalData != null)
            {
                if (latestAntennaHistoricalData.AircraftStatus == AircraftStatusType.Parking)
                {
                    flightLegStatus = FlightLegStatus.Arrived;
                }
                else if (latestAntennaHistoricalData.AircraftStatus == AircraftStatusType.Landing && latestAntennaHistoricalData.AircraftPositionDateTimeUtc > DateTime.UtcNow.AddMinutes(-30))
                {
                    flightLegStatus = FlightLegStatus.TaxiingDestination;
                }

            }

            swimFlightLeg.IsAircraftOnGround = isAircraftOnGround;
            swimFlightLeg.Status = flightLegStatus;
        }
    }
}
