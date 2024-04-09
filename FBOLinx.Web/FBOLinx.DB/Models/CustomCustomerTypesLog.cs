using FBOLinx.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models
{
  public partial class CustomCustomerTypesLog
    {
        [Key]
        public int OID { get; set; }
        public int userId { get; set; }
        public UserRoles Role { get; set; }
        public int newcustomertypetId { get; set; }
        public int oldcustomertypeId { get; set; }
        public DateTime Time { get; set; }
        public Locations Location { get; set; }
        public Actions Action { get; set; }
        public int customerId { get; set; }
    }
}
