using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class Group
    {
        public Group()
        {
            Fbos = new HashSet<Fbos>();
        }

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
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
