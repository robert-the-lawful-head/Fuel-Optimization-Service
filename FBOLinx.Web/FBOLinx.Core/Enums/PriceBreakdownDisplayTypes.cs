using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Enums
{
    public enum PriceBreakdownDisplayTypes : short
    {
        SingleColumnAllFlights = 0,
        TwoColumnsDomesticInternationalOnly = 1,
        TwoColumnsApplicableFlightTypesOnly = 2,
        FourColumnsAllRules = 3
    }
}
