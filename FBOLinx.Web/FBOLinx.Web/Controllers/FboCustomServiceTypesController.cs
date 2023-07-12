using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
using FBOLinx.DB.Models.ServicesAndFees;
using FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FboCustomServiceTypesController : FBOLinxControllerBase
    {
        private readonly IFboServiceTypeService _fboServiceTypeService;

        public FboCustomServiceTypesController(IFboServiceTypeService fboServiceTypeService, ILoggingService logger) : base(logger)
        {
            _fboServiceTypeService = fboServiceTypeService;
        }

        // GET: api/FboCustomServiceTypes/fbo/3
        [HttpGet("fbo/{fboId}")]
        public async Task<ActionResult<List<ServiceTypeResponse>>> Get(int fboId)
        {
            var result = await _fboServiceTypeService.Get(fboId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        // POST: api/FboCustomServiceTypes/fbo/3
        [HttpPost("fbo/{fboId}")]
        public async Task<ActionResult<ServiceTypeResponse>> Post(int fboId, [FromBody] ServiceTypeResponse fboCustomServiceTypes)
        {
            var result = await _fboServiceTypeService.Create(fboId, fboCustomServiceTypes);

            return Ok(result);
        }
        // PUT: api/FboCustomServiceTypes/3
        [HttpPut("{serviceTypeId}")]
        public async Task<ActionResult<List<ServiceTypeResponse>>> Put(int serviceTypeId, [FromBody] ServiceTypeResponse fboCustomServiceTypes)
        {
            var result = await _fboServiceTypeService.Update(serviceTypeId, fboCustomServiceTypes);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        // Delete: api/FboCustomServiceTypes/1234
        [HttpDelete("{FboCustomServiceTypesId}")]
        public async Task<ActionResult<List<ServiceTypeResponse>>> Delete(int FboCustomServiceTypesId)
        {
            var result = await _fboServiceTypeService.Delete(FboCustomServiceTypesId);

            if (result == null)
                return NotFound();

            return Ok();
        }
    }
}
