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
    public class SWIMFLightLegByDepartureAirportDatesSpecification : Specification<DB.Models.SWIMFlightLeg>
    {
        public SWIMFLightLegByDepartureAirportDatesSpecification(string airportIcao, DateTime minDepartureDateUTC, DateTime maxDepartureDateUTC) : base(x => ((x.ATD >= minDepartureDateUTC) && (x.ATD <= maxDepartureDateUTC))
        && ((!string.IsNullOrEmpty(x.DepartureICAO) && x.DepartureICAO == airportIcao)))
        {
        }
    }
}
