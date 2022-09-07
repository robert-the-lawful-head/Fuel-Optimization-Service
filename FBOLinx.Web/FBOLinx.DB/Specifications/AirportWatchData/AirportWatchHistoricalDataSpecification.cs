using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchHistoricalDataSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataSpecification(string atcFlightNumber, DateTime startDate)
            : base(x => x.AtcFlightNumber == atcFlightNumber && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }

        public AirportWatchHistoricalDataSpecification(IList<string> atcFlightNumbers, DateTime startDate)
            : base(x => atcFlightNumbers.Contains(x.AtcFlightNumber) && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }

        public AirportWatchHistoricalDataSpecification(IList<string> atcFlightNumbers, IList<string> tailNumbers, DateTime startDate)
            : base(x => (atcFlightNumbers.Contains(x.AtcFlightNumber) || tailNumbers.Contains(x.TailNumber)) && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }

        public AirportWatchHistoricalDataSpecification(IList<string> aircraftIdentifications, DateTime startDate, DateTime endDate)
            : base(x => (aircraftIdentifications.Contains(x.AtcFlightNumber) || aircraftIdentifications.Contains(x.TailNumber)) && x.AircraftPositionDateTimeUtc >= startDate && x.AircraftPositionDateTimeUtc <= endDate)
        {
        }
    }
}
