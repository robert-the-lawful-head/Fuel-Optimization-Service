using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.Aircraft
{
    public class AFSAircraftSpecification : Specification<AFSAircraft>
    {
        public AFSAircraftSpecification(IList<int> aircraftIds) : base(x => aircraftIds.Contains(x.DegaAircraftID))
        {
        }
    }
}
