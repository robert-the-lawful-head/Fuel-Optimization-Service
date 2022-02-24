using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models.Requests;
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
        public Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int groupId);
        public Task UploadFileAttachment(FbolinxPricingTemplateAttachmentsRequest fbolinxPricingTemplateFileAttachment);
        public Task<string> GetFileAttachment(int pricingTemplateId);
        public Task<FbolinxPricingTemplateFileAttachment> GetFileAttachmentObject(int pricingTemplateId);
        public Task<string> GetFileAttachmentName(int pricingTemplateId);
        public Task DeleteFileAttachment(int pricingTemplateId);
    }
}