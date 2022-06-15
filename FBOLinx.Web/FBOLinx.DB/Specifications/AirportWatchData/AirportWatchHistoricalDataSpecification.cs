using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchHistoricalDataSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataSpecification(string atcFlightNumber)
            : base(x => x.AtcFlightNumber == atcFlightNumber)
        {
        }

        public AirportWatchHistoricalDataSpecification(IList<string> atcFlightNumbers)
            : base(x => atcFlightNumbers.Contains(x.AtcFlightNumber))
        {
        }
    }
}
