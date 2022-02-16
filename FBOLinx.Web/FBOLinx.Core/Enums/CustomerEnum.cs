using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum CustomerSources
    {
        [Description("Not Specified")]
        NotSpecified = 0,
        [Description("FuelerLinx")]
        FuelerLinx = 1,
        [Description("Base Tenant")]
        BaseTenant = 2,
        [Description("Transient")]
        Transient = 3,
        [Description("Contract Fuel Vendor")]
        ContractFuelVendor = 4
    }
}
