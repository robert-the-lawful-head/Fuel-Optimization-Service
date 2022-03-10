using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
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
