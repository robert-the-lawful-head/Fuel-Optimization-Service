using FBOLinx.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class EmailContent
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [Column("EmailContentHTML")]
        public string EmailContentHtml { get; set; }
        [Column("FBOID")]
        public int? FboId { get; set; }
        public int? GroupId { get; set; }
        public string FromAddress { get; set; }
        public string ReplyTo{ get; set; }
        public EmailContentTypes EmailContentType { get; set; }

        [NotMapped]
        public string EmailContentTypeDescription
        {
            get { return Core.Utilities.Enum.GetDescription(EmailContentType); }
        }

        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Subject { get; set; }
    }
}
