using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Models;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO
{
    public class GroupDTO : BaseEntityModelDTO<DB.Models.Group>, IEntityModelDTO<DB.Models.Group, int>
    {
        public int Oid { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        public bool? Isfbonetwork { get; set; }
        public string Domain { get; set; }
        [StringLength(50)]
        public string LoggedInHomePage { get; set; }
        public bool Active { get; set; }
        public bool? IsLegacyAccount { get; set; }

        //public ICollection<Fbos> Fbos { get; set; }
        
        //public ICollection<User> Users { get; set; }
        
        //public ICollection<ContactInfoByGroup> ContactInfoByGroup { get; set; }
    }
}
