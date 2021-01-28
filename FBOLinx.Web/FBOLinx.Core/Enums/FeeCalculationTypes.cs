using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum FeeCalculationTypes : short
    {
        /// <summary>
        /// Flat Per Gallon
        /// </summary>
        [Description("Flat Per Gallon")]
        FlatPerGallon = 0,
        /// <summary>
        /// Percentage (%)
        /// </summary>
        [Description("Percentage of Base")]
        Percentage = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("Percentage of Total")]
        PercentageOfTotal = 2
    }
}
