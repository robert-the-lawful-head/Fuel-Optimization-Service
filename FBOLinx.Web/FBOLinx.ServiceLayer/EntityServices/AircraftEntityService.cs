using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AircraftEntityService: Repository<AirCrafts, DegaContext>
    {
        public AircraftEntityService(DegaContext context) : base(context)
        {
        }

        public IIncludableQueryable<AirCrafts, AircraftSpecifications> GetAllAircraftsAsQueryable()
        {
            return context.AirCrafts.Include(a => a.AFSAircraft).Include(a => a.AircraftSpecifications);
        }

        public IQueryable<AirCrafts> GetAllAircraftsOnlyAsQueryable()
        {
            return _degaContext.AirCrafts.AsNoTracking();
        }
    }
}
