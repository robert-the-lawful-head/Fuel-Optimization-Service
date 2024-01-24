using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;

namespace FBOLinx.ServiceLayer.DTO
{
    public class AirportWatchLiveDataDto: BaseEntityModelDTO<DB.Models.AirportWatchLiveData>, IBaseAirportWatchModel, IEntityModelDTO<DB.Models.AirportWatchLiveData, int>
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
        public string AircraftHexCode { get; set; }
        public string TailNumber { get; set; }
        public int? AircraftId { get; set; }
        public string AircraftICAO { get; set; }
        public bool? IsInNetwork { get; set; }
        public bool? IsFuelerLinxCustomer { get; set; }
        public bool? IsOutOfNetwork { get; set; }
        public bool? IsActiveFuelRelease { get; set; }
        public bool? IsFuelerLinxClient { get; set; }
        public FuelReqDto FuelOrder { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string ToCsvString()
        {
            return $"{AircraftHexCode},{AtcFlightNumber.ToStringOrEmpty()},{BoxName.ToStringOrEmpty()},{AircraftTypeCode.ToStringOrEmpty()},{BoxTransmissionDateTimeUtc},{AircraftPositionDateTimeUtc},{AltitudeInStandardPressure.ToStringOrEmpty()},{GroundSpeedKts.ToStringOrEmpty()},{TrackingDegree.ToStringOrEmpty()},{Latitude},{Longitude},{VerticalSpeedKts.ToStringOrEmpty()},{TransponderCode.ToStringOrEmpty()},{GpsAltitude.ToStringOrEmpty()},{IsAircraftOnGround}";
        }
    }
}