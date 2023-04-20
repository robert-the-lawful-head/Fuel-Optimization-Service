using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Groups
{
    public class GroupTransitionService
    {
        private readonly FboLinxContext _context;
        public GroupTransitionService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task PerformLegacyGroupTransition(int groupId)
        {            
            var oldAircraftPrices = await (from ap in _context.AircraftPrices
                                           join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                           join f in _context.Fbos on new
                                           {
                                               fboId = pt.Fboid,
                                               groupId
                                           } equals new
                                           {
                                               fboId = f.Oid,
                                               groupId = f.GroupId
                                           }
                                           select ap)
                                              .ToListAsync();
            _context.AircraftPrices.RemoveRange(oldAircraftPrices);
            await _context.SaveChangesAsync();            
        }
    }
}
