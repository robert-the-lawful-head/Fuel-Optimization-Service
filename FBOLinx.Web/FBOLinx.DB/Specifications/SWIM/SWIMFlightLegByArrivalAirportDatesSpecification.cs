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
    public class SWIMFlightLegByArrivalAirportDatesSpecification : Specification<DB.Models.SWIMFlightLeg>
    {
        public SWIMFlightLegByArrivalAirportDatesSpecification(string airportIcao, DateTime minArrivalDateUTC, DateTime maxArrivalDateUTC) : base(x => x.IsPlaceholder == false && ((x.ETA >= minArrivalDateUTC) && (x.ETA <= maxArrivalDateUTC))
        && (x.ArrivalICAO != null && x.ArrivalICAO == airportIcao))
        {
        }
    }
}
