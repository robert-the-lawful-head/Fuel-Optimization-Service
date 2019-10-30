using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbopricesController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public FbopricesController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/Fboprices
        [HttpGet]
        public IEnumerable<Fboprices> GetFboprices()
        {
            return _context.Fboprices;
        }

        // GET: api/Fboprices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFboprices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboprices = await _context.Fboprices.FindAsync(id);

            if (fboprices == null)
            {
                return NotFound();
            }

            return Ok(fboprices);
        }

        // GET: api/Fboprices/fbo/current/5
        [HttpGet("fbo/{fboId}/current")]
        public async Task<IActionResult> GetFbopricesByFboIdCurrent([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));

            var result = (from p in products
                          join f in (from f in _context.Fboprices
                                     where f.EffectiveFrom <= DateTime.Now && f.EffectiveTo > DateTime.Now.AddDays(-1)
                                     && f.Fboid == fboId
                                     select f) on new { Product = p.Description, FboId = fboId } equals new
                                     {
                                         f.Product,
                                         FboId = f.Fboid.GetValueOrDefault()
                                     }
                              into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in (from s in _context.TempAddOnMargin
                                     where s.EffectiveFrom <= DateTime.Now && s.EffectiveTo > DateTime.Now.AddDays(-1)
                                     && s.FboId == fboId
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

            return Ok(result);
        }

        // GET: api/Fboprices/fbo/staged/5
        [HttpGet("fbo/{fboId}/staged")]
        public async Task<IActionResult> GetFbopricesByFboIdStaged([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));

            var result = (from p in products
                          join f in (from f in _context.Fboprices
                                     where f.EffectiveFrom > DateTime.Now
                                     select f) on new { Product = p.Description, FboId = fboId } equals new
                                     {
                                         f.Product,
                                         FboId = f.Fboid.GetValueOrDefault()
                                     }
                              into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()

                          select new
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              Price = f?.Price,
                              EffectiveFrom = f?.EffectiveFrom,
                              EffectiveTo = f?.EffectiveTo,
                              TimeStamp = f?.Timestamp,
                              SalesTax = f?.SalesTax,
                              Currency = f?.Currency
                          });

            return Ok(result);
        }

        // GET: api/Fboprices/fbo/current/5
        [HttpGet("fbo/{fboId}/product/{product}/current")]
        public async Task<IActionResult> GetFbopricesByFboIdAndProductCurrent([FromRoute] int fboId, [FromRoute] string product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboprices = await GetAllFboPrices().Where((f => f.Fboid == fboId &&
                                                                f.Product != null &&
                                                                f.Product.ToLower() == product.ToLower() &&
                                                                f.EffectiveFrom <= DateTime.Now && f.EffectiveTo > DateTime.Now.AddDays(-1))).FirstOrDefaultAsync();

            return Ok(fboprices);
        }

        // POST: api/Fboprices/analysis/prices-by-month/fbo/5
        [HttpPost("analysis/prices-by-month/fbo/{fboId}")]
        public async Task<IActionResult> GetPricesByMonthForFbo([FromRoute] int fboId,
            [FromBody] FboPricesByMonthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboPricesByMonth = (from f in _context.Fboprices
                                    where f.Product.ToLower() == request.Product.ToLower()
                                          && f.Fboid == fboId
                                          && f.EffectiveFrom >= request.StartDateTime
                                          && f.EffectiveFrom <= request.EndDateTime
                                    group f by new
                                    {
                                        Month = f.EffectiveFrom.GetValueOrDefault().Month,
                                        Year = f.EffectiveFrom.Value.Year
                                    }
                into results
                                    select new
                                    {
                                        Month = results.Key.Month,
                                        Year = results.Key.Year,
                                        AveragePrice = results.Average((x => x.Price))
                                    });

            return Ok(fboPricesByMonth);
        }

        // PUT: api/Fboprices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFboprices([FromRoute] int id, [FromBody] Fboprices fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fboprices.Oid)
            {
                return BadRequest();
            }

            _context.Entry(fboprices).State = EntityState.Modified;

            try
            {
                fboprices.Timestamp = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbopricesExists(id))
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

        // POST: api/Fboprices
        [HttpPost]
        public async Task<IActionResult> PostFboprices([FromBody] Fboprices fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fboprices.Timestamp = DateTime.Now;
            _context.Fboprices.Add(fboprices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFboprices", new { id = fboprices.Oid }, fboprices);
        }

        // DELETE: api/Fboprices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFboprices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboprices = await _context.Fboprices.FindAsync(id);
            if (fboprices == null)
            {
                return NotFound();
            }

            _context.Fboprices.Remove(fboprices);
            await _context.SaveChangesAsync();

            return Ok(fboprices);
        }

        private bool FbopricesExists(int id)
        {
            return _context.Fboprices.Any(e => e.Oid == id);
        }

        private IQueryable<Fboprices> GetAllFboPrices()
        {
            return _context.Fboprices.AsQueryable();
        }
    }
}