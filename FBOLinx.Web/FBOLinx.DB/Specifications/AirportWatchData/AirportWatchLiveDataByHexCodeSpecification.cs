using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public class AirportWatchLiveDataByHexCodeSpecification : Specification<AirportWatchLiveData>
    {
        public AirportWatchLiveDataByHexCodeSpecification(List<string> aircraftHexCodes, DateTime aircraftPositionStateDate) 
            : base(x => aircraftHexCodes.Contains(x.AircraftHexCode)
                        && x.AircraftPositionDateTimeUtc > aircraftPositionStateDate)
        {
        }
    }
}
