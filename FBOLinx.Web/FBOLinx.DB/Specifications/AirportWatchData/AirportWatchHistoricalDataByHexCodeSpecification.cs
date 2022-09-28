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
    public class AirportWatchHistoricalDataByHexCodeSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataByHexCodeSpecification(List<string> aircraftHexCodes, DateTime aircraftPositionStateDate)
            : base(x => !string.IsNullOrEmpty(x.AircraftHexCode) 
                        && aircraftHexCodes.Contains(x.AircraftHexCode)
                        && x.AircraftPositionDateTimeUtc > aircraftPositionStateDate)
        {
        }
    }
}
