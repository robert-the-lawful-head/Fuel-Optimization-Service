using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.BusinessServices.RampFee;
using FBOLinx.ServiceLayer.DTO.Requests.FuelPricing;
using FBOLinx.ServiceLayer.DTO.Responses.FuelPricing;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FboPrices;
using FBOLinx.ServiceLayer.DTO.Requests;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.DB.Specifications.Group;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelPricing
{
    public interface IFboPricesService : IBaseDTOService<FboPricesDTO, DB.Models.Fboprices>
    {
        public Task<List<FbopricesResult>> GetPrices(int fboId);
        public Task<PriceLookupResponse> GetFuelPricesForCustomer(PriceLookupRequest request);
        public Task<List<FboPricesUpdateGenerator>> GetFbopricesByFboIdAllStaged(int fboId);
        public Task ExpirePricingForFbo(int fboId);
        public Task<FboPricesDTO> GetFboPricesRecord(int id);
        public Task<List<FboPricesDTO>> GetFboPricesRecords(int fboId);
        public Task<List<FboPricesDTO>> GetCurrentPricesByFboId(int fboId);
        public Task<List<FbopricesResult>> GetCurrentPrices(int fboId);
        public Task<FboPricesDTO> GetCurrentRetailPrice(int fboId);
        public Task<FboPricesDTO> GetCurrentCostPrice(int fboId);
        public Task<List<FbopricesResult>> GetStagedPrices(int fboId);
        public Task DeletePricesFromGenerator(int oidCost, int oidPap);
        public Task<List<FboPricesDTO>> GetAllActivePrices();
        public Task ExpireOldPricesByProduct(int fboId, string product, DateTime utcEffectiveFrom);
        public Task<FboPricesUpdateGenerator> PostPriceGenerator(FboPricesUpdateGenerator fboPrices);
        public Task DeletePricesByProduct(int fboId, string product);
        public Task SuspendPricing(int oid);
        public Task<FboPricesUpdateGenerator> SuspendPricingGenerator(FboPricesUpdateGenerator fboPricesUpdateGenerator);
        public Task<string> UpdateIntegrationPricing(IntegrationUpdatePricingLogDto integrationUpdatePricingLog, PricingUpdateRequest request, int claimedId, int integrationPartnerId);
        public Task<string> UpdateIntegrationStagePricing(IntegrationUpdatePricingLogDto integrationUpdatePricingLog, PricingUpdateRequest request, int claimedId, int integrationPartnerId);

    }

    public class FbopricesService : BaseDTOService<FboPricesDTO, DB.Models.Fboprices, FboLinxContext>, IFboPricesService
    {
        private readonly FboLinxContext _context;
        private readonly AircraftService _aircraftService;
        private IPriceFetchingService _priceFetchingService;
        private readonly RampFeesService _rampFeesService;
        private readonly IFboPricesEntityService _fboPricesEntityService;
        private readonly DateTimeService _dateTimeService;
        private IFuelPriceAdjustmentCleanUpService _fuelPriceAdjustmentCleanUpService;
        private IUserService _userService;
        private readonly IntegrationUpdatePricingLogService _integrationUpdatePricingLogService;
        private readonly IFboService _fboService;
        private readonly IFboPreferencesService _fboPreferencesService;
        private readonly IIntegrationPartnersEntityService _integrationPartnersEntityService;

        public FbopricesService(FboLinxContext context, AircraftService aircraftService, IPriceFetchingService priceFetchingService, RampFeesService rampFeesService, IFboPricesEntityService fboPricesEntityService, DateTimeService dateTimeService, IFuelPriceAdjustmentCleanUpService fuelPriceAdjustmentCleanUpService, IUserService userService, IntegrationUpdatePricingLogService integrationUpdatePricingLogService, IFboService fboService, IFboPreferencesService fboPreferencesService, IIntegrationPartnersEntityService integrationPartnersEntityService) : base(fboPricesEntityService)
        {
            _context = context;
            _aircraftService = aircraftService;
            _priceFetchingService = priceFetchingService;
            _rampFeesService = rampFeesService;
            _fboPricesEntityService = fboPricesEntityService;
            _dateTimeService = dateTimeService;
            _fuelPriceAdjustmentCleanUpService = fuelPriceAdjustmentCleanUpService;
            _userService = userService;
            _integrationUpdatePricingLogService = integrationUpdatePricingLogService;
            _fboService = fboService;
            _fboPreferencesService = fboPreferencesService;
            _integrationPartnersEntityService = integrationPartnersEntityService;
        }

        public async Task<List<FbopricesResult>> GetPrices(int fboId)
        {
            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(FuelProductPriceTypes));
            var universalTime = DateTime.Today.ToUniversalTime();

            var oldPrices = await _context.Fboprices.Where(f => f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && (f.Expired == null || f.Expired != true)).ToListAsync();
            foreach (var p in oldPrices)
            {
                p.Expired = true;
                _context.Fboprices.Update(p);
            }
            await _context.SaveChangesAsync();

            var fboprices = await (
                            from f in _context.Fboprices
                            where f.EffectiveTo > DateTime.UtcNow
                            && f.Fboid == fboId && f.Expired != true
                            orderby f.Oid
                            select f).ToListAsync();

            var oldJetAPriceExists = fboprices.Where(f => f.Product.Contains("JetA") && f.EffectiveFrom <= DateTime.UtcNow).ToList();
            if (oldJetAPriceExists.Count() > 2)
            {
                // Set old prices to expire, remove from collection
                for (int i = 0; i <= 1; i++)
                {
                    oldJetAPriceExists[i].Expired = true;
                    _context.Fboprices.Update(oldJetAPriceExists[i]);
                    await _context.SaveChangesAsync();

                    fboprices.Remove(oldJetAPriceExists[i]);
                }
            }

            var oldSafPriceExists = fboprices.Where(f => f.Product.Contains("SAF") && f.EffectiveFrom <= DateTime.UtcNow).ToList();
            if (oldSafPriceExists.Count() > 2)
            {
                // Set old prices to expire, remove from collection
                for (int i = 0; i <= 1; i++)
                {
                    oldSafPriceExists[i].Expired = true;
                    _context.Fboprices.Update(oldSafPriceExists[i]);
                    await _context.SaveChangesAsync();

                    fboprices.Remove(oldSafPriceExists[i]);
                }
            }

            fboprices = fboprices.Where(f => f.Price != null).ToList();

            if (fboprices.Count > 0)
            {
                var addOnMargins = await (
                                from s in _context.TempAddOnMargin
                                where s.FboId == fboId && s.EffectiveTo >= universalTime
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
                              where p.Description.StartsWith("JetA") || p.Description.StartsWith("SAF")
                              select new FbopricesResult
                              {
                                  Oid = f?.Oid ?? 0,
                                  Fboid = fboId,
                                  Product = p.Description,
                                  Price = f?.Price == null ? 0 : f?.Price,
                                  EffectiveFrom = f?.EffectiveFrom ?? DateTime.UtcNow,
                                  EffectiveTo = f?.EffectiveTo ?? null,
                                  TimeStamp = f?.Timestamp,
                                  SalesTax = f?.SalesTax,
                                  Currency = f?.Currency,
                                  TempJet = s?.MarginJet,
                                  TempAvg = s?.MarginAvgas,
                                  TempId = s?.Id,
                                  TempDateFrom = s?.EffectiveFrom,
                                  TempDateTo = s?.EffectiveTo,
                                  Source = f == null ? FboPricesSource.FboLinx : f.Source,
                                  IntegrationPartnerId = f?.IntegrationPartnerId
                              }).ToList();

                return result;
            }
            else
            {
                var noPrices = new List<FbopricesResult>();
                noPrices.Add(new FbopricesResult() { Product = "JetA Retail" });
                noPrices.Add(new FbopricesResult() { Product = "JetA Cost" });
                noPrices.Add(new FbopricesResult() { Product = "SAF Retail" });
                noPrices.Add(new FbopricesResult() { Product = "SAF Cost" });
                return noPrices;
            }
        }
        public async Task<PriceLookupResponse> GetFuelPricesForCustomer(PriceLookupRequest request)
        {
            PriceLookupResponse validPricing = new PriceLookupResponse();

            if (!string.IsNullOrEmpty(request.TailNumber))
            {
                var customerInfoByGroup = await _context.CustomerInfoByGroup.FirstOrDefaultAsync(c => c.Oid == request.CustomerInfoByGroupId);
                if (customerInfoByGroup == null)
                    return null;

                var customerAircraft = await _context.CustomerAircrafts.FirstOrDefaultAsync(s => s.TailNumber == request.TailNumber && s.GroupId == request.GroupID && s.CustomerId == customerInfoByGroup.CustomerId);
                if (customerAircraft == null)
                    return null;

                var validPricingList =
                    await _priceFetchingService.GetCustomerPricingByLocationAsync(request.ICAO, customerAircraft.CustomerId, request.FlightTypeClassification, request.DepartureType, request.ReplacementFeesAndTaxes, request.FBOID);
                if (validPricingList == null)
                    return null;

                validPricing.PricingList = validPricingList.Where(x =>
                    !string.IsNullOrEmpty(x.TailNumbers) &&
                    x.TailNumbers.ToUpper().Split(',').Contains(request.TailNumber.ToUpper()) && x.FboId == request.FBOID &&
                    (request.CustomerInfoByGroupId == x.CustomerInfoByGroupId)).ToList();
                var custAircraftMakeModel = await _aircraftService.GetAllAircraftsAsQueryable().FirstOrDefaultAsync(s => s.AircraftId == customerAircraft.AircraftId);

                if (custAircraftMakeModel != null)
                {
                    validPricing.MakeModel = custAircraftMakeModel.Make + " " + custAircraftMakeModel.Model;
                }
                validPricing.RampFee = await _rampFeesService.GetRampFeeForAircraft(request.FBOID, request.TailNumber);
            }
            else
            {
                var customerInfoByGroup = await _context.CustomerInfoByGroup
                    .Where(x => x.GroupId == request.GroupID && ((x.Active.HasValue && x.Active.Value && request.CustomerInfoByGroupId == 0) || (request.CustomerInfoByGroupId > 0 && x.Oid == request.CustomerInfoByGroupId)))
                    .Include(x => x.Customer)
                    .Where(x => !x.Customer.Suspended.HasValue || !x.Customer.Suspended.Value)
                    .FirstOrDefaultAsync();
                validPricing.PricingList = await _priceFetchingService.GetCustomerPricingAsync(request.FBOID, request.GroupID, customerInfoByGroup?.Oid > 0 ? customerInfoByGroup.Oid : 0, new List<int>() { request.PricingTemplateID }, request.FlightTypeClassification, request.DepartureType, request.ReplacementFeesAndTaxes);
            }

            if (validPricing.PricingList == null || validPricing.PricingList.Count == 0)
                return null;

            validPricing.Template = validPricing.PricingList[0].PricingTemplateName;
            validPricing.Company = validPricing.PricingList[0].Company;

            return validPricing;
        }

        public async Task<List<FboPricesUpdateGenerator>> GetFbopricesByFboIdAllStaged(int fboId)
        {
            var prices = new List<FboPricesUpdateGenerator>();
            var fboProducts = await _fboPreferencesService.GetFboProducts(fboId);
            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(FuelProductPriceTypes)).ToArray();
            var result = await GetPrices(fboId);

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
                    if (currentRetailResult != null)
                    {
                        if (currentRetailResult.Oid > 0 && !currentRetailResult.EffectiveTo.ToString().Contains("12/31/99"))
                        {
                            fboPricesUpdateGenerator.EffectiveFrom = currentRetailResult.EffectiveTo.GetValueOrDefault().AddMinutes(1);
                            fboPricesUpdateGenerator.EffectiveTo = DateTimeHelper.GetNextTuesdayDate(DateTime.Parse(fboPricesUpdateGenerator.EffectiveFrom.ToShortDateString()));

                            if (currentRetailResult.Source == FboPricesSource.Integration)
                            {
                                fboPricesUpdateGenerator.Source = 1;
                                fboPricesUpdateGenerator.IntegrationPartner = await GetIntegrationPartnerName(currentRetailResult.IntegrationPartnerId);
                            }
                        }
                        else if (currentRetailResult.EffectiveTo.ToString().Contains("12/31/99"))
                        {
                            fboPricesUpdateGenerator.EffectiveFrom = currentRetailResult.TimeStamp == null ? DateTime.UtcNow : currentRetailResult.TimeStamp.GetValueOrDefault();
                            fboPricesUpdateGenerator.EffectiveTo = DateTime.Parse("12/31/9999");
                            fboPricesUpdateGenerator.PricePap = currentRetailResult.Price;
                            fboPricesUpdateGenerator.Source = 1;
                            fboPricesUpdateGenerator.IntegrationPartner = await GetIntegrationPartnerName(currentRetailResult.IntegrationPartnerId);

                            var currentCostResult = result.Where(f => f.Product == product.ToString() + " Cost" && (f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();
                            fboPricesUpdateGenerator.PriceCost = currentCostResult.Price;
                        }
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
                        }

                        if(fboPricesUpdateGenerator.OidCost==0)
                        {
                            fboPricesUpdateGenerator.EffectiveFrom = filteredResultRetail.EffectiveFrom;
                            fboPricesUpdateGenerator.EffectiveTo = filteredResultRetail.EffectiveTo;
                        }
                    }
                }

                if (!DateTimeHelper.IsDateNothing(fboPricesUpdateGenerator.EffectiveFrom))
                    fboPricesUpdateGenerator.EffectiveFrom = await _fboService.GetAirportLocalDateTimeByUtcFboId(fboPricesUpdateGenerator.EffectiveFrom, fboId);

                if (!DateTimeHelper.IsDateNothing(fboPricesUpdateGenerator.EffectiveTo.GetValueOrDefault()))
                    fboPricesUpdateGenerator.EffectiveTo =
                        await _fboService.GetAirportLocalDateTimeByUtcFboId(
                            fboPricesUpdateGenerator.EffectiveTo.GetValueOrDefault(), fboId);

                prices.Add(fboPricesUpdateGenerator);
            }

            return prices;
        }

        public async Task ExpirePricingForFbo(int fboId)
        {
            var fboprices = await GetListbySpec(new NonExpiredPricesByFboIdSpecification(fboId));
            var fboPricesToUpdate = new List<FboPricesDTO>();

            foreach (var fboPrice in fboprices)
            {
                fboPrice.Expired = true;
                fboPricesToUpdate.Add(fboPrice);
            }

            await BulkUpdate(fboPricesToUpdate);
        }

        public async Task<FboPricesDTO> GetFboPricesRecord(int id)
        {
            var fboprices = await GetSingleBySpec(new FboPricesByIdSpecification(id));
            if (fboprices == null)
            {
                return new FboPricesDTO();
            }

            fboprices.Id = _context.MappingPrices.Where(x => x.FboPriceId == fboprices.Oid).Select(x => x.GroupId).FirstOrDefault();
            return fboprices;
        }

        public async Task<List<FboPricesDTO>> GetFboPricesRecords(int fboId)
        {
            var fboprices = await GetListbySpec(new FboPricesByFboIdSpecification(fboId));
            if (fboprices == null)
            {
                return new List<FboPricesDTO>();
            }

            return fboprices;
        }

        public async Task<List<FboPricesDTO>> GetCurrentPricesByFboId(int fboId)
        {
            var currentPrices = await GetListbySpec(new CurrentFboPricesByFboIdSpecification(fboId));
            return currentPrices;
        }

        public async Task<List<FbopricesResult>> GetCurrentPrices(int fboId)
        {
            var result = await GetPrices(fboId);

            var filteredResult = new List<FbopricesResult>();
            filteredResult = result.Where(f => f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null).ToList();

            foreach (var price in filteredResult)
            {
                if (price.Price != null && price.Price > 0)
                {
                    price.EffectiveFrom = await _fboService.GetAirportLocalDateTimeByUtcFboId(price.EffectiveFrom, fboId);
                    price.EffectiveTo = await _fboService.GetAirportLocalDateTimeByUtcFboId(price.EffectiveTo.GetValueOrDefault(), fboId);
                    price.IntegrationPartner = await GetIntegrationPartnerName(price.IntegrationPartnerId);
                }
            }

            if (filteredResult.Where(f => f.Product == "JetA Retail").ToList().Count == 0)
                filteredResult.Add(new FbopricesResult() { Product = "JetA Retail" });

            if (filteredResult.Where(f => f.Product == "SAF Retail").ToList().Count == 0)
                filteredResult.Add(new FbopricesResult() { Product = "SAF Retail" });

            if (filteredResult.Count == 0)
                filteredResult = result;

            return filteredResult;
        }

        public async Task<FboPricesDTO> GetCurrentRetailPrice(int fboId)
        {
            var currentPrices = await GetListbySpec(new CurrentFboPricesByFboIdSpecification(fboId));
            var retailPrice = currentPrices.Where(s => s.Product == "JetA Retail").FirstOrDefault();
            return retailPrice;
        }

        public async Task<FboPricesDTO> GetCurrentCostPrice(int fboId)
        {
            var currentPrices = await GetListbySpec(new CurrentFboPricesByFboIdSpecification(fboId));
            var costPrice = new FboPricesDTO();
            costPrice = currentPrices.Where(s => s.Product == "JetA Cost").FirstOrDefault();

            return costPrice;
        }

        public async Task<List<FbopricesResult>> GetStagedPrices(int fboId)
        {
            var result = await GetPrices(fboId);

            var filteredResult = result.Where(f => f.EffectiveFrom > DateTime.UtcNow || f.EffectiveTo == null).ToList();

            return filteredResult;
        }

        public async Task DeletePricesFromGenerator(int oidCost, int oidPap)
        {
            FboPricesDTO costPrice = await GetSingleBySpec(new FboPricesByIdSpecification(oidCost));
            if (costPrice != null && costPrice.Oid > 0)
                await DeleteAsync(costPrice);

            FboPricesDTO retailPrice = await GetSingleBySpec(new FboPricesByIdSpecification(oidPap));
            if(retailPrice != null && retailPrice.Oid > 0)
                await DeleteAsync(retailPrice);
        }

        public async Task<List<FboPricesDTO>> GetAllActivePrices()
        {
            var allCurrentPrices = await GetListbySpec(new AllCurrentFboPricesSpecification());
            return allCurrentPrices;
        }

        public async Task<string> UpdateIntegrationPricing(IntegrationUpdatePricingLogDto integrationUpdatePricingLog, PricingUpdateRequest request, int claimedId, int integrationPartnerId)
        {
            var user = await _userService.GetUserByClaimedId(claimedId);

            var effectiveFrom = DateTime.UtcNow;
            var currentPrices = await GetCurrentPrices(user.FboId);
            var currentRetailPrice = currentPrices.Where(p => p.Product == "JetA Retail" && p.Oid > 0).FirstOrDefault();
            if (currentRetailPrice != null && currentRetailPrice.Oid > 0)
                effectiveFrom = currentRetailPrice.EffectiveTo.Value.AddMinutes(1);

            if (request.EffectiveDate != null)
            {
                if (request.TimeStandard == TimeStandards.Local)
                {
                    //Convert from local to Zulu
                    effectiveFrom = await _dateTimeService.ConvertLocalTimeToUtc(user.FboId, request.EffectiveDate.Value);
                }
                else
                {
                    effectiveFrom = request.EffectiveDate.Value.ToUniversalTime();
                }
            }

            if (effectiveFrom > DateTime.UtcNow)
            {
                var message = await UpdateIntegrationStagePricing(integrationUpdatePricingLog, request, claimedId, integrationPartnerId);
                return message;
            }
            else
            {
                integrationUpdatePricingLog.FboId = user.FboId;
                integrationUpdatePricingLog = await _integrationUpdatePricingLogService.InsertLog(integrationUpdatePricingLog);

                var effectiveTo = DateTime.MaxValue;
                if (request.ExpirationDate != null)
                {
                    if (request.TimeStandard == TimeStandards.Local)
                    {
                        //Convert from local to Zulu
                        effectiveTo = await _dateTimeService.ConvertLocalTimeToUtc(user.FboId, request.ExpirationDate.Value);
                    }
                    else
                    {
                        effectiveTo = request.ExpirationDate.Value.ToUniversalTime();
                    }
                }

                if (request.Retail != null)
                {
                    var retailPrice = new FboPricesDTO
                    {
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        Product = "JetA Retail",
                        Price = request.Retail,
                        Fboid = user.FboId,
                        Timestamp = DateTime.UtcNow,
                        Source = FboPricesSource.Integration,
                        IntegrationPartnerId = integrationPartnerId
                    };
                    List<FboPricesDTO> oldPrices = await GetFboPricesRecords(user.FboId);
                    oldPrices = oldPrices.Where(f => f.Product.Equals("JetA Retail")).ToList();

                    foreach (FboPricesDTO oldPrice in oldPrices.Where(o => o.Expired != true && o.EffectiveFrom <= effectiveFrom).ToList())
                    {
                        if (oldPrice.Expired != true && oldPrice.EffectiveTo > effectiveTo)
                        {
                            oldPrice.EffectiveTo = effectiveTo;
                        }
                        oldPrice.Expired = true;
                        await UpdateAsync(oldPrice);
                    }
                    await AddAsync(retailPrice);
                }
                if (request.Cost != null)
                {
                    var costPrice = new FboPricesDTO
                    {
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        Product = "JetA Cost",
                        Price = request.Cost,
                        Fboid = user.FboId,
                        Timestamp = DateTime.UtcNow,
                        Source = FboPricesSource.Integration,
                        IntegrationPartnerId = integrationPartnerId
                    };
                    List<FboPricesDTO> oldPrices = await GetFboPricesRecords(user.FboId);
                    oldPrices = oldPrices.Where(f => f.Product.Equals("JetA Cost")).ToList();

                    foreach (FboPricesDTO oldPrice in oldPrices.Where(o => o.Expired != true && o.EffectiveFrom <= effectiveFrom).ToList())
                    {
                        if (oldPrice.EffectiveTo > effectiveTo)
                        {
                            oldPrice.EffectiveTo = effectiveTo;
                        }
                        oldPrice.Expired = true;
                        await UpdateAsync(oldPrice);
                    }
                    await AddAsync(costPrice);
                }

                await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(user.FboId);

                integrationUpdatePricingLog.Response = "Success, your prices have been updated";
                await _integrationUpdatePricingLogService.UpdateLog(integrationUpdatePricingLog);

                return integrationUpdatePricingLog.Response;
            }
        }

        public async Task<string> UpdateIntegrationStagePricing(IntegrationUpdatePricingLogDto integrationUpdatePricingLog, PricingUpdateRequest request, int claimedId, int integrationPartnerId)
        {
            var user = await _userService.GetUserByClaimedId(claimedId);
            integrationUpdatePricingLog.FboId = user.FboId;
            integrationUpdatePricingLog = await _integrationUpdatePricingLogService.InsertLog(integrationUpdatePricingLog);

            var effectiveFrom = DateTime.UtcNow;
            if (request.EffectiveDate != null)
            {
                if (request.TimeStandard == TimeStandards.Local)
                {
                    //Convert from local to Zulu
                    effectiveFrom = await _dateTimeService.ConvertLocalTimeToUtc(user.FboId, request.EffectiveDate.Value);
                }
                else
                {
                    effectiveFrom = request.EffectiveDate.Value.ToUniversalTime();
                }
            }

            var effectiveTo = DateTime.MaxValue;
            if (request.ExpirationDate != null)
            {
                if (request.TimeStandard == TimeStandards.Local)
                {
                    //Convert from local to Zulu
                    effectiveTo = await _dateTimeService.ConvertLocalTimeToUtc(user.FboId, request.EffectiveDate.Value);
                }
                else
                {
                    effectiveTo = request.ExpirationDate.Value.ToUniversalTime();
                }
            }

            if (request.Retail != null)
            {
                var retailPrice = new FboPricesDTO
                {
                    EffectiveFrom = effectiveFrom,
                    EffectiveTo = effectiveTo,
                    Product = "JetA Retail",
                    Price = request.Retail,
                    Fboid = user.FboId,
                    Timestamp = DateTime.UtcNow,
                    Source = FboPricesSource.Integration,
                    IntegrationPartnerId = integrationPartnerId
                };

                await AddAsync(retailPrice);
            }
            if (request.Cost != null)
            {
                var costPrice = new FboPricesDTO
                {
                    EffectiveFrom = effectiveFrom,
                    EffectiveTo = effectiveTo,
                    Product = "JetA Cost",
                    Price = request.Cost,
                    Fboid = user.FboId,
                    Timestamp = DateTime.UtcNow,
                    Source = FboPricesSource.Integration,
                    IntegrationPartnerId = integrationPartnerId
                };

                await AddAsync(costPrice);
            }
            integrationUpdatePricingLog.Response = "Success, your prices are staged to be in effect on " + effectiveFrom.ToShortDateString();
            await _integrationUpdatePricingLogService.UpdateLog(integrationUpdatePricingLog);

            return integrationUpdatePricingLog.Response;
        }

        public async Task ExpireOldPricesByProduct(int fboId, string product, DateTime utcEffectiveFrom)
        {
            List<FboPricesDTO> oldPrices = await GetListbySpec(new OldFboPricesByProductSpecification(fboId, product));
            List<FboPricesDTO> oldPricesToUpdate = new List<FboPricesDTO>();

            foreach (FboPricesDTO oldPrice in oldPrices)
            {
                oldPrice.Expired = true;
                oldPrice.EffectiveTo = utcEffectiveFrom.AddMinutes(-1);
                oldPricesToUpdate.Add(oldPrice);
            }

            await BulkUpdate(oldPricesToUpdate);
        }

        public async Task<FboPricesUpdateGenerator> PostPriceGenerator(FboPricesUpdateGenerator fboPrices)
        {
            fboPrices.IsStaged = true;

            try
            {
                var utcEffectiveFrom = await _dateTimeService.ConvertLocalTimeToUtc(fboPrices.Fboid, fboPrices.EffectiveFrom);
                var utcEffectiveTo = await _dateTimeService.ConvertLocalTimeToUtc(fboPrices.Fboid, fboPrices.EffectiveTo.GetValueOrDefault());

                if (utcEffectiveFrom <= DateTime.UtcNow)
                {
                    await ExpireOldPricesByProduct(fboPrices.Fboid, fboPrices.Product, utcEffectiveFrom);

                    fboPrices.IsStaged = false;
                }

                var papPrice = await GetSingleBySpec(new FboPricesByIdSpecification(fboPrices.OidPap));
                if (papPrice != null && papPrice.Oid > 0)
                {
                    papPrice.Price = fboPrices.PricePap;
                    papPrice.EffectiveFrom = utcEffectiveFrom;
                    papPrice.EffectiveTo = utcEffectiveTo;
                    papPrice.Timestamp = DateTime.Now;
                    await UpdateAsync(papPrice);
                }
                else
                {
                    FboPricesDTO newFboPrice = new FboPricesDTO();
                    newFboPrice.EffectiveFrom = utcEffectiveFrom;
                    newFboPrice.EffectiveTo = utcEffectiveTo;
                    newFboPrice.Fboid = fboPrices.Fboid;
                    newFboPrice.Price = fboPrices.PricePap;
                    newFboPrice.Product = fboPrices.Product + " Retail";
                    newFboPrice.Timestamp = DateTime.Now;
                    newFboPrice = await AddAsync(newFboPrice);

                    fboPrices.OidPap = newFboPrice.Oid;
                }

                var costPrice = await GetSingleBySpec(new FboPricesByIdSpecification(fboPrices.OidCost));
                if (costPrice != null && costPrice.Oid > 0)
                {
                    costPrice.Price = fboPrices.PriceCost;
                    costPrice.EffectiveFrom = utcEffectiveFrom;
                    costPrice.EffectiveTo = utcEffectiveTo;
                    costPrice.Timestamp = DateTime.Now;
                    await UpdateAsync(costPrice);
                }
                else
                {
                    FboPricesDTO newFboPrice = new FboPricesDTO();
                    newFboPrice.EffectiveFrom = utcEffectiveFrom;
                    newFboPrice.EffectiveTo = utcEffectiveTo;
                    newFboPrice.Fboid = fboPrices.Fboid;
                    newFboPrice.Price = fboPrices.PriceCost;
                    newFboPrice.Product = fboPrices.Product + " Cost";
                    newFboPrice.Timestamp = DateTime.Now;
                    newFboPrice = await AddAsync(newFboPrice);

                    fboPrices.OidCost = newFboPrice.Oid;
                }

                await _fuelPriceAdjustmentCleanUpService.PerformFuelPriceAdjustmentCleanUp(fboPrices.Fboid);

                return fboPrices;
            }
            catch (DbUpdateConcurrencyException)
            {
                var papPrice = await GetSingleBySpec(new FboPricesByIdSpecification(fboPrices.OidPap));
                if (papPrice == null || papPrice.Oid == 0)
                {
                    return new FboPricesUpdateGenerator();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeletePricesByProduct(int fboId, string product)
        {
            List<FboPricesDTO> prices = await GetListbySpec(new NonExpiredPricesByFboIdGenericProductSpecification(fboId, product));

            await BulkDeleteAsync(prices);

            var fboPrices = await GetPrices(fboId);
            var fboPrice = (from f in fboPrices
                            where f.Oid > 0 && f.Product.Contains(product)
                            select new FboPricesDTO
                            {
                                Oid = f.Oid
                            }).ToList();
            if (fboPrice.Count > 0)
            {
                await BulkDeleteAsync(fboPrice);
            }
        }

        public async Task SuspendPricing(int oid)
        {
            FboPricesDTO price = await GetFboPricesRecord(oid);
            await DeleteAsync(price);
        }

        public async Task<FboPricesUpdateGenerator> SuspendPricingGenerator(FboPricesUpdateGenerator fboPricesGenerator)
        {
            // Delete prices
            await DeletePricesFromGenerator(fboPricesGenerator.OidCost, fboPricesGenerator.OidPap);

            // Populate correct dates if necessary
            var result = await GetPrices(fboPricesGenerator.Fboid);
            if (result.Count > 0)
            {
                var currentRetailResult = result.Where(f => f.Product == fboPricesGenerator.Product + " Retail" && (f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();

                fboPricesGenerator.EffectiveFrom = new DateTime();
                fboPricesGenerator.EffectiveTo = new DateTime();
                fboPricesGenerator.IsLive = false;
                if (currentRetailResult != null && currentRetailResult.Oid > 0)
                {
                    fboPricesGenerator.EffectiveFrom = await _fboService.GetAirportLocalDateTimeByUtcFboId(currentRetailResult.EffectiveTo.GetValueOrDefault().AddMinutes(1), fboPricesGenerator.Fboid);
                    fboPricesGenerator.EffectiveTo = DateTimeHelper.GetNextTuesdayDate(DateTime.Parse(fboPricesGenerator.EffectiveFrom.ToShortDateString()));
                    fboPricesGenerator.IsLive = true;
                }
            }

            return fboPricesGenerator;
        }

        private async Task<string> GetIntegrationPartnerName(int? integrationPartnerId)
        {
            if (integrationPartnerId.HasValue)
            {
                var partner = await _integrationPartnersEntityService.GetSingleBySpec(new IntegrationPartnersSpecification(integrationPartnerId.Value));
                return partner.PartnerName;
            }

            return "";
        }
    }
}
