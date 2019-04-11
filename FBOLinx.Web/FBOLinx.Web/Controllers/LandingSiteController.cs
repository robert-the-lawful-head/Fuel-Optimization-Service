using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.Web.Configurations;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingSiteController : ControllerBase
    {
        private readonly FBOLinx.Web.Configurations.MailSettings _MailSettings;
        private readonly FboLinxContext _Context;

        public LandingSiteController(FboLinxContext context, IOptions<FBOLinx.Web.Configurations.MailSettings> mailSettings)
        {
            _Context = context;
            _MailSettings = mailSettings.Value;
        }

        // POST: api/ContactUs
        [HttpPost("ContactUs")]
        public async Task<IActionResult> PostContactUsMessage([FromBody] LandingSiteContactUsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MailService mailService = new MailService(_MailSettings);
            string headr = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">";
            headr += @"<html>";
            headr += @"<body>";
            string bdy = "Name: " + request.Name;
            bdy += @"<br/>Email: " + request.Email;
            bdy += @"<br/>Phone Number: " + request.PhoneNumber;
            bdy += @"<br/>Message: " + request.Message;
            bdy += @"</body></html>";

            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("donotreply@fbolinx.com");
            mm.To.Add(new MailAddress("info@fbolinx.com"));

            mm.Subject = "Sent From FBOlinx Contact Form";
            mm.Body = headr + bdy;

            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.Normal;
            var sendGridMessage = mm.GetSendGridMessage();
            await mailService.SendAsync(sendGridMessage);

            return Ok();
        }
    }
}