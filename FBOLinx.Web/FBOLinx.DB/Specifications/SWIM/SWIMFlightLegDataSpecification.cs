using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class SWIMFlightLegDataSpecification : Specification<SWIMFlightLegData>
    {
        public SWIMFlightLegDataSpecification(IList<int> flightLegIds, DateTime startDate)
            : base(x => flightLegIds.Contains(x.SWIMFlightLegId) && x.MessageTimestamp > startDate)
        {
        }
    }
}
