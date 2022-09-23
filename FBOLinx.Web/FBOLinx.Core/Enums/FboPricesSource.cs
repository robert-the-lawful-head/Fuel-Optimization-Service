using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum FboPricesSource : short
    {
        [Description("FBOLinx")]
        FboLinx = 0,
        [Description("X-1")]
        X1 = 1
    }
}
