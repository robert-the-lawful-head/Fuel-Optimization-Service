using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Models.Responses;
using FBOLinx.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class FbopricesService
    {
        private readonly FboLinxContext _context;
        private readonly AircraftService _aircraftService;
        private IPriceFetchingService _priceFetchingService;
        private readonly RampFeesService _rampFeesService;

        public FbopricesService(FboLinxContext context, AircraftService aircraftService, IPriceFetchingService priceFetchingService, RampFeesService rampFeesService)
        {
            _context = context;
            _aircraftService = aircraftService;
            _priceFetchingService = priceFetchingService;
            _rampFeesService = rampFeesService;
        }

        public async Task<List<FbopricesResult>> GetPrices(int fboId)
        {
            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(FuelProductPriceTypes));
            var universalTime = DateTime.Today.ToUniversalTime();

            var oldPrices = await _context.Fboprices.Where(f => f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && f.Price != null && f.Expired != true).ToListAsync();
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
                              Price = f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.UtcNow,
                              EffectiveTo = f?.EffectiveTo ?? null,
                              TimeStamp = f?.Timestamp,
                              SalesTax = f?.SalesTax,
                              Currency = f?.Currency,
                              TempJet = s?.MarginJet,
                              TempAvg = s?.MarginAvgas,
                              TempId = s?.Id,
                              TempDateFrom = s?.EffectiveFrom,
                              TempDateTo = s?.EffectiveTo
                          }).ToList();

            return result;
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
    }
}
