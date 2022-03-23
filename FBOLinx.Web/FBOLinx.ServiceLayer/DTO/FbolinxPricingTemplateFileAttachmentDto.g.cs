using System;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbolinxPricingTemplateFileAttachmentDto
    {
        public int Oid { get; set; }
        public byte[] FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public int PricingTemplateId { get; set; }
    }
}