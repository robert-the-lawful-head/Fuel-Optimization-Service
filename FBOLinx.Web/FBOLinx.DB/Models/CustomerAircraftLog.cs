using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FBOLinx.DB.Models
{
    public partial class CustomerAircraftLog
    {
       


        [Key]
        public int OID { get; set; }
        public int userId { get; set; }
        public CustomerInfoByGroupLog.UserRoles Role { get; set; }
        public int newcustomeraircraftId { get; set; }
        public int oldcustomeraircraftId { get; set; }
        public DateTime Time { get; set; }
        public CustomerInfoByGroupLog.Locations Location { get; set; }
        public CustomerInfoByGroupLog.Actions Action { get; set; }
        public int customerId { get; set; }
    }
}
