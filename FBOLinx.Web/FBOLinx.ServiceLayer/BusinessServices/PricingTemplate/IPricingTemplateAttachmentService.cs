using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Dto.Requests;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public interface IPricingTemplateAttachmentService
    {
        Task<FbolinxPricingTemplateFileAttachmentDto> GetFileAttachmentObject(int pricingTemplateId);
        Task<bool> UploadFileAttachment(FbolinxPricingTemplateAttachmentsRequest fbolinxPricingTemplateFileAttachment);
        Task<string> GetFileAttachment(int pricingTemplateId);
        Task<string> GetFileAttachmentName(int pricingTemplateId);
        Task<bool> DeleteFileAttachment(int pricingTemplateId);
    }
}