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
    public class SWIMFlightLegByAirportSpecification : Specification<DB.Models.SWIMFlightLeg>
    {
        public SWIMFlightLegByAirportSpecification(string airportIcao, DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC))
        && ((!string.IsNullOrEmpty(x.DepartureICAO) && x.DepartureICAO == airportIcao) || (!string.IsNullOrEmpty(x.ArrivalICAO) && x.ArrivalICAO == airportIcao)))
        {
        }

        public SWIMFlightLegByAirportSpecification(List<string> airportIcaos, DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC))
            && ((!string.IsNullOrEmpty(x.DepartureICAO) && airportIcaos.Contains(x.DepartureICAO)) || (!string.IsNullOrEmpty(x.ArrivalICAO) && airportIcaos.Contains(x.ArrivalICAO))))
        {
        }
    }
}
