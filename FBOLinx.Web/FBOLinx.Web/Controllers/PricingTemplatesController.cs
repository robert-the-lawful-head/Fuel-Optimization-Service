using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.Dto.Requests;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Customers;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PricingTemplatesController : ControllerBase
    {
        private readonly IPricingTemplateService _pricingTemplateService;
        private readonly ICustomerMarginService _customerMarginService;
        private readonly ICustomCustomerTypeService _customCustomerTypeService;
        

        public PricingTemplatesController(IPricingTemplateService pricingTemplateService, ICustomerMarginService customerMarginService, ICustomCustomerTypeService customCustomerTypeService)
        {
            _pricingTemplateService = pricingTemplateService;
            _customerMarginService = customerMarginService;
            _customCustomerTypeService = customCustomerTypeService;
        }

        // GET: api/PricingTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PricingTemplateDto>> GetPricingTemplate([FromRoute] int id)
        {
            var pricingTemplate =  await _pricingTemplateService.GetPricingTemplateById(id);

            if (pricingTemplate == null) return NotFound();

            return Ok(pricingTemplate);
        }

        [HttpGet("fbodefaultpricingtemplate/group/{groupId}/fbo/{fboId}")]
        public async Task<ActionResult<List<PricingTemplateGrid>>> GetPricingTemplateByFboIdForDefaultTemplate([FromRoute] int groupId, [FromRoute] int fboId)
        {
            await _pricingTemplateService.FixDefaultPricingTemplate(fboId);

            List<PricingTemplateGrid> marginTemplates = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);

            return Ok(marginTemplates);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<ActionResult<List<PricingTemplateGrid>>> GetPricingTemplateByGroupIdAndFboId([FromRoute] int groupId, [FromRoute] int fboId)
        { 
            List<PricingTemplateGrid> marginTemplates = await _pricingTemplateService.GetPricingTemplates(fboId, groupId);

            return Ok(marginTemplates);
        }

        [HttpGet("with-email-content/group/{groupId}/fbo/{fboId}")]
        public async Task<ActionResult<List<PricingTemplateGrid>>> GetPricingTemplatesWithEmailContentByGroupIdAndFboId([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var templatesWithEmailContent = await _pricingTemplateService.GetTemplatesWithEmailContent(fboId, groupId);

            return Ok(templatesWithEmailContent);
        }

        [HttpGet("getcostpluspricingtemplate/{fboId}")]
        public async Task<IActionResult> GetCostPlusPricingTemplates([FromRoute] int fboId)
        {
           var result = await _pricingTemplateService.GetCostPlusPricingTemplates(fboId);

            if (result.Count == 0) return Ok(new ExistReponse() { Exist = false });

            var custAssigned = result.FirstOrDefault(s => s.CustomersAssigned > 0);

            if(custAssigned == null) return Ok(new ExistReponse() { Exist = false });

            return Ok(new ExistReponse() { Exist = true });
        }


        // GET: api/PricingTemplates/fbo/5/default
        [HttpGet("fbo/{fboId}/default")]
        public async Task<ActionResult<List<PricingTemplateGrid>>> GetDefaultPricingTemplateByFboId([FromRoute] int fboId)
        {
            var result = _pricingTemplateService.GetDefaultPricingTemplateByFboId(fboId);

            return Ok(result);
        }

        // PUT: api/PricingTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricingTemplate([FromRoute] int id, [FromBody] PricingTemplateDto pricingTemplate)
        {
            if (id != pricingTemplate.Oid)
            {
                return BadRequest();
            }

            var result = await _pricingTemplateService.PutPricingTemplate(id,pricingTemplate);

            if(result) return NoContent();

            return NoContent();
        }

        // POST: api/PricingTemplates
        [HttpPost]
        public async Task<ActionResult<PricingTemplateDto>> PostPricingTemplate([FromBody] PricingTemplateDto pricingTemplate)
        {
            var result = await _pricingTemplateService.PostPricingTemplate(pricingTemplate);

            return CreatedAtAction(nameof(GetPricingTemplate), new { id = result.Oid }, result);
        }

        [HttpPost("copypricingtemplate")]
        public async Task<ActionResult<PricingTemplateDto>> CopyPricingTemplate([FromBody] PrincingTemplateRequest pricingTemplate)
        {
            if (pricingTemplate.currentPricingTemplateId == null && pricingTemplate.name == string.Empty) return null;

            var copiedPricingTemplate = await _pricingTemplateService.CopyPricingTemplate(pricingTemplate);

            if (copiedPricingTemplate == null) return null;
            
           
            if(copiedPricingTemplate.Oid == 0) return Ok(copiedPricingTemplate.Oid);

            await _customerMarginService.CreateCustomerMargins(pricingTemplate.currentPricingTemplateId, copiedPricingTemplate.Oid);
            
            return Ok(copiedPricingTemplate.Oid);
        }

        [HttpGet("checkdefaulttemplate/{fboId}")]
        public async Task<ActionResult<PricingTemplateDto>> CheckDefaultTemplate([FromRoute] int fboId)
        {
            if(fboId == 0) return Ok(null);
            
            var result = _pricingTemplateService.GetDefaultTemplate(fboId);

            if (result != null) return Ok(result);
        
            return Ok(null);
        }

        // DELETE: api/PricingTemplates/5/fbo/124
        [HttpDelete("{oid}/fbo/{fboId}")]
        public async Task<ActionResult<PricingTemplateDto>> DeletePricingTemplate([FromRoute] int oid, [FromRoute] int fboId)
        {
            var pricingTemplate = await _pricingTemplateService.GetPricingTemplateById(oid);

            if (pricingTemplate == null)    return NotFound();

            await _pricingTemplateService.DeletePricingTemplate(pricingTemplate);

            var defaultPricingTemplate = await _pricingTemplateService.GetDefaultTemplateIncludeNullCheck(fboId);

            if (defaultPricingTemplate == null) Ok(pricingTemplate);

            await _customCustomerTypeService.SaveCustomersTypes(fboId,oid, defaultPricingTemplate.Oid);

            return Ok(pricingTemplate);
        }
    }
}
