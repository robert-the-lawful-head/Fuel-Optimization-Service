﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Linq;
using FBOLinx.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegDataEntityService : Repository<SWIMFlightLegData, FlightDataContext>
    {
        public SWIMFlightLegDataEntityService(FlightDataContext context) : base(context)
        {
        }

        public async Task<List<SWIMFlightLegData>> GetSwimFlightLegData(
            List<long> swimFlightLegIds, DateTime? minMessageDateTimeUtc = null)
        {
            var query = GetSwimFlightLegDataQueryable(swimFlightLegIds, minMessageDateTimeUtc);
            return await query.ToListAsync();
        }

        private IQueryable<SWIMFlightLegData> GetSwimFlightLegDataQueryable(
            List<long> swimFlightLegIds, DateTime? minMessageDateTimeUtc = null)
        {
            var idsAsString = swimFlightLegIds.Select(x => x.ToString()).ToList();
            var query = (from swimData in context.SWIMFlightLegData
                join swimLegId in context.AsTable(idsAsString) on swimData.SWIMFlightLegId equals (System.Convert.ToInt64(swimLegId.Value))
                         where (!minMessageDateTimeUtc.HasValue || swimData.MessageTimestamp >= minMessageDateTimeUtc)
                         select swimData);

            return query;
        }
    }
}
