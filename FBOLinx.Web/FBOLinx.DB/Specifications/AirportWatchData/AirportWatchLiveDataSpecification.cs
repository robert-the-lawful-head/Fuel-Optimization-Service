using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchLiveDataSpecification : Specification<AirportWatchLiveData>
    {
        public AirportWatchLiveDataSpecification(string atcFlightNumber, DateTime startDate)
            : base(x => x.AtcFlightNumber == atcFlightNumber && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }
    }
}
