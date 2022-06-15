using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchLiveDataSpecification : Specification<AirportWatchLiveData>
    {
        public AirportWatchLiveDataSpecification(string atcFlightNumber)
            : base(x => x.AtcFlightNumber == atcFlightNumber)
        {
        }

        public AirportWatchLiveDataSpecification(IList<string> atcFlightNumbers)
            : base(x => atcFlightNumbers.Contains(x.AtcFlightNumber))
        {
        }
    }
}
