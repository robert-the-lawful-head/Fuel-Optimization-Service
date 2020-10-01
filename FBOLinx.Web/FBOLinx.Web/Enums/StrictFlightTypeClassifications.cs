using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FBOLinx.Web.Enums
{
    public enum StrictFlightTypeClassifications : short
    {
        /// <summary>
        /// Private
        /// </summary>
        [Description("Private")]
        Private = 1,
        /// <summary>
        /// Commercial
        /// </summary>
        [Description("Commercial")]
        Commercial = 2
    }
}