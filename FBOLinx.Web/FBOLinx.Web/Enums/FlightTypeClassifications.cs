using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Enums
{
    /// <summary>
    /// NotSet = 0,
    /// Private = 1,
    /// Commercial = 2
    /// </summary>
    public enum FlightTypeClassifications : short
    {
        /// <summary>
        /// Not Set
        /// </summary>
        [Description("Not Set")]
        NotSet = 0,
        /// <summary>
        /// Private
        /// </summary>
        [Description("Private")]
        Private = 1,
        /// <summary>
        /// Commercial
        /// </summary>
        [Description("Commercial")]
        Commercial = 2,
        /// <summary>
        /// All
        /// </summary>
        [Description("All")]
        All = 3
    }
}