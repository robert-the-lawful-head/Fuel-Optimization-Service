using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class FbolinxPricingTemplateFileAttachment
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public byte[] FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public int PricingTemplateId { get; set; }
    }
}
