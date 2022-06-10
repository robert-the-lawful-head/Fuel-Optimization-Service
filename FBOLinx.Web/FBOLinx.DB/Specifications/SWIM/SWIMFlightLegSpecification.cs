using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.SWIM
{
    public sealed class SWIMFlightLegSpecification : Specification<SWIMFlightLegs>
    {
        public SWIMFlightLegSpecification(string aircraftIdentification, string departureICAO, string arrivalICAO, DateTime atd)
            : base(x => x.AircraftIdentification == aircraftIdentification && x.DepartureICAO == departureICAO && x.ArrivalICAO == arrivalICAO && x.ATD == atd)
        {
            AddInclude(x => x.SWIMFlightLegDataMessages);
        }

        public SWIMFlightLegSpecification(IList<string> aircraftIdentificationNumbers, DateTime atdMin, DateTime atdMax)
            : base(x => aircraftIdentificationNumbers.Contains(x.AircraftIdentification) && x.ATD >= atdMin && x.ATD <= atdMax)
        {
            AddInclude(x => x.SWIMFlightLegDataMessages);
        }
    }
}
