using System.ComponentModel;

namespace FBOLinx.Core.Enums
{
    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }

    public enum AuditLocation
    {
        CustomerInfoByGroup = 0,
        CustomerAircrafts = 1,
        CustomCustomerTypes = 2,
        ContactInfoByGroup = 3,
        CustomerContacts = 4
    }
    public enum AuditEntryType
    {
        [Description("Deactivate")]
        Deactivated = 0,
        [Description("Activated")]
        Acitviated = 1,
        [Description("Contact Added")]
        ContactAdded = 2,
        [Description("Contact Deleted")]
        ContactDeleted = 3,
        [Description("Created")]
        Created = 4,
        [Description("Edited")]
        Edited = 5,
        [Description("Itp Template Assigned")]
        ItpTemplateAssigned = 6,
        [Description("Aircaft Added")]
        AircaftAdded = 7,
        [Description("Aircraft Deleted")]
        AircraftDeleted = 8,
        [Description("Aircraft Edited")]
        AircraftEdited = 9
    }
}
