using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.Dto.Responses;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public interface IPricingTemplateGridService
    {
        Task<List<PricingTemplateGrid>> GetPricingTemplates(int fboId, int groupId);
        Task<List<PricingTemplateGrid>> GetTemplatesWithEmailContent(int fboId, int groupId);
        List<PricingTemplateGrid> GetCostPlusPricingTemplates(int fboId);
        Task<List<PricingTemplateGrid>> GetDefaultPricingTemplateByFboId(int fboId);
    }
}