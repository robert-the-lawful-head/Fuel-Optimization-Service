using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    public partial class CustomCustomerTypes
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        public int CustomerType { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [InverseProperty("CustomCustomerType")]
        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }
    }
}
