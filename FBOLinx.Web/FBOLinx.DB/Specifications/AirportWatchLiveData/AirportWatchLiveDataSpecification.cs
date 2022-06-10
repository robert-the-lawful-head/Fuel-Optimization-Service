using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class AirportWatchLiveDataSpecification : Specification<AirportWatchLiveData>
    {
        public AirportWatchLiveDataSpecification(IList<string> atcFlightNumbers)
            : base(x => atcFlightNumbers.Contains(x.AtcFlightNumber))
        {
        }
    }
}
