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
            IEnumerable<SWIMFlightLeg> swimFlightLegs = await _FlightLegEntityService.GetListBySpec(new SWIMFlightLegSpecification(icao, null, DateTime.UtcNow.AddMinutes(FlightLegsFetchingThresholdMins)));
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
            List<SWIMFlightLeg> flightLegsToAdd = new List<SWIMFlightLeg>();
            
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
                    flightLegsToAdd.Add(swimFlightLegDto.Adapt<SWIMFlightLeg>());
                }
            }

            if (flightLegsToAdd.Count > 0)
            {
                LogMissedTailNumbers(flightLegsToAdd);
                await _FlightLegEntityService.BulkInsert(flightLegsToAdd, true);
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

        private async Task<IEnumerable<FlightLegDTO>> GetFlightLegs(IEnumerable<SWIMFlightLeg> swimFlightLegs, bool isArrivals, int groupId, int fboId,
            bool useHistoricalData = false, DateTime? historicalETD = null, DateTime? historicalETA = null, DateTime? historicalAircraftPositionDateTime = null)
        {
            List<SWIMFlightLegData> swimFlightLegMessages = await _FlightLegDataEntityService.GetListBySpec(new SWIMFlightLegDataSpecification(swimFlightLegs.Select(x => x.Oid).ToList()));

            List<string> airportICAOs = swimFlightLegs.Select(x => x.DepartureICAO).ToList();
            airportICAOs.AddRange(swimFlightLegs.Select(x => x.ArrivalICAO).ToList());
            airportICAOs = airportICAOs.Distinct().ToList();
            List<AcukwikAirport> airports = await _AcukwikAirportEntityService.GetListBySpec(new AcukwikAirportSpecification(airportICAOs));
            List<string> tailNumbers = swimFlightLegs.Select(x => x.AircraftIdentification).ToList();
            var flightDepartmentsByTailNumbers = await _CustomerAircraftEntityService.GetAircraftsByFlightDepartments(tailNumbers);
            IEnumerable<Tuple<int, string, string>> pricingTemplates = await _CustomerAircraftEntityService.GetPricingTemplates(tailNumbers);
            var aircraftIds = flightDepartmentsByTailNumbers.Select(x => x.Item1).Distinct().ToList();
            List<AirCrafts> aircrafts = await _AircraftEntityService.GetListBySpec(new AircraftSpecification(aircraftIds));
            var afsAircrafts = await _AFSAircraftEntityService.GetListBySpec(new AFSAircraftSpecification(aircraftIds));
            List<AirportWatchLiveData> antennaLiveData = await _AirportWatchLiveDataEntityService.GetListBySpec(new AirportWatchLiveDataByFlightNumberSpecification(tailNumbers, DateTime.UtcNow.AddHours(-1)));
            List<AirportWatchHistoricalData> antennaHistoricalData;
            if (useHistoricalData)
            {
                antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(tailNumbers, historicalETD.Value, historicalETA.Value));
            }
            else
            {
                antennaHistoricalData = await _AirportWatchHistoricalDataEntityService.GetListBySpec(new AirportWatchHistoricalDataSpecification(tailNumbers, DateTime.UtcNow.AddDays(-1)));
            }

            PricingTemplateDto defaultCompanyPricingTemplate = await _pricingTemplateService.GetDefaultTemplate(fboId);

            IList<FlightLegDTO> result = new List<FlightLegDTO>();
            foreach (SWIMFlightLeg swimFlightLeg in swimFlightLegs)
            {
                FlightLegDTO flightLegDto = new FlightLegDTO();
                //flightLegDto.Id = swimFlightLeg.Oid;
                flightLegDto.TailNumber = swimFlightLeg.AircraftIdentification;
                flightLegDto.DepartureICAO = swimFlightLeg.DepartureICAO;
                flightLegDto.ArrivalICAO = swimFlightLeg.ArrivalICAO;

                bool isAircraftOnGround = false;
                if (useHistoricalData)
                {
                    AirportWatchHistoricalData airportWatchHistoricalData = antennaHistoricalData.FirstOrDefault(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification && 
                        x.AircraftPositionDateTimeUtc >= historicalAircraftPositionDateTime.Value.AddMinutes(-5) && x.AircraftPositionDateTimeUtc <= historicalAircraftPositionDateTime.Value.AddMinutes(5));
                    if (airportWatchHistoricalData != null)
                    {
                        isAircraftOnGround = airportWatchHistoricalData.IsAircraftOnGround;
                    }
                }
                else
                {
                    AirportWatchLiveData latestAntennaLiveDataRecord = antennaLiveData.Where(x => x.AtcFlightNumber == swimFlightLeg.AircraftIdentification).OrderByDescending(x => x.AircraftPositionDateTimeUtc).FirstOrDefault();
                    if (latestAntennaLiveDataRecord != null)
                    {
                        isAircraftOnGround = latestAntennaLiveDataRecord.IsAircraftOnGround;
                    }
                }
                flightLegDto.IsAircraftOnGround = isAircraftOnGround;

                var aircraftByFlightDepartment = flightDepartmentsByTailNumbers.FirstOrDefault(x => x.Item2 == swimFlightLeg.AircraftIdentification);
                if (aircraftByFlightDepartment != null)
                {
                    flightLegDto.FlightDepartment = aircraftByFlightDepartment.Item3;
                    flightLegDto.Phone = aircraftByFlightDepartment.Item4;
                    var aircraft = aircrafts.FirstOrDefault(x => x.AircraftId == aircraftByFlightDepartment.Item1);
                    if (aircraft != null)
                    {
                        flightLegDto.Make = aircraft.Make;
                        flightLegDto.Model = aircraft.Model;
                        flightLegDto.FuelCapacityGal = aircraft.FuelCapacityGal;
                    }

                    var afsAircraft = afsAircrafts.FirstOrDefault(x => x.DegaAircraftID == aircraftByFlightDepartment.Item1);
                    if (afsAircraft != null)
                    {
                        flightLegDto.ICAOAircraftCode = afsAircraft.Icao;
                    }

                    SetPricingTemplate(flightLegDto, defaultCompanyPricingTemplate, swimFlightLeg.AircraftIdentification, pricingTemplates);
                }
                
                flightLegDto.ATDZulu = swimFlightLeg.ATD;
                AcukwikAirport departureAirport = airports.FirstOrDefault(x => x.Icao == flightLegDto.DepartureICAO);
                if (departureAirport != null)
                {
                    flightLegDto.ATDLocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ATD, departureAirport.IntlTimeZone, departureAirport.DaylightSavingsYn?.ToLower() == "y");
                    flightLegDto.DepartureCity = departureAirport.AirportCity;
                }
                else
                {
                    flightLegDto.ATDLocal = swimFlightLeg.ATD;
                }

                flightLegDto.ETAZulu = swimFlightLeg.ETA;
                AcukwikAirport arrivalAirport = airports.FirstOrDefault(x => x.Icao == flightLegDto.ArrivalICAO);
                if (arrivalAirport != null)
                {
                    flightLegDto.ETALocal = DateTimeHelper.GetLocalTime(swimFlightLeg.ETA, arrivalAirport.IntlTimeZone, arrivalAirport.DaylightSavingsYn?.ToLower() == "y");
                    flightLegDto.ArrivalCity = arrivalAirport.AirportCity;
                }
                else
                {
                    flightLegDto.ETALocal = swimFlightLeg.ETA;
                }
                
                flightLegDto.ETE = (flightLegDto.ETAZulu - DateTime.UtcNow).Duration();

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

                SWIMFlightLegData latestSWIMMessage =
                    swimFlightLegMessages.Where(x => x.SWIMFlightLegId == swimFlightLeg.Oid && x.Latitude != null && x.Longitude != null).OrderByDescending(x => x.Oid).FirstOrDefault();
                if (latestSWIMMessage != null)
                {
                    flightLegDto.ActualSpeed = latestSWIMMessage.ActualSpeed;
                    flightLegDto.Altitude = latestSWIMMessage.Altitude;
                    flightLegDto.Latitude = latestSWIMMessage.Latitude;
                    flightLegDto.Longitude = latestSWIMMessage.Longitude;

                    flightLegDto.Status = GetFlightStatus(swimFlightLeg, latestSWIMMessage, arrivalAirport, departureAirport, isAircraftOnGround, antennaHistoricalData);
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
                DateTime etaMax = flightLegs.Max(x => x.ETAZulu);
                var fuelOrders = (await _FuelReqService.GetFuelReqsByGroupAndFbo(groupId, fboId, atdMin.AddHours(-2), etaMax.AddHours(2))).OrderByDescending(x => x.DateCreated).ToList();
                foreach (FlightLegDTO flightLeg in flightLegs)
                {
                    var existingFuelOrder = fuelOrders.FirstOrDefault(x => x.TailNumber == flightLeg.TailNumber && x.Etd >= flightLeg.ATDZulu.AddHours(-2) && x.Etd <= flightLeg.ATDZulu.AddHours(2) && x.Eta >= flightLeg.ETAZulu.AddHours(-2) && x.Eta <= flightLeg.ETAZulu.AddHours(2));
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

        private FlightLegStatus GetFlightStatus(SWIMFlightLeg swimFlightLeg, SWIMFlightLegData latestSWIMFlightLegMessage, AcukwikAirport arrivalAirport, AcukwikAirport departureAirport, 
            bool isAircraftOnGround, List<AirportWatchHistoricalData> antennaHistoricalData)
        {
            FlightLegStatus flightLegStatus = FlightLegStatus.EnRoute;
            
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
