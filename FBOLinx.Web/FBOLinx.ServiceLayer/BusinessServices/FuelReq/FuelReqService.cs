using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public class FuelReqService
    {
        private readonly FboLinxContext _context;
        public FuelReqService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<List<FuelReq>> GetRecentFuelRequestsForFbo(int fboId)
        {
            var startDate = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0));
            var endDate = DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0));
            var requests = await _context.FuelReq.Where(f => f.Fboid == fboId && f.Eta >= startDate && f.Eta <= endDate).ToListAsync();

            return requests;
        }
    }
}
