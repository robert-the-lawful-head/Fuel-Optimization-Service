using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Services
{
    public class EmailContentService
    {
        private FboLinxContext _context;

        public EmailContentService(FboLinxContext context)
        {
            _context = context;
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
            var fbo = await _context.Fbos.Where(x => x.Oid == fboId).Include(x => x.fboAirport).FirstOrDefaultAsync();
            if (fbo == null)
                return null;
            var emailContent = new EmailContent()
                {
                    EmailContentHtml = "Greeting from " + fbo.Fbo + " / " + fbo.fboAirport?.Icao + ". Attached please see your custom pricing from our FBO!",
                    EmailContentType = EmailContentTypes.Body,
                    FboId = fboId,
                    Name = "Default Email Template",
                    Subject = "Pricing from " + fbo.Fbo + " / " + fbo.fboAirport?.Icao
                };
            await _context.EmailContent.AddAsync(emailContent);
            await _context.SaveChangesAsync();
            return emailContent;
        }
    }
}
