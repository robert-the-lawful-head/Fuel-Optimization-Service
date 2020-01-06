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

            var jetaACostRecord = await _context.Fboprices.Where(x => x.Fboid == fboId && x.Product == "JetA Cost")
                .FirstOrDefaultAsync();
            var products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));
            var resultPrices = (from p in products
                          join f in (from f in _context.Fboprices
                                         //where f.EffectiveFrom <= DateTime.Now && f.EffectiveTo > DateTime.Now.AddDays(-1)
                                     where f.EffectiveTo > DateTime.Now.AddDays(-1)
                                     && f.Fboid == fboId
                                     select f) on new { Product = p.Description, FboId = fboId } equals new
                                     {
                                         f.Product,
                                         FboId = f.Fboid.GetValueOrDefault()
                                     }
                              into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in (from s in _context.TempAddOnMargin
                                     where s.FboId == fboId && s.EffectiveTo >= DateTime.Today.ToUniversalTime()
                                     select s) on new { FboId = fboId } equals new
                                     {
                                         FboId = s.FboId
                                     }
                              into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          select new
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              Price = f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.Now,
                              EffectiveTo = f?.EffectiveTo,
                              TimeStamp = f?.Timestamp,
                              SalesTax = f?.SalesTax,
                              Currency = f?.Currency,
                              tempJet = s?.MarginJet,
                              tempAvg = s?.MarginAvgas,
                              tempId = s?.Id,
                              tempDateFrom = s?.EffectiveFrom,
                              tempDateTo = s?.EffectiveTo
                          });

            var jetACost = resultPrices.FirstOrDefault(s => s.Product == "JetA Cost").Price;
            var jetARetail = resultPrices.FirstOrDefault(s => s.Product == "JetA Retail").Price;
            var marginPrice = _context.TempAddOnMargin.FirstOrDefault(s => s.FboId == fboId && s.EffectiveTo > DateTime.Now);
            

            var result = await (from p in _context.PricingTemplate
                join f in (_context.Fbos.Include("Preferences")) on p.Fboid equals f.Oid
                join cm in (from c in _context.CustomerMargins join tm in (_context.PriceTiers) on c.PriceTierId equals tm.Oid
                            group c by new {c.TemplateId} into cmResults select new {templateId = cmResults.Key.TemplateId, maxPrice = cmResults.FirstOrDefault().Amount} )on p.Oid equals cm.templateId into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in (from f in _context.Fboprices
                                //where f.EffectiveFrom.HasValue && f.EffectiveFrom <= DateTime.Now && f.EffectiveTo.HasValue && f.EffectiveTo > DateTime.Now.AddDays(-1)
                            where f.EffectiveTo > DateTime.Now.AddDays(-1)
                                  && f.Fboid.GetValueOrDefault() == fboId
                    select f) on p.MarginTypeProduct equals fp.Product into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                where p.Fboid == fboId
                select new PricingTemplatesGridViewModel
                {
                    CustomerId = p.CustomerId.GetValueOrDefault(),
                    Default = p.Default.GetValueOrDefault(),
                    Fboid = p.Fboid,
                    Margin = cm == null ? 0 : cm.maxPrice.Value,
                    MarginType = p.MarginType.GetValueOrDefault(),
                    Name = p.Name,
                    Notes = p.Notes,
                    Oid = p.Oid,
                    Type = p.Type.GetValueOrDefault(),
                    Subject = p.Subject,
                    Email = p.Email,
                    //IntoPlanePrice = (fp == null ? 0 : fp.Price.GetValueOrDefault()) +
                    //                 (cm == null ? 0 : cm.maxPrice.Value),
                    IntoPlanePrice = (jetaACostRecord == null ? 0 : jetaACostRecord.Price.GetValueOrDefault()) +
                                     (cm == null ? 0 : cm.maxPrice.Value),
                    IsInvalid = (f != null && f.Preferences != null && ((f.Preferences.OmitJetACost.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == Models.PricingTemplate.MarginTypes.CostPlus) || f.Preferences.OmitJetARetail.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == Models.PricingTemplate.MarginTypes.RetailMinus)) ? true : false,
                    IsPricingExpired = (fp == null && (p.MarginType == null || p.MarginType != PricingTemplate.MarginTypes.FlatFee)),
                    //YourMargin = jetaACostRecord == null || jetaACostRecord.Price.GetValueOrDefault() <= 0 ? 0 : ((fp == null ? 0 : fp.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.maxPrice)) - (jetaACostRecord.Price.GetValueOrDefault())
                    YourMargin = jetaACostRecord == null || jetaACostRecord.Price.GetValueOrDefault() <= 0 ? 0 : ((fp == null ? 0 : fp.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.maxPrice)) - (jetaACostRecord.Price.GetValueOrDefault())
                }).ToListAsync();

            //foreach(var res in result)
            //{
            //    if(res.MarginTypeDescription == "Retail -")
            //    {

            //    }
            //    else if(res.MarginTypeDescription == "Cost +")
            //    {
            //        res.IntoPlanePrice = res.Margin + jetACost.Value;
            //    }
            //}

            foreach (var res in result)
            {
                if (res.Oid != 0)
                {
                    var margins = _context.CustomerMargins.FirstOrDefault(s => s.TemplateId == res.Oid && s.PriceTierId != 0);

                    if (margins != null)
                    {
                        if (res.MarginTypeDescription == "Retail -")
                        {
                            if (jetARetail != null)
                            {
                                res.IntoPlanePrice = jetARetail.Value - Convert.ToDouble(margins.Amount);
                            }
                            else
                            {
                                res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + 0;
                            }
                            
                            if(jetACost != null)
                            {
                                res.YourMargin = res.IntoPlanePrice - jetACost.Value;
                            }
                            else
                            {
                                res.YourMargin = res.IntoPlanePrice - 0;
                            }
                            
                        }
                        else if (res.MarginTypeDescription == "Cost +")
                        {
                            if(jetACost != null)
                            {
                                res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + jetACost.Value;
                                res.YourMargin = res.IntoPlanePrice - jetACost.Value;
                            }
                            else
                            {
                                res.IntoPlanePrice = Convert.ToDouble(margins.Amount) + 0;
                                res.YourMargin = res.IntoPlanePrice - 0;
                            }
                            
                            
                        }
                    }
                }
            }

            result = result.GroupBy(s => s.Oid).Select(g => g.First()).ToList();

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
                    Subject = p.Subject,
                    Email = p.Email,
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
            FixOtherDefaults(pricingTemplate);

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

            FixOtherDefaults(pricingTemplate);

            _context.PricingTemplate.Add(pricingTemplate);

            //Commented out by Angel (Dec 17th) because it creates duplicate null price tiers https://prnt.sc/qbvy3b
            //var priceTier = new PriceTiers() {Min = 1, Max = 99999, MaxEntered = 0};
            //_context.PriceTiers.Add(priceTier);

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

        private void FixOtherDefaults(PricingTemplate pricingTemplate)
        {
            if (pricingTemplate.Default.GetValueOrDefault())
            {
                var otherDefaults = _context.PricingTemplate.Where(x =>
                    x.Fboid == pricingTemplate.Fboid && x.Default.GetValueOrDefault() && x.Oid != pricingTemplate.Oid);
                foreach (var otherDefault in otherDefaults)
                {
                    otherDefault.Default = false;
                    _context.Entry(otherDefault).State = EntityState.Modified;
                }
            }
        }
    }
}