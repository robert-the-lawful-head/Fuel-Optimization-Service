using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum IntegrationPartnerTypes : short
    {
        /// <summary>
        /// Not Specified
        /// </summary>
        [Description("Not Specified")]
        NotSpecified = 0,
        /// <summary>
        /// Other Software
        /// </summary>
        [Description("Other Software")]
        OtherSoftware = 1,
        /// <summary>
        /// Fuel Vendor
        /// </summary>
        [Description("Fuel Vendor")]
        FuelVendor = 2,
        /// <summary>
        /// Scheduling System
        /// </summary>
        [Description("Scheduling System")]
        SchedulingSystem = 3,
        /// <summary>
        /// Flight Planning
        /// </summary>
        [Description("Flight Planning")]
        FlightPlanning = 4,
        /// <summary>
        /// Flight Department
        /// </summary>
        [Description("Flight Department")]
        FlightDepartment = 5,
        /// <summary>
        /// Accounting
        /// </summary>
        [Description("Accounting")]
        Accounting = 6,
        /// <summary>
        /// Internal
        /// </summary>
        [Description("Internal")]
        Internal = 7
    }

    public enum TrustLevels : short
    {
        /// <summary>
        /// Low
        /// </summary>
        [Description("Low")]
        Low = 0,
        /// <summary>
        /// Medium
        /// </summary>
        [Description("Medium")]
        Medium = 1,
        /// <summary>
        /// High
        /// </summary>
        [Description("High")]
        High = 2,
        /// <summary>
        /// Full Trust
        /// </summary>
        [Description("Full Trust")]
        Full = 3
    }
}
