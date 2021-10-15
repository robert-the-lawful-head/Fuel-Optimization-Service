using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
    public class CustomerTag
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public int GroupId { get; set; }
        public int? CustomerId { get; set; }

        public string Name { get; set; }
    }
}
