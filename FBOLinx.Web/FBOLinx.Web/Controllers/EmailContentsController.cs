using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailContentsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private IHttpContextAccessor _HttpContextAccessor;
        private EmailContentService _emailContentService;

        public EmailContentsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor,
            EmailContentService emailContentService)
        {
            _emailContentService = emailContentService;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // GET: api/EmailContents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailContent>> GetEmailContent(int id)
        {
            var emailContent = await _context.EmailContent.FindAsync(id);

            if (emailContent == null)
            {
                return null;
            }

            if (JwtManager.GetClaimedFboId(_HttpContextAccessor) != emailContent.FboId &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest();
            }

            return emailContent;
        }

        // GET: api/EmailContents/5
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<List<EmailContent>>> GetEmailContentForFbo([FromRoute] int fboId)
        {
            if (JwtManager.GetClaimedFboId(_HttpContextAccessor) != fboId &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest();
            }

            var emailContent = await _emailContentService.GetEmailContentsForFbo(fboId);

            if (emailContent == null)
            {
                return NotFound();
            }

            return emailContent;
        }

        // GET: api/EmailContents/5
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<EmailContent>> GetEmailContentForGroup([FromRoute] int groupId)
        {
            if (JwtManager.GetClaimedGroupId(_HttpContextAccessor) != groupId &&
                (JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor
                    && JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.GroupAdmin)
            )
            {
                return BadRequest();
            }

            var emailContent = await _context.EmailContent.Where((x => x.GroupId == groupId)).FirstOrDefaultAsync();

            return emailContent;
        }

        // PUT: api/EmailContents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmailContent(int id, EmailContent emailContent)
        {
            if (id != emailContent.Oid)
            {
                return BadRequest();
            }

            if (JwtManager.GetClaimedFboId(_HttpContextAccessor) != emailContent.FboId &&
                JwtManager.GetClaimedGroupId(_HttpContextAccessor) != emailContent.GroupId &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest();
            }

            _context.Entry(emailContent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailContentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmailContents
        [HttpPost]
        public async Task<ActionResult<EmailContent>> PostEmailContent(EmailContent emailContent)
        {
            if (JwtManager.GetClaimedFboId(_HttpContextAccessor) != emailContent.FboId &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest();
            }

            _context.EmailContent.Add(emailContent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmailContent", new { id = emailContent.Oid }, emailContent);
        }

        // DELETE: api/EmailContents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmailContent>> DeleteEmailContent(int id)
        {
            var emailContent = await _context.EmailContent.FindAsync(id);
            if (emailContent == null)
            {
                return NotFound();
            }

            if (JwtManager.GetClaimedFboId(_HttpContextAccessor) != emailContent.FboId &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.Conductor &&
                JwtManager.GetClaimedRole(_HttpContextAccessor) != DB.Models.User.UserRoles.GroupAdmin)
            {
                return BadRequest();
            }

            _context.EmailContent.Remove(emailContent);
            await _context.SaveChangesAsync();

            return emailContent;
        }

        // GET: api/EmailContents/fileattachment/5
        [HttpGet("fileattachment/{emailContentId}")]
        public async Task<IActionResult> GetFileAttachment([FromRoute] int emailContentId)
        {
            var file = await _emailContentService.GetFileAttachment(emailContentId);

            return Ok(file);
        }

        // GET: api/EmailContent/fileattachmentname/5
        [HttpGet("fileattachmentname/{emailContentId}")]
        public async Task<IActionResult> GetFileAttachmentName([FromRoute] int emailContentId)
        {
            var file = await _emailContentService.GetFileAttachmentName(emailContentId);

            return Ok(file);
        }

        // POST: api/EmailContents/uploadfileattachment
        [HttpPost("uploadfileattachment")]
        public async Task<IActionResult> UploadFileAttachment([FromBody] FbolinxEmailContentAttachmentsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (request.FileData.Contains(","))
                {
                    request.FileData = request.FileData.Substring(request.FileData.IndexOf(",") + 1);
                }

                await _emailContentService.UploadFileAttachment(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message });
            }
        }

        // DELETE: api/EmailContents/fileattachment/4
        [HttpDelete("fileattachment/{emailContentId}")]
        public async Task<IActionResult> DeleteFileAttachment([FromRoute] int emailContentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _emailContentService.DeleteFileAttachment(emailContentId);

            return Ok();
        }

        private bool EmailContentExists(int id)
        {
            return _context.EmailContent.Any(e => e.Oid == id);
        }
    }
}
