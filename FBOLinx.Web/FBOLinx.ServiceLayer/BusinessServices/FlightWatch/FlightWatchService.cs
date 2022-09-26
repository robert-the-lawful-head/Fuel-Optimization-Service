using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FlightWatchService(IAirportWatchLiveDataService airportWatchLiveDataService,
            IFboService fboService,
            ISWIMFlightLegService swimFlightLegService,
            IAirportService airportService,
            IFuelReqService fuelReqService,
            ICustomerAircraftService customerAircraftService
            )
        {
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

            var liveDataWithHistoricalInfo =
                await _AirportWatchLiveDataService.GetAirportWatchLiveDataWithHistoricalStatuses(options.FboIdForCenterPoint.GetValueOrDefault(), 1, options.IncludeRecentHistoricalRecords ? 1 : 0);

            //Then load all SWIM flight legs that we have from the last hour.
            var swimFlightLegs = (await _SwimFlightLegService.GetRecentSWIMFlightLegs(options.FboIdForCenterPoint.GetValueOrDefault())).Where(x => !string.IsNullOrEmpty(x.AircraftIdentification));

            var result = CombineAirportWatchAndSWIMData(liveDataWithHistoricalInfo, swimFlightLegs);

            await PopulateAdditionalDataFromOptions(result);

            return result;
        }

        private List<FlightWatchModel> CombineAirportWatchAndSWIMData(List<AirportWatchLiveDataWithHistoricalStatusDto> liveDataWithHistoricalInfo,
            IEnumerable<SWIMFlightLegDTO> swimFlightLegs)
        {
            var result = new List<FlightWatchModel>();

            //Combine SWIM and AirportWatch data by tail number
            result = (from liveData in liveDataWithHistoricalInfo
                join flightLeg in swimFlightLegs on liveData.TailNumber equals flightLeg.AircraftIdentification
                select new FlightWatchModel(liveData.AirportWatchLiveData,
                    liveData.RecentAirportWatchHistoricalDataCollection,
                    flightLeg)
                {
                }).ToList();

            //Combine SWIM and AirportWatch data by flight number
            result = (from liveData in liveDataWithHistoricalInfo
                    join flightLeg in swimFlightLegs on liveData.AtcFlightNumber equals flightLeg.AircraftIdentification
                    join existing in result on liveData.Oid equals existing.AirportWatchLiveData.Oid
                        into leftJoinResult
                    from existing in leftJoinResult.DefaultIfEmpty()
                    where existing == null
                    select new FlightWatchModel(liveData.AirportWatchLiveData,
                        liveData.RecentAirportWatchHistoricalDataCollection,
                        flightLeg)
                    {
                    }
                ).ToList();

            //Add any remaining AirportWatch data to the result without a SWIM match
            result.AddRange(liveDataWithHistoricalInfo
                .Where(x => !result.Any(r => r.AirportWatchLiveData == x.AirportWatchLiveData)).Select(x =>
                    new FlightWatchModel(x.AirportWatchLiveData, x.RecentAirportWatchHistoricalDataCollection, null)));

            //Add any remaining SWIM data to the result without an AirportWatch match
            result.AddRange(swimFlightLegs.Where(x => !result.Any(r => r.SwimFlightLeg == x)).Select(x =>
                new FlightWatchModel(null, null, x)));
            return result;
        }

        private async Task PopulateAdditionalDataFromOptions(List<FlightWatchModel> result)
        {
            //Set the nearest airport positions of each result
            foreach (var flightWatchModel in result)
            {
                if (_Options.IncludeNearestAirportPosition)
                    await PopulateNearestAirportPosition(flightWatchModel);
                if (_Options.IncludeFuelOrderInformation)
                    await PopulateUpcomingFuelOrders(flightWatchModel);
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
            if (_Fbo == null || _Fbo.Oid == 0)
                return;
            var orders =
                await _FuelReqService.GetUpcomingDirectAndContractOrdersForTailNumber(_Fbo.GroupId.GetValueOrDefault(),
                    _Fbo.Oid, flightWatchModel.TailNumber, true);
        }

        private async Task PopulateCustomerAircraftInformation(FlightWatchModel flightWatchModel)
        {

        }
    }
}
