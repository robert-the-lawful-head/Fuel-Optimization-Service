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
    public class AirportWatchLiveDataSpecification : Specification<DB.Models.AirportWatchLiveData>
    {
        public AirportWatchLiveDataSpecification(DateTime aircraftPositionStartDateTime, DateTime aircraftPositionEndDateTime) : base(x => x.AircraftPositionDateTimeUtc >= aircraftPositionStartDateTime && x.AircraftPositionDateTimeUtc <= aircraftPositionEndDateTime)
        {
        }
    }
}
