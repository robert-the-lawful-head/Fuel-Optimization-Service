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
using Remotion.Linq.Clauses;

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

            var result = await (from p in _context.PricingTemplate
                join cm in (from c in _context.CustomerMargins group c by new {c.TemplateId} into cmResults select new {templateId = cmResults.Key.TemplateId, maxPrice = cmResults.Max(x => x.Amount.GetValueOrDefault())} )on p.Oid equals cm.templateId into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in (from f in _context.Fboprices
                    where f.EffectiveFrom.HasValue && f.EffectiveFrom <= DateTime.Now && f.EffectiveTo.HasValue && f.EffectiveTo > DateTime.Now.AddDays(-1)
                          && f.Fboid.GetValueOrDefault() == fboId
                    select f) on p.MarginTypeProduct equals fp.Product into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                where p.Fboid == fboId
                select new PricingTemplatesGridViewModel
                {
                    CustomerId = p.CustomerId,
                    Default = p.Default,
                    Fboid = p.Fboid,
                    Margin = cm.maxPrice,
                    MarginType = p.MarginType,
                    Name = p.Name,
                    Notes = p.Notes,
                    Oid = p.Oid,
                    Type = p.Type,
                    IntoPlanePrice = (fp == null ? 0 : fp.Price.GetValueOrDefault()) +
                                     (cm == null ? 0 : cm.maxPrice)
                }).ToListAsync();

            return Ok(result);
        }

        // GET: api/PricingTemplates/fbo/5/default
        [HttpGet("fbo/{fboId}/default")]
        public async Task<IActionResult> GetDefaultPricingTemplateByFboId([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await (from p in _context.PricingTemplate
                join cm in (from c in _context.CustomerMargins group c by new { c.TemplateId } into cmResults select new { templateId = cmResults.Key.TemplateId, maxPrice = cmResults.Max(x => x.Amount.GetValueOrDefault()) }) on p.Oid equals cm.templateId into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in (from f in _context.Fboprices
                    where f.EffectiveFrom.HasValue && f.EffectiveFrom <= DateTime.Now && f.EffectiveTo.HasValue && f.EffectiveTo > DateTime.Now.AddDays(-1)
                          && f.Fboid.GetValueOrDefault() == fboId
                    select f) on p.MarginTypeProduct equals fp.Product into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                where p.Fboid == fboId
                    && p.Default.GetValueOrDefault()
                select new PricingTemplatesGridViewModel
                {
                    CustomerId = p.CustomerId,
                    Default = p.Default,
                    Fboid = p.Fboid,
                    Margin = cm.maxPrice,
                    MarginType = p.MarginType,
                    Name = p.Name,
                    Notes = p.Notes,
                    Oid = p.Oid,
                    Type = p.Type,
                    IntoPlanePrice = (fp == null ? 0 : fp.Price.GetValueOrDefault()) +
                                     (cm == null ? 0 : cm.maxPrice)
                }).ToListAsync();


            return Ok(result);
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