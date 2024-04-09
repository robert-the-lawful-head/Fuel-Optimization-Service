using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;
using FBOLinx.DB.Projections.AirportWatch;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public class AirportWatchLiveHexTailMappingSpecification : Specification<AirportWatchLiveData, AirportWatchLiveHexTailMapping>
    {
        public AirportWatchLiveHexTailMappingSpecification(IList<string> atcFlightNumbers, DateTime startDate) 
            : base(x => atcFlightNumbers.Contains(x.AtcFlightNumber) && (x.TailNumber != null || x.AircraftHexCode != null) && x.AircraftPositionDateTimeUtc >= startDate, 
                x => new AirportWatchLiveHexTailMapping() { TailNumber = x.TailNumber, AircraftHexCode = x.AircraftHexCode, AtcFlightNumber = x.AtcFlightNumber, AircraftPositionDateTimeUtc = x.AircraftPositionDateTimeUtc})
        {
        }
    }
}