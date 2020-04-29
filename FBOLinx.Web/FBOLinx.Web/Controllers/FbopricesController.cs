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
using FBOLinx.Web.Auth;
using FBOLinx.Web.DTO;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbopricesController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FuelerLinxContext _fuelerlinxContext;

        public FbopricesController(FboLinxContext context, FuelerLinxContext fuelerlinxContext)
        {
            _context = context;
            _fuelerlinxContext = fuelerlinxContext;
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
                            where f.EffectiveTo > DateTime.Now.AddDays(-1)
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

            var products = Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var activePricingCost = _context.Fboprices.FirstOrDefault(s => s.EffectiveFrom <= DateTime.Now && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Cost" && s.Fboid == fboId && s.Expired != true);
            var activePricingRetail = _context.Fboprices.FirstOrDefault(s => s.EffectiveFrom <= DateTime.Now && s.EffectiveTo > DateTime.Now.AddDays(-1) && s.Product == "JetA Retail" && s.Fboid == fboId && s.Expired != true);
            
            if (activePricingCost != null && activePricingRetail != null)
            {
                return Ok(true);
            }

            return Ok(null);
        }

        [HttpPost("fbo/{fboId}/suspendpricing/jet")]
        public async Task<IActionResult> SuspendJetPricing([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Fboprices> oldPrices = _context.Fboprices
                                                .Where(f => f.Fboid.Equals(fboId) && f.Product.Equals("JetA Cost") && f.Expired != true)
                                                .ToList();
            foreach (Fboprices oldPrice in oldPrices)
            {
                oldPrice.Expired = true;
                _context.Fboprices.Update(oldPrice);
                List<MappingPrices> checkForMappingPrices = _context.MappingPrices.Where(s => s.FboPriceId == oldPrice.Oid).ToList();
                if (checkForMappingPrices != null)
                {
                    _context.MappingPrices.RemoveRange(checkForMappingPrices);
                }
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
            List<Fboprices> oldPrices = _context.Fboprices
                                               .Where(f => f.Fboid.Equals(fboId) && f.Product.Equals("JetA Retail") && f.Expired != true)
                                               .ToList();
            foreach (Fboprices oldPrice in oldPrices)
            {
                oldPrice.Expired = true;
                _context.Fboprices.Update(oldPrice);
                List<MappingPrices> checkForMappingPrices = _context.MappingPrices.Where(s => s.FboPriceId == oldPrice.Oid).ToList();
                if (checkForMappingPrices != null)
                {
                    _context.MappingPrices.RemoveRange(checkForMappingPrices);
                }
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

            var products = Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var result = (from p in products
                          join f in (from f in _context.Fboprices
                                     where Convert.ToDateTime(f.EffectiveFrom).Date > DateTime.Now.Date && f.Expired != true
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
                              f?.Price,
                              f?.EffectiveFrom,
                              f?.EffectiveTo,
                              TimeStamp = f?.Timestamp,
                              f?.SalesTax,
                              f?.Currency,
                              groupId = m?.GroupId
                          }).OrderBy(x=>x.EffectiveFrom);

            return Ok(result);
        }

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

        [HttpPost("fbo/{fboId}/check")]
        public async Task<IActionResult> checkifExistFrboPrice([FromRoute] int fboId, [FromBody] IEnumerable<Fboprices> fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (Fboprices price in fboprices)
            {
                price.Timestamp = DateTime.Now;
                if (price.Oid > 0)
                {
                    _context.Fboprices.Update(price);
                }
                else if (price.Price != null)
                {
                    List<Fboprices> oldPrices = _context.Fboprices
                                               .Where(f => f.Fboid.Equals(price.Fboid) && f.Product.Equals(price.Product))
                                               .ToList();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        oldPrice.Expired = true;
                        _context.Fboprices.Update(oldPrice);
                    }
                    _context.Fboprices.Add(price);
                }
                await _context.SaveChangesAsync();
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
                                        f.EffectiveFrom.GetValueOrDefault().Month,
                                        f.EffectiveFrom.Value.Year
                                    }
                into results
                                    select new
                                    {
                                        results.Key.Month,
                                        results.Key.Year,
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

            try
            {
                if (FbopricesExists(id))
                {
                    _context.Entry(fboprices).State = EntityState.Modified;
                    fboprices.Timestamp = DateTime.Now;
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
                }
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

        [HttpGet("group/{groupid}/ispricingexpiredgroupadmin")]
        public async Task<IActionResult> CheckPricingIsExpiredGroupAdmin([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = FBOLinx.Web.Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));

            var groupFbos = _context.Fbos.Where(s => s.GroupId == groupId).Select(s => s.Oid).ToList();

            var activePricing = _context.Fboprices.Where(s => 
                                        s.EffectiveFrom <= DateTime.Now && 
                                        s.EffectiveTo > DateTime.Now.AddDays(-1) && 
                                        (s.Product == "JetA Cost" || s.Product == "JetA Retail") && 
                                        groupFbos.Contains(Convert.ToInt32(s.Fboid)) && 
                                        s.Expired != true).ToList();
            List<FBOGroupPriceUpdateVM> groupPriceUpdate = new List<FBOGroupPriceUpdateVM>();
            foreach (var groupFbo in groupFbos)
            {
                var isFboActivePricing = activePricing.FirstOrDefault(s => s.Fboid == groupFbo);
                
                if (isFboActivePricing == null)
                {
                    FBOGroupPriceUpdateVM gPU = new FBOGroupPriceUpdateVM();
                    gPU.FboId = groupFbo;
                    gPU.FboName = _context.Fbos.FirstOrDefault(s => s.Oid == groupFbo).Fbo;
                    groupPriceUpdate.Add(gPU);
                }
            }

            if(groupPriceUpdate.Count > 0)
            {
                return Ok(groupPriceUpdate);
            }

            return Ok(null);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpPost("volume-discounts-for-fuelerlinx")]
        public async Task<IActionResult> GetFuelPricesForFuelerlinx([FromBody] VolumeDiscountLoadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                int aircraftId = await (
                                    from ca in _context.CustomerAircrafts
                                    join c in _context.Customers
                                        on
                                        new { ca.CustomerId, FuelerlinxId = request.FuelerlinxCompanyID }
                                        equals
                                        new { CustomerId = c.Oid, FuelerlinxId = c.FuelerlinxId ?? 0 }
                                    where ca.TailNumber == request.TailNumber
                                    select ca.Oid).FirstOrDefaultAsync();

                if (aircraftId > 0)
                {
                    List<AircraftPrices> aircraftPrices = (
                                                            from f in _context.Fbos
                                                            join pt in _context.PricingTemplate on
                                                                    new { Fboid = f.Oid, GroupId = f.GroupId ?? 0 }
                                                                    equals
                                                                    new { pt.Fboid, GroupId = 1 }
                                                            join c in _context.Customers on request.FuelerlinxCompanyID equals c.FuelerlinxId
                                                            join vsd in _context.VolumeScaleDiscount on
                                                                    new { CustomerId = c.Oid, pt.Fboid }
                                                                    equals
                                                                    new { CustomerId = vsd.CustomerId ?? 0, vsd.Fboid }
                                                            join cibg in _context.CustomerInfoByGroup on
                                                                    new { CustomerId = c.Oid, GroupId = f.GroupId ?? 0 }
                                                                    equals
                                                                    new { cibg.CustomerId, cibg.GroupId }
                                                            join cdt in _context.CustomerDefaultTemplates on
                                                                    new { PricingTemplateID = pt.Oid, Fboid = f.Oid, CustomerID = c.Oid }
                                                                    equals
                                                                    new { cdt.PricingTemplateID, cdt.Fboid, cdt.CustomerID }
                                                            into leftJoinCdt
                                                            from cdt in leftJoinCdt.DefaultIfEmpty()
                                                            join ap in
                                                                    (from pt in _context.PricingTemplate
                                                                     join ap in _context.AircraftPrices on pt.Oid equals ap.PriceTemplateId
                                                                     select new
                                                                     {
                                                                         ap.CustomerAircraftId,
                                                                         pt.Fboid
                                                                     }) on
                                                                    new { CustomerAircraftId = aircraftId, Fboid = f.Oid }
                                                                    equals
                                                                    new { ap.CustomerAircraftId, ap.Fboid }
                                                            into leftJoinAp
                                                            from ap in leftJoinAp.DefaultIfEmpty()
                                                            where ap == null
                                                            select new AircraftPrices
                                                            {
                                                                CustomerAircraftId = aircraftId,
                                                                PriceTemplateId = cdt == null ? (vsd.JetAvolumeDiscount ?? 0) : cdt.PricingTemplateID
                                                            }).ToList();
                    _context.AircraftPrices.AddRange(aircraftPrices);

                    CompanyPricingLog companyPricingLog = new CompanyPricingLog
                    {
                        CompanyId = request.FuelerlinxCompanyID,
                        ICAO = request.ICAO
                    };
                    _context.CompanyPricingLog.Add(companyPricingLog);

                    _context.SaveChanges();

                    List<AircraftPrices> aircraftPricesPT = (
                                                 from a in _context.AircraftPrices
                                                 join pt in _context.PricingTemplate on a.PriceTemplateId equals pt.Oid
                                                 join fa in _context.Fboairports on request.ICAO equals fa.Icao
                                                 join f in _context.Fbos on
                                                        new { fa.Fboid, PtId = pt.Fboid, Active = true, Suspended = false }
                                                        equals
                                                        new { Fboid = f.Oid, PtId = f.Oid, Active = f.Active ?? false, Suspended = f.Suspended ?? false }
                                                 join g in _context.Group on
                                                        new { GroupId = f.GroupId ?? 0, Isfbonetwork = true }
                                                        equals
                                                        new { GroupId = g.Oid, Isfbonetwork = g.Isfbonetwork ?? false }
                                                 join c in _context.Customers on request.FuelerlinxCompanyID equals c.FuelerlinxId
                                                 join cibg in _context.CustomerInfoByGroup on
                                                        new { CustomerId = c.Oid, GroupId = g.Oid, Suspended = false, Active = true }
                                                        equals
                                                        new { cibg.CustomerId, cibg.GroupId, Suspended = cibg.Suspended ?? false, Active = cibg.Active ?? false }
                                                 join ca in _context.CustomerAircrafts on
                                                        new { CustomerId = c.Oid, request.TailNumber, GroupId = g.Oid, a.CustomerAircraftId }
                                                        equals
                                                        new { ca.CustomerId, ca.TailNumber, GroupId = ca.GroupId ?? 0, CustomerAircraftId = ca.Oid }
                                                 where pt.Fboid == f.Oid && c.ShowJetA == true
                                                 orderby a.Oid ascending
                                                 select new AircraftPrices
                                                 {
                                                     CustomerAircraftId = a.CustomerAircraftId,
                                                     PriceTemplateId = a.PriceTemplateId
                                                 }
                                                ).Take(1).ToList();

                    List<CustomerMarginPriceTier> customerMarginPrices = (
                                                from cm in _context.CustomerMargins
                                                join pt in _context.PriceTiers on cm.PriceTierId equals pt.Oid
                                                select new CustomerMarginPriceTier
                                                {
                                                    Min = pt.Min ?? 1,
                                                    TemplateId = cm.TemplateId,
                                                    Amount = cm.Amount
                                                }).ToList();

                    var volumes = (from vsd in _context.VolumeScaleDiscount
                                   from fp in _context.Fboprices
                                   where ((vsd.MarginType == 0 && vsd.Margin > 0 && fp.Product == "JetA Cost") ||
                                       (vsd.MarginType == 1 && fp.Product == "JetA Retail")) &&
                                       fp.EffectiveFrom <= DateTime.Now && fp.EffectiveTo > DateTime.Now &&
                                       fp.Expired != true
                                   select new VolumeScaleDiscountFboPrice
                                   {
                                       CustomerId = vsd.CustomerId ?? 0,
                                       Fboid = vsd.Fboid,
                                       DefaultSettings = vsd.DefaultSettings ?? false,
                                       MarginType = vsd.MarginType ?? 0,
                                       Price = fp.Price ?? 0,
                                       Margin = vsd.Margin ?? 0,
                                       SalesTax = fp.SalesTax ?? 0,
                                       ExpirationDate = fp.EffectiveTo
                                   }).ToList();

                    var result1 = await (
                        from fa in _context.Fboairports
                        join f in _context.Fbos on
                                new { fa.Fboid, Active = true, Suspended = false }
                                equals
                                new { Fboid = f.Oid, Active = f.Active ?? false, Suspended = f.Suspended ?? false }
                        join t in _context.PricingTemplate on f.Oid equals t.Fboid
                        join g in _context.Group on
                                new { GroupId = f.GroupId ?? 0, Isfbonetwork = true }
                                equals
                                new { GroupId = g.Oid, Isfbonetwork = g.Isfbonetwork ?? false }
                        join c in _context.Customers on request.FuelerlinxCompanyID equals c.FuelerlinxId ?? 0
                        join cibg in _context.CustomerInfoByGroup on
                                new { CustomerId = c.Oid, GroupId = g.Oid, Suspended = false, Active = true }
                                equals
                                new { cibg.CustomerId, cibg.GroupId, Suspended = cibg.Suspended ?? false, Active = cibg.Active ?? false }
                        join ca in _context.CustomerAircrafts on
                                new { CustomerId = c.Oid, request.TailNumber }
                                equals
                                new { ca.CustomerId, ca.TailNumber }
                        join ap in aircraftPricesPT on
                                new { CustomerAircraftId = ca.Oid, PriceTemplateId = t.Oid }
                                equals
                                new { ap.CustomerAircraftId, PriceTemplateId = ap.PriceTemplateId ?? 0 }
                        join vsd in volumes on
                                new { ca.CustomerId, Fboid = f.Oid, DefaultSettings = false }
                                equals
                                new { vsd.CustomerId, vsd.Fboid, vsd.DefaultSettings }
                        join p in customerMarginPrices on t.Oid equals p.TemplateId
                        into leftJoinP
                        from p in leftJoinP.DefaultIfEmpty()
                        where fa.Icao == request.ICAO && c.ShowJetA == true
                        select new
                        {
                            FboId = f.Oid,
                            f.Fbo,
                            GroupId = g.Oid,
                            Group = g.GroupName,
                            Product = "JetA",
                            MinVolume = (p == null ? 1 : p.Min),
                            t.Notes,
                            Default = t.Default ?? false,
                            Price = GetTPrice(vsd, p, g.Oid),
                            vsd.ExpirationDate
                        }).ToListAsync();

                    var result2 = await (
                        from fa in _context.Fboairports
                        join f in _context.Fbos on
                                new { fa.Fboid, Active = true, Suspended = false }
                                equals
                                new { Fboid = f.Oid, Active = f.Active ?? false, Suspended = f.Suspended ?? false }
                        join t in _context.PricingTemplate on f.Oid equals t.Fboid
                        join g in _context.Group on
                                new { GroupId = f.GroupId ?? 0, Isfbonetwork = false }
                                equals
                                new { GroupId = g.Oid, Isfbonetwork = g.Isfbonetwork ?? false }
                        join c in _context.Customers on request.FuelerlinxCompanyID equals c.FuelerlinxId
                        join cibg in _context.CustomerInfoByGroup on
                                new { CustomerId = c.Oid, GroupId = g.Oid, Distribute = true }
                                equals
                                new { cibg.CustomerId, cibg.GroupId, Distribute = cibg.Distribute ?? false }
                        join ca in _context.CustomerAircrafts on
                                new { CustomerId = c.Oid, request.TailNumber }
                                equals
                                new { ca.CustomerId, ca.TailNumber }
                        join ap in _context.AircraftPrices on
                                new { CustomerAircraftId = ca.Oid, PriceTemplateId = t.Oid }
                                equals
                                new { ap.CustomerAircraftId, PriceTemplateId = ap.PriceTemplateId ?? 0 }
                        into leftJoinAP
                        from ap in leftJoinAP.DefaultIfEmpty()
                        join cct in _context.CustomCustomerTypes on
                                new { cibg.CustomerId, Fboid = f.Oid }
                                equals
                                new { cct.CustomerId, cct.Fboid }
                        join fp in _context.Fboprices on f.Oid equals fp.Fboid
                        join cvbf in _context.CustomersViewedByFbo on
                                new { cibg.CustomerId, Fboid = f.Oid }
                                equals
                                new { cvbf.CustomerId, cvbf.Fboid }
                        join ff in _context.Fbofees on
                                new { Fboid = f.Oid, FeeType = 8 }
                                equals
                                new { ff.Fboid, ff.FeeType }
                        into leftJoinFF
                        from ff in leftJoinFF.DefaultIfEmpty()
                        join p in customerMarginPrices on t.Oid equals p.TemplateId
                        into leftJoinP
                        from p in leftJoinP.DefaultIfEmpty()
                        where fa.Icao == request.ICAO && (c.ShowJetA == null || c.ShowJetA == true) &&
                            ((ap.PriceTemplateId ?? 0) == 0 ? cct.CustomerType : ap.PriceTemplateId) == t.Oid &&
                            fp.Price > 0 &&
                            ((t.MarginType == PricingTemplate.MarginTypes.CostPlus && fp.Product == "JetA Cost") ||
                              (t.MarginType == PricingTemplate.MarginTypes.RetailMinus && fp.Product == "JetA Retail") ||
                              (t.MarginType == PricingTemplate.MarginTypes.FlatFee && fp.Product == "JetA Retail")) &&
                              fp.EffectiveFrom <= DateTime.Now && fp.EffectiveTo > DateTime.Now &&
                              fp.Expired != true
                        select new
                        {
                            FboId = f.Oid,
                            f.Fbo,
                            GroupId = g.Oid,
                            Group = g.GroupName,
                            Product = "JetA",
                            MinVolume = p == null ? 1 : p.Min,
                            t.Notes,
                            Default = t.Default ?? false,
                            Price = GetDTPrice(fp, p, ff, t),
                            ExpirationDate = fp.EffectiveTo
                        }).Distinct().ToListAsync();

                    var result = result1.Concat(result2)
                                        .Distinct()
                                        .ToList();

                    var clonedResult = result.Select(x => x)
                                             .OrderBy(x => x.FboId)
                                             .ThenBy(x => x.MinVolume)
                                             .ToList();
                    foreach(var price in result.Where(x => x.Default))
                    {
                        var aircraftNonDefaultPT = result.Any(x => x.FboId == price.FboId && x.GroupId == price.GroupId && x.Default);
                        if (aircraftNonDefaultPT)
                        {
                            clonedResult.Remove(price);
                        }
                    }

                    return Ok(clonedResult);
                }

                return Ok(null);
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
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
            var fbopricesRange = await _context.Fboprices.Where(x => 
                                                            x.EffectiveFrom == fboprices.EffectiveFrom && 
                                                            x.EffectiveTo == fboprices.EffectiveTo && 
                                                            x.Fboid == fboprices.Fboid
                                                            ).ToListAsync();

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

        private double GetTPrice(VolumeScaleDiscountFboPrice vsd, CustomerMarginPriceTier p, int groupId)
        {
            double? price;
            double? pAmount = (p == null || p.Amount == null) ? 0 : p.Amount;
            double? salesTax = 1 + vsd.SalesTax;
            if (vsd.MarginType == 0)
            {
                price = (vsd.Price + vsd.Margin) * salesTax + pAmount;
            }
            else if (vsd.MarginType == 1 && vsd.Margin == 0 && groupId == 1)
            {
                price = vsd.Price + pAmount;
            }
            else if (vsd.MarginType == 1 && vsd.Margin == 0)
            {
                price = vsd.Price * salesTax + pAmount;
            }
            else
            {
                price = (vsd.Price / salesTax - vsd.Margin) * salesTax + pAmount;
            }
            return price ?? 0;
        }

        private double GetDTPrice(Fboprices fp, CustomerMarginPriceTier p, Fbofees ff, PricingTemplate t)
        {
            double? price = 0;
            double? pAmount = (p == null || p.Amount == null) ? 0 : Math.Abs(p.Amount.Value);
            if (t.MarginType == null || t.MarginType == PricingTemplate.MarginTypes.CostPlus)
            {
                price = (fp.Price + pAmount) * (1 + (ff.FeeAmount.GetValueOrDefault() / 100));
            }
            else if (t.MarginType == PricingTemplate.MarginTypes.RetailMinus)
            {
                price = fp.Price - pAmount;
            }
            else if (t.MarginType == PricingTemplate.MarginTypes.FlatFee)
            {
                price = pAmount;
            }
            return price ?? 0;
        }
    }
}