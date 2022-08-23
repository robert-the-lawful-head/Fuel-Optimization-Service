using System.ComponentModel;

namespace FBOLinx.Core.Enums
{

    public enum AirportTypeEnum : short
    {
        [Description("Joint Civil / Military")]
        JointCivilMilitary = 0,
        [Description("Military")]
        Military = 1,
        [Description("Civil")]
        Civil = 2,
    }
    public enum FuelTypeEnum : short
    {
        [Description("AVGAS")]
        Avgas = 0,
        [Description("NO FUEL")]
        NoFuel = 1,
        [Description("AVGAS ONLY")]
        AvgasOnly = 2,
        [Description("Unknown")]
        Unknown = 3,
        [Description("JET ONLY")]
        JetOnly = 4,
        [Description("JET")]
        Jet = 5,
        [Description("AVGAS JET")]
        AvgasJet = 5,
    }
}
