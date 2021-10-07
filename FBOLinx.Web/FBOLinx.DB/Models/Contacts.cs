using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class Contacts
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Mobile { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        public bool? Primary { get; set; }
        public bool? CopyAlerts { get; set; }
        [StringLength(50)]
        public string Extension { get; set; }
        public bool? CopyOrders { get; set; }

        [InverseProperty("Contact")]
        public Fbocontacts FboContact { get; set; }

        [InverseProperty("Contact")]
        public ICollection<CustomerContacts> CustomerContacts { get; set; }

        [InverseProperty("Contact")]
        public ICollection<ContactInfoByGroup> ContactInfoByGroups { get; set; }
    }
}