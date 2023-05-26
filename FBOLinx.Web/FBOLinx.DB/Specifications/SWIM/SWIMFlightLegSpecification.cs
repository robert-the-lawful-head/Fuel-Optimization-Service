using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class SWIMFlightLegSpecification : Specification<SWIMFlightLeg>
    {
        public SWIMFlightLegSpecification(int swimFlightLegId) : base(x => x.Oid == swimFlightLegId)
        {
            AddInclude(x => x.SWIMFlightLegDataMessages);
        }

        public SWIMFlightLegSpecification(string departureICAO, string arrivalICAO, DateTime startETADate, List<FlightLegStatus> statusesToExclude)
            : base(x => ((departureICAO != null && departureICAO == x.DepartureICAO) || (arrivalICAO != null && arrivalICAO == x.ArrivalICAO)) &&
                        x.ETA != null && x.ETA > startETADate && x.Status != null && !statusesToExclude.Contains(x.Status.Value))
        {
            //AddInclude(x => x.SWIMFlightLegDataMessages);
        }

        public SWIMFlightLegSpecification(string departureIcao, DateTime minATD, bool isPlaceholder)
            : base(x => x.DepartureICAO == departureIcao &&  x.ATD > minATD && x.IsPlaceholder == isPlaceholder)
        {
        }
    }
}
