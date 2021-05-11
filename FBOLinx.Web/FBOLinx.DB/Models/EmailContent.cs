using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
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

        [NotMapped]
        public string EmailContentTypeDescription
        {
            get { return FBOLinx.Core.Utilities.Enum.GetDescription(EmailContentType); }
        }

        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Subject { get; set; }
    }
}
