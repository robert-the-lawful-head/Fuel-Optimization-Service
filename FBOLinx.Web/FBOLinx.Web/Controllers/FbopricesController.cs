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
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.Web.Services.Interfaces;
using static FBOLinx.DB.Models.Fboprices;

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
        private readonly RampFeesService _RampFeesService;
        private readonly AircraftService _aircraftService;
        private IPriceFetchingService _PriceFetchingService;
        private readonly FbopricesService _fbopricesService;
        private readonly DateTimeService _dateTimeService;
        private readonly FboService _fboService;
        private readonly FboPreferencesService _fboPreferencesService;

        public FbopricesController(
            FboLinxContext context,
            IHttpContextAccessor httpContextAccessor,
            JwtManager jwtManager, 
            RampFeesService rampFeesService, 
            AircraftService aircraftService, 
            IPriceFetchingService priceFetchingService,
            FbopricesService fbopricesService,
            DateTimeService dateTimeService,
            FboService fboService,
            FboPreferencesService fboPreferencesService)
        {
            _PriceFetchingService = priceFetchingService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _jwtManager = jwtManager;
            _RampFeesService = rampFeesService;
            _aircraftService = aircraftService;
            _fbopricesService = fbopricesService;
            _dateTimeService = dateTimeService;
            _fboService = fboService;
            _fboPreferencesService = fboPreferencesService;
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

            var result = await _fbopricesService.GetPrices(fboId);

            var filteredResult = result.Where(f => f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null).ToList();

            foreach(var price in filteredResult)
            {
                if (price.Price != null)
                {
                    price.EffectiveFrom = await _fboService.GetAirportLocalDateTimeByUtcFboId(price.EffectiveFrom, fboId);
                    price.EffectiveTo = await _fboService.GetAirportLocalDateTimeByUtcFboId(price.EffectiveTo.GetValueOrDefault(), fboId);
                }
            }

            return Ok(filteredResult);
        }

        // GET: api/Fboprices/fbo/staged/5
        [HttpGet("fbo/{fboId}/staged")]
        public async Task<IActionResult> GetFbopricesByFboIdStaged([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _fbopricesService.GetPrices(fboId);

            var filteredResult = result.Where(f => f.EffectiveFrom > DateTime.UtcNow || f.EffectiveTo == null).ToList();
            return Ok(filteredResult);
        }

        // GET: api/Fboprices/fbo/all-staged/5
        [HttpGet("fbo/{fboId}/all-staged")]
        public async Task<IActionResult> GetFbopricesByFboIdAllStaged([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prices = new List<FboPricesUpdateGenerator>();
            var fboProducts = await _fboPreferencesService.GetFboProducts(fboId);
            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes)).ToArray();
            var result = await _fbopricesService.GetPrices(fboId);

            foreach (var product in fboProducts)
            {
                var fboPricesUpdateGenerator = new FboPricesUpdateGenerator();

                var filteredResultCost = result.Where(f => f.Product == product.ToString() + " Cost" && (f.EffectiveFrom > DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();
                var filteredResultRetail = result.Where(f => f.Product == product.ToString() + " Retail" && (f.EffectiveFrom > DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();

                if ((filteredResultCost == null || filteredResultCost.Oid == 0) && (filteredResultRetail == null || filteredResultRetail.Oid == 0))
                {
                    fboPricesUpdateGenerator.Product = product.ToString();
                    fboPricesUpdateGenerator.Fboid = fboId;
                }
                else
                {
                    if (filteredResultCost != null)
                    {
                        fboPricesUpdateGenerator.OidCost = filteredResultCost.Oid;
                        fboPricesUpdateGenerator.PriceCost = filteredResultCost.Price;
                        fboPricesUpdateGenerator.Product = product.ToString();
                        fboPricesUpdateGenerator.Fboid = fboId;
                        fboPricesUpdateGenerator.EffectiveFrom = filteredResultCost.EffectiveFrom;
                        fboPricesUpdateGenerator.EffectiveTo = filteredResultCost.EffectiveTo;
                    }

                    if (filteredResultRetail != null)
                    {
                        fboPricesUpdateGenerator.OidPap = filteredResultRetail.Oid;
                        fboPricesUpdateGenerator.PricePap = filteredResultRetail.Price;

                        if (fboPricesUpdateGenerator.Product == "")
                        {
                            fboPricesUpdateGenerator.Product = product.ToString();
                            fboPricesUpdateGenerator.Fboid = fboId;
                            fboPricesUpdateGenerator.EffectiveFrom = filteredResultRetail.EffectiveFrom;
                            fboPricesUpdateGenerator.EffectiveTo = filteredResultRetail.EffectiveTo;
                        }
                    }

                    fboPricesUpdateGenerator.EffectiveFrom = await _fboService.GetAirportLocalDateTimeByUtcFboId(fboPricesUpdateGenerator.EffectiveFrom, fboId);
                    fboPricesUpdateGenerator.EffectiveTo = await _fboService.GetAirportLocalDateTimeByUtcFboId(fboPricesUpdateGenerator.EffectiveTo.GetValueOrDefault(), fboId);
                }

                prices.Add(fboPricesUpdateGenerator);
            }

            return Ok(prices);
        }

        // GET: api/Fboprices/fbo/5/ispricingexpired
        [HttpGet("fbo/{fboId}/ispricingexpired")]
        public async Task<IActionResult> CheckPricingIsExpired([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var activePricingCost = await _context.Fboprices.FirstOrDefaultAsync(s => s.EffectiveTo > DateTime.UtcNow && s.Product == "JetA Cost" && s.Fboid == fboId && s.Expired != true);
            var activePricingRetail = await _context.Fboprices.FirstOrDefaultAsync(s => s.EffectiveTo > DateTime.UtcNow && s.Product == "JetA Retail" && s.Fboid == fboId && s.Expired != true);
            
            if (activePricingCost != null && activePricingRetail != null)
            {
                return Ok(true);
            }

            return Ok(null);
        }

        // GET: api/Fboprices/getallfboswithexpiredretailpricing
        [HttpGet("getallfboswithexpiredretailpricing")]
        public async Task<IActionResult> GetAllFbosWithExpiredRetailPricing([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboIdsWithExpiredPrice = new List<int>();
            var fbos = await GetAllActiveFbos().Include(f => f.Users).Include("fboAirport").Where(x => x.GroupId > 1).ToListAsync();

            foreach (var fbo in fbos)
            {
                var retailPricing = await _context.Fboprices.Where(s => s.Product == "JetA Retail" && s.Fboid == fbo.Oid).OrderByDescending(t => t.Oid).FirstOrDefaultAsync();

                if (retailPricing != null && retailPricing.Expired.GetValueOrDefault())
                    fboIdsWithExpiredPrice.Add(fbo.Oid);
            }

            var fbosWithExpiredPricing = fbos.Where(t => fboIdsWithExpiredPrice.Contains(t.Oid)).Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport?.Icao,
                Oid = f.Oid,
                GroupId = f.GroupId ?? 0,
                Users = f.Users
            }).ToList();

            return Ok(fbosWithExpiredPricing);
        }

        [HttpPost("suspendpricing/{oid}")]
        public async Task<IActionResult> SuspendPricing([FromRoute] int oid)    
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Fboprices price = _context.Fboprices.Where(f => f.Oid == oid).FirstOrDefault();
            _context.Fboprices.Remove(price);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("suspend-pricing-generator")]
        public async Task<IActionResult> SuspendPricingGenerator([FromBody] FboPricesUpdateGenerator fboPricesUpdateGenerator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Fboprices> prices = await _context.Fboprices.Where(f => f.Oid == fboPricesUpdateGenerator.OidCost || f.Oid == fboPricesUpdateGenerator.OidPap).ToListAsync();
            _context.Fboprices.RemoveRange(prices);

            await _context.SaveChangesAsync();

            return Ok();
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
        public async Task<IActionResult> checkifExistFboPrice([FromRoute] int fboId, [FromBody] IEnumerable<Fboprices> fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (Fboprices price in fboprices)
            {
                if (price.Price != null)
                {
                    price.Timestamp = DateTime.UtcNow;
                    List<Fboprices> oldPrices = await _context.Fboprices
                                            .Where(f => f.EffectiveFrom <= DateTime.UtcNow && f.Fboid.Equals(price.Fboid) && f.Product.Equals(price.Product) && !f.Expired.Equals(true))
                                            .ToListAsync();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        oldPrice.Expired = true;
                        _context.Fboprices.Update(oldPrice);
                    }

                    _context.Fboprices.Add(price);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(null);
        }

        [HttpPost("fbo/{fboId}/checkstaged")]
        public async Task<IActionResult> checkifExistStagedFboPrice([FromRoute] int fboId, [FromBody] IEnumerable<Fboprices> fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (Fboprices price in fboprices)
            {
                if (price.Price != null)
                {
                    price.Timestamp = DateTime.Now;
                    if (price.Oid > 0)
                    {
                        _context.Fboprices.Update(price);
                    }
                    else if (price.Price != null)
                    {
                        _context.Fboprices.Add(price);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(null);
        }

        [HttpPost("update")]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdatePricing([FromBody] PricingUpdateRequest request)
        {
            if (request.Retail == null && request.Cost == null)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _context.User.FindAsync(claimedId);

                var effectiveFrom = request.EffectiveDate != null ? request.EffectiveDate : DateTime.UtcNow;
                var effectiveTo = request.ExpirationDate != null ? request.ExpirationDate : DateTime.MaxValue;

                if (request.Retail != null)
                {
                    var retailPrice = new Fboprices
                    {
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        Product = "JetA Retail",
                        Price = request.Retail,
                        Fboid = user.FboId
                    };
                    List<Fboprices> oldPrices = await _context.Fboprices
                                                   .Where(f => f.Fboid.Equals(user.FboId) && f.Product.Equals("JetA Retail"))
                                                   .ToListAsync();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        if (oldPrice.Expired != true && oldPrice.EffectiveTo > effectiveTo)
                        {
                            oldPrice.EffectiveTo = effectiveTo;
                        }
                        oldPrice.Expired = true;
                        _context.Fboprices.Update(oldPrice);
                    }
                    _context.Fboprices.Add(retailPrice);
                }
                if (request.Cost != null)
                {
                    var costPrice = new Fboprices
                    {
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        Product = "JetA Cost",
                        Price = request.Cost,
                        Fboid = user.FboId
                    };
                    List<Fboprices> oldPrices = await _context.Fboprices
                                                   .Where(f => f.Fboid.Equals(user.FboId) && f.Product.Equals("JetA Cost"))
                                                   .ToListAsync();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        if (oldPrice.Expired != true && oldPrice.EffectiveTo > effectiveTo)
                        {
                            oldPrice.EffectiveTo = effectiveTo;
                        }
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

        [HttpPost("update/stage")]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdateStagePricing([FromBody] PricingUpdateRequest request)
        {
            if (request.Retail == null && request.Cost == null)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _context.User.FindAsync(claimedId);

                var effectiveFrom = request.EffectiveDate != null ? request.EffectiveDate : DateTime.UtcNow;
                var effectiveTo = request.ExpirationDate != null ? request.ExpirationDate : DateTime.MaxValue;

                if (request.Retail != null)
                {
                    var retailPrice = new Fboprices
                    {
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        Product = "JetA Retail",
                        Price = request.Retail,
                        Fboid = user.FboId
                    };

                    _context.Fboprices.Add(retailPrice);
                }
                if (request.Cost != null)
                {
                    var costPrice = new Fboprices
                    {
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        Product = "JetA Cost",
                        Price = request.Cost,
                        Fboid = user.FboId
                    };
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

            var fboPricesByMonth = await (from f in _context.Fboprices
                                    where f.Product.ToLower() == request.Product.ToLower()
                                          && f.Fboid == fboId
                                          && f.EffectiveFrom >= request.StartDateTime
                                          && f.EffectiveFrom <= request.EndDateTime
                                    group f by new
                                    {
                                        f.EffectiveFrom.Value.Month,
                                        f.EffectiveFrom.Value.Year
                                    }
                into results
                                    select new
                                    {
                                        results.Key.Month,
                                        results.Key.Year,
                                        AveragePrice = results.Average((x => x.Price))
                                    }).ToListAsync();

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

            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));

            var groupFbos = _context.Fbos.Where(s => s.GroupId == groupId && s.Active == true).Select(s => s.Oid).ToList();

            var activePricing = await _context.Fboprices.Where(s => 
                                        s.EffectiveTo > DateTime.UtcNow && 
                                        (s.Product == "JetA Cost" || s.Product == "JetA Retail") && 
                                        groupFbos.Any(g => g == s.Fboid) && 
                                        s.Expired != true).ToListAsync();
            List<FBOGroupPriceUpdateVM> groupPriceUpdate = new List<FBOGroupPriceUpdateVM>();
            var fbos = await _context.Fbos.Where(x => groupFbos.Any(f => f == x.Oid)).ToListAsync();
            foreach (var groupFbo in groupFbos)
            {
                var isFboActivePricing = activePricing.FirstOrDefault(s => s.Fboid == groupFbo);
                
                if (isFboActivePricing == null)
                {
                    FBOGroupPriceUpdateVM gPU = new FBOGroupPriceUpdateVM
                    {
                        FboId = groupFbo,
                        FboName = fbos.FirstOrDefault(s => s.Oid == groupFbo).Fbo
                    };
                    groupPriceUpdate.Add(gPU);
                }
            }

            return Ok(groupPriceUpdate);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpPost("price-lookup-for-fuelerlinx")]
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

                await _context.SaveChangesAsync();

                var customer =
                    await _context.Customers.Where(x =>
                        x.FuelerlinxId == request.FuelerlinxCompanyID
                        ).OrderBy(x => ((!x.GroupId.HasValue) ? 0 : x.GroupId.Value))
                        .FirstOrDefaultAsync();
                if (customer == null)
                    return Ok(null);

                
                List<CustomerWithPricing> validPricing =
                    await _PriceFetchingService.GetCustomerPricingByLocationAsync(request.ICAO, customer.Oid, (FBOLinx.Core.Enums.FlightTypeClassifications) request.FlightTypeClassification, Core.Enums.ApplicableTaxFlights.All, null, 0);
                if (validPricing == null)
                    return Ok(null);

                var customerTemplates = await
                    (from ct in _context.CustomCustomerTypes
                    join pt in _context.PricingTemplate on ct.CustomerType equals pt.Oid
                    select new
                    {
                        ct.CustomerId,
                        ct.Fboid,
                        ct.CustomerType,
                        PricingTemplateName = pt.Name
                    }).ToListAsync();

                var result = (
                    from p in validPricing
                    join ct in customerTemplates on new { p.CustomerId, p.FboId } equals new { ct.CustomerId, FboId = ct.Fboid }
                    into leftJoinCustomerTypes
                    from ct in leftJoinCustomerTypes.DefaultIfEmpty()
                    select new Models.Responses.FuelPriceResponse
                    {
                        CustomerId = p.CustomerId,
                        Icao = p.Icao,
                        Iata = p.Iata,
                        FboId = p.FboId,
                        Fbo = p.Fbo,
                        GroupId = (p.GroupId ?? 0),
                        Group = p.Group,
                        Product = p.Product,
                        MinVolume = (p.MinGallons ?? 0),
                        Notes = p.Notes,
                        Default = string.IsNullOrEmpty(p.TailNumbers),
                        Price = (p.AllInPrice ?? 0),
                        TailNumberList = p.TailNumbers,
                        ExpirationDate = p.ExpirationDate,
                        PricingTemplateId = (p.PricingTemplateId ?? 0) == 0 ? ct.CustomerType : (p.PricingTemplateId ?? 0),
                        PricingTemplateName = (p.PricingTemplateId ?? 0) == 0 ? ct.PricingTemplateName : p.PricingTemplateName,
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

        [HttpPost("price-lookup-for-customer")]
        public async Task<IActionResult> GetFuelPricesForCustomer([FromBody] PriceLookupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                PriceLookupResponse validPricing = new PriceLookupResponse();

                if (!string.IsNullOrEmpty(request.TailNumber))
                {
                    var customerInfoByGroup = await _context.CustomerInfoByGroup.FirstOrDefaultAsync(c => c.Oid == request.CustomerInfoByGroupId);
                    if (customerInfoByGroup == null)
                        return Ok(null);

                    var customerAircraft = await _context.CustomerAircrafts.FirstOrDefaultAsync(s => s.TailNumber == request.TailNumber && s.GroupId == request.GroupID && s.CustomerId == customerInfoByGroup.CustomerId);
                    if (customerAircraft == null)
                        return Ok(null);

                    var validPricingList =
                        await _PriceFetchingService.GetCustomerPricingByLocationAsync(request.ICAO, customerAircraft.CustomerId, request.FlightTypeClassification, request.DepartureType, request.ReplacementFeesAndTaxes, request.FBOID);
                    if (validPricingList == null)
                        return Ok(null);

                    validPricing.PricingList = validPricingList.Where(x =>
                        !string.IsNullOrEmpty(x.TailNumbers) &&
                        x.TailNumbers.ToUpper().Split(',').Contains(request.TailNumber.ToUpper())  && x.FboId == request.FBOID &&
                        (request.CustomerInfoByGroupId == x.CustomerInfoByGroupId)).ToList();
                    var custAircraftMakeModel = await _aircraftService.GetAllAircraftsAsQueryable().FirstOrDefaultAsync(s => s.AircraftId == customerAircraft.AircraftId);

                    if (custAircraftMakeModel != null)
                    {
                        validPricing.MakeModel = custAircraftMakeModel.Make + " " + custAircraftMakeModel.Model;
                    }
                    validPricing.RampFee = await _RampFeesService.GetRampFeeForAircraft(request.FBOID, request.TailNumber);
                } 
                else
                {
                    var customerInfoByGroup = await _context.CustomerInfoByGroup
                        .Where(x => x.GroupId == request.GroupID && ((x.Active.HasValue && x.Active.Value && request.CustomerInfoByGroupId == 0) || (request.CustomerInfoByGroupId > 0 && x.Oid == request.CustomerInfoByGroupId)))
                        .Include(x => x.Customer)
                        .Where(x => !x.Customer.Suspended.HasValue || !x.Customer.Suspended.Value)
                        .FirstOrDefaultAsync();
                    validPricing.PricingList = await _PriceFetchingService.GetCustomerPricingAsync(request.FBOID, request.GroupID, customerInfoByGroup?.Oid > 0 ? customerInfoByGroup.Oid : 0, new List<int>() { request.PricingTemplateID }, request.FlightTypeClassification, request.DepartureType, request.ReplacementFeesAndTaxes);
                }

                if (validPricing.PricingList == null || validPricing.PricingList.Count == 0)
                    return Ok(null);
                
                validPricing.Template = validPricing.PricingList[0].PricingTemplateName;
                validPricing.Company = validPricing.PricingList[0].Company;                

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

        // POST: api/Fboprices/update-price-generator
        [HttpPost("update-price-generator")]
        public async Task<IActionResult> PostPriceGenerator([FromBody] FboPricesUpdateGenerator fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isStaged = true;

            try
            {
                var utcEffectiveFrom = await _dateTimeService.ConvertLocalTimeToUtc(fboprices.Fboid, fboprices.EffectiveFrom);
                var utcEffectiveTo = await _dateTimeService.ConvertLocalTimeToUtc(fboprices.Fboid, fboprices.EffectiveTo.GetValueOrDefault());

                if (utcEffectiveFrom <= DateTime.UtcNow)
                {
                    List<Fboprices> oldPrices = await _context.Fboprices
                                           .Where(f => f.EffectiveFrom <= DateTime.UtcNow && f.Fboid.Equals(fboprices.Fboid) && f.Product.Contains(fboprices.Product) && !f.Expired.Equals(true))
                                           .ToListAsync();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        oldPrice.Expired = true;
                        _context.Fboprices.Update(oldPrice);
                    }

                    isStaged = false;
                }

                if (FbopricesExists(fboprices.OidPap))
                {
                    var fboPrice = await _context.Fboprices.Where(f => f.Oid == fboprices.OidPap).FirstOrDefaultAsync();
                    fboPrice.Price = fboprices.PricePap;
                    fboPrice.EffectiveFrom = utcEffectiveFrom;
                    fboPrice.EffectiveTo = utcEffectiveTo;
                    fboPrice.Timestamp = DateTime.Now;
                    _context.Entry(fboPrice).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Fboprices newFboPrice = new Fboprices();
                    newFboPrice.EffectiveFrom = utcEffectiveFrom;
                    newFboPrice.EffectiveTo = utcEffectiveTo;
                    newFboPrice.Fboid = fboprices.Fboid;
                    newFboPrice.Price = fboprices.PricePap;
                    newFboPrice.Product = fboprices.Product + " Retail";
                    newFboPrice.Timestamp = DateTime.Now;
                    _context.Fboprices.Add(newFboPrice);
                    await _context.SaveChangesAsync();
                }

                if (FbopricesExists(fboprices.OidCost))
                {
                    var fboPrice = await _context.Fboprices.Where(f => f.Oid == fboprices.OidCost).FirstOrDefaultAsync();
                    fboPrice.Price = fboprices.PriceCost;
                    fboPrice.EffectiveFrom = utcEffectiveFrom;
                    fboPrice.EffectiveTo = utcEffectiveTo;
                    fboPrice.Timestamp = DateTime.Now;
                    _context.Entry(fboPrice).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Fboprices newFboPrice = new Fboprices();
                    newFboPrice.EffectiveFrom = utcEffectiveFrom;
                    newFboPrice.EffectiveTo = utcEffectiveTo;
                    newFboPrice.Fboid = fboprices.Fboid;
                    newFboPrice.Price = fboprices.PriceCost;
                    newFboPrice.Product = fboprices.Product + " Cost";
                    newFboPrice.Timestamp = DateTime.Now;
                    _context.Fboprices.Add(newFboPrice);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbopricesExists(fboprices.OidPap))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Status = isStaged ? "staged" : "published"});
        }

        // DELETE: api/Fboprices/delete-price/fbo/5/jeta
        [HttpDelete("delete-price-by-product/fbo/{fboId}/product/{product}")]
        public async Task<IActionResult> DeletePriceByProduct([FromRoute] int fboId, [FromRoute] string product)
        {
            List<Fboprices> prices = await _context.Fboprices.Where(f => f.Fboid == fboId && f.Expired == null && (f.Product == product + " Retail" || f.Product == product + " Cost")).ToListAsync();
            _context.Fboprices.RemoveRange(prices);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool FbopricesExists(int id)
        {
            return _context.Fboprices.Any(e => e.Oid == id);
        }

        private IQueryable<Fboprices> GetAllFboPrices()
        {
            return _context.Fboprices.AsQueryable();
        }

        private IQueryable<Fbos> GetAllActiveFbos()
        {
            return _context.Fbos.Where(f => f.Active == true).AsQueryable();
        }
    }
}
