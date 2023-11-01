using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Enums
{
    public enum AcukwikAirportTypes : short
    {
        [Description("")]
        NotSpecified = 0,
        [Description("Heliport / Vertiport")]
        Heliport_Vertiport = 1,
        [Description("Seaplane Base")]
        SeaplaneBase = 2,
        [Description("Joint Civil / Military")]
        JointCivil_Military = 3,
        [Description("Military")]
        Military = 4,
        [Description("Civil")]
        Civil = 5,
        [Description("Gliderport")]
        Gliderport = 6
    }
}
