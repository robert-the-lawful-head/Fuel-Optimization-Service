using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
  public partial class CustomCustomerTypesLogData
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("FBOID")]
        public int Fboid { get; set; }
        public int CustomerType { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
    }
}
