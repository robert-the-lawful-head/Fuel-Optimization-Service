using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum AccountTypes : short
    {
        [Description("Rev FBO")]
        RevFbo = 0,
        [Description("Non-Rev FBO")]
        NonRevFBO = 1
    }
}
