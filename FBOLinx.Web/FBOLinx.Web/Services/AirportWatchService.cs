using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private FboLinxContext _context;

        public AirportWatchService(FboLinxContext context)
        {
            _context = context;
        }

        public void ProcessAirportWatchData(List<AirportWatchDataTransition> data)
        {
            var live10sData = data
                .Where(row => row.BoxTransmissionDateTimeUtc.AddSeconds(10) >= DateTime.UtcNow)
                .Select(row => new AirportWatchLiveData
                {
                    BoxTransmissionDateTimeUtc = row.BoxTransmissionDateTimeUtc,
                    AircraftHexCode = row.AircraftHexCode,
                    AtcFlightNumber = row.AtcFlightNumber,
                    AltitudeInStandardPressure = row.AltitudeInStandardPressure,
                    GroundSpeedKts = row.GroundSpeedKts,
                    TrackingDegree = row.TrackingDegree,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    VerticalSpeedKts = row.VerticalSpeedKts,
                    TransponderCode = row.TransponderCode,
                    BoxName = row.BoxName,
                    AircraftPositionDateTimeUtc = row.AircraftPositionDateTimeUtc,
                    AircraftTypeCode = row.AircraftTypeCode,
                    GpsAltitude = row.GpsAltitude,
                    IsAircraftOnGround = row.IsAircraftOnGround,
                })
                .ToList();

            var historicalData = new List<AirportWatchHistoricalData>();
            foreach (var record in data)
            {
                var lastUpdatedRecord = _context.AirportWatchHistoricalData
                    .Where(h => h.AircraftHexCode == record.AircraftHexCode && h.AtcFlightNumber == record.AtcFlightNumber)
                    .OrderByDescending(h => h.BoxTransmissionDateTimeUtc)
                    .FirstOrDefault();
                if (lastUpdatedRecord == null || lastUpdatedRecord.IsAircraftOnGround != record.IsAircraftOnGround)
                {
                    if (lastUpdatedRecord != null)
                    {
                        _context.AirportWatchHistoricalData.Remove(lastUpdatedRecord);
                    }

                    historicalData.Add(new AirportWatchHistoricalData
                    {
                        BoxTransmissionDateTimeUtc = record.BoxTransmissionDateTimeUtc,
                        AircraftHexCode = record.AircraftHexCode,
                        AtcFlightNumber = record.AtcFlightNumber,
                        AltitudeInStandardPressure = record.AltitudeInStandardPressure,
                        GroundSpeedKts = record.GroundSpeedKts,
                        TrackingDegree = record.TrackingDegree,
                        Latitude = record.Latitude,
                        Longitude = record.Longitude,
                        VerticalSpeedKts = record.VerticalSpeedKts,
                        TransponderCode = record.TransponderCode,
                        BoxName = record.BoxName,
                        AircraftPositionDateTimeUtc = record.AircraftPositionDateTimeUtc,
                        AircraftTypeCode = record.AircraftTypeCode,
                        GpsAltitude = record.GpsAltitude,
                        IsAircraftOnGround = record.IsAircraftOnGround,
                    });
                }
            }

            _context.AirportWatchDataTransition.AddRange(data);
            _context.AirportWatchLiveData.AddRange(live10sData);
            _context.AirportWatchHistoricalData.AddRange(historicalData);

            _context.SaveChanges();
        }
    }
}
