using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO
{
    public class IntegrationUpdatePricingLogDto : BaseEntityModelDTO<DB.Models.IntegrationUpdatePricingLog>, IEntityModelDTO<DB.Models.IntegrationUpdatePricingLog, int>
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int? FboId { get; set; } = 0;
        public DateTime DateTimeRecorded { get; set; }
    }
}
