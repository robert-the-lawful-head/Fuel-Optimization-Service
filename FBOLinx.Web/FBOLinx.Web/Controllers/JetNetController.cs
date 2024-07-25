using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JetNetController : FBOLinxControllerBase
    {
        private readonly IJetNetService _JetNetService;
        public JetNetController(ILoggingService logger, IJetNetService jetNetService) : base(logger)
        {
            _JetNetService = jetNetService;
        }

        // GET: api/JetNet/N12345
        [HttpGet("{tailNumber}")]
        public async Task<IActionResult> GetJetNetInformation([FromRoute] string tailNumber)
        {
            var jetNetInformation = await _JetNetService.GetJetNetInformation(tailNumber);
            return Ok(jetNetInformation);
        }
    }
}