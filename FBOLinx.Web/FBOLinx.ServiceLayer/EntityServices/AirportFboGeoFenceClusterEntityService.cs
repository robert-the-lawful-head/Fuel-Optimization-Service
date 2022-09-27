using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IAirportFboGeoFenceClusterEntityService : IRepository<AirportFboGeofenceClusters, FboLinxContext>
    {
    }

    public class AirportFboGeoFenceClusterEntityService : Repository<AirportFboGeofenceClusters, FboLinxContext>, IAirportFboGeoFenceClusterEntityService
    {
        public AirportFboGeoFenceClusterEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
