using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesAndFeesController : FBOLinxControllerBase
    {
        private readonly IFboServicesAndFeesService _fboServicesAndFeesService;

        public ServicesAndFeesController(IFboServicesAndFeesService fboServicesAndFeesService, ILoggingService logger) : base(logger)
        {
            _fboServicesAndFeesService = fboServicesAndFeesService;
        }
        // GET: api/ServicesAndFees/fbo/3
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<List<FbosServicesAndFeesResponse>>> Get(int fboId)
        {
            var result =  await _fboServicesAndFeesService.Get(fboId);

            if (result == null)
                return NotFound();
            
            return Ok(result);
        }
        // POST: api/ServicesAndFees/fbo/3
        [HttpPost("fbo/{fboId}")]
        public async Task<ActionResult<List<ServicesAndFeesDto>>> Post(int fboId, [FromBody] ServicesAndFeesDto servicesAndFees)
        {
            var result = await _fboServicesAndFeesService.Create(fboId,servicesAndFees);

            return Ok(result);
        }
        // PUT: api/ServicesAndFees/fbo/3
        [HttpPut("fbo/{fboId}")]
        public async Task<ActionResult<List<ServicesAndFeesDto>>> Put(int fboId, [FromBody] ServicesAndFeesDto servicesAndFees)
        {
            var result = await _fboServicesAndFeesService.Update(fboId, servicesAndFees);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        // Delete: api/ServicesAndFees/1234
        [HttpDelete("{servicesAndFeesId}")]
        public async Task<ActionResult<List<ServicesAndFeesDto>>> Delete(int servicesAndFeesId, int? handlerId, int? serviceOfferedId)
        {
            var result = await _fboServicesAndFeesService.Delete(servicesAndFeesId, handlerId, serviceOfferedId);

            if (result == null)
                return NotFound();

            return Ok();
        }
    }
}