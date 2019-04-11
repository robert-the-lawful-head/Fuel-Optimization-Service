using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class ContactInfoByGroup
    {
        [Column("GroupID")]
        public int GroupId { get; set; }
        [Column("ContactID")]
        public int ContactId { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(100)]
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
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [InverseProperty("ContactInfoByGroups")]
        [ForeignKey("ContactId")]
        public Contacts Contact { get; set; }

        [InverseProperty("ContactInfoByGroup")]
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
    }
}
