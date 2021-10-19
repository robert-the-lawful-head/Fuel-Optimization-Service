using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Web.DTO;
using FBOLinx.Web.ViewModels;

namespace FBOLinx.Web.Services.Interfaces
{
    public interface IPricingTemplateService
    {
        public Task FixCustomCustomerTypes(int groupId, int fboId);

        public Task FixDefaultPricingTemplate(int fboId);

        public Task<List<PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0);

        public Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0);

        public Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0);

        Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int groupId);
    }
}