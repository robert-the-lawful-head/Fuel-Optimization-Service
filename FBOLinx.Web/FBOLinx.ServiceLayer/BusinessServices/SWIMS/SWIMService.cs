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
using FBOLinx.ServiceLayer.BusinessServices.SWIMS;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.Responses.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
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
        private ISWIMFlightLegService _SwimFlightLegService;

        public SWIMService(SWIMFlightLegEntityService flightLegEntityService, SWIMFlightLegDataEntityService flightLegDataEntityService,
            AirportWatchLiveDataEntityService airportWatchLiveDataEntityService, AircraftHexTailMappingEntityService aircraftHexTailMappingEntityService,
            AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService, AcukwikAirportEntityService acukwikAirportEntityService,
            ICustomerAircraftEntityService customerAircraftEntityService, AircraftEntityService aircraftEntityService, ILoggingService loggingService, 
            AirportWatchService airportWatchService, IFuelReqService fuelReqService, IPricingTemplateService pricingTemplateService,
            AFSAircraftEntityService afsAircraftEntityService,
            FAAAircraftMakeModelEntityService faaAircraftMakeModelEntityService,
            IAirportWatchFlightLegStatusService airportWatchFlightLegStatusService,
            ISWIMFlightLegService swimFlightLegService)
        {
            _SwimFlightLegService = swimFlightLegService;
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
                swimFlightLegDto.SWIMFlightLegDataMessages = new List<SWIMFlightLegDataDTO>() { flightLegGrouping.SelectMany(x => x.SWIMFlightLegDataMessages)
                    .OrderBy(x => x.Latitude.HasValue ? 1 : 2)
                    .ThenByDescending(x => x.MessageTimestamp)
                    .ToList().First() };
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
                    foreach (SWIMFlightLegDataDTO flightLegDataMessageDto in swimFlightLegDto.SWIMFlightLegDataMessages.Where(x => (x.Latitude != null && x.Longitude != null) || x.Altitude != null || x.ActualSpeed != null))
                    {
                        flightLegDataMessageDto.SWIMFlightLegId = existingLeg.Oid;
                        flightLegDataMessageDto.MessageTimestamp = DateTime.UtcNow;
                        flightLegDataMessagesToInsert.Add(flightLegDataMessageDto.Adapt<SWIMFlightLegData>());
                    }

                    if (existingLeg.ETA != swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA)
                    {
                        existingLeg.LastUpdated = DateTime.UtcNow;
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
                    //[#3jv5g9w] Setting the tail number is no longer needed.
                    //await SetTailNumber(swimFlightLegDto, antennaLiveData, antennaHistoricalData);
                    swimFlightLegDto.ETA = swimFlightLegDto.SWIMFlightLegDataMessages.First().ETA;
                    var flightLegToInsert = swimFlightLegDto.Adapt<SWIMFlightLeg>();
                    flightLegToInsert.Status = FlightLegStatus.Departing;
                    swimFlightLegDto.LastUpdated = DateTime.UtcNow;

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

        public async Task SyncRecentAndUpcomingFlightLegs()
        {
            //Load our data to work with from both SWIM and AirportWatch
            var flightWatchModelsWithLegStatuses =
                await _AirportWatchFlightLegStatusService.GetAirportWatchLiveDataWithFlightLegStatuses();

            //Ensure we set the FAA make/model and any other static data needed for any that don't have it yet
            await SetFlightLegsStaticData(flightWatchModelsWithLegStatuses);

            //Update the flight leg with the latest lat/lng, altitude, and speed
            await SetFlightLegsLocationAndTrajectory(flightWatchModelsWithLegStatuses);

            //Grab all existing record that will need to be updated
            List<SWIMFlightLegDTO> existingFlightLegsToUpdate = flightWatchModelsWithLegStatuses.Where(x => x.SWIMFlightLegId.GetValueOrDefault() > 0 && x.DoesSWIMFlightLegNeedUpdate())
                .Select(x => x.GetSwimFlightLeg()).Distinct().ToList();

            //Add new records as "placeholders" for taxi'ing departure/departing legs that haven't been sent by the FAA feed yet
            List<SWIMFlightLegDTO> placeholderRecordsToInsert = flightWatchModelsWithLegStatuses.Where(x =>
                    x.SWIMFlightLegId.GetValueOrDefault() <= 0 && x.GetAirportPosition() != null && (x.Status == FlightLegStatus.TaxiingOrigin || x.Status == FlightLegStatus.Departing))
                .Select(x => new SWIMFlightLegDTO()
                {
                    DepartureICAO = x.GetAirportPosition().Icao,
                    AircraftIdentification = x.AircraftIdentification,
                    ATD = DateTime.UtcNow,
                    ATDLocal = DateTimeHelper.GetLocalTime(DateTime.UtcNow, x.GetAirportPosition().IntlTimeZone, x.GetAirportPosition().DaylightSavingsYn?.ToLower() == "y"),
                    IsPlaceholder = true,
                    Status = x.Status.GetValueOrDefault(),
                    IsAircraftOnGround = true,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    LastUpdated = DateTime.UtcNow
                }).ToList();
            
            //Bulk update and insert all legs that need it
            if (existingFlightLegsToUpdate.Count > 0)
            {
                await _SwimFlightLegService.BulkUpdate(existingFlightLegsToUpdate);
            }

            if (placeholderRecordsToInsert.Count > 0)
            {
                await _SwimFlightLegService.BulkInsert(placeholderRecordsToInsert);
            }
        }
        

        private async Task SetFlightLegsLocationAndTrajectory(List<FlightWatchModel> flightWatchModels)
        {
            var flightWatchModelsToProcess = flightWatchModels.Where(x =>
                x.SWIMFlightLegId > 0 && x.GetSwimFlightLeg() != null).ToList();

            Stopwatch stopwatch = new Stopwatch();
            List<SWIMFlightLegData> swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(flightWatchModelsToProcess.Where(x => x.SWIMFlightLegId > 0).Select(x => x.SWIMFlightLegId.GetValueOrDefault()).ToList(), DateTime.UtcNow.AddMinutes(-2)));
            stopwatch.Stop();
            stopwatch.Restart();
            Parallel.ForEach(flightWatchModelsToProcess, flightWatchModel =>
            {
                var swimFlightLeg = flightWatchModel.GetSwimFlightLeg();
                SWIMFlightLegData latestSwimMessage =
                    swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid && x.Latitude != null && x.Longitude != null).OrderByDescending(x => x.MessageTimestamp).FirstOrDefault();

                if (latestSwimMessage != null)
                {
                    //swimFlightLeg.ETA = latestSwimMessage.ETA; // Updated on SaveFlightLegData
                    swimFlightLeg.ActualSpeed = latestSwimMessage.ActualSpeed;
                    swimFlightLeg.Altitude = latestSwimMessage.Altitude;
                    swimFlightLeg.Latitude = latestSwimMessage.Latitude;
                    swimFlightLeg.Longitude = latestSwimMessage.Longitude;
                    swimFlightLeg.LastUpdated = latestSwimMessage.MessageTimestamp;
                    flightWatchModel.MarkSWIMFlightLegAsNeedingUpdate();
                }
            });
            
            stopwatch.Stop();
        }

        private async Task SetFlightLegsStaticData(List<FlightWatchModel> flightWatchModels)
        {
            var flightWatchModelsToProcess = flightWatchModels.Where(x =>
                x.SWIMFlightLegId > 0 && x.GetSwimFlightLeg() != null && !x.GetSwimFlightLeg().IsProcessed &&
                !string.IsNullOrEmpty(x.TailNumber)).ToList();
            List<string> tailNumbers = flightWatchModelsToProcess
                .Select(x => x.TailNumber).Distinct().ToList();

            List<AircraftHexTailMapping> hexTailMappings = await _AircraftHexTailMappingEntityService.GetListBySpec(new AircraftHexTailMappingSpecification(tailNumbers));
            var aircraftMakeModels = await _FAAAircraftMakeModelEntityService.GetListBySpec(new AircraftMakeModelSpecification(hexTailMappings.Select(x => x.FAAAircraftMakeModelCode).Distinct().ToList()));

            Parallel.ForEach(flightWatchModelsToProcess, flightWatchModel =>
            {
                var swimFlightLeg = flightWatchModel.GetSwimFlightLeg();
                var tailMapping = hexTailMappings.FirstOrDefault(x => x.TailNumber == flightWatchModel.TailNumber);
                if (tailMapping != null)
                {
                    swimFlightLeg.FAARegisteredOwner = tailMapping.FAARegisteredOwner;

                    FAAAircraftMakeModelReference aircraftMakeModelReference =
                        aircraftMakeModels.FirstOrDefault(x => x.CODE == tailMapping.FAAAircraftMakeModelCode);
                    if (aircraftMakeModelReference != null)
                    {
                        swimFlightLeg.FAAMake = aircraftMakeModelReference.MFR;
                        swimFlightLeg.FAAModel = aircraftMakeModelReference.MODEL;
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
                flightWatchModel.MarkSWIMFlightLegAsNeedingUpdate();
            });
        }
        
        //TODO: Refactor this.  Get a lot of this code into it's own set of services.
        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs, bool isArrivals, int groupId, int fboId)
        {
            List<string> aircraftIdentifications = swimFlightLegs.Where(x => !string.IsNullOrEmpty(x.AircraftIdentification)).Select(x => x.AircraftIdentification).Distinct().ToList();
            IEnumerable<Tuple<int, string, string>> pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(aircraftIdentifications);
            var customerAircrafts = await _CustomerAircraftEntityService.GetListBySpec(new CustomerAircraftsByGroupSpecification(groupId, aircraftIdentifications));

            List<AirportWatchHistoricalData> antennaHistoricalData = null;
            List<AcukwikAirport> airports = null;
            List<SWIMFlightLegData> swimFlightLegMessages = null;

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
                flightLegDto.FAARegisteredOwner = swimFlightLeg.FAARegisteredOwner;

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
        
        [Obsolete("3jv5g9w - Setting the tail number of the SWIM flight leg feed is no longer needed.  The SWIM data should maintain it's original aircraft identification and the tail number is determined through the FlightWatchService join on antenna data.")]
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
    }
}
