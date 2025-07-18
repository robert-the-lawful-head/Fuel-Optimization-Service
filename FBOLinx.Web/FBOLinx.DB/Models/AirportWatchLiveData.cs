﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class AirportWatchLiveData: BaseAirportWatchData
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
        [NotMapped]
        //public bool? HasFuelOrders { get; set; }
        public FuelReq FuelOrder { get; set; }
        [NotMapped]
        public bool? IsFuelerLinxCustomer { get; set; }
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;

        public static void CopyEntity(AirportWatchLiveData oldRecord, AirportWatchLiveData newRecord)
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
            oldRecord.TailNumber = newRecord.TailNumber;
            oldRecord.CreatedDateTime = newRecord.CreatedDateTime;
        }
    }
}
