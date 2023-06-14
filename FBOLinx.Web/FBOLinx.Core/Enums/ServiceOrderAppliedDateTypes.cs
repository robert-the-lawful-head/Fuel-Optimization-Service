using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Enums
{
    public enum ServiceOrderAppliedDateTypes : short
    {
        [Description("Arrival")]
        Arrival = 0,
        [Description("Departure")]
        Departure = 1,
    }
}
