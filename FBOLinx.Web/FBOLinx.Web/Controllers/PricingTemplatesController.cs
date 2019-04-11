using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PricingTemplatesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public PricingTemplatesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/PricingTemplates
        [HttpGet]
        public IEnumerable<PricingTemplate> GetPricingTemplate()
        {
            return _context.PricingTemplate;
        }

        // GET: api/PricingTemplates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPricingTemplate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingTemplate = await _context.PricingTemplate.FindAsync(id);

            if (pricingTemplate == null)
            {
                return NotFound();
            }

            return Ok(pricingTemplate);
        }

        // GET: api/PricingTemplates/fbo/5
        [HttpGet("fbo/{fboId}")]
        public async Task<IActionResult> GetPricingTemplateByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingTemplate = await GetAllPricingTemplates().Include("CustomerMargins").Where((x => x.Fboid == fboId)).ToListAsync();

            var pricingTemplatesVM = pricingTemplate.Select(p => new PricingTemplatesGridViewModel
            {
                CustomerId = p.CustomerId,
                Default = p.Default,
                Fboid = p.Fboid,
                Margin = p.CustomerMargins.DefaultIfEmpty().Max(x => x?.Amount ?? 0),
                MarginType = p.MarginType,
                Name = p.Name,
                Notes = p.Notes,
                Oid = p.Oid,
                Type = p.Type
            });

            return Ok(pricingTemplatesVM);
        }

        // PUT: api/PricingTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricingTemplate([FromRoute] int id, [FromBody] PricingTemplate pricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pricingTemplate.Oid)
            {
                return BadRequest();
            }

            _context.Entry(pricingTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingTemplateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PricingTemplates
        [HttpPost]
        public async Task<IActionResult> PostPricingTemplate([FromBody] PricingTemplate pricingTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PricingTemplate.Add(pricingTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPricingTemplate", new { id = pricingTemplate.Oid }, pricingTemplate);
        }

        // DELETE: api/PricingTemplates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePricingTemplate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pricingTemplate = await _context.PricingTemplate.FindAsync(id);
            if (pricingTemplate == null)
            {
                return NotFound();
            }

            _context.PricingTemplate.Remove(pricingTemplate);
            await _context.SaveChangesAsync();

            return Ok(pricingTemplate);
        }

        private bool PricingTemplateExists(int id)
        {
            return _context.PricingTemplate.Any(e => e.Oid == id);
        }

        private IQueryable<PricingTemplate> GetAllPricingTemplates()
        {
            return _context.PricingTemplate.AsQueryable();
        }
    }
}