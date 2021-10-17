using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FBOLinx.DB.Models
{
    public partial class CustomerContactLog
    {

        public enum UserRoles : short
        {
            [Description("Not set")]
            NotSet = 0,
            [Description("Primary")]
            Primary = 1,
            [Description("Group Admin")]
            GroupAdmin = 2,
            [Description("Conductor")]
            Conductor = 3,
            [Description("Member")]
            Member = 4,
            [Description("CSR")]
            CSR = 5,
            [Description("Non-Rev")]
            NonRev = 6
        }

        public enum Actions : short
        {
            [Description("Deactivated")]
            Deactivated = 0,
            [Description("Contact Added")]
            ContactAdded = 1,
            [Description("Contact Deleted")]
            ContactDeleted = 2,
            [Description("Activated")]
            Activated = 3,
            [Description("Created")]
            Created = 4,
            [Description("Edited")]
            Edited = 5,
            [Description("itp-template Assigned")]
            itetemplateassigned = 6,
            [Description("Aircraft Added")]
            AircraftAdded = 7,
            [Description("Aircraft Deleted")]
            AircraftDeleted = 8

        }

        public enum Locations : short
        {
            [Description("EditCustomer")]
            EditCustomer = 0,
            [Description("CustomerAircarft")]
            CustomerAircarft = 1,
            [Description("Customer")]
            Customer = 2,
        }


        [Key]
        public int OID { get; set; }
        public int userId { get; set; }
        public int Role { get; set; }
        public int newcustomercontactId { get; set; }
        public int oldcustomercontactId { get; set; }
        public DateTime Time { get; set; }
        public Locations Location { get; set; }
        public Actions Action { get; set; }
    }
}
