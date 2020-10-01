using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FBOLinx.Web.Enums
{
    public enum ApplicableTaxFlights : short
    {
        /// <summary>
        /// Never
        /// </summary>
        [Description("Never")]
        Never = 0,
        /// <summary>
        /// Domestic Only
        /// </summary>
        [Description("Domestic Only")]
        DomesticOnly = 1,
        /// <summary>
        /// International Only
        /// </summary>
        [Description("International Only")]
        InternationalOnly = 2,
        /// <summary>
        /// All
        /// </summary>
        [Description("All")]
        All = 3
    }
}
