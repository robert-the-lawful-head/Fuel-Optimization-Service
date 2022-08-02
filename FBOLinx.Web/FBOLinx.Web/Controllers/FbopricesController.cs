using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.ViewModels;
using FBOLinx.Web.Auth;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Services;
using System.Security.Claims;
using EFCore.BulkExtensions;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.DB.Specifications;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.RampFee;
using FBOLinx.ServiceLayer.DTO.Requests.FuelPricing;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

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
        private IPriceFetchingService _PriceFetchingService;
        private readonly FbopricesService _fbopricesService;
        private readonly DateTimeService _dateTimeService;
        private readonly IFboService _fboService;
        private readonly IFboService _iFboService; 
        private IFuelPriceAdjustmentCleanUpService _fuelPriceAdjustmentCleanUpService;
        private readonly FboPreferencesService _fboPreferencesService;
        private readonly MissedQuoteLogService _missedQuoteLogService;
        private readonly IntegrationUpdatePricingLogService _integrationUpdatePricingLogService;
        private AppPartnerSDKSettings.FuelerlinxSDKSettings _fuelerlinxSdkSettings;
        private IServiceScopeFactory _ScopeFactory;

        public FbopricesController(
            FboLinxContext context,
            IHttpContextAccessor httpContextAccessor,
            JwtManager jwtManager, 
            RampFeesService rampFeesService, 
            AircraftService aircraftService, 
            IPriceFetchingService priceFetchingService,
            FbopricesService fbopricesService,
            DateTimeService dateTimeService,
            IFboService fboService,
            IFuelPriceAdjustmentCleanUpService fuelPriceAdjustmentCleanUpService,
            FboPreferencesService fboPreferencesService,
            MissedQuoteLogService missedQuoteLogService,
            IntegrationUpdatePricingLogService integrationUpdatePricingLogService,
            IFboService iFboService,
            IOptions<AppPartnerSDKSettings> appPartnerSDKSettings,
            IServiceScopeFactory scopeFactory
            )
        {
            _ScopeFactory = scopeFactory;
            _fuelPriceAdjustmentCleanUpService = fuelPriceAdjustmentCleanUpService;
            _PriceFetchingService = priceFetchingService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _jwtManager = jwtManager;
            _fbopricesService = fbopricesService;
            _dateTimeService = dateTimeService;
            _fboService = fboService;
            _fboPreferencesService = fboPreferencesService;
            _missedQuoteLogService = missedQuoteLogService;
            _integrationUpdatePricingLogService = integrationUpdatePricingLogService;
            _iFboService = iFboService;
            _fuelerlinxSdkSettings = appPartnerSDKSettings.Value.FuelerLinx;
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
                    price.EffectiveFrom = await _iFboService.GetAirportLocalDateTimeByUtcFboId(price.EffectiveFrom, fboId);
                    price.EffectiveTo = await _iFboService.GetAirportLocalDateTimeByUtcFboId(price.EffectiveTo.GetValueOrDefault(), fboId);
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
            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(FuelProductPriceTypes)).ToArray();
            var result = await _fbopricesService.GetPrices(fboId);

            foreach (var product in fboProducts)
            {
                var fboPricesUpdateGenerator = new FboPricesUpdateGenerator();

                var filteredResultCost = result.Where(f => f.Product == product.ToString() + " Cost" && (f.EffectiveFrom > DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();
                var filteredResultRetail = result.Where(f => f.Product == product.ToString() + " Retail" && (f.EffectiveFrom > DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();

                var currentRetailResult = result.Where(f => f.Product == product.ToString() + " Retail" && (f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();

                if ((filteredResultCost == null || filteredResultCost.Oid == 0) && (filteredResultRetail == null || filteredResultRetail.Oid == 0))
                {
                    fboPricesUpdateGenerator.Product = product.ToString();
                    fboPricesUpdateGenerator.Fboid = fboId;
                    if (currentRetailResult.Oid > 0 && !currentRetailResult.EffectiveTo.ToString().Contains("12/31/99"))
                    {
                        fboPricesUpdateGenerator.EffectiveFrom = currentRetailResult.EffectiveTo.GetValueOrDefault().AddMinutes(1);
                        fboPricesUpdateGenerator.EffectiveTo = DateTimeHelper.GetNextTuesdayDate(DateTime.Parse(fboPricesUpdateGenerator.EffectiveFrom.ToShortDateString()));
                    }
                    else if (currentRetailResult.EffectiveTo.ToString().Contains("12/31/99"))
                    {
                        fboPricesUpdateGenerator.EffectiveFrom = currentRetailResult.TimeStamp == null ? DateTime.UtcNow : currentRetailResult.TimeStamp.GetValueOrDefault();
                        fboPricesUpdateGenerator.EffectiveTo = DateTime.Parse("12/31/9999");
                        fboPricesUpdateGenerator.PricePap = currentRetailResult.Price;

                        var currentCostResult = result.Where(f => f.Product == product.ToString() + " Cost" && (f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();
                        fboPricesUpdateGenerator.PriceCost = currentCostResult.Price;
                    }
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
                }

                if (!DateTimeHelper.IsDateNothing(fboPricesUpdateGenerator.EffectiveFrom))
                    fboPricesUpdateGenerator.EffectiveFrom = await _iFboService.GetAirportLocalDateTimeByUtcFboId(fboPricesUpdateGenerator.EffectiveFrom, fboId);

                if (!DateTimeHelper.IsDateNothing(fboPricesUpdateGenerator.EffectiveTo.GetValueOrDefault()))
                    fboPricesUpdateGenerator.EffectiveTo =
                        await _iFboService.GetAirportLocalDateTimeByUtcFboId(
                            fboPricesUpdateGenerator.EffectiveTo.GetValueOrDefault(), fboId);

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

            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(FuelProductPriceTypes));

            var activePricingCost = await _context.Fboprices.FirstOrDefaultAsync(s => s.EffectiveFrom <= DateTime.UtcNow && s.EffectiveTo > DateTime.UtcNow && s.Product == "JetA Cost" && s.Fboid == fboId && s.Expired != true);
            var activePricingRetail = await _context.Fboprices.FirstOrDefaultAsync(s => s.EffectiveFrom <= DateTime.UtcNow && s.EffectiveTo > DateTime.UtcNow && s.Product == "JetA Retail" && s.Fboid == fboId && s.Expired != true);
            
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
                Icao = f.FboAirport?.Icao,
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

            var result = await _fbopricesService.GetPrices(fboPricesUpdateGenerator.Fboid);
            var currentRetailResult = result.Where(f => f.Product == fboPricesUpdateGenerator.Product + " Retail" && (f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();

            var effectiveFrom = new DateTime();
            var effectiveTo = new DateTime();
            var isPricingLive = false;
            if (currentRetailResult != null && currentRetailResult.Oid > 0)
            {
                effectiveFrom = await _iFboService.GetAirportLocalDateTimeByUtcFboId(currentRetailResult.EffectiveTo.GetValueOrDefault().AddMinutes(1), fboPricesUpdateGenerator.Fboid);
                effectiveTo = DateTimeHelper.GetNextTuesdayDate(DateTime.Parse(effectiveFrom.ToShortDateString()));
                isPricingLive = true;
            }

            return Ok(new { IsPricingLive = isPricingLive, EffectiveFrom = effectiveFrom, EffectiveTo = effectiveTo});
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
                                                                f.EffectiveFrom <= DateTime.UtcNow &&
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

            await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboId);

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
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdatePricing([FromBody] PricingUpdateRequest request)
        {
            if (request.Retail == null && request.Cost == null)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var integrationUpdatePricingLog = new IntegrationUpdatePricingLogDto();
            integrationUpdatePricingLog.Request = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            integrationUpdatePricingLog.DateTimeRecorded = DateTime.UtcNow;

            try
            {
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _context.User.FindAsync(claimedId);

                if (user.FboId > 0)
                {
                    integrationUpdatePricingLog.FboId = user.FboId;
                    integrationUpdatePricingLog = await _integrationUpdatePricingLogService.InsertLog(integrationUpdatePricingLog);

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
                            Fboid = user.FboId,
                            Timestamp = DateTime.UtcNow
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
                            Fboid = user.FboId,
                            Timestamp = DateTime.UtcNow
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

                    await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(user.FboId);

                    integrationUpdatePricingLog.Response = "Success";
                    await _integrationUpdatePricingLogService.UpdateLog(integrationUpdatePricingLog);

                    return Ok(new { message = "Success" });
                }
                else
                {
                    integrationUpdatePricingLog.Response = "Invalid user";
                    integrationUpdatePricingLog = await _integrationUpdatePricingLogService.InsertLog(integrationUpdatePricingLog);
                    return BadRequest(new { message = "Invalid user" });
                }
            }
            catch (Exception ex)
            {
                integrationUpdatePricingLog.Response = ex.InnerException == null ? ex.Message : ex.InnerException.ToString();
                if (integrationUpdatePricingLog.Oid > 0)
                    await _integrationUpdatePricingLogService.UpdateLog(integrationUpdatePricingLog);
                else
                    await _integrationUpdatePricingLogService.InsertLog(integrationUpdatePricingLog);
                return BadRequest(ex);
            }
        }

        [HttpPost("update/stage")]
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
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

                await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboprices.Fboid.GetValueOrDefault());
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

            await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboprices.Oid);

            return CreatedAtAction("GetFboprices", new { id = fboprices.Oid }, fboprices);

        }

        [HttpGet("group/{groupid}/ispricingexpiredgroupadmin")]
        public async Task<IActionResult> CheckPricingIsExpiredGroupAdmin([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<FBOGroupPriceUpdateVM> groupPriceUpdate = new List<FBOGroupPriceUpdateVM>();
            try
            {
                var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(FuelProductPriceTypes));

                var groupFbos = _context.Fbos.Where(s => s.GroupId == groupId && s.Active == true).Select(s => s.Oid).ToList();

                var activePricing = await _context.Fboprices.Where(s =>
                                            s.EffectiveFrom <= DateTime.UtcNow &&
                                            s.EffectiveTo > DateTime.UtcNow &&
                                            (s.Product == "JetA Cost" || s.Product == "JetA Retail") &&
                                            s.Expired != true).ToListAsync();

                var fbosActivePricing = activePricing.Where(a => groupFbos.Any(g => g == a.Fboid)).ToList();
               
                var fbos = await _context.Fbos.Where(x => groupFbos.Any(f => f == x.Oid)).ToListAsync();
                foreach (var groupFbo in groupFbos)
                {
                    var isFboActivePricing = fbosActivePricing.FirstOrDefault(s => s.Fboid == groupFbo);

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
            }
            catch(Exception ex)
            {

            }

            return Ok(groupPriceUpdate);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpPost("price-lookup-for-fuelerlinx")]
        public async Task<IActionResult> GetFuelPricesForFuelerlinx([FromBody] VolumeDiscountLoadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await LogFuelerLinxPriceQuote(request);


                var customer =
                    await _context.Customers.Where(x =>
                        x.FuelerlinxId == request.FuelerlinxCompanyID
                        ).OrderBy(x => ((!x.GroupId.HasValue) ? 0 : x.GroupId.Value))
                        .AsNoTracking()
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
                    })
                    .AsNoTracking()
                    .ToListAsync();

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

                foreach (var icao in request.ICAO.Split(',').Select(x => x.Trim()))
                {
                    var fbos = await _fboService.GetFbosByIcaos(icao);

                    foreach (var fbo in fbos)
                    {
                        if (!result.Any(r => r.Icao == icao && r.Fbo == fbo.Fbo))
                        {
                            List<MissedQuoteLogDto> missedQuotesToLog = new List<MissedQuoteLogDto>();
                            var customerGroup = await _context.CustomerInfoByGroup.Where(c => c.CustomerId == customer.Oid && c.GroupId == fbo.GroupId).AsNoTracking().FirstOrDefaultAsync();

                            if (customerGroup != null && customerGroup.Oid > 0)
                            {
                                var universalTime = DateTime.UtcNow;
                                var customCustomerType = await _context.CustomCustomerTypes.Where(c => c.CustomerId == customer.Oid && c.Fboid == fbo.Oid).AsNoTracking().FirstOrDefaultAsync();
                                var customerType = new CustomCustomerTypes();
                                customerType.CustomerType = customCustomerType.CustomerType;
                                customerType.Oid = customCustomerType.Oid;
                                var fboPrices = await _context.Fboprices.Where(x =>
                        (!x.EffectiveTo.HasValue || x.EffectiveTo > universalTime) &&
                        (!x.EffectiveFrom.HasValue || x.EffectiveFrom <= universalTime) &&
                        x.Expired != true && x.Fboid == fbo.Oid).ToListAsync();

                                var debugging = "Valid Pricing: " + Newtonsoft.Json.JsonConvert.SerializeObject(validPricing);
                                debugging += " Custom Customer Type: " + Newtonsoft.Json.JsonConvert.SerializeObject(customerType);
                                debugging += " FBO Prices: " + Newtonsoft.Json.JsonConvert.SerializeObject(fboPrices);

                            var missedQuoteLog = await _missedQuoteLogService.GetRecentMissedQuotes(fbo.Oid);
                                var recentMissedQuote = missedQuoteLog.Where(m => m.Emailed.GetValueOrDefault() == true).ToList();
                                var isEmailed = false;

                                if (recentMissedQuote.Count == 0)
                                {
                                try
                                {
                                    if (!_fuelerlinxSdkSettings.APIEndpoint.Contains("-"))
                                    {
                                        var toEmails = await _fboService.GetToEmailsForEngagementEmails(fbo.Oid);

                                        if (toEmails.Count > 0)
                                            await _fbopricesService.NotifyFboNoPrices(toEmails, fbo.Fbo, customer.Company);

                                        isEmailed = true;
                                    }
                                }
                                catch(Exception ex)
                                {

                                    }
                                }

                                var missedQuote = new MissedQuoteLogDto();
                                missedQuote.CreatedDate = DateTime.UtcNow;
                                missedQuote.FboId = fbo.Oid;
                                missedQuote.CustomerId = customer.Oid;
                                missedQuote.Emailed = isEmailed;
                                missedQuote.Debugs = debugging;
                                missedQuotesToLog.Add(missedQuote);
                            }
                            await SaveMissedQuotes(missedQuotesToLog);
                        }
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task LogFuelerLinxPriceQuote(VolumeDiscountLoadRequest request)
        {
            //TODO: Refactoring: move this to a bulk insert method once we establish a service for CompanyPricingLog
            List<string> icaoList = request.ICAO.Split(',').Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<CompanyPricingLog> companyPricingLogs = new List<CompanyPricingLog>();
            foreach (string icao in icaoList)
            {
                companyPricingLogs.Add(new CompanyPricingLog
                {
                    CompanyId = request.FuelerlinxCompanyID,
                    ICAO = icao,
                    CreatedDate = DateTime.Now
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.BulkInsertAsync(companyPricingLogs);
            await transaction.CommitAsync();
        }

        [HttpPost("price-lookup-for-customer")]
        public async Task<IActionResult> GetFuelPricesForCustomer([FromBody] PriceLookupRequest request)
        {
            try
            {
                var validPricing = await _fbopricesService.GetFuelPricesForCustomer(request);

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
                                           .Where(f => f.EffectiveFrom <= DateTime.UtcNow && f.Fboid.Equals(fboprices.Fboid) && f.Product.Contains(fboprices.Product) && f.Expired == null)
                                           .ToListAsync();
                    foreach (Fboprices oldPrice in oldPrices)
                    {
                        oldPrice.Expired = true;
                        oldPrice.EffectiveTo = utcEffectiveFrom.AddMinutes(-1);
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

                    fboprices.OidPap = newFboPrice.Oid;
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

                    fboprices.OidCost = newFboPrice.Oid;
                }

                await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboprices.Fboid);
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

            return Ok(new { OidPap = fboprices.OidPap, OidCost = fboprices.OidCost,  Status = isStaged ? "staged" : "published" }) ;
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

        // POST: api/Fboprices/handle-price-change-cleanup/{fboId}
        [HttpPost("handle-price-change-cleanup/{fboId}")]
        public async Task<IActionResult> HandlePriceChangeCleanup([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboId);

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

        private async Task SaveMissedQuotes(List<MissedQuoteLogDto> missedQuoteLogs)
        {
            if (missedQuoteLogs?.Count == 0)
                return;

            using (var scope = _ScopeFactory.CreateScope())
            {
                var missedQuoteLogService = scope.ServiceProvider.GetRequiredService<MissedQuoteLogService>();
                foreach(var missedQuoteLog in missedQuoteLogs)
                {
                    await missedQuoteLogService.AddAsync(missedQuoteLog);
                }
            }
        }
    }
}
