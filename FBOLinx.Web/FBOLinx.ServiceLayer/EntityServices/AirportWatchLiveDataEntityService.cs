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

        // public async Task<IEnumerable<Tuple<int, string, string>>> GetPricingTemplates(IList<string> tailNumbers, DateTime startDate)
        // {
        //     var pricingTemplateList = 
        //         from airportWatchLiveData in _context.AirportWatchLiveData
        //         join airportWatchHistoricalData in _context.AirportWatchHistoricalData on airportWatchLiveData.TailNumber equals airportWatchHistoricalData.TailNumber
        //         where airportWatchLiveData.AircraftPositionDateTimeUtc >= startDate && airportWatchLiveData.IsAircraftOnGround && airportWatchHistoricalData.AircraftStatus == AircraftStatusType.Parking
        //         select new { airportWatchLiveData.Latitude, airportWatchLiveData.Longitude };
        //     return (await pricingTemplateList.ToListAsync()).Select(x => new Tuple<int, string, string>(x.AircraftId, x.TailNumber, x.Name));
        // }
    }
}
