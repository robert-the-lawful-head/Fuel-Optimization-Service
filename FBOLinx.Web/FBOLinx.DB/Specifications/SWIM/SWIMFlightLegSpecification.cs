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
            //AddInclude(x => x.SWIMFlightLegDataMessages);
        }

        public SWIMFlightLegSpecification(string departureICAO, string arrivalICAO, DateTime startETADate)
            : base(x => ((departureICAO != null && departureICAO == x.DepartureICAO) || (arrivalICAO != null && arrivalICAO == x.ArrivalICAO)) && x.ETA > startETADate)
        {
            //AddInclude(x => x.SWIMFlightLegDataMessages);
        }

        public SWIMFlightLegSpecification(string departureICAO, string arrivalICAO, DateTime startDate, DateTime endDate)
            : base(x => ((departureICAO != null && departureICAO == x.DepartureICAO) || (arrivalICAO != null && arrivalICAO == x.ArrivalICAO)) && x.ETA >= startDate && x.ETA <= endDate)
        {
        }

        public SWIMFlightLegSpecification(IList<string> tailNumbers, IList<string> atcFlightNumbers, DateTime minATD)
            : base(x => (tailNumbers.Contains(x.AircraftIdentification) || atcFlightNumbers.Contains(x.AircraftIdentification)) && x.ATD > minATD)
        {
        }

        public SWIMFlightLegSpecification(IList<string> tailNumbers, DateTime minATD, bool isPlaceholder)
            : base(x => tailNumbers.Contains(x.AircraftIdentification) && x.ATD > minATD && x.IsPlaceholder == isPlaceholder)
        {
        }

        public SWIMFlightLegSpecification(string departureIcao, DateTime minATD, bool isPlaceholder)
            : base(x => x.DepartureICAO == departureIcao &&  x.ATD > minATD && x.IsPlaceholder == isPlaceholder)
        {
        }

        public SWIMFlightLegSpecification(DateTime startETADate)
            : base(x => x.ETA > startETADate)
        {
        }
    }
}
