using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.Core.Utilities.Geography;
using FBOLinx.DB;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.AcukwikAirport;
using FBOLinx.DB.Specifications.Aircraft;
using FBOLinx.DB.Specifications.AircraftHexTailMapping;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.EntityServices.SWIM;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
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

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService,
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService,
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService, AircraftEntityService aircraftEntityService, ILoggingService loggingService, 
            AirportWatchService airportWatchService, IFuelReqService fuelReqService, IPricingTemplateService pricingTemplateService,
            AFSAircraftEntityService afsAircraftEntityService)
        {
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
        }

        public async Task<IEnumerable<FlightLegDTO>> GetDepartures(int groupId, int fboId, string icao)
        {
            List<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            List<SWIMFlightLeg> placeholderRecords = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(DateTime.UtcNow.AddMinutes(PlaceholderRecordsFetchingThresholdMins), true));
            if (placeholderRecords != null && placeholderRecords.Any())
            {
                swimFlightLegs.AddRange(placeholderRecords);
            }
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, false, groupId, fboId);

            return result;
        }

        public async Task<IEnumerable<FlightLegDTO>> GetArrivals(int groupId, int fboId, string icao)
        {
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(null, icao, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            IEnumerable<FlightLegDTO> result = await GetFlightLegs(swimFlightLegs, true, groupId, fboId);

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
                //swimFlightLegDto.SWIMFlightLegDataMessages = flightLegGrouping.SelectMany(x => x.SWIMFlightLegDataMessages).ToList();
                swimFlightLegDto.SWIMFlightLegDataMessages = new List<SWIMFlightLegDataDTO>() { flightLegGrouping.SelectMany(x => x.SWIMFlightLegDataMessages).ToList().First() };
                swimFlightLegDTOs.Add(swimFlightLegDto);
            }

            DateTime atdMin = swimFlightLegDTOs.Min(x => x.ATD);
            DateTime atdMax = swimFlightLegDTOs.Max(x => x.ATD);
            IList<string> departureICAOs = swimFlightLegDTOs.Select(x => x.DepartureICAO).Distinct().ToList();
            IList<string> arrivalICAOs = swimFlightLegDTOs.Select(x => x.ArrivalICAO).Distinct().ToList();
            
            List<string> flightNumbers = swimFlightLegDTOs.Where(x => !x.AircraftIdentification.ToUpperInvariant().StartsWith('N')).Select(x => x.AircraftIdentification).ToList();
            List<AirportWatchLiveData> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(
                new AirportWatchLiveDataByFlightNumberSpecification(flightNumbers, DateTime.UtcNow.AddHours(-1)));
            List<AirportWatchHistoricalData> antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                new AirportWatchHistoricalDataSpecification(flightNumbers, DateTime.UtcNow.AddDays(-1)));

            List<SWIMFlightLeg> existingFlightLegs = (await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(departureICAOs, arrivalICAOs, atdMin, atdMax))).OrderByDescending(x => x.ATD).ToList();
            //var existingFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(existingFlightLegs.Select(x => x.Oid).ToList()));
            
            List<SWIMFlightLegData> flightLegDataMessagesToInsert = new List<SWIMFlightLegData>();
            List<SWIMFlightLeg> flightLegsToUpdate = new List<SWIMFlightLeg>();
            List<SWIMFlightLeg> flightLegsToInsert = new List<SWIMFlightLeg>();
            
            foreach (SWIMFlightLegDTO swimFlightLegDto in swimFlightLegDTOs)
            {
                var existingLeg = existingFlightLegs.FirstOrDefault(
                    x => x.DepartureICAO == swimFlightLegDto.DepartureICAO && x.ArrivalICAO == swimFlightLegDto.ArrivalICAO && x.ATD == swimFlightLegDto.ATD);
                if (existingLeg != null)
                {
                    // var existingLegMessages = existingFlightLegMessages.Where(x => x.SWIMFlightLegId == existingLeg.Oid).ToList();
                    // if (!existingLegMessages.Any())
                    // {
                    //     continue;
                    // }
                    // DateTime latestMessageTimestamp = existingLegMessages.Max(x => x.MessageTimestamp);
                    // DateTime latestExistingETA = existingLegMessages.OrderByDescending(x => x.Oid).First().ETA;
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages)
                    {
                        // double messageTimestampThreshold = Math.Abs((flightLegDataMessageDto.MessageTimestamp - latestMessageTimestamp).TotalSeconds);
                        // if (messageTimestampThreshold > MessageThresholdSec &&
                        //     (flightLegDataMessageDto.ActualSpeed != null ||
                        //      flightLegDataMessageDto.Altitude != null || flightLegDataMessageDto.Latitude != null ||
                        //      flightLegDataMessageDto.Longitude != null || flightLegDataMessageDto.ETA != latestExistingETA))
                        // {
                        //     flightLegDataMessageDto.SWIMFlightLegId = existingLeg.Oid;
                        //     flightLegDataMessagesToInsert.Add(flightLegDataMessageDto.Adapt<SWIMFlightLegData>());
                        // }
                        flightLegDataMessageDto.SWIMFlightLegId = existingLeg.Oid;
                        flightLegDataMessagesToInsert.Add(flightLegDataMessageDto.Adapt<SWIMFlightLegData>());
                    }

                    if (existingLeg.ETA != swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA)
                    {
                        existingLeg.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                        flightLegsToUpdate.Add(existingLeg);
                    }
                }
                else
                {
                    await SetTailNumber(swimFlightLegDto, antennaLiveData, antennaHistoricalData);
                    swimFlightLegDto.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                    var flightLegToInsert = swimFlightLegDto.Adapt<SWIMFlightLeg>();
                    flightLegToInsert.Status = FlightLegStatus.Departing;
                    flightLegsToInsert.Add(flightLegToInsert);
                }
            }

            List<SWIMFlightLeg> existingPlaceholderRecordsToDelete = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(flightLegsToInsert.Select(x => x.AircraftIdentification).ToList(), DateTime.UtcNow.AddMinutes(PlaceholderRecordsFetchingThresholdMins), true));

            if (existingPlaceholderRecordsToDelete.Count > 0)
            {
                await _FlightLegEntityService.BulkDeleteEntities(existingPlaceholderRecordsToDelete);
            }

            if (flightLegsToInsert.Count > 0)
            {
                LogMissedTailNumbers(flightLegsToInsert);
                await _FlightLegEntityService.BulkInsert(flightLegsToInsert, true);
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

        public async Task CreatePlaceholderRecordsAndUpdateFlightStatus()
        {
            var tailNumbersByLiveAndParkingCoordinates =
                await _AirportWatchLiveDataEntityService.GetTailNumbersByLiveAndParkingCoordinates(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(-24));

            List<SWIMFlightLeg> existingFlightLegs = await _FlightLegEntityService.GetListBySpec(
                new SWIMFlightLegSpecification(tailNumbersByLiveAndParkingCoordinates.Select(x => x.Item1).ToList(), tailNumbersByLiveAndParkingCoordinates.Select(x => x.Item2).ToList(), DateTime.UtcNow.AddHours(-1)));
            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification());

            List<SWIMFlightLeg> placeholderRecordsToInsert = new List<SWIMFlightLeg>();
            List<SWIMFlightLeg> existingFlightLegsToUpdate = new List<SWIMFlightLeg>();

            foreach (var tailNumberByCoordinates in tailNumbersByLiveAndParkingCoordinates)
            {
                SWIMFlightLeg existingFlightLeg = existingFlightLegs.FirstOrDefault(
                    x => x.AircraftIdentification == tailNumberByCoordinates.Item1 || x.AircraftIdentification == tailNumberByCoordinates.Item2);
                
                var distanceFromParkingOccurrence = new Coordinates(tailNumberByCoordinates.Item3, tailNumberByCoordinates.Item4)
                    .DistanceTo(
                        new Coordinates(tailNumberByCoordinates.Item5, tailNumberByCoordinates.Item6),
                        UnitOfLength.Miles
                    );
                if (distanceFromParkingOccurrence > 0.2)
                {
                    if (existingFlightLeg != null)
                    {
                        existingFlightLeg.Status = FlightLegStatus.Taxiing;
                        existingFlightLegsToUpdate.Add(existingFlightLeg);
                    }
                    else
                    {
                        foreach (AcukwikAirport airport in airports.Where(x => x.Latitude != null && x.Longitude != null)) // && x.Icao != null
                        {
                            double airportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(airport.Latitude);
                            double airportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(airport.Longitude);
                            var distanceFromAirport = new Coordinates(airportLatitude, airportLongitude)
                                .DistanceTo(
                                    new Coordinates(tailNumberByCoordinates.Item5, tailNumberByCoordinates.Item6),
                                    UnitOfLength.Miles
                                );
                            if (distanceFromAirport < 5)
                            {
                                SWIMFlightLeg placeholderRecord = new SWIMFlightLeg();
                                placeholderRecord.DepartureICAO = airport.Icao;
                                placeholderRecord.AircraftIdentification = tailNumberByCoordinates.Item1;
                                placeholderRecord.ATD = DateTime.UtcNow;
                                placeholderRecord.IsPlaceholder = true;
                                placeholderRecord.Status = FlightLegStatus.Taxiing;
                                placeholderRecord.IsAircraftOnGround = true;
                                
                                placeholderRecordsToInsert.Add(placeholderRecord);
                                break;
                            }
                        }
                    }
                }
            }

            if (existingFlightLegsToUpdate.Count > 0)
            {
                await _FlightLegEntityService.BulkUpdate(existingFlightLegsToUpdate);
            }

            if (placeholderRecordsToInsert.Count > 0)
            {
                await _FlightLegEntityService.BulkInsert(placeholderRecordsToInsert);
            }
        }

        public async Task UpdateFlightLegs()
        {
            
            List<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
            List<SWIMFlightLegData> swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList()));

            List<string> airportICAOs = swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.DepartureICAO)).Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.ArrivalICAO)).Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            List<string> tailNumbers = swimFlightLegs.Select(x => x.AircraftIdentification).Distinct().ToList();
            var antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(tailNumbers, DateTime.UtcNow.AddDays(-1)));
            List<AirportWatchLiveData> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(
                new AirportWatchLiveDataByFlightNumberSpecification(tailNumbers, DateTime.UtcNow.AddHours(-1)));

            var flightDepartmentsByTailNumbers = await _CustomerAircraftEntityService.GetAircraftsByFlightDepartments(tailNumbers);
            IEnumerable<Tuple<int, string, string>> pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(tailNumbers);
            var aircraftIds = flightDepartmentsByTailNumbers.Select(x => x.Item1).Distinct().ToList();
            List<AirCrafts> aircrafts = await _AircraftEntityService.GetListBySpec(new AircraftSpecification(aircraftIds));
            var afsAircrafts = await _AFSAircraftEntityService.GetListBySpec(new AFSAircraftSpecification(aircraftIds));

            List<SWIMFlightLeg> swimFlightLegsToUpdate = new List<SWIMFlightLeg>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs.Where(x => x.ArrivalICAO != null)) // skip placeholder records
            {
                AirportWatchLiveData latestAntennaLiveDataRecord = antennaLiveData.Where(
                    x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification || x.TailNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                if (latestAntennaLiveDataRecord != null)
                {
                    swimFlightLeg.IsAircraftOnGround = latestAntennaLiveDataRecord.IsAircraftOnGround;
                }

                SWIMFlightLegData latestSwimMessage =
                    swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid && x.Latitude != null && x.Longitude != null).OrderByDescending(x => x.Oid).FirstOrDefault();

                if (latestSwimMessage != null)
                {
                    swimFlightLeg.Status = GetFlightStatus(swimFlightLeg, latestSwimMessage, airports, swimFlightLeg.IsAircraftOnGround, antennaHistoricalData);

                    AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == swimFlightLeg.DepartureICAO);
                    if (departureAirport != null)
                    {
                        swimFlightLeg.DepartureCity = departureAirport.AirportCity;
                        swimFlightLeg.ATDLocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                    }
                    AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == swimFlightLeg.ArrivalICAO);
                    if (arrivalAirport != null)
                    {
                        swimFlightLeg.ArrivalCity = arrivalAirport.AirportCity;
                        if (swimFlightLeg.ETA != null)
                        {
                            swimFlightLeg.ETALocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ETA.Value, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                        }
                    }

                    //swimFlightLeg.ETA = latestSwimMessage.ETA; // Updated on SaveFlightLegData
                    swimFlightLeg.ActualSpeed = latestSwimMessage.ActualSpeed;
                    swimFlightLeg.Altitude = latestSwimMessage.Altitude;
                    swimFlightLeg.Latitude = latestSwimMessage.Latitude;
                    swimFlightLeg.Longitude = latestSwimMessage.Longitude;
                }

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

                swimFlightLegsToUpdate.Add(swimFlightLeg);
            }

            await _FlightLegEntityService.BulkUpdate(swimFlightLegsToUpdate);
        }
        
        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs, bool isArrivals, int groupId, int fboId,
            bool useHistoricalData = false, DateTime? historicalETD = null, DateTime? historicalETA = null, DateTime? historicalAircraftPositionDateTime = null)
        {
            List<string> tailNumbers = swimFlightLegs.Select(x => x.AircraftIdentification).ToList();
            IEnumerable<Tuple<int, string, string>> pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(tailNumbers);

            List<AirportWatchHistoricalData> antennaHistoricalData = null;
            List<AcukwikAirport> airports = null;
            List<SWIMFlightLegData> swimFlightLegMessages = null;
            if (useHistoricalData)
            {
                List<string> airportICAOs = swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.DepartureICAO)).Select(x => x.DepartureICAO).ToList();
                airportICAOs.AddRange(swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.ArrivalICAO)).Select(x => x.ArrivalICAO).ToList());
                airportICAOs = airportICAOs.Distinct().ToList();

                antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(
                    new AirportWatchHistoricalDataSpecification(tailNumbers, historicalETD.Value, historicalETA.Value));
                airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
                swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList()));
            }

            PricingTemplateDto defaultCompanyPricingTemplate = await _pricingTemplateService.GetDefaultTemplate(fboId);

            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs)
            {
                FlightLegDTO flightLegDto = new FlightLegDTO();
                //flightLegDto.Id = swimFlightLeg.Oid;
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
                flightLegDto.Model = swimFlightLeg.Model;
                flightLegDto.FuelCapacityGal = swimFlightLeg.FuelCapacityGal;
                flightLegDto.Phone = swimFlightLeg.Phone;
                flightLegDto.ICAOAircraftCode = swimFlightLeg.ICAOAircraftCode;
                
                flightLegDto.ATDZulu = swimFlightLeg.ATD;
                flightLegDto.ATDLocal = swimFlightLeg.ATDLocal ?? swimFlightLeg.ATD;

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

            await SetVisitsToMyFBO(groupId, fboId, result);
            await SetOrderInfo(groupId, fboId, result);

            return result;
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

                flightLegDto.Status = GetFlightStatus(swimFlightLeg, latestSWIMMessage, airports, flightLegDto.IsAircraftOnGround, antennaHistoricalData);
            }
        }

        private async Task SetTailNumber(SWIMFlightLegDTO swimFlightLegDto, List<FBOLinx.DB.Models.AirportWatchLiveData> antennaLiveData, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            if (IsCorrectTailNumber(swimFlightLegDto.AircraftIdentification))
            {
                return;
            }

            FBOLinx.DB.Models.AirportWatchLiveData antennaLiveDataRecord = antennaLiveData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
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
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
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
                DateTime atdMin = flightLegs.Min(x => x.ATDZulu);
                DateTime etaMax = flightLegs.Where(x => x.ETAZulu != null).Max(x => x.ETAZulu.Value);
                var fuelOrders = (await _FuelReqService.GetFuelReqsByGroupAndFbo(groupId, fboId, atdMin.AddHours(-2), etaMax.AddHours(2))).OrderByDescending(x => x.DateCreated).ToList();
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
                List<AirportWatchHistoricalDataResponse> historicalData = await _AirportWatchService.GetArrivalsDeparturesRefactored(
                    groupId, fboId, new AirportWatchHistoricalDataRequest() { StartDateTime = DateTime.UtcNow.AddMonths(-1), EndDateTime = DateTime.UtcNow });
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

        private FlightLegStatus GetFlightStatus(SWIMFlightLeg swimFlightLeg, SWIMFlightLegData latestSWIMFlightLegMessage, List<AcukwikAirport> airports, 
            bool isAircraftOnGround, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            FlightLegStatus flightLegStatus = FlightLegStatus.Arrived;

            AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == swimFlightLeg.DepartureICAO);
            AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == swimFlightLeg.ArrivalICAO);
            if (arrivalAirport == null || string.IsNullOrEmpty(arrivalAirport.Latitude) || string.IsNullOrEmpty(arrivalAirport.Longitude) || 
                departureAirport == null || string.IsNullOrEmpty(departureAirport.Latitude) || string.IsNullOrEmpty(departureAirport.Longitude) ||
                latestSWIMFlightLegMessage == null)
            {
                return flightLegStatus;
            }

            double arrivalAirportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(arrivalAirport.Latitude);
            double arrivalAirportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(arrivalAirport.Longitude);
            var distanceFromArrivalAirport = new Coordinates(arrivalAirportLatitude, arrivalAirportLongitude)
                .DistanceTo(
                    new Coordinates(latestSWIMFlightLegMessage.Latitude.Value, latestSWIMFlightLegMessage.Longitude.Value),
                    UnitOfLength.Miles
                );
            double departureAirportLatitude = LocationHelper.GetLatitudeGeoLocationFromGPS(departureAirport.Latitude);
            double departureAirportLongitude = LocationHelper.GetLongitudeGeoLocationFromGPS(departureAirport.Longitude);
            var distanceFromDepartureAirport = new Coordinates(departureAirportLatitude, departureAirportLongitude)
                .DistanceTo(
                    new Coordinates(latestSWIMFlightLegMessage.Latitude.Value, latestSWIMFlightLegMessage.Longitude.Value),
                    UnitOfLength.Miles
                );
            
            if (!isAircraftOnGround)
            {
                if (distanceFromArrivalAirport < 5)
                {
                    flightLegStatus = FlightLegStatus.Landing;
                }
                else if (distanceFromDepartureAirport < 5)
                {
                    flightLegStatus = FlightLegStatus.Departing;
                }
                else
                {
                    flightLegStatus = FlightLegStatus.EnRoute;
                }
            }
            else
            {
                AirportWatchHistoricalData antennaHistoricalDataRecord = antennaHistoricalData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                
                if (antennaHistoricalDataRecord != null)
                {
                    if (antennaHistoricalDataRecord.AircraftStatus == AircraftStatusType.Parking)
                    {
                        flightLegStatus = FlightLegStatus.Arrived;
                    }
                    else if (antennaHistoricalDataRecord.AircraftStatus == AircraftStatusType.Landing && antennaHistoricalDataRecord.AircraftPositionDateTimeUtc > DateTime.UtcNow.AddMinutes(-30))
                    {
                        flightLegStatus = FlightLegStatus.Taxiing;
                    }

                }
            }

            return flightLegStatus;
        }
    }
}
