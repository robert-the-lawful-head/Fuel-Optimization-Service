using System.ComponentModel;

namespace FBOLinx.Core.Enums
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
