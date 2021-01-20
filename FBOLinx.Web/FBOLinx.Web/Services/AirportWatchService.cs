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

        public void ProcessAirportWatchData(List<AirportWatchLiveData> data)
        {
            // Add the most recent records for the "live" view
            foreach (var record in data)
            {
                var lastUpdatedRecord = _context.AirportWatchLiveData
                    .Where(h => h.AircraftHexCode == record.AircraftHexCode && h.AtcFlightNumber == record.AtcFlightNumber)
                    .OrderByDescending(h => h.BoxTransmissionDateTimeUtc)
                    .FirstOrDefault();

                if (lastUpdatedRecord == null || lastUpdatedRecord.IsAircraftOnGround != record.IsAircraftOnGround)
                {
                    if (lastUpdatedRecord != null)
                    {
                        _context.AirportWatchLiveData.Remove(lastUpdatedRecord);
                    }

                    _context.AirportWatchLiveData.Add(record);
                }
            }

            // Add the distinct aircraft records from the HexCode and FlightNumber
            var aircraftTailNumbers = data
                .Select(record => new AirportWatchAircraftTailNumber
                {
                    AircraftHexCode = record.AircraftHexCode,
                    AtcFlightNumber = record.AtcFlightNumber,
                })
                .ToList();
            foreach (var aircraftTailNumber in aircraftTailNumbers)
            {
                var oldAircraftTailNumber = _context.AirportWatchAircraftTailNumber
                    .Where(h => h.AircraftHexCode == aircraftTailNumber.AircraftHexCode && h.AtcFlightNumber == aircraftTailNumber.AtcFlightNumber)
                    .FirstOrDefault();
                if (oldAircraftTailNumber == null)
                {
                    _context.AirportWatchAircraftTailNumber.Add(aircraftTailNumber);
                }
            }

            // Add all historical occurrences of a landing, takeoff, and parking.
            foreach (var record in data)
            {
                var lastUpdatedRecord = _context.AirportWatchLiveData
                    .Where(h => h.AircraftHexCode == record.AircraftHexCode && h.AtcFlightNumber == record.AtcFlightNumber)
                    .OrderByDescending(h => h.BoxTransmissionDateTimeUtc)
                    .FirstOrDefault();

                //  landing, takeoff, parked
                if ((lastUpdatedRecord == null || lastUpdatedRecord.IsAircraftOnGround != record.IsAircraftOnGround) ||
                    (
                        lastUpdatedRecord != null &&
                        lastUpdatedRecord.IsAircraftOnGround != true &&
                        record.IsAircraftOnGround != true &&
                        record.BoxTransmissionDateTimeUtc >= lastUpdatedRecord.BoxTransmissionDateTimeUtc.AddMinutes(10)
                    )
                )
                {
                    var airportWatchHistoricalData = AirportWatchHistoricalData.ConvertFromAirportWatchLiveData(record);
                    _context.AirportWatchHistoricalData.Add(airportWatchHistoricalData);
                }
            }

            _context.SaveChanges();
        }
    }
}
