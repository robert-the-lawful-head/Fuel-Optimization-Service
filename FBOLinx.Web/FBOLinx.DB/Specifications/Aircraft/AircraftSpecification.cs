using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.Aircraft
{
    public sealed class AircraftSpecification : Specification<AirCrafts>
    {
        public AircraftSpecification() : base(x => true)
        {
            AddInclude(x => x.AFSAircraft);
            AddInclude(x => x.AircraftSpecifications);
        }

        public AircraftSpecification(IList<int> aircraftIds) : base(x => aircraftIds.Contains(x.AircraftId))
        {
            AddInclude(x => x.AFSAircraft);
            AddInclude(x => x.AircraftSpecifications);
        }
    }
}
