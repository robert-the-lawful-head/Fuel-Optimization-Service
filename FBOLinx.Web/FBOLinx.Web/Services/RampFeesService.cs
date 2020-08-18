using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class RampFeesService
    {
        private readonly FboLinxContext _context;
        private int _FboId;
        private int _GroupId;

        public RampFeesService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<double?> GetRampFeesForFbo(int fboId, AirCrafts.AircraftSizes? sizeId)
        {
            var planeRampFee = _context.RampFees.FirstOrDefault(s => s.Fboid == fboId && s.Size == sizeId);

            if(planeRampFee != null)
            {
                return planeRampFee.Price;
            }

            return null;

        }
    }
}
