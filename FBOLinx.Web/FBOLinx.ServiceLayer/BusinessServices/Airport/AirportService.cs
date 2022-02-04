using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Airport
{
    public interface IAirportService
    {
        Task<Fboairports> GetAirportForFboId(int fboId);
    }

    public class AirportService : IAirportService
    {
        private FboLinxContext _fboLinxContext;
        private DegaContext _degaContext;

        public AirportService(FboLinxContext fboLinxContext, DegaContext degaContext)
        {
            _degaContext = degaContext;
            _fboLinxContext = fboLinxContext;
        }

        public async Task<Fboairports> GetAirportForFboId(int fboId)
        {
            var airport = await _fboLinxContext.Fboairports.FirstOrDefaultAsync(x => x.Fboid == fboId);
            return airport;
        }
    }
}
