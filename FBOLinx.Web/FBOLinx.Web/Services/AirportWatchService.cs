using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Linq;

namespace FBOLinx.Web.Services
{
    public class AirportWatchService
    {
        private FboLinxContext _context;

        public AirportWatchService(FboLinxContext context)
        {
            _context = context;
        }

        public void ProcessAirportWatchData(List<AirportWatchLiveData> data)
        {
            var distinctedAirportWatchData = _context.AirportWatchLiveData
                .Select(row => new
                {
                    row.Oid,
                    row.IsAircraftOnGround,
                    row.BoxTransmissionDateTimeUtc,
                    row.Latitude,
                    row.Longitude,
                    row.AircraftHexCode,
                    row.AtcFlightNumber
                })
                .ToList()
                .GroupBy(aw => new { aw.AircraftHexCode, aw.AtcFlightNumber })
                .Select(g => g.OrderByDescending(x => x.BoxTransmissionDateTimeUtc).First())
                .ToList();

            var pairedData = (from r in data
                         join aw in distinctedAirportWatchData on new {r.AircraftHexCode, r.AtcFlightNumber } equals new { aw.AircraftHexCode, aw.AtcFlightNumber }
                         into leftJoinAw
                         from aw in leftJoinAw.DefaultIfEmpty()
                         select new
                         {
                             Record = r,
                             PreviousRecord = aw
                         })
                         .OrderBy(aw => aw.Record.BoxTransmissionDateTimeUtc)
                         .ToList();

            foreach (var pair in pairedData)
            {

                var lastUpdatedRecord = pair.PreviousRecord;
                var record = pair.Record;

                if (lastUpdatedRecord == null || lastUpdatedRecord.IsAircraftOnGround != record.IsAircraftOnGround)
                {
                    if (lastUpdatedRecord != null)
                    {
                        var oldRecord = new AirportWatchLiveData { Oid = lastUpdatedRecord.Oid };
                        _context.AirportWatchLiveData.Remove(oldRecord);
                    }
                    else
                    {
                        // Add the distinct aircraft records from the HexCode and FlightNumber
                        _context.AirportWatchAircraftTailNumber.Add(new AirportWatchAircraftTailNumber
                        {
                            AircraftHexCode = record.AircraftHexCode,
                            AtcFlightNumber = record.AtcFlightNumber,
                        });
                    }

                    // Add the most recent records for the "live" view
                    _context.AirportWatchLiveData.Add(record);

                    // Add all historical occurrences of a landing, takeoff
                    var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                }

                // Add all historical occurrences of a parking.
                if (lastUpdatedRecord != null &&
                    lastUpdatedRecord.IsAircraftOnGround == true &&
                    record.IsAircraftOnGround == true &&
                    record.BoxTransmissionDateTimeUtc >= lastUpdatedRecord.BoxTransmissionDateTimeUtc.AddMinutes(10) &&
                    record.Longitude == lastUpdatedRecord.Longitude &&
                    record.Latitude == lastUpdatedRecord.Latitude)
                {
                    var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                }
            }

            _context.SaveChanges();
        }
    }
}
