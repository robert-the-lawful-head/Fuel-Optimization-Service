using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Geolocation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private readonly FboLinxContext _context;

        public AirportWatchService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<List<AirportWatchLiveData>> GetAirportWatchLiveData(Coordinate coordinate, int distance, DistanceUnit distanceUnit)
        {
            CoordinateBoundaries boundaries = new CoordinateBoundaries(coordinate, distance, distanceUnit);
            double minLatitude = boundaries.MinLatitude;
            double maxLatitude = boundaries.MaxLatitude;
            double minLongitude = boundaries.MinLongitude;
            double maxLongitude = boundaries.MaxLongitude;

            var timelimit = DateTime.UtcNow.AddMinutes(-10);

            var filteredResult = await _context.AirportWatchLiveData
                .Where(x => x.AircraftPositionDateTimeUtc >= timelimit)
                .Where(x => x.Latitude >= minLatitude && x.Latitude <= maxLatitude)
                .Where(x => x.Longitude >= minLongitude && x.Longitude <= maxLongitude)
                .OrderBy(x => x.AircraftPositionDateTimeUtc)
                .ThenBy(x => x.AircraftHexCode)
                .ThenBy(x => x.AtcFlightNumber)
                .ToListAsync();

            return filteredResult
                .Where(x => GeoCalculator.GetDistance(coordinate.Latitude, coordinate.Longitude, x.Latitude, x.Longitude, 1, distanceUnit) <= distance)
                .ToList();
        }

        public void ProcessAirportWatchData(List<AirportWatchLiveData> data)
        {
            var pairedData = (from r in data
                              join aw in _context.AirportWatchLiveData on new { r.AircraftHexCode, r.AtcFlightNumber } equals new { aw.AircraftHexCode, aw.AtcFlightNumber }
                              into leftJoinAw
                              from aw in leftJoinAw.DefaultIfEmpty()
                              join ah in _context.AirportWatchHistoricalData on new { r.AircraftHexCode, r.AtcFlightNumber } equals new { ah.AircraftHexCode, ah.AtcFlightNumber }
                              into leftJoinAh
                              from ah in leftJoinAh.DefaultIfEmpty()
                              select new
                              {
                                  Record = r,
                                  OldAirportWatchLiveData = aw,
                                  OldAirportWatchHistoricalData = ah
                              })
                              .OrderBy(aw => aw.Record.AircraftPositionDateTimeUtc)
                              .ToList();

            foreach (var pair in pairedData)
            {

                var lastUpdatedAirportWatchLiveData = pair.OldAirportWatchLiveData;
                var lastUpdatedAirportWatchHistoricalData = pair.OldAirportWatchHistoricalData;
                var record = pair.Record;

                
                if (lastUpdatedAirportWatchLiveData != null)
                {
                    AirportWatchLiveData.CopyEntity(lastUpdatedAirportWatchLiveData, record);
                    _context.AirportWatchLiveData.Update(lastUpdatedAirportWatchLiveData);
                }
                else
                {
                    // Add the distinct aircraft records from the HexCode and FlightNumber
                    _context.AirportWatchAircraftTailNumber.Add(new AirportWatchAircraftTailNumber
                    {
                        AircraftHexCode = record.AircraftHexCode,
                        AtcFlightNumber = record.AtcFlightNumber,
                    });
                    // Add the most recent records for the "live" view
                    _context.AirportWatchLiveData.Add(record);
                }

                // Add/Update historical occurrences of landing/takeoff/parking
                if (lastUpdatedAirportWatchHistoricalData == null)
                {
                    var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                }
                else
                {
                    var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);

                    // Parking occurrences
                    if (lastUpdatedAirportWatchHistoricalData.IsAircraftOnGround == true &&
                        record.IsAircraftOnGround == true &&
                        record.AircraftPositionDateTimeUtc >= lastUpdatedAirportWatchHistoricalData.AircraftPositionDateTimeUtc.AddMinutes(10) &&
                        record.Longitude == lastUpdatedAirportWatchHistoricalData.Longitude &&
                        record.Latitude == lastUpdatedAirportWatchHistoricalData.Latitude)
                    {
                        airportWatchHistoricalData.AircraftStatus = AirportWatchHistoricalData.AircraftStatusType.Parking;
                    }

                    AirportWatchHistoricalData.CopyEntity(lastUpdatedAirportWatchHistoricalData, airportWatchHistoricalData);
                    _context.AirportWatchHistoricalData.Update(lastUpdatedAirportWatchHistoricalData);
                }
            }

            _context.SaveChanges();
        }
    }
}
