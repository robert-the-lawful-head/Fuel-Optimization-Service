using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace FBOLinx.ServiceLayer.Demo
{
    public interface IDemoFlightWatch
    {
        bool isDemoDataVisibleByFboId(int? fboId);
        FuelReqDto GetFuelReqDemo();
        FlightWatchModel GetFlightWatchModelDemo(FbosDto fbo);

    }
    public class DemoFlightWatch : IDemoFlightWatch
    {
        private IOptions<DemoData> _demoData;
        public DemoFlightWatch(IOptions<DemoData> demoData) {
            _demoData = demoData;
        }

        private  Func<int?, bool> _isDemoDataVisibleByFboId = fboId =>
        {
            return fboId == 276 || fboId == 525;
        };
        public bool isDemoDataVisibleByFboId(int? fboId)
        {
            return _isDemoDataVisibleByFboId(fboId);
        } 
        public FuelReqDto GetFuelReqDemo()
        {
            var demoData = _demoData.Value.FlightWatch;

            return new FuelReqDto()
            {
                Oid = demoData.FuelOrder.Oid,
                        CustomerId = demoData.FuelOrder.CustomerId,
                        Icao = demoData.FuelOrder.Icao,
                        Fboid = demoData.FuelOrder.Fboid,
                        CustomerAircraftId = demoData.FuelOrder.CustomerAircraftId,
                        TimeStandard = demoData.FuelOrder.TimeStandard,
                        QuotedVolume = demoData.FuelOrder.QuotedVolume,
                        CustomerAircraft = new CustomerAircraftsDto() { TailNumber = demoData.FuelOrder.CustomerAircraft.TailNumber },
                        CustomerName = demoData.FuelOrder.CustomerName,
                        PricingTemplateName = demoData.FuelOrder.PricingTemplateName,
                        Eta = demoData.FuelOrder.Eta,
                        Etd = demoData.FuelOrder.Etd,
                        QuotedPpg = demoData.FuelOrder.QuotedPpg,
                        PhoneNumber = demoData.FuelOrder.PhoneNumber,
                        Source = demoData.FuelOrder.Source,
                        SourceId = demoData.FuelOrder.SourceId,
                        Email = demoData.FuelOrder.Email
                    };
        }
        public FlightWatchModel GetFlightWatchModelDemo(FbosDto fbo)
        {
            if (_demoData == null || _demoData.Value == null || _demoData.Value.FlightWatch == null)
                return null;

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
                ActualSpeed = demoData.GroundSpeedKts,
                FlightDepartment = "Test Company",
                FAARegisteredOwner = "Test FAA Owner",
                ICAOAircraftCode = "ICAOAircraftCodeKVNY"
            };
            var airportWatchLiveData = new AirportWatchLiveDataDto()
            {
                Oid = demoData.Oid,
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
                Longitude = demoData.Longitude,
                AircraftICAO = "AircraftICAOtKVNY"
            };
            var airportWatchHistoricalDataCollection = new List<AirportWatchHistoricalDataDto>();

            var flightWatch = new FlightWatchModel(airportWatchLiveData, airportWatchHistoricalDataCollection, swim, null);

            flightWatch.FavoriteAircraft = new FboFavoriteAircraft() { CustomerAircraftsId = 0, FboId = 276 };
            flightWatch.FocusedAirportICAO = fbo.FboAirport.Icao;
            flightWatch.IsCustomerManagerAircraft = true;
            return flightWatch;
        }
    }
}
