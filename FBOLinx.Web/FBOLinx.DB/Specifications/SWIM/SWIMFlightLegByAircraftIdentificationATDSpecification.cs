using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class SWIMFlightLegByAircraftIdentificationATDSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegByAircraftIdentificationATDSpecification(IList<string> aircraftIdentification, DateTime startDate, DateTime endDate)
            : base(x => aircraftIdentification.Contains(x.AircraftIdentification) && x.ATD > startDate && x.ATD < endDate)
        {
        }
    }
}
