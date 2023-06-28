using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.ServicesAndFees;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    //[Authorize]
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
        public async Task<List<ServicesAndFeesDto>> Get(int fboId, int? pageoffset, int? pageLimit)
        {
            return await _fboServicesAndFeesService.Get(fboId,pageoffset,pageLimit);
        }
        // GET: api/ServicesAndFees/fbo/5/serviceType/test service
        [HttpGet("fbo/{fboId}/serviceType/{serviceType}")]
        public async Task<List<ServicesAndFeesDto>> Get(int fboId, string serviceType, int? pageoffset, int? pageLimit)
        {
            return await _fboServicesAndFeesService.Get(fboId,serviceType,pageLimit,pageoffset);
        }
    }
}