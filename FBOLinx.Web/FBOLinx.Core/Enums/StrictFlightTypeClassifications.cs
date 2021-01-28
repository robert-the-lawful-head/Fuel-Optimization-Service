using System.ComponentModel;

namespace FBOLinx.Core.Enums
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