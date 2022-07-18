using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class SWIMFlightLegSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegSpecification(IList<string> departureICAOs, IList<string> arrivalICAOs, DateTime atdMin, DateTime atdMax)
            : base(x => departureICAOs.Contains(x.DepartureICAO) && arrivalICAOs.Contains(x.ArrivalICAO) && x.ATD >= atdMin && x.ATD <= atdMax)
        {
            AddInclude(x => x.SWIMFlightLegDataMessages);
        }

        public SWIMFlightLegSpecification(string departureICAO, string arrivalICAO, DateTime currentTime)
            : base(x => ((departureICAO != null && departureICAO == x.DepartureICAO) || (arrivalICAO != null && arrivalICAO == x.ArrivalICAO)) && x.ETA > currentTime)
        {
            AddInclude(x => x.SWIMFlightLegDataMessages);
        }
    }
}
