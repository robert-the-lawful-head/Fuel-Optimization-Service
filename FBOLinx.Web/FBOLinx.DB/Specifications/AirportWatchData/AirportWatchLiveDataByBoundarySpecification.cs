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
    public class AirportWatchLiveDataByBoundarySpecification : Specification<DB.Models.AirportWatchLiveData>
    {
        public AirportWatchLiveDataByBoundarySpecification(DateTime aircraftPositionStartDateTime, DateTime aircraftPositionEndDateTime,
            double minLatitude, double maxLatitude, double minLongitude, double maxLongitude) : base(x => x.AircraftPositionDateTimeUtc >= aircraftPositionStartDateTime && x.AircraftPositionDateTimeUtc <= aircraftPositionEndDateTime
        && x.Latitude >= minLatitude
        && x.Latitude <= maxLatitude
        && x.Longitude >= minLongitude
        && x.Longitude <= maxLongitude)
        {
        }
    }
}
