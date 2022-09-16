using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchHistoricalDataByTailNumberSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataByTailNumberSpecification(IList<string> tailNumbers, DateTime startDate)
            : base(x => x.TailNumber != null && tailNumbers.Contains(x.TailNumber) && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }
    }
}
