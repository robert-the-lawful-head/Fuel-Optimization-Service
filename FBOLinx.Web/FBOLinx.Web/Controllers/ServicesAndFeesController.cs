using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
﻿using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Auth;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.Requests.FuelReq;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesAndFeesController : FBOLinxControllerBase
    {
        private readonly IFboServicesAndFeesService _fboServicesAndFeesService;
        private readonly IFboService _fboService;
        private readonly IAuthService _authService;

        public ServicesAndFeesController(IFboServicesAndFeesService fboServicesAndFeesService, ILoggingService logger, IFboService fboService, IAuthService authService) : base(logger)
        {
            _fboServicesAndFeesService = fboServicesAndFeesService;
            _fboService = fboService;
            _authService = authService;
        }
        // GET: api/ServicesAndFees/fbo/3
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<List<FbosServicesAndFeesResponse>>> Get(int fboId)
        {
            var result = await _fboServicesAndFeesService.Get(fboId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        // POST: api/ServicesAndFees/fbo/3
        [HttpPost("fbo/{fboId}")]
        public async Task<ActionResult<ServicesAndFeesResponse>> Post(int fboId, [FromBody] ServicesAndFeesDto servicesAndFees)
        {
            var result = await _fboServicesAndFeesService.Create(fboId,servicesAndFees);

            return Ok(result);
        }
        // PUT: api/ServicesAndFees/fbo/3
        [HttpPut("fbo/{fboId}")]
        public async Task<ActionResult<List<ServicesAndFeesResponse>>> Put(int fboId, [FromBody] ServicesAndFeesDto servicesAndFees)
        {
            var result = await _fboServicesAndFeesService.Update(fboId, servicesAndFees);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        // Delete: api/ServicesAndFees/1234
        [HttpDelete("{servicesAndFeesId}")]
        public async Task<IActionResult> Delete(int servicesAndFeesId, int? handlerId, int? serviceOfferedId)
        {
            var result = await _fboServicesAndFeesService.Delete(servicesAndFeesId, handlerId, serviceOfferedId);

            if (result == null)
                return NotFound();

            return NoContent();
        }

        [AllowAnonymous]
        [APIKey(Core.Enums.IntegrationPartnerTypes.Internal)]
        [HttpGet("handlerid/{handlerId}")]
        public async Task<ActionResult<List<string>>> GetFboServicesAndFeesByHandlerId([FromRoute] int handlerId)
        {
            var servicesList = new List<string>();
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));

            if (fbo == null)
            {
                var email = await _authService.CreateNonRevAccount(handlerId);
                if (!email.Contains("@"))
                    return servicesList;
                fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));
            }

            var services = await _fboServicesAndFeesService.Get(fbo.Oid);
            servicesList = services.Select(s => s.Service).ToList();

            return servicesList;
        }

    }
}