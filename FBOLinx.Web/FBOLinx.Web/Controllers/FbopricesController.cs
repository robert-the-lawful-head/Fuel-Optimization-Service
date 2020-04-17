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
using FBOLinx.Web.ViewModels;

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

            fboprices.Id = _context.MappingPrices.Where(x => x.FboPriceId == fboprices.Oid).Select(x => x.GroupId).FirstOrDefault();
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

            var products = Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var fboprices = await (
                            from f in _context.Fboprices
                            where Convert.ToDateTime(f.EffectiveFrom).Date <= DateTime.Now.Date && f.EffectiveTo > DateTime.Now.AddDays(-1)
                            && f.Fboid == fboId && f.Price != null && f.Expired != true
                            select f).ToListAsync();
            var addOnMargins = await (
                            from s in _context.TempAddOnMargin
                            where s.FboId == fboId && s.EffectiveTo >= DateTime.Today.ToUniversalTime()
                            select s).ToListAsync();

            var result = (from p in products
                          join f in fboprices on
                                new { Product = p.Description, FboId = fboId }
                                equals
                                new { f.Product, FboId = f.Fboid.GetValueOrDefault() }
                          into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in addOnMargins on new { FboId = fboId } equals new { s.FboId }
                          into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          select new
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.Now,
                              EffectiveTo = f?.EffectiveTo ?? null,
                              TimeStamp = f?.Timestamp,
                              f?.SalesTax,
                              f?.Currency,
                              tempJet = s?.MarginJet,
                              tempAvg = s?.MarginAvgas,
                              tempId = s?.Id,
                              tempDateFrom = s?.EffectiveFrom,
                              tempDateTo = s?.EffectiveTo
                          })
                          .GroupBy(p => p.Product)
                          .Select(p => p.OrderByDescending(q => q.Oid).FirstOrDefault())
                          .ToList();
            return Ok(result);
        }

        // GET: api/Fboprices/fbo/current/5
        [HttpGet("fbo/{fboId}/ispricingexpired")]
        public async Task<IActionResult> CheckPricingIsExpired([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));


            var activePricing = _context.Fboprices.FirstOrDefault(s => s.EffectiveFrom <= DateTime.Now && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Cost" && s.Fboid == fboId);

            return Ok(activePricing);
        }

        [HttpPost("fbo/{fboId}/suspendpricing/jet")]
        public async Task<IActionResult> SuspendJetPricing([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var activeJetPricing = _context.Fboprices.FirstOrDefault(s => Convert.ToDateTime(s.EffectiveFrom).Date <= DateTime.Now.Date && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Cost" && s.Fboid == fboId);
            if (activeJetPricing != null)
            {
                var checkForMappingPrices = _context.MappingPrices.Where(s => s.FboPriceId == activeJetPricing.Oid).ToList();
                if (checkForMappingPrices != null)
                {
                    _context.MappingPrices.RemoveRange(checkForMappingPrices);
                    await _context.SaveChangesAsync();
                }

                _context.Fboprices.Remove(activeJetPricing);
            }
            await _context.SaveChangesAsync();

            return Ok(fboId);
        }

        [HttpPost("fbo/{fboId}/suspendpricing/retail")]
        public async Task<IActionResult> SuspendRetailPricing([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var activeRetailPricing = _context.Fboprices.FirstOrDefault(s => Convert.ToDateTime(s.EffectiveFrom).Date <= DateTime.Now.Date && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Retail" && s.Fboid == fboId);
            if (activeRetailPricing != null)
            {
                var checkForMappingPrices = _context.MappingPrices.Where(s => s.FboPriceId == activeRetailPricing.Oid).ToList();
                if (checkForMappingPrices != null)
                {
                    _context.MappingPrices.RemoveRange(checkForMappingPrices);
                    await _context.SaveChangesAsync();
                }

                _context.Fboprices.Remove(activeRetailPricing);
            }
            await _context.SaveChangesAsync();

            return Ok(fboId);
        }


        // GET: api/Fboprices/fbo/current/5
        [HttpPost("fbo/{fboId}/suspendpricing")]
        public async Task<IActionResult> SuspendPricing([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var activeJetPricing = _context.Fboprices.FirstOrDefault(s => Convert.ToDateTime(s.EffectiveFrom).Date <= DateTime.Now.Date && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Cost" && s.Fboid == fboId);
            if (activeJetPricing != null)
            {
                var checkForMappingPrices = _context.MappingPrices.Where(s => s.FboPriceId == activeJetPricing.Oid).ToList();
                if (checkForMappingPrices != null)
                {
                    _context.MappingPrices.RemoveRange(checkForMappingPrices);
                    await _context.SaveChangesAsync();
                }

                _context.Fboprices.Remove(activeJetPricing);
            }
            var activeRetailPricing = _context.Fboprices.FirstOrDefault(s => Convert.ToDateTime(s.EffectiveFrom).Date <= DateTime.Now.Date && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Retail" && s.Fboid == fboId);
            if (activeRetailPricing != null)
            {
                var checkForMappingPrices = _context.MappingPrices.Where(s => s.FboPriceId == activeRetailPricing.Oid).ToList();
                if (checkForMappingPrices != null)
                {
                    _context.MappingPrices.RemoveRange(checkForMappingPrices);
                    await _context.SaveChangesAsync();
                }

                _context.Fboprices.Remove(activeRetailPricing);
            }
            await _context.SaveChangesAsync();

            return Ok(fboId);
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
                                         //where f.EffectiveFrom > DateTime.Now
                                     where Convert.ToDateTime(f.EffectiveFrom).Date > DateTime.Now.Date
                                     select f) on new { Product = p.Description, FboId = fboId } equals new
                                     {
                                         f.Product,
                                         FboId = f.Fboid.GetValueOrDefault()
                                     }
                              into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join m in _context.MappingPrices on (f?.Oid).GetValueOrDefault() equals m.FboPriceId
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
                              Currency = f?.Currency,
                              groupId = m?.GroupId
                          }).OrderBy(x=>x.EffectiveFrom);

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
                                                                //f.EffectiveFrom <= DateTime.Now && f.EffectiveTo > DateTime.Now.AddDays(-1))).FirstOrDefaultAsync();
                                                                f.EffectiveTo > DateTime.Now.AddDays(-1))).FirstOrDefaultAsync();

            return Ok(fboprices);
        }

        // POST: api/Fboprices/fbo/5/check/from/12.10.2019/to/18.10.2019
        [HttpPost("fbo/{fboId}/check")]
        public async Task<IActionResult> checkifExistFrboPrice([FromRoute] int fboId, [FromBody] IEnumerable<Fboprices> vfboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pm = vfboprices.FirstOrDefault();
            int num = 0;
            if (pm.EffectiveFrom > DateTime.Now)
            {
                var fboprices = await GetAllFboPrices().Where(f => f.Fboid == fboId && f.Price != null && ((pm.EffectiveFrom >= f.EffectiveFrom && pm.EffectiveFrom < f.EffectiveTo) || (pm.EffectiveTo > f.EffectiveFrom && pm.EffectiveTo < f.EffectiveTo))).FirstOrDefaultAsync();
                foreach (var pom in vfboprices)
                {
                    MappingPrices map = new MappingPrices();
                    pom.Timestamp = DateTime.Now;
                    _context.Fboprices.Add(pom);
                    await _context.SaveChangesAsync();
                    if (num == 0)
                    {
                        num = _context.MappingPrices.Select(x => x.GroupId).DefaultIfEmpty(0).Max() + 1;
                    }
                    map.GroupId = num;
                    map.FboPriceId = pom.Oid;
                    _context.MappingPrices.Add(map);
                    await _context.SaveChangesAsync();
                }

                return Ok(fboprices);
            }
            else
            {
                try
                {
                    var fboprices = await GetAllFboPrices().Where(f => f.Fboid == fboId && f.Price != null && ((pm.EffectiveFrom >= f.EffectiveFrom && pm.EffectiveFrom < f.EffectiveTo) || (pm.EffectiveTo > f.EffectiveFrom && pm.EffectiveTo < f.EffectiveTo))).FirstOrDefaultAsync();

                    if (fboprices == null)
                    {
                        foreach (var pom in vfboprices)
                        {
                            MappingPrices map = new MappingPrices();
                            pom.Timestamp = DateTime.Now;
                            _context.Fboprices.Add(pom);
                            await _context.SaveChangesAsync();
                            if (num == 0)
                            {
                                num = _context.MappingPrices.Select(x => x.GroupId).DefaultIfEmpty(0).Max() + 1;
                            }
                            map.GroupId = num;
                            map.FboPriceId = pom.Oid;
                            _context.MappingPrices.Add(map);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        foreach (var price in vfboprices)
                        {
                            var firstPrice = price;
                            var fboprice = await GetAllFboPrices().Where(f => f.Fboid == fboId && f.Price != null && f.Product == price.Product).OrderByDescending(s => s.Oid).FirstOrDefaultAsync();
                            if (fboprice != null)
                            {

                                fboprice.Timestamp = DateTime.Now;
                                fboprice.EffectiveFrom = price.EffectiveFrom;
                                fboprice.EffectiveTo = price.EffectiveTo;
                                fboprice.Price = price.Price;
                                _context.Fboprices.Update(fboprice);
                                await _context.SaveChangesAsync();

                            }
                        }

                        return Ok(vfboprices);
                    }
                }
                catch (Exception ex)
                {
                    var exxx = ex.Message;
                }

            }

            return Ok(null);
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

         //   _context.Entry(fboprices).State = EntityState.Modified;

            try
            {
                if (FbopricesExists(id))
                {
                    _context.Entry(fboprices).State = EntityState.Modified;
                    fboprices.Timestamp = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Fboprices newFboPrice = new Fboprices();
                    newFboPrice.Currency = fboprices.Currency;
                    newFboPrice.EffectiveFrom = fboprices.EffectiveFrom;
                    newFboPrice.EffectiveTo = fboprices.EffectiveTo;
                    newFboPrice.Fboid = fboprices.Fboid;
                    newFboPrice.Price = fboprices.Price;
                    newFboPrice.Product = fboprices.Product;
                    newFboPrice.SalesTax = fboprices.SalesTax;
                    newFboPrice.Timestamp = DateTime.Now;
                    newFboPrice.Timestamp = DateTime.Now;
                    _context.Fboprices.Add(newFboPrice);
                    await _context.SaveChangesAsync();
                }
                //fboprices.Timestamp = DateTime.Now;
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
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
            int num = 0;
            MappingPrices map = new MappingPrices();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fboprices.Timestamp = DateTime.Now;
            _context.Fboprices.Add(fboprices);
            await _context.SaveChangesAsync();
            if (fboprices.Id == null)
            {
                num = _context.MappingPrices.Select(x => x.GroupId).DefaultIfEmpty(0).Max() + 1;
            }
            map.GroupId = fboprices.Id ?? num;
            map.FboPriceId = fboprices.Oid;
            _context.MappingPrices.Add(map);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFboprices", new { id = fboprices.Oid }, fboprices);

        }

        // GET: api/Fboprices/fbo/current/5
        [HttpGet("group/{groupid}/ispricingexpiredgroupadmin")]
        public async Task<IActionResult> CheckPricingIsExpiredGroupAdmin([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));

            var groupFbos = _context.Fbos.Where(s => s.GroupId == groupId).Select(s => s.Oid).ToList();

            var activePricing = _context.Fboprices.Where(s => s.EffectiveFrom <= DateTime.Now && s.EffectiveTo > DateTime.Now.AddDays(-1) && (s.Product == "JetA Cost" || s.Product == "JetA Retail") && groupFbos.Contains(Convert.ToInt32(s.Fboid)));

            if (activePricing.ToList().Count.Equals(0))
            {
                List<FBOGroupPriceUpdateVM> groupPriceUpdate = new List<FBOGroupPriceUpdateVM>();

                foreach (var groupFbo in groupFbos)
                {
                    FBOGroupPriceUpdateVM gPU = new FBOGroupPriceUpdateVM();
                    gPU.FboId = groupFbo;
                    gPU.FboName = _context.Fbos.FirstOrDefault(s => s.Oid == groupFbo).Fbo;
                    groupPriceUpdate.Add(gPU);
                }

                return Ok(groupPriceUpdate);
            }


            return Ok(null);
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
            var fbopricesRange = await _context.Fboprices.Where(x => x.EffectiveFrom == fboprices.EffectiveFrom && x.EffectiveTo == fboprices.EffectiveTo && x.Fboid == fboprices.Fboid).ToListAsync();
            //_context.Fboprices.Remove(fboprices);
            _context.Fboprices.RemoveRange(fbopricesRange);
            await _context.SaveChangesAsync();

            return Ok(fbopricesRange);
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