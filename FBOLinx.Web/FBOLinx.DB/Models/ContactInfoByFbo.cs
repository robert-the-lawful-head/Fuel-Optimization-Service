using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FBOLinx.DB.Models
{
    public class ContactInfoByFbo
    {
        [Key]
        public int Oid { get; set; }
        public int? ContactId { get; set; }
        public int? Fboid { get; set; }

        public bool? CopyAlerts { get; set; }
    }
}
