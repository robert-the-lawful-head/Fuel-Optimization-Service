using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<AcukwikAirports> GetAirportByAcukwikAirportId(int acukwikAirportId);
        Task<List<AcukwikAirports>> GetAirportsByAcukwikAirportIds(List<int> acukwikAirportIds);
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

        //TODO: Move these to entity service when Irving is ready
        public async Task<AcukwikAirports> GetAirportByAcukwikAirportId(int acukwikAirportId)
        {
            var airport = await _degaContext.AcukwikAirports.FirstOrDefaultAsync(x => x.AirportId == acukwikAirportId);
            return airport;
        }

        public async Task<List<AcukwikAirports>> GetAirportsByAcukwikAirportIds(List<int> acukwikAirportIds)
        {
            var airports = await _degaContext.AcukwikAirports.Where(x => acukwikAirportIds.Contains(x.AirportId)).ToListAsync();
            return airports;
        }
    }
}
