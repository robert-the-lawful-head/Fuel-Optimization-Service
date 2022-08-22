using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchLiveDataEntityService : Repository<AirportWatchLiveData, FboLinxContext>
    {
        private readonly FboLinxContext _context;

        public AirportWatchLiveDataEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tuple<string, string, double, double, double, double>>> GetTailNumbersByLiveAndParkingCoordinates(DateTime liveStartDate, DateTime historicalStartDate)
        {
            var result = (await _context.AirportWatchLiveData.Join(
                _context.AirportWatchHistoricalData,
                l => l.TailNumber,
                h => h.TailNumber,
                (l, h) => new {
                    Oid = l.Oid,
                    TailNumber = l.TailNumber,
                    AtcFlightNumber = l.AtcFlightNumber,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    AircraftPositionDateTimeUtc = l.AircraftPositionDateTimeUtc,
                    IsAircraftOnGround = l.IsAircraftOnGround,
                    HistoricalOid = h.Oid,
                    HistoricalLatitude = h.Latitude,
                    HistoricalLongitude = h.Longitude,
                    HistoicalAircraftStatus = h.AircraftStatus,
                    HistoricalAircraftPositionDateTimeUtc = h.AircraftPositionDateTimeUtc
                })
                .Where(x => x.TailNumber != null && x.IsAircraftOnGround && x.HistoicalAircraftStatus == AircraftStatusType.Parking && 
                            x.AircraftPositionDateTimeUtc > liveStartDate && x.HistoricalAircraftPositionDateTimeUtc > historicalStartDate)
                .GroupBy(x => x.TailNumber)
                .Select(x => new { x.Key, Count = x.Count(), Item = x.OrderByDescending(e => e.HistoricalAircraftPositionDateTimeUtc).Take(1) })
                .Where(x => x.Count > 1)
                .ToListAsync())
                .Select(x => new Tuple<string, string, double, double, double, double>(x.Item.First().TailNumber, x.Item.First().AtcFlightNumber, x.Item.First().Latitude, x.Item.First().Longitude,
                    x.Item.First().HistoricalLatitude, x.Item.First().HistoricalLongitude));
            
            return result;
        }
    }
}
