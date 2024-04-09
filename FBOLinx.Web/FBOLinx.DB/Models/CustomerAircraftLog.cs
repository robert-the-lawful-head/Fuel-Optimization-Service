using FBOLinx.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class CustomerAircraftLog
    {
        [Key]
        [Column("OID")]
        public int OID { get; set; }
        public int userId { get; set; }
        public UserRoles Role { get; set; }
        public int newcustomeraircraftId { get; set; }
        public int oldcustomeraircraftId { get; set; }
        public DateTime Time { get; set; }
        public Locations Location { get; set; }
        public Actions Action { get; set; }
        public int customerId { get; set; }
    }
}
