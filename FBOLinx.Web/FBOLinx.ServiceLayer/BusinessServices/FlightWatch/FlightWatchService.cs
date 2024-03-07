﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Constants;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.CompanyPricingLog;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.SWIMS;
using FBOLinx.ServiceLayer.Demo;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using FBOLinx.ServiceLayer.Logging;
using Geolocation;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.FlightWatch
{
    public interface IFlightWatchService
    {
        Task<List<FlightWatchModel>> GetCurrentFlightWatchData(FlightWatchDataRequestOptions options, bool isFlightWatchMapData = false);
        Task<FlightWatchLegAdditionalDetailsModel> GetAdditionalDetailsForLeg(int swimFlightLegId);
        Task<List<FlightWatchModel>> GetCurrentLightWeightFlightWatchData(FlightWatchDataRequestOptions options);
        Task<FlightWatchModel> GetCurrentFlightWatchLiveDataWithHistorical(FlightWatchDataRequestOptions options,int liveDataOid);
    }

    public class FlightWatchService : IFlightWatchService
    {
        private FlightWatchDataRequestOptions _Options;
        private FbosDto _Fbo;
        private IAirportWatchLiveDataService _AirportWatchLiveDataService;
        private IFboService _FboService;
        private ISWIMFlightLegService _SwimFlightLegService;
        private IAirportService _AirportService;
        private IFuelReqService _FuelReqService;
        private ICustomerAircraftService _CustomerAircraftService;
        private List<CustomerAircraftsViewModel> _CustomerAircrafts;
        private IAirportFboGeofenceClustersService _AirportFboGeofenceClustersService;
        private AirportFboGeofenceClustersDto _GeoFenceCluster;
        private List<string> _DistinctTailNumbers;
        private ICompanyPricingLogService _CompanyPricingLogService;
        private List<CompanyPricingLogMostRecentQuoteModel> _MostRecentQuotes;
        private IDemoFlightWatch _demoFlightWatch;
        private IAircraftHexTailMappingService _AircraftHexTailMappingService;
        private readonly ILoggingService _LoggingService;
        public FlightWatchService(IAirportWatchLiveDataService airportWatchLiveDataService,
            IFboService fboService,
            ISWIMFlightLegService swimFlightLegService,
            IAirportService airportService,
            IFuelReqService fuelReqService,
            ICustomerAircraftService customerAircraftService,
            IAirportFboGeofenceClustersService airportFboGeofenceClustersService,
            ICompanyPricingLogService companyPricingLogService,
            IDemoFlightWatch demoFlightWatch,
            IAircraftHexTailMappingService aircraftHexTailMappingService,
            ILoggingService loggingService
        )
        {
            _AircraftHexTailMappingService = aircraftHexTailMappingService;
            _CompanyPricingLogService = companyPricingLogService;
            _AirportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _CustomerAircraftService = customerAircraftService;
            _FuelReqService = fuelReqService;
            _AirportService = airportService;
            _SwimFlightLegService = swimFlightLegService;
            _FboService = fboService;
            _AirportWatchLiveDataService = airportWatchLiveDataService;
            _demoFlightWatch = demoFlightWatch;
            _LoggingService = loggingService;
        }

        public async Task<FlightWatchLegAdditionalDetailsModel> GetAdditionalDetailsForLeg(int swimFlightLegId)
        {
            FlightWatchLegAdditionalDetailsModel result = new FlightWatchLegAdditionalDetailsModel();
            result.SWIMFlightLeg = await _SwimFlightLegService.GetSingleBySpec(new SWIMFlightLegSpecification(swimFlightLegId));
            if (result.SWIMFlightLeg == null)
                return null;
            if (!string.IsNullOrEmpty(result.SWIMFlightLeg.DepartureICAO))
                result.DepartureAirportPositionInfo =
                    await _AirportService.GetAirportPositionByAirportIdentifier(result.SWIMFlightLeg.DepartureICAO);
            if (!string.IsNullOrEmpty(result.SWIMFlightLeg.ArrivalICAO))
                result.ArrivalAirportPositionInfo =
                    await _AirportService.GetAirportPositionByAirportIdentifier(result.SWIMFlightLeg.ArrivalICAO);
            result.SWIMFlightLeg?.SWIMFlightLegDataMessages?.RemoveAll(x =>
                !x.Latitude.HasValue || !x.Longitude.HasValue);

            return result;
        }

        public async Task<List<FlightWatchModel>> GetCurrentFlightWatchData(FlightWatchDataRequestOptions options, bool isFlightWatchMapData = false)
        {
            _Options = options;
            _Fbo = await _FboService.GetSingleBySpec(
                new FboByIdSpecification(options.FboIdForCenterPoint.GetValueOrDefault()));

            //Default to 1 day back unless we need to check on visits for the past 30 days.
            var daysToCheckBackForHistoricalData = options.IncludeRecentHistoricalRecords ? 1 : 0;
            if (options.IncludeVisitsAtFbo)
                daysToCheckBackForHistoricalData = 30;

            //Load all AirportWatch Live data along with related Historical data.
            var liveDataWithHistoricalInfo =
                await _AirportWatchLiveDataService.GetAirportWatchLiveDataWithHistoricalStatuses(GetFocusedAirportIdentifier(), 2, daysToCheckBackForHistoricalData);

            //Grab the airport to be considered for arrivals and departures.
            var airportsForArrivalsAndDepartures = await GetViableAirportsForSWIMData();

            //Then load all SWIM flight legs that we have from the last hour.
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs;
            if ((airportsForArrivalsAndDepartures?.Count).GetValueOrDefault() > 0)
                swimFlightLegs = (await _SwimFlightLegService.GetRecentSWIMFlightLegs(airportsForArrivalsAndDepartures)).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));
            else
                swimFlightLegs = (await _SwimFlightLegService.GetRecentSWIMFlightLegs(30000)).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));

            var distinctTails = liveDataWithHistoricalInfo.Select(x => x.TailNumber).Concat(swimFlightLegs.Select(x => x.AircraftIdentification)).Distinct().ToList();
            var hexTailMappings = await _AircraftHexTailMappingService.GetAircraftHexTailMappingsForTails(distinctTails);

            //Combine the results so we see every flight picked up by both AirportWatch and SWIM.
            var result = CombineAirportWatchAndSWIMData(liveDataWithHistoricalInfo, swimFlightLegs, hexTailMappings);

            //Load any additional data needed that was related to each flight.
            await PopulateAdditionalDataFromOptions(result);

            if (_demoFlightWatch.IsDemoDataVisibleByFboId(options.FboIdForCenterPoint))
                AddDemoDataToFlightWatchResult(result,_Fbo);

            return result;
        }
        public async Task<FlightWatchModel> GetCurrentFlightWatchLiveDataWithHistorical(FlightWatchDataRequestOptions options, int liveDataOid)
        {
            _Options = options;
            _Fbo = await _FboService.GetSingleBySpec(
                new FboByIdSpecification(options.FboIdForCenterPoint.GetValueOrDefault()));

            //Default to 1 day back unless we need to check on visits for the past 30 days.
            var daysToCheckBackForHistoricalData = options.IncludeRecentHistoricalRecords ? 1 : 0;
            if (options.IncludeVisitsAtFbo)
                daysToCheckBackForHistoricalData = 30;

            var airportsForArrivalsAndDepartures = await GetViableAirportsForSWIMData();

            var liveData = await _AirportWatchLiveDataService.FindAsync(liveDataOid);

            var flightLeg = (liveData == null) ?
                (await _SwimFlightLegService.GetRecentSWIMFlightLegs(airportsForArrivalsAndDepartures)).Where(x => x.Oid == liveDataOid).FirstOrDefault() :
                (await _SwimFlightLegService.GetRecentSWIMFlightLegs(airportsForArrivalsAndDepartures)).Where(x => x.AircraftIdentification == liveData.TailNumber).FirstOrDefault();


            var tailNumber = liveData?.TailNumber ?? flightLeg?.AircraftIdentification;

            var aicraftHistoricalInfo =
                (await _AirportWatchLiveDataService.GetHistoricalData(new List<string>() { liveData?.AircraftHexCode }, GetFocusedAirportIdentifier(), daysToCheckBackForHistoricalData)).Where(x => x.TailNumber == tailNumber ).ToList();

            var hexTailMapping = (liveData?.Oid == null) ? null : (await _AircraftHexTailMappingService.GetAircraftHexTailMappingsForTails((tailNumber == null)? new List<string>() : new List<string>() { tailNumber })).FirstOrDefault();

            var flightWatch = new FlightWatchModel(liveData,
                          aicraftHistoricalInfo,
                          flightLeg,
                          hexTailMapping) { };

            var result = new List<FlightWatchModel>() { flightWatch };
            //Load any additional data needed that was related to each flight.
            await PopulateAdditionalDataFromOptions(result);

            return result.FirstOrDefault();
        }
        public async Task<List<FlightWatchModel>> GetCurrentLightWeightFlightWatchData(FlightWatchDataRequestOptions options)
        {
            _Options = options;
            _Fbo = await _FboService.GetSingleBySpec(
                new FboByIdSpecification(options.FboIdForCenterPoint.GetValueOrDefault()));

            var liveData = await _AirportWatchLiveDataService.GetLiveData(options.AirportIdentifier, 2);

            var swimFlightLegs = await _SwimFlightLegService.GetSwimFlightLegsForFlightWatchMap(options.AirportIdentifier,30,30);

            //Combine the results so we see every flight picked up by both AirportWatch and SWIM.
            var result = CombineAirportWatchAndSWIMData(liveData, swimFlightLegs);

            await PopulateAdditionalDataFromOptions(result);

            if (_demoFlightWatch.IsDemoDataVisibleByFboId(options.FboIdForCenterPoint))
                AddDemoDataToFlightWatchResult(result, _Fbo);
            return result;
        }
        private List<FlightWatchModel> FilterFlightWatchData(List<FlightWatchModel> FlightWatchList)
        {
            FlightWatchList.RemoveAll(x => x.SourceOfCoordinates == FlightWatchConstants.CoordinatesSource.None);
            FlightWatchList.RemoveAll(x => x.DepartureICAO == x.FocusedAirportICAO && x.Status == null);
            FlightWatchList = FlightWatchList.Where(x => x.ArrivalICAO == x.FocusedAirportICAO || x.DepartureICAO == x.FocusedAirportICAO).ToList();
            return FlightWatchList;
        }

        private void AddDemoDataToFlightWatchResult(List<FlightWatchModel> result, FbosDto fbo)
        {
            var flightWatch = _demoFlightWatch.GetFlightWatchModelDemo(fbo);

            if(flightWatch == null)
                return;

            var fuelReqOrders = new List<FuelReqDto>() { _demoFlightWatch.GetFuelReqDemo() };

            flightWatch.SetUpcomingFuelOrderCollection(fuelReqOrders);

            result.Add(flightWatch);
        }
        private string GetFocusedAirportIdentifier()
        {
            if (!string.IsNullOrEmpty(_Options.AirportIdentifier))
                return _Options.AirportIdentifier;
            if (!string.IsNullOrEmpty(_Fbo?.FboAirport?.Icao))
                return _Fbo?.FboAirport?.Icao;
            return string.Empty;
        }

        private async Task<List<string>> GetViableAirportsForSWIMData()
        {
            if (!string.IsNullOrEmpty(_Options.AirportIdentifier))
                return new List<string>() { _Options.AirportIdentifier };
            if (_Options.FboIdForCenterPoint.GetValueOrDefault() <= 0)
                return null;
            var nearbyAirports = await _AirportService.GetAirportsWithinRange(GetFocusedAirportIdentifier(),
                _Options.NauticalMileRadiusForData);
            return nearbyAirports?.Select(x => x.ProperAirportIdentifier).Distinct().ToList();
        }
        private List<FlightWatchModel> CombineAirportWatchAndSWIMData(List<AirportWatchLiveDataDto> liveDataList,
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs)
        {
            var result = new List<FlightWatchModel>();

            //Combine SWIM and AirportWatch data by tail number
            result = (from liveData in liveDataList
                      join flightLeg in swimFlightLegs on liveData.TailNumber?.ToUpper() equals flightLeg.AircraftIdentification?.ToUpper()
                      select new FlightWatchModel(liveData,flightLeg){}).ToList();

            //Combine SWIM and AirportWatch data by flight number
            result.AddRange((from liveData in liveDataList
                             join flightLeg in swimFlightLegs on liveData.AtcFlightNumber?.ToUpper() equals flightLeg.AircraftIdentification?.ToUpper()
                             join existing in result on liveData.Oid equals existing.AirportWatchLiveDataId.GetValueOrDefault()
                                 into leftJoinResult
                             from existing in leftJoinResult.DefaultIfEmpty()
                             where existing == null
                             select new FlightWatchModel(liveData,flightLeg) {}).ToList());

            //Add any remaining SWIM data to the result without an AirportWatch match
            result.AddRange(from swimFlight in swimFlightLegs
                            join resultItem in result on swimFlight.Oid equals resultItem.SWIMFlightLegId.GetValueOrDefault()
                                into leftJoinResult
                            from resultItem in leftJoinResult.DefaultIfEmpty()
                            where resultItem == null
                            select new FlightWatchModel(null, swimFlight));

            return result;
        }
        private List<FlightWatchModel> CombineAirportWatchAndSWIMData(List<AirportWatchLiveDataWithHistoricalStatusDto> liveDataWithHistoricalInfo,
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs,
            List<AircraftHexTailMappingDTO> hexTailMapping)
        {
            var result = new List<FlightWatchModel>();

            //Combine SWIM and AirportWatch data by tail number
            result = (from liveData in liveDataWithHistoricalInfo
                join flightLeg in swimFlightLegs on liveData.TailNumber?.ToUpper() equals flightLeg.AircraftIdentification?.ToUpper()
                join hexTail in hexTailMapping on liveData.TailNumber?.ToUpper() equals hexTail.TailNumber?.ToUpper()
                into leftJoinHexTail
                from hexTail in leftJoinHexTail.DefaultIfEmpty()
                select new FlightWatchModel(liveData.AirportWatchLiveData,
                    liveData.RecentAirportWatchHistoricalDataCollection,
                    flightLeg,
                    hexTail)
                {
                }).ToList();

            //Combine SWIM and AirportWatch data by flight number
            result.AddRange((from liveData in liveDataWithHistoricalInfo
                    join flightLeg in swimFlightLegs on liveData.AtcFlightNumber?.ToUpper() equals flightLeg.AircraftIdentification?.ToUpper()
                    join existing in result on liveData.Oid equals existing.AirportWatchLiveDataId.GetValueOrDefault()
                        into leftJoinResult
                    from existing in leftJoinResult.DefaultIfEmpty()
                    join hexTail in hexTailMapping on liveData.TailNumber?.ToUpper() equals hexTail.TailNumber?.ToUpper()
                        into leftJoinHexTail
                    from hexTail in leftJoinHexTail.DefaultIfEmpty()
                             where existing == null
                    select new FlightWatchModel(liveData.AirportWatchLiveData,
                        liveData.RecentAirportWatchHistoricalDataCollection,
                        flightLeg,
                        hexTail)
                    {
                    }
                ).ToList());

            //Add any remaining AirportWatch data to the result without a SWIM match
            result.AddRange((from liveData in liveDataWithHistoricalInfo
                    join resultItem in result on liveData.Oid equals resultItem.AirportWatchLiveDataId.GetValueOrDefault()
                        into leftJoinResult
                    from resultItem in leftJoinResult.DefaultIfEmpty()
                    join hexTail in hexTailMapping on liveData.TailNumber?.ToUpper() equals
                        hexTail.TailNumber?.ToUpper()
                        into leftJoinHexTail
                    from hexTail in leftJoinHexTail.DefaultIfEmpty()
                    where resultItem == null
                    select new FlightWatchModel(liveData.AirportWatchLiveData,
                        liveData.RecentAirportWatchHistoricalDataCollection, null, hexTail)
                ));

            //Add any remaining SWIM data to the result without an AirportWatch match
            result.AddRange(from swimFlight in swimFlightLegs
                    join resultItem in result on swimFlight.Oid equals resultItem.SWIMFlightLegId.GetValueOrDefault()
                        into leftJoinResult
                    from resultItem in leftJoinResult.DefaultIfEmpty()
                    join hexTail in hexTailMapping on swimFlight.AircraftIdentification?.ToUpper() equals hexTail.TailNumber?.ToUpper()
                        into leftJoinHexTail
                    from hexTail in leftJoinHexTail.DefaultIfEmpty()
                    where resultItem == null
                        select new FlightWatchModel(null, null, swimFlight, hexTail)
                    );

            //If we are focused on an FBO then remove any potential records that we've lost track of on antenna + swim data
            if (_Options.FboIdForCenterPoint.GetValueOrDefault() > 0)
            {
                var cutoffDateTime = DateTime.UtcNow.AddMinutes(-15);
                result.RemoveAll(x =>
                    x.AirportWatchLiveDataId.GetValueOrDefault() == 0 &&
                    (!(x.GetSwimFlightLeg()?.LastUpdated).HasValue ||
                     x.GetSwimFlightLeg().LastUpdated < cutoffDateTime));
            }

            return result;
        }

        private async Task PopulateAdditionalDataFromOptions(List<FlightWatchModel> result)
        {
            _DistinctTailNumbers = result.Where(x => !string.IsNullOrEmpty(x.TailNumber)).Select(x => x.TailNumber)
                .Distinct().ToList();

            //Set the nearest airport positions of each result
            foreach (var flightWatchModel in result)
            {
                flightWatchModel.FocusedAirportICAO = GetFocusedAirportIdentifier();

                if (_Options.IncludeNearestAirportPosition)
                    await PopulateNearestAirportPosition(flightWatchModel);
                if (_Options.IncludeFuelOrderInformation)
                    await PopulateUpcomingFuelOrders(flightWatchModel);
                if (_Options.IncludeCustomerAircraftInformation)
                    await PopulateCustomerAircraftInformation(flightWatchModel);
                if (_Options.IncludeVisitsAtFbo)
                    await PopulateVisitsAtFbo(flightWatchModel);
                if (_Options.IncludeCompanyPricingLogLastQuoteDate)
                    await PopulateLastQuoteDate(flightWatchModel);
                await AssignTrackingDegree(flightWatchModel);
            }
        }

        private async Task PopulateNearestAirportPosition(FlightWatchModel flightWatchModel)
        {
            //Set the nearest airport positions of each result
            var coordinates = flightWatchModel.GetCoordinates();
            if (coordinates.Latitude == 0 && coordinates.Longitude == 0)
                return;
            AirportPosition airportPosition =
                await _AirportService.GetNearestAirportPosition(coordinates.Latitude, coordinates.Longitude);

            if (airportPosition != null)
                flightWatchModel.SetAirportPosition(airportPosition);
        }

        private async Task PopulateUpcomingFuelOrders(FlightWatchModel flightWatchModel)
        {
            //Set any upcoming orders to each result
            if (_Fbo == null || _Fbo.Oid == 0)
                return;
            if (string.IsNullOrEmpty(flightWatchModel.TailNumber))
                return;
            var orders =
                await _FuelReqService.GetUpcomingDirectAndContractOrdersForTailNumber(_Fbo.GroupId,
                    _Fbo.Oid, flightWatchModel.TailNumber, true);

            //[#31pb4b8] Changed to mark the aircraft as green if it has any upcoming orders, even if it's not for the current leg.
            //flightWatchModel.SetUpcomingFuelOrderCollection(orders?.Where(x => x.Icao == flightWatchModel.ArrivalICAO).ToList());
            
            flightWatchModel.SetUpcomingFuelOrderCollection(orders.ToList());
        }

        private async Task PopulateCustomerAircraftInformation(FlightWatchModel flightWatchModel)
        {
            if (_Fbo == null || _Fbo.Oid == 0)
                return;
            if (string.IsNullOrEmpty(flightWatchModel.TailNumber))
                return;
            if (_CustomerAircrafts == null)
                _CustomerAircrafts = await _CustomerAircraftService.GetCustomerAircraftsWithDetails(
                    _Fbo.GroupId,
                    _Fbo.Oid,
                    0,
                    _DistinctTailNumbers, true);
            var matchingCustomerAircraft =
                _CustomerAircrafts.FirstOrDefault(x => x.TailNumber?.ToUpper() == flightWatchModel.TailNumber);
            if (matchingCustomerAircraft != null)
                flightWatchModel.SetCustomerAircraft(matchingCustomerAircraft);
        }

        private async Task PopulateVisitsAtFbo(FlightWatchModel flightWatchModel)
        {
            if (_Fbo == null || _Fbo.Oid == 0)
                return;

            if (_GeoFenceCluster == null)
                _GeoFenceCluster = (await _AirportFboGeofenceClustersService.GetAllClusters(0,
                    _Fbo.AcukwikFBOHandlerId.GetValueOrDefault())).FirstOrDefault();

            var historicalDataPoints = flightWatchModel.GetAirportWatchHistoricalDataCollection();

            if (historicalDataPoints == null || historicalDataPoints.Count == 0)
                return;

            if (_GeoFenceCluster == null)
                flightWatchModel.VisitsToMyFBO = 0;
            else
                flightWatchModel.VisitsToMyFBO = ((historicalDataPoints.Where(x => x.AircraftStatus == AircraftStatusType.Parking))?.Count(x => _GeoFenceCluster.AreCoordinatesInFence(x.Latitude, x.Longitude))).GetValueOrDefault();
            flightWatchModel.Arrivals = historicalDataPoints.Count(x => x.AircraftStatus == AircraftStatusType.Landing);
            flightWatchModel.Departures = historicalDataPoints.Count(x => x.AircraftStatus == AircraftStatusType.Takeoff);
        }

        private async Task PopulateLastQuoteDate(FlightWatchModel flightWatchModel)
        {
            if (_Fbo == null || _Fbo.Oid == 0 || string.IsNullOrEmpty(_Fbo.FboAirport.Icao))
                return;

            if (flightWatchModel.FuelerlinxCompanyId.GetValueOrDefault() <= 0)
                return;

            if (_MostRecentQuotes == null)
                _MostRecentQuotes = await _CompanyPricingLogService.GetMostRecentQuoteDatesForAirport(_Fbo.FboAirport.Icao);

            flightWatchModel.LastQuoteDate = _MostRecentQuotes
                .FirstOrDefault(x => x.FuelerLinxCompanyId == flightWatchModel.FuelerlinxCompanyId.GetValueOrDefault())
                ?.MostRecentQuoteDateTime;
        }

        private async Task AssignTrackingDegree(FlightWatchModel flightWatchModel)
        {
            if (flightWatchModel.TrackingDegree.HasValue)
                return;

            if (string.IsNullOrEmpty(flightWatchModel.ArrivalICAO))
                return;

            var destinationAirportPosition =
                await _AirportService.GetAirportPositionByAirportIdentifier(flightWatchModel.ArrivalICAO);

            if (destinationAirportPosition == null)
                return;

            if (!flightWatchModel.Latitude.HasValue || !flightWatchModel.Longitude.HasValue)
                return;

            flightWatchModel.SetTrackingDegree(
                FBOLinx.Core.Utilities.Geography.LocationHelper.GetBearingDegreesBetweenTwoPoints(
                    new Coordinate(flightWatchModel.Latitude.GetValueOrDefault(), flightWatchModel.Longitude.GetValueOrDefault()),
                    new Coordinate(destinationAirportPosition.Latitude, destinationAirportPosition.Longitude)));
        }

        //Commenting this out until we find a faster way to load this
        //private async Task PopulateCurrentPricing(FlightWatchModel flightWatchModel)
        //{
        //    if (_Fbo == null || _Fbo.Oid == 0 || string.IsNullOrEmpty(_Fbo.FboAirport.Icao))
        //        return;

        //    if (!flightWatchModel.IsInNetwork || flightWatchModel.PricingTemplateId.GetValueOrDefault() <= 0)
        //        return;

        //    if (_CurrentPricingResults == null)
        //        _CurrentPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_Fbo.Oid,
        //            _Fbo.GroupId.GetValueOrDefault(), 0, new List<int>(), FlightTypeClassifications.Private,
        //            ApplicableTaxFlights.DomesticOnly);

        //    flightWatchModel.CurrentPricing = _CurrentPricingResults
        //        ?.FirstOrDefault(x => x.PricingTemplateId == flightWatchModel.PricingTemplateId)?.AllInPrice;

        //}
    }
}
