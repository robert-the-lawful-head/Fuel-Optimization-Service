using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchLiveDataByFlightNumberSpecification : Specification<AirportWatchLiveData>
    {
        public AirportWatchLiveDataByFlightNumberSpecification(string atcFlightNumber, DateTime startDate)
            : base(x => x.AtcFlightNumber == atcFlightNumber && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }

        public AirportWatchLiveDataByFlightNumberSpecification(IList<string> atcFlightNumbers, DateTime startDate)
            : base(x => atcFlightNumbers.Contains(x.AtcFlightNumber) && x.TailNumber != null && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }

        public AirportWatchLiveDataByFlightNumberSpecification(IList<string> atcFlightNumbers, IList<string> tailNumbers, DateTime startDate)
            : base(x => (atcFlightNumbers.Contains(x.AtcFlightNumber) || tailNumbers.Contains(x.TailNumber)) && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }
    }
}
