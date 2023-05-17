using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum DocumentAcceptanceFlag : short
    {
        [Description("Force Accepted")]
        ForceAccepted = 0,
        [Description("Accepted")]
        Accepted = 1,
        [Description("Not Accepted Required")]
        NotAcceptedRequired = 2,
    }
    public enum DocumentTypeEnum : short
    {
        [Description("EULA")]
        EULA = 0,
        [Description("Cookie Policy")]
        Cookie = 1,
        [Description("Privacy Policy")]
        Privacy = 2,
    }
}
