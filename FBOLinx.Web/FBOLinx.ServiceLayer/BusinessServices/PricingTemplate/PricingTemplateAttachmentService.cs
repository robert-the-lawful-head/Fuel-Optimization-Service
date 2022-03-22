using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Dto.Requests;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.PricingTemplate
{
    public class PricingTemplateAttachmentService : IPricingTemplateAttachmentService
    {        
        private IFbolinxPricingTemplateAttachmentsEntityService _fbolinxPricingTemplateAttachmentsEntityService;
        public PricingTemplateAttachmentService(
            IFbolinxPricingTemplateAttachmentsEntityService fbolinxPricingTemplateAttachmentsEntityService)
        {
            _fbolinxPricingTemplateAttachmentsEntityService = fbolinxPricingTemplateAttachmentsEntityService;
        }
        public async Task<string> GetFileAttachment(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            if (pricingTemplateFile != null)
            {
                var fileBase64 = Convert.ToBase64String(pricingTemplateFile.FileData, 0, pricingTemplateFile.FileData.Length);
                return "data:" + pricingTemplateFile.ContentType + ";base64," + fileBase64;
            }

            return "";
        }

        public async Task<FbolinxPricingTemplateFileAttachmentDto> GetFileAttachmentObject(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();
            if(pricingTemplateFile == null) return null;
            return pricingTemplateFile.Map<FbolinxPricingTemplateFileAttachmentDto>();
        }

        public async Task<string> GetFileAttachmentName(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();

            if (pricingTemplateFile == null || pricingTemplateFile.Oid == 0)
                return "";
            
            return pricingTemplateFile.FileName;
        }

        public async Task<bool> UploadFileAttachment(FbolinxPricingTemplateAttachmentsRequest request)
        {
            var fileAsArray = Convert.FromBase64String(request.FileData);

            var existingRecord = await _fbolinxPricingTemplateAttachmentsEntityService.Where(f => f.PricingTemplateId == request.PricingTemplateId).FirstOrDefaultAsync();

            if (existingRecord != null && existingRecord.Oid > 0)
            {
                existingRecord.FileData = fileAsArray;
                existingRecord.FileName = request.FileName;
                existingRecord.ContentType = request.ContentType;
                await _fbolinxPricingTemplateAttachmentsEntityService.UpdateAsync(existingRecord);
            }
            else
            {
                FbolinxPricingTemplateFileAttachment fboLinxFileData = new FbolinxPricingTemplateFileAttachment();
                fboLinxFileData.FileData = fileAsArray;
                fboLinxFileData.FileName = request.FileName;
                fboLinxFileData.ContentType = request.ContentType;
                fboLinxFileData.PricingTemplateId = request.PricingTemplateId;
                await _fbolinxPricingTemplateAttachmentsEntityService.AddAsync(fboLinxFileData);
            }
            return true;
        }

        public async Task<bool> DeleteFileAttachment(int pricingTemplateId)
        {
            var pricingTemplateFile = await _fbolinxPricingTemplateAttachmentsEntityService.Where(p => p.PricingTemplateId == pricingTemplateId).FirstOrDefaultAsync();
            var result = await _fbolinxPricingTemplateAttachmentsEntityService.DeleteAsync(pricingTemplateFile.Oid);

            if (result == null) return false;

            return true;
        }
    }
}