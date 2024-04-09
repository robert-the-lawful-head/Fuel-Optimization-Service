using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.Dto.Requests
{
    public class FbolinxPricingTemplateAttachmentsRequest
    {
        public int Oid { get; set; }
        public string FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public int PricingTemplateId { get; set; }
    }
}
