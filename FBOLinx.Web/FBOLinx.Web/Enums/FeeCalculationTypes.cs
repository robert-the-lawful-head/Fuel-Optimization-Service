using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Enums
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
