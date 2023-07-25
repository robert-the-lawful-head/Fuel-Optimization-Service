using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Services
{
    public interface IEmailContentService
    {
        Task<List<EmailContent>> GetEmailContentsForFbo(int fboId);
        Task<EmailContent> AddDefaultEmailContentForFbo(int fboId);
        Task<string> GetFileAttachment(int contentTemplateId);
        Task<FbolinxEmailContentFileAttachment> GetFileAttachmentObject(int contentTemplateId);
        Task<string> GetFileAttachmentName(int contentTemplateId);
        Task UploadFileAttachment(FbolinxEmailContentAttachmentsRequest request);
        Task DeleteFileAttachment(int emailContentId);
    }
    public class EmailContentService: IEmailContentService
    {
        private FboLinxContext _context;
        private readonly FilestorageContext _fileStorageContext;

        public EmailContentService(FboLinxContext context, FilestorageContext filestorageContext)
        {
            _context = context;
            _fileStorageContext = filestorageContext;
        }

        public async Task<List<EmailContent>> GetEmailContentsForFbo(int fboId)
        {
            var emailContent = await _context.EmailContent.Where((x => x.FboId == fboId)).OrderBy(x => x.Name).ToListAsync();

            if (emailContent == null || emailContent.Count == 0)
            {
                await AddDefaultEmailContentForFbo(fboId);
                emailContent = await _context.EmailContent.Where((x => x.FboId == fboId)).OrderBy(x => x.Name).ToListAsync();
            }

            return emailContent;
        }

        public async Task<EmailContent> AddDefaultEmailContentForFbo(int fboId)
        {
            var fbo = await _context.Fbos.Where(x => x.Oid == fboId).Include(x => x.FboAirport).FirstOrDefaultAsync();
            if (fbo == null)
                return null;
            var emailContent = new EmailContent()
                {
                    EmailContentHtml = "Greeting from " + fbo.Fbo + " / " + fbo.FboAirport?.Icao + ". Attached please see your custom pricing from our FBO!",
                    EmailContentType = EmailContentTypes.Body,
                    FboId = fboId,
                    Name = "Default Email Template",
                    Subject = "Pricing from " + fbo.Fbo + " / " + fbo.FboAirport?.Icao
                };
            await _context.EmailContent.AddAsync(emailContent);
            await _context.SaveChangesAsync();
            return emailContent;
        }

        public async Task<string> GetFileAttachment(int contentTemplateId)
        {
            var emailContentFile = await _fileStorageContext.FbolinxEmailContentAttachments.Where(p => p.EmailContentId == contentTemplateId).FirstOrDefaultAsync();

            var fileBase64 = Convert.ToBase64String(emailContentFile.FileData, 0, emailContentFile.FileData.Length);
            return "data:" + emailContentFile.ContentType + ";base64," + fileBase64;
        }

        public async Task<FbolinxEmailContentFileAttachment> GetFileAttachmentObject(int contentTemplateId)
        {
            var emailContentFile = await _fileStorageContext.FbolinxEmailContentAttachments.Where(p => p.EmailContentId == contentTemplateId).FirstOrDefaultAsync();
            return emailContentFile;
        }

        public async Task<string> GetFileAttachmentName(int contentTemplateId)
        {
            var emailContentFile = await _fileStorageContext.FbolinxEmailContentAttachments.Where(p => p.EmailContentId == contentTemplateId).FirstOrDefaultAsync();

            if (emailContentFile == null || emailContentFile.Oid == 0)
                return "";

            return emailContentFile.FileName;
        }

        public async Task UploadFileAttachment(FbolinxEmailContentAttachmentsRequest request)
        {
            var fileAsArray = Convert.FromBase64String(request.FileData);

            var existingRecord = await _fileStorageContext.FbolinxEmailContentAttachments.Where(f => f.EmailContentId == request.EmailContentId).FirstOrDefaultAsync();

            if (existingRecord != null && existingRecord.Oid > 0)
            {
                existingRecord.FileData = fileAsArray;
                existingRecord.FileName = request.FileName;
                existingRecord.ContentType = request.ContentType;
                _fileStorageContext.FbolinxEmailContentAttachments.Update(existingRecord);
            }
            else
            {
                FBOLinx.DB.Models.FbolinxEmailContentFileAttachment fboLinxFileData = new DB.Models.FbolinxEmailContentFileAttachment();
                fboLinxFileData.FileData = fileAsArray;
                fboLinxFileData.FileName = request.FileName;
                fboLinxFileData.ContentType = request.ContentType;
                fboLinxFileData.EmailContentId = request.EmailContentId;
                _fileStorageContext.FbolinxEmailContentAttachments.Add(fboLinxFileData);
            }

            await _fileStorageContext.SaveChangesAsync();
        }

        public async Task DeleteFileAttachment(int emailContentId)
        {
            var emailContentFile = await _fileStorageContext.FbolinxEmailContentAttachments.Where(p => p.EmailContentId == emailContentId).FirstOrDefaultAsync();

            _fileStorageContext.FbolinxEmailContentAttachments.Remove(emailContentFile);
            await _fileStorageContext.SaveChangesAsync();
        }
    }
}
