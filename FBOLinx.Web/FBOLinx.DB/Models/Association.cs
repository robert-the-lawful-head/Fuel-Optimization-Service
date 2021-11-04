using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
    public partial class Associations
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; } = 0;

        [Column("Association")]
        [StringLength(50)]
        public string AssociationName { get; set; }
    }
}
