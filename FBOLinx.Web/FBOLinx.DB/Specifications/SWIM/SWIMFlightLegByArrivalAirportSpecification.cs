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
    public class SWIMFlightLegByArrivalAirportSpecification : Specification<DB.Models.SWIMFlightLeg>
    {
        public SWIMFlightLegByArrivalAirportSpecification(string airportIcao, DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC))
        && ((!string.IsNullOrEmpty(x.ArrivalICAO) && x.ArrivalICAO == airportIcao)))
        {
        }

        public SWIMFlightLegByArrivalAirportSpecification(List<string> airportIcaos, DateTime minArrivalOrDepartureDateUTC) : base(x => ((x.ATD > minArrivalOrDepartureDateUTC) || (x.ETA.HasValue && x.ETA.Value > minArrivalOrDepartureDateUTC))
            && ((!string.IsNullOrEmpty(x.ArrivalICAO) && airportIcaos.Contains(x.ArrivalICAO))))
        {
        }
    }
}
