using System;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AircraftHexTailMapping
{
    public sealed class AircraftHexTailMappingSpecification : Specification<DB.AircraftHexTailMapping>
    {
        public AircraftHexTailMappingSpecification(string aircraftHexCode)
            : base(x => x.AircraftHexCode == aircraftHexCode)
        {
        }
    }
}