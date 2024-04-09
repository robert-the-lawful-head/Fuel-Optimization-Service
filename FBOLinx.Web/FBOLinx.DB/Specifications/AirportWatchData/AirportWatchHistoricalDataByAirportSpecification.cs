using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchHistoricalDataByAirportSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataByAirportSpecification(List<string> icaoList, DateTime startDateTimeUtc, DateTime endDateTimeUtc) : base(x => icaoList.Contains(x.AirportICAO)
        && x.AircraftPositionDateTimeUtc > startDateTimeUtc
        && x.AircraftPositionDateTimeUtc < endDateTimeUtc)
        {
            AddInclude(x => x.AirportWatchHistoricalParking);
        }
    }
}
