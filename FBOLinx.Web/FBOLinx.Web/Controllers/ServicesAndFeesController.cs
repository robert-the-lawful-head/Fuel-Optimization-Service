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
        // GET: api/ServicesAndFees
        [HttpGet("fbo/{fboId}")]
        public async Task<List<ServicesAndFeesDto>> Get(int fboId)
        {
            return await _fboServicesAndFeesService.Get(fboId);
        }
    }
}