using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
  public partial  class CustomerContactLogData
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        
        [Column("ContactID")]
        public int ContactId { get; set; }
        
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

    }
}
