using System;
using System.Collections;
using System.Collections.Generic;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AircraftHexTailMapping
{
    public sealed class AircraftMakeModelSpecification : Specification<DB.FAAAircraftMakeModelReference>
    {
        public AircraftMakeModelSpecification(IList<string> aircraftMakeModelCodes)
            : base(x => aircraftMakeModelCodes.Contains(x.CODE))
        {
        }
    }
}