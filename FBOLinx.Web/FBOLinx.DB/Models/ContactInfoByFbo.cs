using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
    public partial class ContactInfoByFbo
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public int? ContactId { get; set; }
        public int? FboId { get; set; }

        public bool? CopyAlerts { get; set; }
    }
}
