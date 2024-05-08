using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IHistoricalAirportWatchSwimFlightLegEntityService : IRepository<HistoricalAirportWatchSwimFlightLegEntityService, FboLinxContext>
    {
        Task<List<HistoricalAirportWatchSwimFlightLeg>> GetHistoricalAirportWatchSwimFlightLegs(List<string> airportWatchIds);
        IQueryable<string> GetSWIMFlightLegsFromHistoricalAirportWatchQueryable(List<string> idsList);
    }

    public class HistoricalAirportWatchSwimFlightLegEntityService : Repository<HistoricalAirportWatchSwimFlightLegEntityService, FboLinxContext>, IHistoricalAirportWatchSwimFlightLegEntityService
    {
        private readonly FboLinxContext _context;

        public HistoricalAirportWatchSwimFlightLegEntityService(FboLinxContext context) : base(context)
        {
            _context = context;

        }

        public async Task<List<HistoricalAirportWatchSwimFlightLeg>> GetHistoricalAirportWatchSwimFlightLegs(List<string> airportWatchIds)
        {
            var historicalAirportWatchSwimFlightLegs = await (from historical in _context.HistoricalAirportWatchSwimFlightLeg
                                                              join ids in _context.AsTable(airportWatchIds) on historical.AirportWatchHistoricalDataId equals Convert.ToInt32(ids.Value)
                                                              select historical).ToListAsync();
            return historicalAirportWatchSwimFlightLegs;
        }

        public IQueryable<string> GetSWIMFlightLegsFromHistoricalAirportWatchQueryable(List<string> idsList)
        {
            var query = (from historical in context.HistoricalAirportWatchSwimFlightLeg
                         join ids in context.AsTable(idsList) on historical.AirportWatchHistoricalDataId equals Convert.ToInt64(ids.Value)
                         select historical.SwimFlightLegId.ToString());
            return query;
        }

    }
}
