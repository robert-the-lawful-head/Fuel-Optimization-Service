using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum MarginTypes : short
    {
        [Description("Cost +")]
        CostPlus = 0,
        [Description("Retail -")]
        RetailMinus = 1,
        [Description("Flat Fee")]
        FlatFee = 2,
        [Description("Inactive")]
        Inactive = 3
    }


    public enum DiscountTypes : short
    {
        [Description("Flat Per Gallon")]
        FlatPerGallon = 0,
        [Description("Percentage")]
        Percentage = 1
    }
}
