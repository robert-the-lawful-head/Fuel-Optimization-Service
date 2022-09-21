using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FBOLinx.Core.Enums;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using Mapster;
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
                                    RecentAirportWatchHistoricalDataCollection = groupedResult.Where(x => x.historical != null).Select(x => x.historical.Adapt<AirportWatchHistoricalDataDto>()).ToList()
                                }
                )
                .ToListAsync();

            return result;
        }
    }
}
