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
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbopricesController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtManager _jwtManager;
        private RampFeesService _rampFeesService;

        public FbopricesController(FboLinxContext context, IHttpContextAccessor httpContextAccessor, JwtManager jwtManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _jwtManager = jwtManager;
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
                            where f.EffectiveTo > DateTime.UtcNow
                            && f.Fboid == fboId && f.Price != null && f.Expired != true
                            select f).ToListAsync();

            var oldPrices = _context.Fboprices.Where(f => f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && f.Price != null && f.Expired != true);
            foreach(var p in oldPrices)
            {
                p.Expired = true;
                _context.Fboprices.Update(p);
            }
            await _context.SaveChangesAsync();

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
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.UtcNow,
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

            var activePricingCost = _context.Fboprices.FirstOrDefault(s => s.EffectiveTo > DateTime.UtcNow && s.Product == "JetA Cost" && s.Fboid == fboId && s.Expired != true);
            var activePricingRetail = _context.Fboprices.FirstOrDefault(s => s.EffectiveTo > DateTime.UtcNow && s.Product == "JetA Retail" && s.Fboid == fboId && s.Expired != true);
            
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
                                     where f.EffectiveTo > DateTime.UtcNow && f.Expired != true
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

            var fboprices = await GetAllFboPrices().Where(f => f.Fboid == fboId &&
                                                                f.Product != null &&
                                                                f.Product.ToLower() == product.ToLower() &&
                                                                f.EffectiveTo > DateTime.UtcNow).FirstOrDefaultAsync();

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

        [HttpPost("update")]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdatePricing([FromBody] PricingUpdateRequest request)
        {
            if ((request.Retail == null && request.Cost == null) || request.ExpirationDate == null)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == "UserID")).Value);

                var user = await _context.User.FindAsync(claimedId);

                if (request.Retail != null)
                {
                    var retailPrice = new Fboprices
                    {
                        EffectiveFrom = DateTime.UtcNow,
                        EffectiveTo = request.ExpirationDate,
                        Product = "JetA Retail",
                        Price = request.Retail,
                        Fboid = user.FboId
                    };
                    List<Fboprices> oldPrices = _context.Fboprices
                                                   .Where(f => f.Fboid.Equals(user.FboId) && f.Product.Equals("JetA Retail"))
                                                   .ToList();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        oldPrice.Expired = true;
                        _context.Fboprices.Update(oldPrice);
                    }
                    _context.Fboprices.Add(retailPrice);
                }
                if (request.Cost != null)
                {
                    var costPrice = new Fboprices
                    {
                        EffectiveFrom = DateTime.UtcNow,
                        EffectiveTo = request.ExpirationDate,
                        Product = "JetA Cost",
                        Price = request.Cost,
                        Fboid = user.FboId
                    };
                    List<Fboprices> oldPrices = _context.Fboprices
                                                   .Where(f => f.Fboid.Equals(user.FboId) && f.Product.Equals("JetA Cost"))
                                                   .ToList();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        oldPrice.Expired = true;
                        _context.Fboprices.Update(oldPrice);
                    }
                    _context.Fboprices.Add(costPrice);
                }
                await _context.SaveChangesAsync();

                return Ok(new { message = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
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

            var products = Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var groupFbos = _context.Fbos.Where(s => s.GroupId == groupId && s.Active == true).Select(s => s.Oid).ToList();

            var activePricing = await _context.Fboprices.Where(s => 
                                        s.EffectiveTo > DateTime.UtcNow && 
                                        (s.Product == "JetA Cost" || s.Product == "JetA Retail") && 
                                        groupFbos.Contains(Convert.ToInt32(s.Fboid)) && 
                                        s.Expired != true).ToListAsync();
            List<FBOGroupPriceUpdateVM> groupPriceUpdate = new List<FBOGroupPriceUpdateVM>();
            foreach (var groupFbo in groupFbos)
            {
                var isFboActivePricing = activePricing.FirstOrDefault(s => s.Fboid == groupFbo);
                
                if (isFboActivePricing == null)
                {
                    FBOGroupPriceUpdateVM gPU = new FBOGroupPriceUpdateVM
                    {
                        FboId = groupFbo,
                        FboName = _context.Fbos.FirstOrDefault(s => s.Oid == groupFbo).Fbo
                    };
                    groupPriceUpdate.Add(gPU);
                }
            }

            return Ok(groupPriceUpdate);
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
                CompanyPricingLog companyPricingLog = new CompanyPricingLog
                {
                    CompanyId = request.FuelerlinxCompanyID,
                    ICAO = request.ICAO
                };
                _context.CompanyPricingLog.Add(companyPricingLog);

                _context.SaveChanges();

                var customer =
                    await _context.Customers.FirstOrDefaultAsync(x =>
                        x.FuelerlinxId == request.FuelerlinxCompanyID);
                if (customer == null)
                    return Ok(null);

                PriceFetchingService priceFetchingService = new PriceFetchingService(_context);
                List<CustomerWithPricing> validPricing =
                    await priceFetchingService.GetCustomerPricingByLocationAsync(request.ICAO, customer.Oid);
                if (validPricing == null)
                    return Ok(null);

                var customerTemplates =
                    from ct in _context.CustomCustomerTypes
                    join pt in _context.PricingTemplate on ct.CustomerType equals pt.Oid
                    select new
                    {
                        ct.CustomerId,
                        ct.Fboid,
                        ct.CustomerType,
                        PricingTemplateName = pt.Name
                    };

                var result = (
                    from p in validPricing
                    join ct in customerTemplates on new { p.CustomerId, p.FboId } equals new { ct.CustomerId, FboId = ct.Fboid }
                    into leftJoinCustomerTypes
                    from ct in leftJoinCustomerTypes.DefaultIfEmpty()
                    select new FuelPriceResponse
                    {
                        CustomerId = p.CustomerId,
                        Icao = p.Icao,
                        Iata = p.Iata,
                        FboId = p.FboId,
                        Fbo = p.Fbo,
                        GroupId = p.GroupId.GetValueOrDefault(),
                        Group = p.Group,
                        Product = "JetA",
                        MinVolume = p.MinGallons.GetValueOrDefault(),
                        Notes = p.Notes,
                        Default = string.IsNullOrEmpty(p.TailNumbers),
                        Price = p.AllInPrice.GetValueOrDefault(),
                        TailNumberList = p.TailNumbers,
                        ExpirationDate = p.ExpirationDate,
                        PricingTemplateId = p.PricingTemplateId.GetValueOrDefault() == 0 ? ct.CustomerType : p.PricingTemplateId.GetValueOrDefault(),
                        PricingTemplateName = p.PricingTemplateId.GetValueOrDefault() == 0 ? ct.PricingTemplateName : p.PricingTemplateName,
                        FuelDeskEmail = p.FuelDeskEmail,
                        CopyEmails = p.CopyEmails
                    }).Distinct();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("volume-discounts-for-customer")]
        public async Task<IActionResult> GetFuelPricesForCustomer([FromBody] TailNumberLoadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var customerAircraft = _context.CustomerAircrafts.FirstOrDefault(s => s.TailNumber == request.TailNumber && s.GroupId == request.GroupID);
                if (customerAircraft == null)
                    return Ok(null);

                PriceFetchingService priceFetchingService = new PriceFetchingService(_context);
                var validPricingList =
                    await priceFetchingService.GetCustomerPricingByLocationAsync(request.ICAO, customerAircraft.CustomerId);
                if (validPricingList == null)
                    return Ok(null);

                var priceResult = validPricingList.FirstOrDefault(x =>
                    !string.IsNullOrEmpty(x.TailNumbers) &&
                    x.TailNumbers.ToUpper().Split(',').Contains(request.TailNumber.ToUpper()) && x.MinGallons <= request.FuelVolume && x.MaxGallons >= request.FuelVolume && x.FboId == request.FBOID);

                if (priceResult == null)
                    return Ok(null);

                TailNumberLoadResponse validPricing = new TailNumberLoadResponse();
                validPricing.PricePerGallon = priceResult.AllInPrice;
                validPricing.Template = priceResult.PricingTemplateName;
                validPricing.Company = priceResult.Company;

                var custAircraftMakeModel = _context.Aircrafts.FirstOrDefault(s => s.AircraftId == customerAircraft.AircraftId);

                if(custAircraftMakeModel != null)
                {
                    validPricing.MakeModel = custAircraftMakeModel.Make + " " + custAircraftMakeModel.Model;
                }

                return Ok(validPricing);
            }
            catch (Exception ex)
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
    }
}