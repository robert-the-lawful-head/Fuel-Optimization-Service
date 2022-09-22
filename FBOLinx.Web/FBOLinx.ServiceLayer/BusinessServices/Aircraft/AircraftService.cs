using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FBOLinx.ServiceLayer.BusinessServices.Aircraft
{
    public class AircraftService
    {
        private FboLinxContext _fboLinxContext;
        private DegaContext _degaContext;

        public AircraftService(FboLinxContext fboLinxContext, DegaContext degaContext)
        {
            _fboLinxContext = fboLinxContext;
            _degaContext = degaContext;
        }

        public async Task<List<AirCrafts>> GetAllAircrafts()
        {
            var aircrafts = await _degaContext.AirCrafts.Include(a => a.AFSAircraft).Include(a => a.AircraftSpecifications).ToListAsync();
            return aircrafts;
        }

        public IIncludableQueryable<AirCrafts, AircraftSpecifications> GetAllAircraftsAsQueryable()
        {
            return _degaContext.AirCrafts.Include(a => a.AFSAircraft).Include(a => a.AircraftSpecifications);
        }

        public IQueryable<AirCrafts> GetAllAircraftsOnlyAsQueryable()
        {
            return _degaContext.AirCrafts.AsNoTracking();
        }

        public async Task<AirCrafts> GetAircrafts(int oid)
        {
            return await _degaContext.AirCrafts.Include(a => a.AFSAircraft).Include(a => a.AircraftSpecifications).FirstOrDefaultAsync(a => a.AircraftId == oid);
        }

        public async Task AddAirCrafts(AirCrafts airCrafts)
        {
            _degaContext.AirCrafts.Add(airCrafts);
            await _degaContext.SaveChangesAsync();
        }

        public async Task UpdateAirCrafts(AirCrafts airCrafts)
        {
            _degaContext.AirCrafts.Update(airCrafts);
            await _degaContext.SaveChangesAsync();
        }

        public async Task RemoveAirCrafts(AirCrafts airCrafts)
        {
            _degaContext.AirCrafts.Remove(airCrafts);
            await _degaContext.SaveChangesAsync();
        }
    }
}
