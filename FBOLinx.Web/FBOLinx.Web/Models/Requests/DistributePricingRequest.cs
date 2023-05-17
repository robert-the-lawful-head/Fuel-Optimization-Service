using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.Web.Models.Requests
{
    public class DistributePricingRequest
    {
        public PricingTemplate PricingTemplate { get; set; }
        public CustomerInfoByGroupDto Customer { get; set; }
        public int CustomerCompanyType { get; set; }
        public int FboId { get; set; }
        public int GroupId { get; set; }
        public string PreviewEmail { get; set; }
    }
}
