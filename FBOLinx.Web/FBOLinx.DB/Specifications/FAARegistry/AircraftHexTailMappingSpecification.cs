using System;
using System.Collections;
using System.Collections.Generic;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AircraftHexTailMapping
{
    public sealed class AircraftHexTailMappingSpecification : Specification<DB.Models.AircraftHexTailMapping>
    {
        public AircraftHexTailMappingSpecification(string aircraftHexCode)
            : base(x => x.AircraftHexCode == aircraftHexCode)
        {
        }

        public AircraftHexTailMappingSpecification(IList<string> tailNumbers)
            : base(x => tailNumbers.Contains(x.TailNumber))
        {
        }
    }
}