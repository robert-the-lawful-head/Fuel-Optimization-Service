using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBOLinx.Core.Enums
{
    public enum CertificateTypes : short
    {
        [Description("Not set")]
        NotSet = 0,
        [Description("Part 91")]
        Part91 = 91,
        [Description("Part 121")]
        Part121 = 121,
        [Description("Part 135")]
        Part135 = 135
    }
}
