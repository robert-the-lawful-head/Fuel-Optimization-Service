﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Models
{
    public class AirportWatchHistoricalData: BaseAirportWatchData
    {
        public DateTime BoxTransmissionDateTimeUtc { get; set; }
        [StringLength(20)]
        public string AtcFlightNumber { get; set; }
        public int? AltitudeInStandardPressure { get; set; }
        public int? GroundSpeedKts { get; set; }
        public double? TrackingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? VerticalSpeedKts { get; set; }
        public int? TransponderCode { get; set; }
        [StringLength(25)]
        public string BoxName { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        [StringLength(3)]
        public string AircraftTypeCode { get; set; }
        public int? GpsAltitude { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public AircraftStatusType AircraftStatus { get; set; }
        [StringLength(255)]
        [Column(TypeName = "varchar")]
        public string AirportICAO { get; set; }
        [NotMapped]
        public long? SwimFlightLegId { get; set; }

        #region Relationships
        [InverseProperty("AirportWatchHistoricalData")]
        public AirportWatchHistoricalParking AirportWatchHistoricalParking { get; set; }
        #endregion

        public static AirportWatchHistoricalData ConvertFromAirportWatchLiveData(AirportWatchLiveData entity)
        {
            return new AirportWatchHistoricalData
            {
                BoxTransmissionDateTimeUtc = entity.BoxTransmissionDateTimeUtc,
                AircraftHexCode = entity.AircraftHexCode,
                AtcFlightNumber = entity.AtcFlightNumber,
                AltitudeInStandardPressure = entity.AltitudeInStandardPressure,
                GroundSpeedKts = entity.GroundSpeedKts,
                TrackingDegree = entity.TrackingDegree,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                VerticalSpeedKts = entity.VerticalSpeedKts,
                TransponderCode = entity.TransponderCode,
                BoxName = entity.BoxName,
                AircraftPositionDateTimeUtc = entity.AircraftPositionDateTimeUtc,
                AircraftTypeCode = entity.AircraftTypeCode,
                GpsAltitude = entity.GpsAltitude,
                IsAircraftOnGround = entity.IsAircraftOnGround,
                TailNumber = entity.TailNumber,
                AircraftStatus = entity.IsAircraftOnGround == true ? AircraftStatusType.Landing : AircraftStatusType.Takeoff,
            };
        }

        public static void CopyEntity(AirportWatchHistoricalData oldRecord, AirportWatchHistoricalData newRecord)
        {
            oldRecord.BoxTransmissionDateTimeUtc = newRecord.BoxTransmissionDateTimeUtc;
            oldRecord.AircraftHexCode = newRecord.AircraftHexCode;
            oldRecord.AtcFlightNumber = newRecord.AtcFlightNumber;
            oldRecord.AltitudeInStandardPressure = newRecord.AltitudeInStandardPressure;
            oldRecord.GroundSpeedKts = newRecord.GroundSpeedKts;
            oldRecord.TrackingDegree = newRecord.TrackingDegree;
            oldRecord.Latitude = newRecord.Latitude;
            oldRecord.Longitude = newRecord.Longitude;
            oldRecord.VerticalSpeedKts = newRecord.VerticalSpeedKts;
            oldRecord.TransponderCode = newRecord.TransponderCode;
            oldRecord.BoxName = newRecord.BoxName;
            oldRecord.AircraftPositionDateTimeUtc = newRecord.AircraftPositionDateTimeUtc;
            oldRecord.AircraftTypeCode = newRecord.AircraftTypeCode;
            oldRecord.GpsAltitude = newRecord.GpsAltitude;
            oldRecord.IsAircraftOnGround = newRecord.IsAircraftOnGround;
            oldRecord.AircraftStatus = newRecord.AircraftStatus;
            oldRecord.AirportICAO = newRecord.AirportICAO;
            oldRecord.TailNumber = newRecord.TailNumber;
        }
    }
}
