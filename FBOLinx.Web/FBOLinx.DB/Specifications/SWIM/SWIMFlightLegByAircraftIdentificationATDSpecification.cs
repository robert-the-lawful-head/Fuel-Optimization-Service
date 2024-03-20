using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class SWIMFlightLegByAircraftIdentificationATDSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegByAircraftIdentificationATDSpecification(string aircraftIdentification, DateTime actualDepartureStartDateRange, DateTime actualDepartureEndDateRange)
            : base(x => aircraftIdentification.Contains(x.AircraftIdentification) && x.ATD > actualDepartureStartDateRange && x.ATD < actualDepartureEndDateRange)
        {
        }
    }
}
