using System;
using FBOLinx.Core.Enums;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerAircraftLogDto
    {
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