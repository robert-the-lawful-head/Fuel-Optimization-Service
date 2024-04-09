using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBOLinx.Core.Enums
{
    public enum AircraftStatusType : short
    {
        [Description("Arrival")]
        Landing = 0,
        [Description("Departure")]
        Takeoff = 1,
        [Description("Parking")]
        Parking = 2,
    }
}
