using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("CustomersViewedByFBO")]
    public partial class CustomersViewedByFbo
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ViewDate { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
    }
}
