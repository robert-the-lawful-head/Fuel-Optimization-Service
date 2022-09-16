using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchLiveDataEntityService : Repository<AirportWatchLiveData, FboLinxContext>
    {
        private readonly FboLinxContext _context;

        public AirportWatchLiveDataEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AirportWatchLiveDataWithHistoricalStatusDto>> GetLiveDataWithRecentHistoryStatuses(DateTime liveStartDate, DateTime historicalStartDate)
        {
            var result = await (from live in _context.AirportWatchLiveData
                join historical in (_context.AirportWatchHistoricalData.Where(x => x.AircraftPositionDateTimeUtc > historicalStartDate)) on new { live.AtcFlightNumber, live.AircraftHexCode } equals new { historical.AtcFlightNumber, historical.AircraftHexCode } 
                into leftJoinHistoricalData
                from historical in leftJoinHistoricalData.DefaultIfEmpty()
                                where !string.IsNullOrEmpty(live.AtcFlightNumber)
                                && live.AircraftPositionDateTimeUtc > liveStartDate
                                group new {live, historical} by new {live.AtcFlightNumber, live.TailNumber, live.AircraftHexCode, historical.AircraftStatus} into groupedResult
                                select new AirportWatchLiveDataWithHistoricalStatusDto
                                {
                                    Oid = groupedResult.Max(x => x.live.Oid),
                                    TailNumber = groupedResult.Key.TailNumber,
                                    AtcFlightNumber = groupedResult.Key.AtcFlightNumber,
                                    Latitude = groupedResult.FirstOrDefault().live.Latitude,
                                    Longitude = groupedResult.FirstOrDefault().live.Longitude,
                                    AircraftPositionDateTimeUtc = groupedResult.Max(x => x.live.AircraftPositionDateTimeUtc),
                                    IsAircraftOnGround = groupedResult.FirstOrDefault().live.IsAircraftOnGround,
                                    HistoricalOid = groupedResult.FirstOrDefault().historical != null ? groupedResult.FirstOrDefault().historical.Oid : 0,
                                    HistoricalLongitude = groupedResult.FirstOrDefault().historical != null ? groupedResult.FirstOrDefault().historical.Longitude : 0,
                                    HistoricalLatitude = groupedResult.FirstOrDefault().historical != null ? groupedResult.FirstOrDefault().historical.Latitude : 0,
                                    HistoicalAircraftStatus = groupedResult.Key.AircraftStatus,
                                    HistoricalAircraftPositionDateTimeUtc = groupedResult.Any(x => x.historical != null) ? groupedResult.Where(x => x.historical != null).Max(x => x.historical.AircraftPositionDateTimeUtc) : null
                                }
                )
                .ToListAsync();

            //var result = (await _context.AirportWatchLiveData.Join(
            //    _context.AirportWatchHistoricalData,
            //    l => l.TailNumber,
            //    h => h.TailNumber,
            //    (l, h) => new {
            //        Oid = l.Oid,
            //        TailNumber = l.TailNumber,
            //        AtcFlightNumber = l.AtcFlightNumber,
            //        Latitude = l.Latitude,
            //        Longitude = l.Longitude,
            //        AircraftPositionDateTimeUtc = l.AircraftPositionDateTimeUtc,
            //        IsAircraftOnGround = l.IsAircraftOnGround,
            //        HistoricalOid = h.Oid,
            //        HistoricalLatitude = h.Latitude,
            //        HistoricalLongitude = h.Longitude,
            //        HistoicalAircraftStatus = h.AircraftStatus,
            //        HistoricalAircraftPositionDateTimeUtc = h.AircraftPositionDateTimeUtc
            //    })
            //    .Where(x => x.TailNumber != null && x.IsAircraftOnGround && x.HistoicalAircraftStatus == AircraftStatusType.Parking && 
            //                x.AircraftPositionDateTimeUtc > liveStartDate && x.HistoricalAircraftPositionDateTimeUtc > historicalStartDate)
            //    .GroupBy(x => x.TailNumber)
            //    .Select(x => new { x.Key, Count = x.Count(), Item = x.OrderByDescending(e => e.HistoricalAircraftPositionDateTimeUtc).Take(1) })
            //    .Where(x => x.Count > 1)
            //    .ToListAsync())
            //    .Select(x => new Tuple<string, string, double, double, double, double>(x.Item.First().TailNumber, x.Item.First().AtcFlightNumber, x.Item.First().Latitude, x.Item.First().Longitude,
            //        x.Item.First().HistoricalLatitude, x.Item.First().HistoricalLongitude));
            
            return result;
        }
    }
}
