using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.Aircraft
{
    public class AircraftSpecification : Specification<AirCrafts>
    {
        public AircraftSpecification(IList<int> aircraftIds) : base(x => aircraftIds.Contains(x.AircraftId))
        {
        }
    }
}
