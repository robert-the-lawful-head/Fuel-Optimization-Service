using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum FuelProductPriceTypes
    {
        [Description("JetA Cost")]
        FuelJetACost = 0,
        [Description("JetA Retail")]
        FuelJetARetail = 1,
        [Description("100LL Cost")]
        Fuel100LLCost = 2,
        [Description("100LL Retail")]
        Fuel100LRetail = 3,
        [Description("SAF Cost")]
        FuelSAFCost = 4,
        [Description("SAF Retail")]
        FuelSAFRetail = 5
    }
}
