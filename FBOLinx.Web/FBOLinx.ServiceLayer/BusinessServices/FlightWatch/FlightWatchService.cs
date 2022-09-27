using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.SWIM;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.SWIMS;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.FlightWatch
{
    public interface IFlightWatchService
    {
        Task<List<FlightWatchModel>> GetCurrentFlightWatchData(FlightWatchDataRequestOptions options);
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

        public FlightWatchService(IAirportWatchLiveDataService airportWatchLiveDataService,
            IFboService fboService,
            ISWIMFlightLegService swimFlightLegService,
            IAirportService airportService,
            IFuelReqService fuelReqService,
            ICustomerAircraftService customerAircraftService,
            IAirportFboGeofenceClustersService airportFboGeofenceClustersService
            )
        {
            _AirportFboGeofenceClustersService = airportFboGeofenceClustersService;
            _CustomerAircraftService = customerAircraftService;
            _FuelReqService = fuelReqService;
            _AirportService = airportService;
            _SwimFlightLegService = swimFlightLegService;
            _FboService = fboService;
            _AirportWatchLiveDataService = airportWatchLiveDataService;
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
                await _AirportWatchLiveDataService.GetAirportWatchLiveDataWithHistoricalStatuses(GetAirportIdentifier(), 1, daysToCheckBackForHistoricalData);

            //Then load all SWIM flight legs that we have from the last hour.
            var swimFlightLegs = (await _SwimFlightLegService.GetRecentSWIMFlightLegs(GetAirportIdentifier())).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));

            //Combine the results so we see every flight picked up by both AirportWatch and SWIM.
            var result = CombineAirportWatchAndSWIMData(liveDataWithHistoricalInfo, swimFlightLegs);

            //Load any additional data needed that was related to each flight.
            await PopulateAdditionalDataFromOptions(result);

            return result;
        }

        private string GetAirportIdentifier()
        {
            if (!string.IsNullOrEmpty(_Options.AirportIdentifier))
                return _Options.AirportIdentifier;
            if (!string.IsNullOrEmpty(_Fbo?.FboAirport?.Icao))
                return _Fbo?.FboAirport?.Icao;
            return string.Empty;

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
            return result;
        }

        private async Task PopulateAdditionalDataFromOptions(List<FlightWatchModel> result)
        {
            _DistinctTailNumbers = result.Where(x => !string.IsNullOrEmpty(x.TailNumber)).Select(x => x.TailNumber)
                .Distinct().ToList();

            //Set the nearest airport positions of each result
            foreach (var flightWatchModel in result)
            {
                if (_Options.IncludeNearestAirportPosition)
                    await PopulateNearestAirportPosition(flightWatchModel);
                if (_Options.IncludeFuelOrderInformation)
                    await PopulateUpcomingFuelOrders(flightWatchModel);
                if (_Options.IncludeCustomerAircraftInformation)
                    await PopulateCustomerAircraftInformation(flightWatchModel);
                if (_Options.IncludeVisitsAtFbo)
                    await PopulateVisitsAtFbo(flightWatchModel);
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
            flightWatchModel.SetUpcomingFuelOrderCollection(orders?.Where(x => x.Icao == flightWatchModel.ArrivalICAO).ToList());
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


            flightWatchModel.VisitsToMyFBO = ((historicalDataPoints.Where(x => x.AircraftStatus == AircraftStatusType.Parking))?.Count(x => _GeoFenceCluster.AreCoordinatesInFence(x.Latitude, x.Longitude))).GetValueOrDefault();
            flightWatchModel.Arrivals = historicalDataPoints.Count(x => x.AircraftStatus == AircraftStatusType.Landing);
            flightWatchModel.Departures = historicalDataPoints.Count(x => x.AircraftStatus == AircraftStatusType.Takeoff);
            flightWatchModel.FocusedAirportICAO = GetAirportIdentifier();

            //string focusedAirport = GetAirportIdentifier();
            //if (string.IsNullOrEmpty(focusedAirport))
            //    return;
            //if (flightWatchModel.ArrivalICAO == focusedAirport)
            //{
            //    flightWatchModel.Origin = flightWatchModel.DepartureICAO;
            //    flightWatchModel.City = flightWatchModel.DepartureCity;
            //}
            //else
            //{
            //    flightWatchModel.Origin = flightWatchModel.ArrivalICAO;
            //    flightWatchModel.City = flightWatchModel.ArrivalCity;
            //}
        }
        
    }
}
