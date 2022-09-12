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
    public class AirportWatchHistoricalDataByIcaoSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataByIcaoSpecification(string icao, DateTime? startDateTimeUtc, DateTime? endDateTimeUtc, List<string> tailNumbers = null) : base(awhd =>
            (string.IsNullOrEmpty(icao) || awhd.AirportICAO == icao)
        && (!startDateTimeUtc.HasValue || awhd.AircraftPositionDateTimeUtc >= startDateTimeUtc.Value.ToUniversalTime())
            && (!endDateTimeUtc.HasValue || awhd.AircraftPositionDateTimeUtc <= endDateTimeUtc.Value.ToUniversalTime()))
        {
        }
    }
}
