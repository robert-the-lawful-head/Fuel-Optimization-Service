using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public class SWIMFlightLegByDateSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegByDateSpecification(DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC)))
        {
        }
    }
}
