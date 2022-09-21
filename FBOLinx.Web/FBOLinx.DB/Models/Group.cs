using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class Group : FBOLinxBaseEntityModel<int>
    {
        public Group()
        {
            Fbos = new HashSet<Fbos>();
        }

        [Column("Group")]
        [StringLength(50)]
        public string GroupName { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [Column("ISFBONetwork")]
        public bool? Isfbonetwork { get; set; }
        public string Domain { get; set; }
        [StringLength(50)]
        public string LoggedInHomePage { get; set; }
        public bool Active { get; set; }
        public bool? IsLegacyAccount { get; set; }

        [InverseProperty("Group")]
        public ICollection<Fbos> Fbos { get; set; }

        [InverseProperty("Group")]
        public ICollection<User> Users { get; set; }

        [InverseProperty("Group")]
        public ICollection<ContactInfoByGroup> ContactInfoByGroup { get; set; }
    }
}
