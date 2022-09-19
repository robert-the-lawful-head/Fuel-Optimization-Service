using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch
{
    public class AirportWatchLiveDataWithHistoricalStatusDto
    {
        public int Oid { get; set; }
        public string TailNumber { get; set; }
        public string AtcFlightNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public bool IsAircraftOnGround { get; set; }
        public List<AirportWatchHistoricalDataDto> RecentAirportWatchHistoricalDataCollection { get; set; }
        //public int? HistoricalOid { get; set; }
        //public double? HistoricalLatitude { get; set; }
        //public double? HistoricalLongitude { get; set; }
        //public AircraftStatusType HistoicalAircraftStatus { get; set; }
        //public DateTime? HistoricalAircraftPositionDateTimeUtc { get; set; }
        public FlightLegStatus? FlightLegStatus { get; set; }
        public AirportPosition AirportPosition { get; set; }
        public SWIMFlightLeg SWIMFlightLeg { get; set; }

        public AirportWatchHistoricalDataDto GetMostRecentHistoricalRecord()
        {
            return RecentAirportWatchHistoricalDataCollection?.OrderByDescending(x => x.AircraftPositionDateTimeUtc)
                ?.FirstOrDefault();
        }

        public AircraftStatusType? GetMostRecentStatus()
        {
            return GetMostRecentHistoricalRecord()?.AircraftStatus;
        }

        public double GetMinutesSinceLastHistoricalStatusChange()
        {
            var mostRecentHistoricalRecord = GetMostRecentHistoricalRecord();
            if (mostRecentHistoricalRecord == null)
                return 0;
            return Math.Abs((AircraftPositionDateTimeUtc - mostRecentHistoricalRecord.AircraftPositionDateTimeUtc)
                .TotalMinutes);
        }
    }
}
