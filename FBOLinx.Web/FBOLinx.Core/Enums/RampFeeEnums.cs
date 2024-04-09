using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum RampFeeCategories : short
    {
        [Description("Not Set")]
        Notset = 0,
        [Description("Aircraft Size")]
        AircraftSize = 1,
        [Description("Aircraft Type")]
        AircraftType = 2,
        [Description("Weight Range")]
        WeightRange = 3,
        [Description("Wingspan")]
        Wingspan = 4,
        [Description("Tailnumber")]
        TailNumber = 5
    }
}
