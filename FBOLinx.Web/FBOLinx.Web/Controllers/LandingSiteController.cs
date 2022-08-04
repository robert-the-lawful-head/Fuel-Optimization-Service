using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using FBOLinx.ServiceLayer.BusinessServices.Mail;


namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingSiteController : ControllerBase
    {
        private readonly IMailService _MailService;
        private readonly FboLinxContext _Context;

        public LandingSiteController(FboLinxContext context, IMailService mailService)
        {
            _Context = context;
            _MailService = mailService;
        }

        // POST: api/ContactUs
        [HttpPost("ContactUs")]
        public async Task<IActionResult> PostContactUsMessage([FromBody] LandingSiteContactUsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string headr = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">";
            headr += @"<html>";
            headr += @"<body>";
            string bdy = "Name: " + request.Name;
            bdy += @"<br/>Email: " + request.Email;
            bdy += @"<br/>Phone Number: " + request.PhoneNumber;
            bdy += @"<br/>Message: " + request.Message;
            bdy += @"</body></html>";

            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            mailMessage.To.Add(new MailAddress("info@fbolinx.com"));

            mailMessage.Subject = "Sent From FBOlinx Contact Form";
            mailMessage.Body = headr + bdy;

            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;
            await _MailService.SendAsync(mailMessage);

            return Ok();
        }
    }
}