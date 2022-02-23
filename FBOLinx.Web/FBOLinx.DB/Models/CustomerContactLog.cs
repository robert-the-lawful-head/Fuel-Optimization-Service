using FBOLinx.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models
{
    public partial class CustomerContactLog
    {
        [Key]
        public int OID { get; set; }
        public int userId { get; set; }
        public UserRoles Role { get; set; }
        public int newcustomercontactId { get; set; }
        public int oldcustomercontactId { get; set; }
        public DateTime Time { get; set; }
        public Locations Location { get; set; }
        public Actions Action { get; set; }

        public Int32 customerId { get; set; }
    }
}
