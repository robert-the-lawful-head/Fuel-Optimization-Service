using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.Service.Mapping.Dto;
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

        public ServicesAndFeesController(IFboServicesAndFeesService fboServicesAndFeesService, ILoggingService logger, IFboService fboService) : base(logger)
        {
            _fboServicesAndFeesService = fboServicesAndFeesService;
            _fboService = fboService;
        }
        // GET: api/ServicesAndFees/fbo/3
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<List<ServicesAndFeesDto>>> Get(int fboId)
        {
            var result = await _fboServicesAndFeesService.Get(fboId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [APIKey(Core.Enums.IntegrationPartnerTypes.Internal)]
        [HttpGet("handlerid/{handlerId}")]
        public async Task<ActionResult<List<string>>> GetFboServicesAndFeesByHandlerId([FromRoute] int handlerId)
        {
            var servicesList = new List<string>();
            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));

            if (fbo != null && fbo.Oid > 0)
            {
                var services = await _fboServicesAndFeesService.Get(fbo.Oid);
                servicesList = services.Select(s => s.Service).ToList();
            }

            return servicesList;
        }

    }
}