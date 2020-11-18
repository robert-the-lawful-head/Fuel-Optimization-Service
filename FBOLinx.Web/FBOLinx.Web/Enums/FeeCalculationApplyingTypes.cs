using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Enums
{
    public enum FeeCalculationApplyingTypes : short
    {
        /// <summary>
        /// Above the line/Pre-margin
        /// </summary>
        [Description("Above the line/Pre-margin")]
        PreMargin = 0,
        /// <summary>
        /// Below the line/Post-margin
        /// </summary>
        [Description("Below the line/Post-margin")]
        PostMargin = 1
    }
}
