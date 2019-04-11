using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models
{
    public class EmailContent
    {
        public enum EmailContentTypes : short
        {
            [Description("Not set")]
            NotSet = 0,
            [Description("Greeting")]
            Greeting = 1,
            [Description("Body")]
            Body = 2,
            [Description("Signature")]
            Signature = 3
        }

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("EmailContentHTML")]
        public string EmailContentHtml { get; set; }
        [Column("FBOID")]
        public int? FboId { get; set; }
        public EmailContentTypes EmailContentType { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Subject { get; set; }
    }
}
