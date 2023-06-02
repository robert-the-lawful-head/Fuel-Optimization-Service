using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO.SWIM;
using System.Linq;
using FBOLinx.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class SWIMFlightLegDataEntityService : Repository<SWIMFlightLegData, DegaContext>
    {
        public SWIMFlightLegDataEntityService(DegaContext context) : base(context)
        {
        }

        public async Task<List<SWIMFlightLegData>> GetSwimFlightLegData(
            List<long> swimFlightLegIds)
        {
            var query = GetSwimFlightLegDataQueryable(swimFlightLegIds);
            return await query.ToListAsync();
        }

        private IQueryable<SWIMFlightLegData> GetSwimFlightLegDataQueryable(
            List<long> swimFlightLegIds)
        {
            var query = (from swimData in context.SWIMFlightLegData
                join swimLegId in context.AsTable(swimFlightLegIds) on swimData.SWIMFlightLegId equals System.Convert.ToInt64(swimLegId.Value)
                select swimData);

            return query;
        }
    }
}
