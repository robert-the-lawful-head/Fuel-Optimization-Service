using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class CustomerCompanyTypes
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        [Column("GroupID")]
        public int GroupId { get; set; }
        public bool? AllowMultiplePricingTemplates { get; set; }
    }
}
