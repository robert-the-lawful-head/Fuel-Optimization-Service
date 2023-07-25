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
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.RampFee;
using FBOLinx.ServiceLayer.DTO.Requests.FuelPricing;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FboPrices;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.ServiceLayer.BusinessServices.CompanyPricingLog;
using FBOLinx.ServiceLayer.DTO.Requests;
using FBOLinx.ServiceLayer.DTO.Responses.FuelPricing;
using FBOLinx.ServiceLayer.Logging;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbopricesController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtManager _jwtManager;
        private IPriceFetchingService _PriceFetchingService;
        private readonly IFboPricesService _fbopricesService;
        private readonly DateTimeService _dateTimeService;
        private readonly IFboService _iFboService;
        private readonly IMissedQuoteLogService _missedQuoteLogService;
        private IFuelPriceAdjustmentCleanUpService _fuelPriceAdjustmentCleanUpService;
        private readonly IFboPreferencesService _fboPreferencesService;
        private readonly IntegrationUpdatePricingLogService _integrationUpdatePricingLogService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private readonly ICustomerService _customerService;
        private readonly ICompanyPricingLogService _companyPricingLogService;
        private readonly FBOLinx.ServiceLayer.BusinessServices.User.IUserService _userService;
        private IAPIKeyManager _apiKeyManager;

        public FbopricesController(
            FboLinxContext context,
            IHttpContextAccessor httpContextAccessor,
            JwtManager jwtManager,
            IPriceFetchingService priceFetchingService,
            IFboPricesService fbopricesService,
            DateTimeService dateTimeService,
            IFuelPriceAdjustmentCleanUpService fuelPriceAdjustmentCleanUpService,
            IFboPreferencesService fboPreferencesService,
            IntegrationUpdatePricingLogService integrationUpdatePricingLogService,
            IFboService iFboService,
            IMissedQuoteLogService missedQuoteLogService,
            IPricingTemplateService pricingTemplateService,
            ICompanyPricingLogService companyPricingLogService,
            ServiceLayer.BusinessServices.User.IUserService userService,
            ICustomerService customerService, ILoggingService logger, IAPIKeyManager apiKeyManager) : base(logger)
            
        {
            _fuelPriceAdjustmentCleanUpService = fuelPriceAdjustmentCleanUpService;
            _PriceFetchingService = priceFetchingService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _jwtManager = jwtManager;
            _fbopricesService = fbopricesService;
            _dateTimeService = dateTimeService;
            _fboPreferencesService = fboPreferencesService;
            _integrationUpdatePricingLogService = integrationUpdatePricingLogService;
            _iFboService = iFboService;
            _missedQuoteLogService = missedQuoteLogService;
            _pricingTemplateService = pricingTemplateService;
            _companyPricingLogService = companyPricingLogService;
            _userService = userService;
            _customerService = customerService;
            _apiKeyManager = apiKeyManager;
        }

        // GET: api/Fboprices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFboprices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboprices = await _fbopricesService.GetFboPricesRecords(id);
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

            var filteredResult = await _fbopricesService.GetCurrentPrices(fboId);

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

            var filteredResult = await _fbopricesService.GetStagedPrices(fboId);

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

            var prices = await _fbopricesService.GetFbopricesByFboIdAllStaged(fboId);

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

            var activePricingCost = await _fbopricesService.GetCurrentCostPrice(fboId);
            var activePricingRetail = await _fbopricesService.GetCurrentRetailPrice(fboId);


            if (activePricingCost != null && activePricingRetail != null)
            {
                return Ok(true);
            }

            return Ok(null);
        }

        [HttpPost("suspendpricing/{oid}")]
        public async Task<IActionResult> SuspendPricing([FromRoute] int oid)    
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fbopricesService.SuspendPricing(oid);
           
            return Ok();
        }

        [HttpPost("suspend-pricing-generator")]
        public async Task<IActionResult> SuspendPricingGenerator([FromBody] FboPricesUpdateGenerator fboPricesGenerator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fbopricesService.SuspendPricingGenerator(fboPricesGenerator);

            return Ok(new { IsPricingLive = fboPricesGenerator.IsLive, EffectiveFrom = fboPricesGenerator.EffectiveFrom, EffectiveTo = fboPricesGenerator.EffectiveTo });
        }

        [HttpGet("fbo/{fboId}/product/{product}/current")]
        public async Task<IActionResult> GetFbopricesByFboIdAndProductCurrent([FromRoute] int fboId, [FromRoute] string product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fboprices = await _fbopricesService.GetCurrentPricesByFboId(fboId);
            var fboPrice = fboprices.Where(f => f.Product.ToLower() == product.ToLower());

            return Ok(fboprices);
        }

        [HttpPost("fbo/{fboId}/check")]
        public async Task<IActionResult> checkifExistFboPrice([FromRoute] int fboId, [FromBody] IEnumerable<Fboprices> fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // No longer in use

            //foreach (Fboprices price in fboprices)
            //{
            //    if (price.Price != null)
            //    {
            //        price.Timestamp = DateTime.UtcNow;
            //        List<Fboprices> oldPrices = await _context.Fboprices
            //                                .Where(f => f.EffectiveFrom <= DateTime.UtcNow && f.Fboid.Equals(price.Fboid) && f.Product.Equals(price.Product) && !f.Expired.Equals(true))
            //                                .ToListAsync();
            //        foreach (Fboprices oldPrice in oldPrices)
            //        {
            //            oldPrice.Expired = true;
            //            _context.Fboprices.Update(oldPrice);
            //        }

            //        _context.Fboprices.Add(price);
            //        await _context.SaveChangesAsync();
            //    }
            //}

            //await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboId);

            return Ok(null);
        }

        [HttpPost("fbo/{fboId}/checkstaged")]
        public async Task<IActionResult> checkifExistStagedFboPrice([FromRoute] int fboId, [FromBody] IEnumerable<Fboprices> fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // No longer in use

            //foreach (Fboprices price in fboprices)
            //{
            //    if (price.Price != null)
            //    {
            //        price.Timestamp = DateTime.Now;
            //        if (price.Oid > 0)
            //        {
            //            _context.Fboprices.Update(price);
            //        }
            //        else if (price.Price != null)
            //        {
            //            _context.Fboprices.Add(price);
            //        }
            //        await _context.SaveChangesAsync();
            //    }
            //}

            return Ok(null);
        }

        /// <summary>
        /// Add a new price into FBOLinx.
        /// Accepts an object with the following properties:
        /// Retail: double?
        /// Cost: double?
        /// EffectiveDate: DateTime?
        /// ExpirationDate: DateTime?
        /// TimeStandard: TimeStandards
        /// TimeStandards:
        /// 0: NotSet
        /// 1: Zulu
        /// 2: Local
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
            integrationUpdatePricingLog.Request = "/update " + Newtonsoft.Json.JsonConvert.SerializeObject(request);
            integrationUpdatePricingLog.DateTimeRecorded = DateTime.UtcNow;

            try
            {
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _userService.GetUserByClaimedId(claimedId);

                if (user.FboId > 0)
                {
                    var apiKeyRecord = await _apiKeyManager.GetIntegrationPartner();

                    var message = await _fbopricesService.UpdateIntegrationPricing(integrationUpdatePricingLog, request, claimedId, apiKeyRecord.Oid);

                    return Ok(new { message = message });
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

        /// <summary>
        /// Add a staged price into FBOLinx.
        /// Accepts an object with the following properties:
        /// Retail: double?
        /// Cost: double?
        /// EffectiveDate: DateTime?
        /// ExpirationDate: DateTime?
        /// TimeStandard: TimeStandards
        /// TimeStandards:
        /// 0: NotSet
        /// 1: Zulu
        /// 2: Local
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("update/stage")]
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdateStagePricing([FromBody] PricingUpdateRequest request)
        {
            if (request.Retail == null && request.Cost == null)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var integrationUpdatePricingLog = new IntegrationUpdatePricingLogDto();
            integrationUpdatePricingLog.Request = "/update/stage " + Newtonsoft.Json.JsonConvert.SerializeObject(request);
            integrationUpdatePricingLog.DateTimeRecorded = DateTime.UtcNow;

            try
            {
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _userService.GetUserByClaimedId(claimedId);

                if (user.FboId > 0)
                {
                    var apiKeyRecord = await _apiKeyManager.GetIntegrationPartner();

                    await _fbopricesService.UpdateIntegrationStagePricing(integrationUpdatePricingLog, request, claimedId, apiKeyRecord.Oid);
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

            return Ok(new { message = "Success" });
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

            // NO LONGER IN USE
            //var fboPricesByMonth = await (from f in _context.Fboprices
            //                        where f.Product.ToLower() == request.Product.ToLower()
            //                              && f.Fboid == fboId
            //                              && f.EffectiveFrom >= request.StartDateTime
            //                              && f.EffectiveFrom <= request.EndDateTime
            //                        group f by new
            //                        {
            //                            f.EffectiveFrom.Value.Month,
            //                            f.EffectiveFrom.Value.Year
            //                        }
            //    into results
            //                        select new
            //                        {
            //                            results.Key.Month,
            //                            results.Key.Year,
            //                            AveragePrice = results.Average((x => x.Price))
            //                        }).ToListAsync();

            //return Ok(fboPricesByMonth);

            return Ok();
        }

        // PUT: api/Fboprices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFboprices([FromRoute] int id, [FromBody] Fboprices fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // NO LONGER IN USE

            //if (id != fboprices.Oid)
            //{
            //    return BadRequest();
            //}

            //try
            //{
            //    if (FbopricesExists(id))
            //    {
            //        _context.Entry(fboprices).State = EntityState.Modified;
            //        fboprices.Timestamp = DateTime.Now;
            //    }
            //    else
            //    {
            //        Fboprices newFboPrice = new Fboprices();
            //        newFboPrice.Currency = fboprices.Currency;
            //        newFboPrice.EffectiveFrom = fboprices.EffectiveFrom;
            //        newFboPrice.EffectiveTo = fboprices.EffectiveTo;
            //        newFboPrice.Fboid = fboprices.Fboid;
            //        newFboPrice.Price = fboprices.Price;
            //        newFboPrice.Product = fboprices.Product;
            //        newFboPrice.SalesTax = fboprices.SalesTax;
            //        newFboPrice.Timestamp = DateTime.Now;
            //        _context.Fboprices.Add(newFboPrice);
            //    }
            //    await _context.SaveChangesAsync();

            //    await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboprices.Fboid.GetValueOrDefault());
            //}
            //catch (DbUpdateConcurrencyException)
            //{                
            //    if (!FbopricesExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Fboprices
        [HttpPost]
        public async Task<IActionResult> PostFboprices([FromBody] Fboprices fboprices)
        {
            // NO LONGER IN USE

            //int num = 0;
            //MappingPrices map = new MappingPrices();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //fboprices.Timestamp = DateTime.Now;
            //_context.Fboprices.Add(fboprices);
            //await _context.SaveChangesAsync();
            //if (fboprices.Id == null)
            //{
            //    num = _context.MappingPrices.Select(x => x.GroupId).DefaultIfEmpty(0).Max() + 1;
            //}
            //map.GroupId = fboprices.Id ?? num;
            //map.FboPriceId = fboprices.Oid;
            //_context.MappingPrices.Add(map);
            //await _context.SaveChangesAsync();

            //await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboprices.Oid);

            //return CreatedAtAction("GetFboprices", new { id = fboprices.Oid }, fboprices);
            return Ok();
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

                var groupFbos = await _iFboService.GetFbosByGroupId(groupId);
                var groupFboIds = groupFbos.Select(s => s.Oid).ToList();

                var activePricing = await _fbopricesService.GetAllActivePrices();

                var fbosActivePricing = activePricing.Where(a => groupFboIds.Any(g => g == a.Fboid)).ToList();
                foreach (var groupFbo in groupFboIds)
                {
                    var isFboActivePricing = fbosActivePricing.FirstOrDefault(s => s.Fboid == groupFbo);

                    if (isFboActivePricing == null)
                    {
                        FBOGroupPriceUpdateVM gPU = new FBOGroupPriceUpdateVM
                        {
                            FboId = groupFbo,
                            FboName = groupFbos.FirstOrDefault(s => s.Oid == groupFbo).Fbo
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

                var customer = await _customerService.GetCustomerByFuelerLinxId(request.FuelerlinxCompanyID);

                if (customer == null || customer.Oid == 0)
                    return Ok(null);

                
                List<CustomerWithPricing> validPricing = await _PriceFetchingService.GetCustomerPricingByLocationAsync(request.ICAO, customer.Oid, (FBOLinx.Core.Enums.FlightTypeClassifications)request.FlightTypeClassification.GetValueOrDefault(), Core.Enums.ApplicableTaxFlights.All, null, 0);
                    
                if (validPricing == null)
                    return Ok(null);

                var customerTemplates = await _pricingTemplateService.GetCustomerTemplates();

                var result = (
                    from p in validPricing
                    join ct in customerTemplates on new { p.CustomerId, p.FboId } equals new { ct.CustomerId, FboId = ct.FboId }
                    into leftJoinCustomerTypes
                    from ct in leftJoinCustomerTypes.DefaultIfEmpty()
                    select new FuelPriceResponse
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

                await _missedQuoteLogService.LogMissedQuote(request.ICAO, result.ToList(), customer);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task LogFuelerLinxPriceQuote(VolumeDiscountLoadRequest request)
        {
            await _companyPricingLogService.AddCompanyPricingLogs(request.ICAO, request.FuelerlinxCompanyID);
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

            // NO LONGER IN USE

            //var fboprices = await _context.Fboprices.FindAsync(id);
            //if (fboprices == null)
            //{
            //    return NotFound();
            //}
            //var fbopricesRange = await _context.Fboprices.Where(x => 
            //                                                x.EffectiveFrom == fboprices.EffectiveFrom && 
            //                                                x.EffectiveTo == fboprices.EffectiveTo && 
            //                                                x.Fboid == fboprices.Fboid
            //                                                ).ToListAsync();

            //_context.Fboprices.RemoveRange(fbopricesRange);
            //await _context.SaveChangesAsync();

            //return Ok(fbopricesRange);
            return Ok();
        }

        // POST: api/Fboprices/update-price-generator
        [HttpPost("update-price-generator")]
        public async Task<IActionResult> PostPriceGenerator([FromBody] FboPricesUpdateGenerator fboprices)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fboprices = await _fbopricesService.PostPriceGenerator(fboprices);

            return Ok(new { OidPap = fboprices.OidPap, OidCost = fboprices.OidCost,  Status = fboprices.IsStaged ? "staged" : "published" }) ;
        }

        // DELETE: api/Fboprices/delete-price/fbo/5/jeta
        [HttpDelete("delete-price-by-product/fbo/{fboId}/product/{product}")]
        public async Task<IActionResult> DeletePriceByProduct([FromRoute] int fboId, [FromRoute] string product)
        {
            await _fbopricesService.DeletePricesByProduct(fboId, product);

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
    }
}
