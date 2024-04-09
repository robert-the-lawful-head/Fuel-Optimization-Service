using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum EmailContentTypes : short
    {
        [Description("Not set")]
        NotSet = 0,
        [Description("Greeting")]
        Greeting = 1,
        [Description("Body")]
        Body = 2,
        [Description("Signature")]
        Signature = 3
    }
}
