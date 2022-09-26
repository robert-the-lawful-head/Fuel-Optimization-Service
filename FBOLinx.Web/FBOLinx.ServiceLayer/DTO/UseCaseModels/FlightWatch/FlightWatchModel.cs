using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using Geolocation;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch
{
    public class FlightWatchModel
    {
        
        private List<AirportWatchHistoricalDataDto> _AirportWatchHistoricalDataCollection;
        private AirportPosition _AirportPosition;
        private List<FuelReqDto> _UpcomingFuelOrderCollection;

        public FlightWatchModel(AirportWatchLiveDataDto airportWatchLiveData, List<AirportWatchHistoricalDataDto> airportWatchHistoricalDataCollection, SWIMFlightLegDTO swimFlightLeg)
        {
            SwimFlightLeg = swimFlightLeg;
            _AirportWatchHistoricalDataCollection = airportWatchHistoricalDataCollection;
            AirportWatchLiveData = airportWatchLiveData;
        }

        public string TailNumber
        {
            get { return AirportWatchLiveData?.TailNumber; }
        }



        public AirportWatchLiveDataDto AirportWatchLiveData { get; set; }
        public SWIMFlightLegDTO SwimFlightLeg { get; set; }

        public void SetAirportWatchHistoricalDataCollection(List<AirportWatchHistoricalDataDto> airportWatchHistoricalDataCollection)
        {
            _AirportWatchHistoricalDataCollection = airportWatchHistoricalDataCollection;
        }

        public List<AirportWatchHistoricalDataDto> GetAirportWatchHistoricalDataCollection()
        {
            return _AirportWatchHistoricalDataCollection;
        }

        public void SetAirportPosition(AirportPosition airportPosition)
        {
            _AirportPosition = airportPosition;
        }

        public AirportPosition GetAirportPosition()
        {
            return _AirportPosition;
        }

        public void SetUpcomingFuelOrderCollection(List<FuelReqDto> upcomingFuelOrderCollection)
        {
            _UpcomingFuelOrderCollection = upcomingFuelOrderCollection;
        }

        public List<FuelReqDto> GetUpcomingFuelOrderCollection()
        {
            return _UpcomingFuelOrderCollection;
        }

        public Geolocation.Coordinate GetCoordinates()
        {
            if (AirportWatchLiveData != null && AirportWatchLiveData.Latitude != 0 &&
                AirportWatchLiveData.Longitude != 0)
                return new Coordinate(AirportWatchLiveData.Latitude, AirportWatchLiveData.Longitude);
            if (SwimFlightLeg != null && SwimFlightLeg.Latitude.GetValueOrDefault() != 0 &&
                SwimFlightLeg.Longitude.GetValueOrDefault() != 0)
                return new Geolocation.Coordinate(SwimFlightLeg.Latitude.GetValueOrDefault(),
                    SwimFlightLeg.Longitude.GetValueOrDefault());
            return new Coordinate();
        }
    }
}
