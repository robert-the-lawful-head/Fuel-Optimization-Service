using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Dto.Requests;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public interface IPricingTemplateService: IPricingTemplateGridService
    {
        Task FixCustomCustomerTypes(int groupId, int fboId);
        Task FixDefaultPricingTemplate(int fboId);
        Task<List<DB.Models.PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0);
        Task UpdatePricingTemplate(int id, PricingTemplateDto pricingTemplate);
        Task<bool> PutPricingTemplate(int id, PricingTemplateDto pricingTemplate);
        Task<PricingTemplateDto> PostPricingTemplate(PricingTemplateDto pricingTemplate);
        Task<PricingTemplateDto> CopyPricingTemplate(PrincingTemplateRequest pricingTemplate);
        Task<PricingTemplateDto> GetDefaultTemplate(int fboId);
        Task<PricingTemplateDto> GetDefaultTemplateIncludeNullCheck(int fboId);
        Task<PricingTemplateDto> DeletePricingTemplate(PricingTemplateDto pricingTemplate, int oid, int fboId);
        Task<PricingTemplateDto> GetPricingTemplateById(int oid);

    }
}