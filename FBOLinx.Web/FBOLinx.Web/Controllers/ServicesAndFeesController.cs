using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
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
        public async Task<ActionResult<List<ServicesAndFeesDto>>> Get(int fboId)
        {
            var result =  await _fboServicesAndFeesService.Get(fboId);

            if (result == null)
                return NotFound();
            
            return Ok(result);
        }
    }
}