using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.CompanyPricingLog;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.SWIMS;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using Geolocation;
using Microsoft.Extensions.Options;

namespace FBOLinx.ServiceLayer.BusinessServices.FlightWatch
{
    public interface IFlightWatchService
    {
        Task<List<FlightWatchModel>> GetCurrentFlightWatchData(FlightWatchDataRequestOptions options);
        Task<FlightWatchLegAdditionalDetailsModel> GetAdditionalDetailsForLeg(int swimFlightLegId);
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
        private IPriceFetchingService _PriceFetchingService;
        private List<CustomerWithPricing> _CurrentPricingResults;
        private IOptions<DemoData> _demoData;

        private Func<int?, bool> _isDemoDataVisibleByFboId = fboId =>
        {
            return fboId == 276 || fboId == 525;
        };

        public FlightWatchService(IAirportWatchLiveDataService airportWatchLiveDataService,
            IFboService fboService,
            ISWIMFlightLegService swimFlightLegService,
            IAirportService airportService,
            IFuelReqService fuelReqService,
            ICustomerAircraftService customerAircraftService,
            IAirportFboGeofenceClustersService airportFboGeofenceClustersService,
            ICompanyPricingLogService companyPricingLogService,
            IPriceFetchingService priceFetchingService,
            IOptions<DemoData> demoData
        )
        {
            _PriceFetchingService = priceFetchingService;
            _CompanyPricingLogService = companyPricingLogService;
            _AirportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _CustomerAircraftService = customerAircraftService;
            _FuelReqService = fuelReqService;
            _AirportService = airportService;
            _SwimFlightLegService = swimFlightLegService;
            _FboService = fboService;
            _AirportWatchLiveDataService = airportWatchLiveDataService;
            _demoData = demoData;
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

        public async Task<List<FlightWatchModel>> GetCurrentFlightWatchData(FlightWatchDataRequestOptions options)
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
                await _AirportWatchLiveDataService.GetAirportWatchLiveDataWithHistoricalStatuses(GetFocusedAirportIdentifier(), 1, daysToCheckBackForHistoricalData);

            //Grab the airport to be considered for arrivals and departures.
            var airportsForArrivalsAndDepartures = await GetViableAirportsForSWIMData();

            //Then load all SWIM flight legs that we have from the last hour.
            var swimFlightLegs = (await _SwimFlightLegService.GetRecentSWIMFlightLegs(airportsForArrivalsAndDepartures)).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));

            //Combine the results so we see every flight picked up by both AirportWatch and SWIM.
            var result = CombineAirportWatchAndSWIMData(liveDataWithHistoricalInfo, swimFlightLegs);

            //Load any additional data needed that was related to each flight.
            await PopulateAdditionalDataFromOptions(result);

            if (_isDemoDataVisibleByFboId(options.FboIdForCenterPoint))
                AddDemoDataToFlightWatchResult(result,_Fbo);

            return result;
        }
        private void AddDemoDataToFlightWatchResult(List<FlightWatchModel> result, FbosDto fbo)
        {
            if (_demoData == null || _demoData.Value == null || _demoData.Value.FlightWatch == null)
                return;

            var demoData = _demoData.Value.FlightWatch;
            var swim = new SWIMFlightLegDTO()
            {
                FAAMake = "CESSNA",
                FAAModel = "172N",
                Altitude = 43650,
                Latitude = demoData.Latitude,
                Longitude = demoData.Longitude,
                Status = FlightLegStatus.EnRoute,
                Phone = "11111111111",
                AircraftIdentification = demoData.AtcFlightNumber,
                ATDLocal = DateTime.UtcNow,
                ATD = DateTime.UtcNow.AddHours(2),
                ETALocal = DateTime.UtcNow,
                ETA = DateTime.UtcNow.AddMinutes(25),
                DepartureCity = "Teterboro",
                DepartureICAO = "KTEB",
                ArrivalCity = fbo.City,
                ArrivalICAO = fbo.FboAirport.Icao,
                ActualSpeed = demoData.GroundSpeedKts
            };
            var airportWatchLiveData = new AirportWatchLiveDataDto(){
                Oid= demoData.Oid,
                AircraftPositionDateTimeUtc = DateTime.UtcNow.AddSeconds(-5),
                BoxTransmissionDateTimeUtc = DateTime.UtcNow.AddSeconds(-5),
                AltitudeInStandardPressure = demoData.AltitudeInStandardPressure,
                AircraftHexCode = demoData.AircraftHexCode,
                VerticalSpeedKts = demoData.VerticalSpeedKts,
                TransponderCode = demoData.TransponderCode,
                BoxName = demoData.BoxName,
                AircraftTypeCode = demoData.AircraftTypeCode,
                GpsAltitude = demoData.GpsAltitude,
                IsAircraftOnGround = demoData.IsAircraftOnGround,
                Latitude = demoData.Latitude, 
                Longitude = demoData.Longitude
            };
            var airportWatchHistoricalDataCollection = new List<AirportWatchHistoricalDataDto>();

            var flightWatch = new FlightWatchModel(airportWatchLiveData, airportWatchHistoricalDataCollection, swim);

            flightWatch.FocusedAirportICAO = fbo.FboAirport.Icao;

            var fuelReqOrders =  new List<FuelReqDto>() { 
                new FuelReqDto()
                {
                    Oid= demoData.FuelOrder.Oid,
                    CustomerId = demoData.FuelOrder.CustomerId,
                    Icao = demoData.FuelOrder.Icao,
                    Fboid = demoData.FuelOrder.Fboid,
                    CustomerAircraftId = demoData.FuelOrder.CustomerAircraftId,
                    TimeStandard = demoData.FuelOrder.TimeStandard,
                    QuotedVolume = demoData.FuelOrder.QuotedVolume,
                    CustomerAircraft = new CustomerAircraftsDto(){ TailNumber = demoData.FuelOrder.CustomerAircraft.TailNumber }
                } 
            };
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

        private List<FlightWatchModel> CombineAirportWatchAndSWIMData(List<AirportWatchLiveDataWithHistoricalStatusDto> liveDataWithHistoricalInfo,
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs)
        {
            var result = new List<FlightWatchModel>();

            //Combine SWIM and AirportWatch data by tail number
            result = (from liveData in liveDataWithHistoricalInfo
                join flightLeg in swimFlightLegs on liveData.TailNumber?.ToUpper() equals flightLeg.AircraftIdentification?.ToUpper()
                select new FlightWatchModel(liveData.AirportWatchLiveData,
                    liveData.RecentAirportWatchHistoricalDataCollection,
                    flightLeg)
                {
                }).ToList();

            //Combine SWIM and AirportWatch data by flight number
            result.AddRange((from liveData in liveDataWithHistoricalInfo
                    join flightLeg in swimFlightLegs on liveData.AtcFlightNumber?.ToUpper() equals flightLeg.AircraftIdentification?.ToUpper()
                    join existing in result on liveData.Oid equals existing.AirportWatchLiveDataId.GetValueOrDefault()
                        into leftJoinResult
                    from existing in leftJoinResult.DefaultIfEmpty()
                    where existing == null
                    select new FlightWatchModel(liveData.AirportWatchLiveData,
                        liveData.RecentAirportWatchHistoricalDataCollection,
                        flightLeg)
                    {
                    }
                ).ToList());

            //Add any remaining AirportWatch data to the result without a SWIM match
            result.AddRange(liveDataWithHistoricalInfo
                .Where(x => !result.Any(r => r.AirportWatchLiveDataId > 0 && r.AirportWatchLiveDataId == x.AirportWatchLiveData.Oid)).Select(x =>
                    new FlightWatchModel(x.AirportWatchLiveData, x.RecentAirportWatchHistoricalDataCollection, null)));

            //Add any remaining SWIM data to the result without an AirportWatch match
            result.AddRange(swimFlightLegs.Where(x => !result.Any(r => r.SWIMFlightLegId > 0 && r.SWIMFlightLegId == x.Oid)).Select(x =>
                new FlightWatchModel(null, null, x)));

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
                await _FuelReqService.GetUpcomingDirectAndContractOrdersForTailNumber(_Fbo.GroupId.GetValueOrDefault(),
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
                    _Fbo.GroupId.GetValueOrDefault(),
                    _Fbo.Oid,
                    0,
                    _DistinctTailNumbers);
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
