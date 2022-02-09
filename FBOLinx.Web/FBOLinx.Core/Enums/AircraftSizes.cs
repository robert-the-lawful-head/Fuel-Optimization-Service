using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBOLinx.Core.Enums
{
    public enum AircraftSizes : short
    {
        [Description("Not Set")]
        NotSet = 0,
        [Description("Light Jet")]
        LightJet = 1,
        [Description("Medium Jet")]
        MediumJet = 2,
        [Description("Heavy Jet")]
        HeavyJet = 3,
        [Description("Light Helicopter")]
        LightHelicopter = 4,
        [Description("Wide Body")]
        WideBody = 5,
        [Description("Single Turboprop")]
        SingleTurboProp = 6,
        [Description("Very Light Jet")]
        VeryLightJet = 7,
        [Description("Single Engine Piston")]
        SingleEnginePiston = 8,
        [Description("Medium Helicopter")]
        MediumHelicopter = 9,
        [Description("Heavy Helicopter")]
        HeavyHelicopter = 10,
        [Description("Light Twin")]
        LightTwin = 11,
        [Description("Heavy Twin")]
        HeavyTwin = 12,
        [Description("Light Turboprop")]
        LightTurboProp = 13,
        [Description("Medium Turboprop")]
        MediumTurboprop = 14,
        [Description("Heavy Turboprop")]
        HeavyTurboprop = 15,
        [Description("Super Heavy Jet")]
        SuperHeavyJet = 16
    }
}
