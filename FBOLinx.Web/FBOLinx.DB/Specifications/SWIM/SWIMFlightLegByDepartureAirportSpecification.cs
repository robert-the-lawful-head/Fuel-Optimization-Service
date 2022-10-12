using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public class SWIMFlightLegByDepartureAirportSpecification : Specification<DB.Models.SWIMFlightLeg>
    {
        public SWIMFlightLegByDepartureAirportSpecification(string airportIcao, DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC))
        && ((!string.IsNullOrEmpty(x.DepartureICAO) && x.DepartureICAO == airportIcao)))
        {
        }

        public SWIMFlightLegByDepartureAirportSpecification(List<string> airportIcaos, DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC))
            && ((!string.IsNullOrEmpty(x.DepartureICAO) && airportIcaos.Contains(x.DepartureICAO))))
        {
        }
    }
}
