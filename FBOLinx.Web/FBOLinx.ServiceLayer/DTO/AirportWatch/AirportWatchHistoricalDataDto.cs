using System;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;

namespace FBOLinx.Service.Mapping.Dto
{
    public class AirportWatchHistoricalDataDto : BaseEntityModelDTO<DB.Models.AirportWatchHistoricalData>, IBaseAirportWatchModel, IEntityModelDTO<DB.Models.AirportWatchHistoricalData, int>
    {
        public int Oid { get; set; }
        public DateTime BoxTransmissionDateTimeUtc { get; set; }
        public string AtcFlightNumber { get; set; }
        public int? AltitudeInStandardPressure { get; set; }
        public int? GroundSpeedKts { get; set; }
        public double? TrackingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? VerticalSpeedKts { get; set; }
        public int? TransponderCode { get; set; }
        public string BoxName { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public string AircraftTypeCode { get; set; }
        public int? GpsAltitude { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public AircraftStatusType AircraftStatus { get; set; }
        public string AirportICAO { get; set; }
        public string AircraftHexCode { get; set; }
        public string TailNumber { get; set; }
        public AirportWatchHistoricalParkingDto AirportWatchHistoricalParking { get; set; }

        public static AirportWatchHistoricalDataDto Cast(AirportWatchLiveDataDto liveDataRecord)
        {
            return new AirportWatchHistoricalDataDto
            {
                BoxTransmissionDateTimeUtc = liveDataRecord.BoxTransmissionDateTimeUtc,
                AircraftHexCode = liveDataRecord.AircraftHexCode,
                AtcFlightNumber = liveDataRecord.AtcFlightNumber,
                AltitudeInStandardPressure = liveDataRecord.AltitudeInStandardPressure,
                GroundSpeedKts = liveDataRecord.GroundSpeedKts,
                TrackingDegree = liveDataRecord.TrackingDegree,
                Latitude = liveDataRecord.Latitude,
                Longitude = liveDataRecord.Longitude,
                VerticalSpeedKts = liveDataRecord.VerticalSpeedKts,
                TransponderCode = liveDataRecord.TransponderCode,
                BoxName = liveDataRecord.BoxName,
                AircraftPositionDateTimeUtc = liveDataRecord.AircraftPositionDateTimeUtc,
                AircraftTypeCode = liveDataRecord.AircraftTypeCode,
                GpsAltitude = liveDataRecord.GpsAltitude,
                IsAircraftOnGround = liveDataRecord.IsAircraftOnGround,
                TailNumber = liveDataRecord.TailNumber,
                AircraftStatus = liveDataRecord.IsAircraftOnGround == true ? AircraftStatusType.Landing : AircraftStatusType.Takeoff,
            };
        }
    }
}