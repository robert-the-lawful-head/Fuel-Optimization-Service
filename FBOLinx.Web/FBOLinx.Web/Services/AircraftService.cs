using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
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

        public List<AirCrafts> GetAllAircrafts()
        {
            var aircrafts = _degaContext.AirCrafts.Include(a => a.AFSAircraft).Include(a => a.AircraftSpecifications).ToList();
            return aircrafts;
        }

        public AirCrafts GetAircrafts(int oid)
        {
            return _degaContext.AirCrafts.Include(a => a.AFSAircraft).Include(a => a.AircraftSpecifications).Where(a => a.AircraftId == oid).FirstOrDefault();
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
