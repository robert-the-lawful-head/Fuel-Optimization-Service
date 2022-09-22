using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public class SWIMFlightLegByAircraftSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegByAircraftSpecification(IList<string> tailOrFlightNumbers, DateTime minArrivalOrDepartureDate)
            : base(x => !string.IsNullOrEmpty(x.AircraftIdentification) &&
                        (tailOrFlightNumbers.Contains(x.AircraftIdentification) || tailOrFlightNumbers.Contains(x.AircraftIdentification))
                        && ((x.ATD > minArrivalOrDepartureDate) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDate)))
        {
        }
    }
}
