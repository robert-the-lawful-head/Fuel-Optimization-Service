using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportFboGeoFence
{
    public sealed class AirportFboGeoFenceClusterSpecification : Specification<Models.AirportFboGeofenceClusters>
    {
        public AirportFboGeoFenceClusterSpecification() : base(x => true)
        {
            AddInclude(x => x.ClusterCoordinatesCollection);
        }

        public AirportFboGeoFenceClusterSpecification(int id) : base(x => x.Oid == id)
        {
            AddInclude(x => x.ClusterCoordinatesCollection);
        }
    }
}
