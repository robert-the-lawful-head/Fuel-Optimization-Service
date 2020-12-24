using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Data;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using static FBOLinx.DB.Models.PricingTemplate;
using static FBOLinx.Core.Utilities.Extensions.ListExtensions;

namespace FBOLinx.Web.Services
{
    public class PriceFetchingService
    {
        private FboLinxContext _context;
        private int _FboId;
        private int _GroupId;
        private CustomerService _CustomerService;
        private FboService _FboService;

        public PriceFetchingService(FboLinxContext context, CustomerService customerService, FboService fboService)
        {
            _FboService = fboService;
            _CustomerService = customerService;
            _context = context;
        }

        #region Public Methods

        public async Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(string icao, int customerId, Core.Enums.FlightTypeClassifications flightTypeClassifications, Core.Enums.ApplicableTaxFlights departureType = Core.Enums.ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null, int fboId = 0)
        {
            List<string> airports = icao.Split(',').Select(x => x.Trim()).ToList();
            List<CustomerWithPricing> result = new List<CustomerWithPricing>();
            List<Fboairports> fboAirports = await _context.Fboairports
                                                            .Include(x => x.Fbo)
                                                            .ThenInclude(x => x.Group)
                                                            .Where(x => airports.Any(a => a == x.Icao) && x.Fbo != null && x.Fbo.Active == true && x.Fbo.Group != null && x.Fbo.Group.Active == true && (fboId == 0 || x.Fbo.Oid == fboId))
                                                            .ToListAsync();
            if (fboAirports == null)
                return result;
            foreach (var fboAirport in fboAirports)
            {
                //Do not include Paragon results from the Paragon Network
                //Do not include "Legacy" accounts that are still using the old system
                if (fboAirport.Fbo.Group.Isfbonetwork.GetValueOrDefault() || fboAirport.Fbo.Group.IsLegacyAccount.GetValueOrDefault())
                    continue;
                Fbos fbo = await _context.Fbos
                                            .Include(x => x.Group)
                                            .Where(x => x.Oid == fboAirport.Fboid)
                                            .FirstOrDefaultAsync();
                CustomerInfoByGroup customerInfoByGroup = await _context.CustomerInfoByGroup
                                                                            .Where(x => x.CustomerId == customerId && x.GroupId == fbo.GroupId)
                                                                            .FirstOrDefaultAsync();
                if (customerInfoByGroup == null)
                    continue;

                List<PricingTemplate> templates = await GetAllPricingTemplatesForCustomerAsync(customerInfoByGroup, fbo.Oid, fbo.GroupId.GetValueOrDefault());
                if (templates == null)
                    continue;

                List<CustomerWithPricing> pricing =
                    await GetCustomerPricingAsync(fbo.Oid, fbo.GroupId.GetValueOrDefault(), customerInfoByGroup.Oid, templates.Select(x => x.Oid).ToList(), flightTypeClassifications, departureType, feesAndTaxes);

                List<string> alertEmailAddresses = await _context.Fbocontacts.Where(x => x.Fboid == fbo.Oid).Include(x => x.Contact).Where(x => x.Contact != null && x.Contact.CopyAlerts.HasValue && x.Contact.CopyAlerts.Value).Select(x => x.Contact.Email).ToListAsync();

                foreach(var price in pricing)
                {
                    var template = templates.FirstOrDefault(x => x.Oid == price.PricingTemplateId);
                    if (template != null && template.TailNumbers != null)
                        price.TailNumbers = string.Join(",", template.TailNumbers);
                    if (alertEmailAddresses != null)
                        price.CopyEmails = string.Join(";", alertEmailAddresses);
                    price.FuelDeskEmail = fbo.FuelDeskEmail;
                }

                if (pricing != null)
                    result.AddRange(pricing);
            }

            return result;
        }

        public async Task<List<CustomerWithPricing>> GetCustomerPricingAsync(int fboId, int groupId,
            int customerInfoByGroupId, List<int> pricingTemplateIds,
            FBOLinx.Core.Enums.FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null)
        {
            _FboId = fboId;
            _GroupId = groupId;

            try
            {
                //Mark old prices as expired
                var oldPrices = await _context.Fboprices.Where(f =>
                        f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && f.Price != null && f.Expired != true)
                    .ToListAsync();
                foreach (var p in oldPrices)
                {
                    p.Expired = true;
                    _context.Fboprices.Update(p);
                }

                await _context.SaveChangesAsync();

                var defaultPricingTemplate = await _context.PricingTemplate
                    .Where(x => x.Fboid == fboId && x.Default.HasValue && x.Default.Value).FirstOrDefaultAsync();
                if (flightTypeClassifications == FlightTypeClassifications.NotSet ||
                    flightTypeClassifications == FlightTypeClassifications.All)
                    flightTypeClassifications = FlightTypeClassifications.Private;
                if (departureType == ApplicableTaxFlights.Never)
                    departureType = ApplicableTaxFlights.All;
                int defaultPricingTemplateId = 0;
                if (defaultPricingTemplate != null)
                    defaultPricingTemplateId = defaultPricingTemplate.Oid;

                //Load all of the required information to get the quote
                var universalTime = DateTime.UtcNow;
                var customerInfoByGroup =
                    await _CustomerService.GetCustomersByGroupAndFbo(groupId, fboId, customerInfoByGroupId);
                var fboPrices = await _context.Fboprices.Where(x =>
                    (!x.EffectiveTo.HasValue || x.EffectiveTo > universalTime) &&
                    (!x.EffectiveFrom.HasValue || x.EffectiveFrom <= universalTime) &&
                    x.Expired != true && x.Fboid == fboId).ToListAsync();
                var pricingTemplates = await _context.PricingTemplate.Where(x => x.Fboid == fboId && pricingTemplateIds.Any(pt => pt == x.Oid))
                    .Include(x => x.CustomerMargins).ToListAsync();
                var customerMargins = await _context.CustomerMargins.Include(x => x.PricingTemplate)
                    .Where(x => x.PricingTemplate != null && x.PricingTemplate.Fboid == fboId).Include(x => x.PriceTier)
                    .ToListAsync();
                var tempAddonMargin = await _context.TempAddOnMargin.Where((x =>
                    x.EffectiveFrom < universalTime &&
                    x.EffectiveTo > universalTime)).ToListAsync();
                var customersViewedByFbo =
                    await _context.CustomersViewedByFbo.Where(x => x.Fboid == fboId).ToListAsync();
                var customerCompanyTypes = await _context.CustomerCompanyTypes
                    .Where(x => x.GroupId == groupId && x.Fboid == fboId).ToListAsync();
                var fbo = await _FboService.GetFbo(fboId);


                //Prepare fees/taxes based on the provided departure type and flight type
                if (feesAndTaxes == null)
                {
                    feesAndTaxes = await _context.FbofeesAndTaxes.Include(x => x.OmitsByCustomer).Where(x =>
                        x.Fboid == fboId && (x.FlightTypeClassification == FlightTypeClassifications.All ||
                                             x.FlightTypeClassification == flightTypeClassifications ||
                                             flightTypeClassifications == FlightTypeClassifications.All)).ToListAsync();
                    if (departureType != ApplicableTaxFlights.All)
                        feesAndTaxes = feesAndTaxes.Where(x =>
                            x.DepartureType == departureType || x.DepartureType == ApplicableTaxFlights.All).ToList();
                    feesAndTaxes.ForEach(x =>
                    {
                        if (x.OmitsByCustomer == null)
                            return;
                        x.OmitsByCustomer.RemoveAll(o =>
                            o.CustomerId != customerInfoByGroup.FirstOrDefault()?.CustomerId);
                    });
                }
                else
                {
                    feesAndTaxes = feesAndTaxes.Where(x =>
                        (x.FlightTypeClassification == FlightTypeClassifications.All ||
                         x.FlightTypeClassification == flightTypeClassifications) &&
                        (x.DepartureType == departureType ||
                         departureType == FBOLinx.Core.Enums.ApplicableTaxFlights.All ||
                         x.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.All)).ToList();
                }

                //Fetch the customer pricing results
                var customerPricingResults = (from cg in customerInfoByGroup
                    join pt in pricingTemplates on fboId equals pt.Fboid into leftJoinPT
                    from pt in leftJoinPT.DefaultIfEmpty()
                    join ppt in customerMargins on pt.Oid equals ppt.TemplateId
                        into leftJoinPPT
                    from ppt in leftJoinPPT.DefaultIfEmpty()
                    join fp in fboPrices on new
                    {
                        fboId = (pt != null ? pt.Fboid : 0),
                        product = (pt != null ? pt.MarginTypeProduct : "")
                    } equals new
                    {
                        fboId = fp.Fboid ?? 0,
                        product = fp.Product
                    }
                    join tmp in tempAddonMargin on new
                    {
                        fboId = (pt != null ? pt.Fboid : 0)
                    } equals new
                    {
                        fboId = tmp.FboId
                    } into leftJoinTMP
                    from tmp in leftJoinTMP.DefaultIfEmpty()
                    join cvf in customersViewedByFbo on new {cg.CustomerId, Fboid = _FboId} equals new
                    {
                        cvf.CustomerId,
                        cvf.Fboid
                    } into letJoinCVF
                    from cvf in letJoinCVF.DefaultIfEmpty()
                    join ccot in customerCompanyTypes on new
                            {CustomerCompanyType = cg.CustomerCompanyType ?? 0, cg.GroupId} equals new
                            {CustomerCompanyType = ccot.Oid, GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId}
                        into leftJoinCCOT
                    from ccot in leftJoinCCOT.DefaultIfEmpty()
                    select new CustomerWithPricing()
                    {
                        CustomerId = cg.CustomerId,
                        CustomerInfoByGroupId = cg.Oid,
                        Company = cg.Company,
                        PricingTemplateId = (pt == null ? 0 : pt.Oid),
                        DefaultCustomerType = cg.CustomerType,
                        MarginType = (pt == null ? 0 : pt.MarginType),
                        FboPrice = (fp == null ? 0 : fp.Price),
                        CustomerMarginAmount = (pt.MarginTypeProduct == "JetA Retail" && tmp != null &&
                                                (tmp.MarginJet.HasValue)
                            ? (ppt == null || ppt == null ? 0 : ppt.Amount) + (double) tmp.MarginJet ?? 0
                            : (ppt == null || ppt == null ? 0 : ppt.Amount)),
                        Suspended = cg.Suspended,
                        FuelerLinxId = (cg.Customer == null ? 0 : cg.Customer.FuelerlinxId),
                        Network = cg.Network,
                        GroupId = cg.GroupId,
                        FboId = (pt == null ? 0 : pt.Fboid),
                        NeedsAttention = !(cvf != null && cvf.Oid > 0),
                        HasBeenViewed = (cvf != null && cvf.Oid > 0),
                        PricingTemplateName = pt == null ? "" : pt.Name,
                        MinGallons = (ppt == null ? 1 : ppt.PriceTier?.Min ?? 1),
                        MaxGallons = (ppt == null ? 99999 : ppt.PriceTier?.Max ?? 99999),
                        CustomerCompanyType = cg.CustomerCompanyType,
                        CustomerCompanyTypeName = ccot == null || string.IsNullOrEmpty(ccot.Name) ? "" : ccot.Name,
                        IsPricingExpired = (fp == null && (pt == null || pt.MarginType == null ||
                                                           pt.MarginType != PricingTemplate.MarginTypes.FlatFee)),
                        ExpirationDate = fp?.EffectiveTo,
                        Icao = (fbo.fboAirport == null ? "" : fbo.fboAirport.Icao),
                        Iata = (fbo.fboAirport == null ? "" : fbo.fboAirport.Iata),
                        Notes = (pt == null ? "" : pt.Notes),
                        Fbo = (fbo == null ? "" : fbo.Fbo),
                        Group = (fbo.Group == null ? "" : fbo.Group.GroupName)
                    }).OrderBy(x => x.Company).ThenBy(x => x.PricingTemplateId).ThenBy(x => x.MinGallons).ToList();

                if (feesAndTaxes.Count == 0)
                    return customerPricingResults;

                //Add domestic-departure-only price options
                List<CustomerWithPricing> domesticOptions = new List<CustomerWithPricing>();
                if ((feesAndTaxes.Any(x => x.DepartureType == ApplicableTaxFlights.DomesticOnly) &&
                     departureType == ApplicableTaxFlights.All) || departureType == ApplicableTaxFlights.DomesticOnly)
                {
                    domesticOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                    domesticOptions.ForEach(x =>
                    {
                        x.Product = "JetA (Domestic Departure)";
                        x.FeesAndTaxes = feesAndTaxes.Where(fee =>
                            fee.DepartureType == ApplicableTaxFlights.DomesticOnly ||
                            fee.DepartureType == ApplicableTaxFlights.All).ToList();
                    });
                }

                //Add international-departure-only price options
                List<CustomerWithPricing> internationalOptions = new List<CustomerWithPricing>();
                if ((feesAndTaxes.Any(x => x.DepartureType == ApplicableTaxFlights.InternationalOnly) &&
                     departureType == ApplicableTaxFlights.All) ||
                    departureType == ApplicableTaxFlights.InternationalOnly)
                {
                    internationalOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                    internationalOptions.ForEach(x =>
                    {
                        x.Product = "JetA (International Departure)";
                        x.FeesAndTaxes = feesAndTaxes.Where(fee =>
                            fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                            fee.DepartureType == ApplicableTaxFlights.All).ToList();
                    });
                }

                //Add price options for all departure types
                List<CustomerWithPricing> allDepartureOptions = new List<CustomerWithPricing>();
                if ((feesAndTaxes.Any(x => x.DepartureType == ApplicableTaxFlights.All) &&
                     departureType == ApplicableTaxFlights.All) &&
                    (domesticOptions.Count == 0 || internationalOptions.Count == 0))
                {
                    allDepartureOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                    allDepartureOptions.ForEach(x =>
                    {
                        var productName = "JetA";
                        if (internationalOptions.Count > 0 && domesticOptions.Count == 0)
                            productName += " (Domestic Departure)";
                        else if (domesticOptions.Count > 0 && internationalOptions.Count == 0)
                            productName += " (International Departure)";
                        x.Product = productName;
                        x.FeesAndTaxes = feesAndTaxes.Where(fee => fee.DepartureType == ApplicableTaxFlights.All)
                            .ToList();
                    });
                }

                List<CustomerWithPricing> resultsWithFees = new List<CustomerWithPricing>();
                resultsWithFees.AddRange(domesticOptions);
                resultsWithFees.AddRange(internationalOptions);
                resultsWithFees.AddRange(allDepartureOptions);
                return resultsWithFees;
            }
            catch (System.Exception exception)
            {
                var test = exception;
                return new List<CustomerWithPricing>();
            }
        }

        public async Task<List<PricingTemplate>> GetAllPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            _FboId = fboId;
            _GroupId = groupId;

            List<PricingTemplate> result = new List<PricingTemplate>();
            var standardTemplates = await GetStandardPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);
            var aircraftPricesResult = await GetTailSpecificPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);

            result.AddRange(standardTemplates);

            //Set the applicable tail numbers for the aircraft-specific templates
            foreach (PricingTemplate aircraftPricingTemplate in aircraftPricesResult)
            {
                if (standardTemplates.Any(x => x.Oid == aircraftPricingTemplate.Oid))
                    continue;
                List<string> tailNumberList = (from ca in _context.CustomerAircrafts
                                      join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                                      where ap.PriceTemplateId == aircraftPricingTemplate.Oid
                                            && ca.CustomerId == customer.CustomerId
                                            && ca.GroupId == _GroupId
                                      select ca.TailNumber).ToList();
                if (tailNumberList == null || tailNumberList.Count == 0)
                    continue;
                aircraftPricingTemplate.Name += " - " + string.Join(",", tailNumberList);
                aircraftPricingTemplate.TailNumbers = tailNumberList;
                result.Add(aircraftPricingTemplate);
            }

            //Set the applicable tail numbers for the standard/default templates
            var customerAircrafts = await _context.CustomerAircrafts.Where(x => x.CustomerId == customer.CustomerId && x.GroupId == groupId).ToListAsync();

            standardTemplates.ForEach(x => x.TailNumbers = customerAircrafts.Where(c => !aircraftPricesResult.Any(a => a.TailNumbers != null && a.TailNumbers.Contains(c.TailNumber))).Select(c => c.TailNumber).ToList());

            return result;
        }

        public async Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            _FboId = fboId;
            _GroupId = groupId;

            var result = await (from cg in _context.CustomerInfoByGroup
                join c in _context.Customers on cg.CustomerId equals c.Oid
                join cct in _context.CustomCustomerTypes on new
                {
                    customerId = cg.CustomerId,
                    fboId = _FboId
                } equals new
                {
                    customerId = cct.CustomerId,
                    fboId = cct.Fboid
                }
                join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid

                where cg.GroupId == _GroupId
                      && cg.CustomerId == customer.CustomerId
                      && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
                select pt).ToListAsync();

            return result;
        }

        public async Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            _FboId = fboId;
            _GroupId = groupId;

            var aircraftPricesResult = await (from ap in _context.AircraftPrices
                join ca in _context.CustomerAircrafts on ap.CustomerAircraftId equals ca.Oid
                join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                where ca.CustomerId == customer.CustomerId
                      && ca.GroupId == _GroupId
                      && pt.Fboid == _FboId
                      && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
                group pt by new { pt.Oid, pt.Name, pt.Fboid, pt.CustomerId, pt.Notes, pt.Type, pt.MarginType }
                into groupedPrices
                select new PricingTemplate()
                {
                    Oid = groupedPrices.Key.Oid,
                    Name = groupedPrices.Key.Name,
                    Fboid = groupedPrices.Key.Fboid,
                    CustomerId = groupedPrices.Key.CustomerId,
                    Notes = groupedPrices.Key.Notes,
                    Type = groupedPrices.Key.Type,
                    MarginType = groupedPrices.Key.MarginType

                }).ToListAsync();

            return aircraftPricesResult;
        }

        public async Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int groupId)
        {
            //Load customer assignments by template ID
            var customerAssignments = await _context.CustomCustomerTypes.Where(x => x.Fboid == fboId).ToListAsync();
            
            //Separate inner queries first for FBO Prices and Margin Tiers
            var tempFboPrices = await _context.Fboprices
                                                .Where(fp => fp.EffectiveTo > DateTime.UtcNow && fp.Fboid == fboId && fp.Expired != true).ToListAsync();

            var tempMarginTiers = await (from c in _context.CustomerMargins
                                             join tm in _context.PriceTiers on c.PriceTierId equals tm.Oid
                                             group c by c.TemplateId into cmGroupResult
                                             select new
                                             {
                                                 TemplateId = cmGroupResult.Key,
                                                 MaxPriceTierValue = cmGroupResult.Max(c => Math.Abs(c.Amount.HasValue ? c.Amount.Value : 0)),
                                                 MinPriceTierValue = cmGroupResult.Min(c => Math.Abs(c.Amount.HasValue ? c.Amount.Value : 0))
                                             }).ToListAsync();
            var tempPricingTemplates = await (_context.PricingTemplate.Where(x => x.Fboid == fboId).ToListAsync());
            
            //Join the inner queries on the pricing templates
            var pricingTemplates = (from p in tempPricingTemplates
                join cm in tempMarginTiers on p.Oid equals cm.TemplateId
                    into leftJoinCmTiers
                from cm in leftJoinCmTiers.DefaultIfEmpty()
                join fp in tempFboPrices on p.MarginTypeProduct equals fp.Product
                    into leftJoinFp
                from fp in leftJoinFp.DefaultIfEmpty()
                where p.Fboid == fboId && (fp.EffectiveFrom <= DateTime.UtcNow || fp.EffectiveFrom == null)
                              select new
                              {
                                  p.CustomerId,
                                  p.Default,
                                  p.Fboid,
                                  Margin = cm == null ? 0 : (p.MarginType == MarginTypes.CostPlus ? cm.MaxPriceTierValue : cm.MinPriceTierValue),
                                  p.MarginType,
                                  p.Name,
                                  p.Notes,
                                  p.Oid,
                                  p.Type,
                                  p.Subject,
                                  p.Email,
                                  IsPricingExpired = fp == null && (p.MarginType == null || p.MarginType == MarginTypes.FlatFee) ? true : false,
                                  IntoPlanePrice = cm == null ? (fp != null ? fp.Price : 0) :
                                      p.MarginType == MarginTypes.CostPlus ? (fp != null ? fp.Price : 0) + cm.MaxPriceTierValue :
                                      (p.MarginType == MarginTypes.RetailMinus ? (fp != null ? fp.Price : 0) - cm.MinPriceTierValue : 0),
                                  TemplateId = cm == null ? 0 : cm.TemplateId
                              }).ToList();


            //Group the final result
            var result = (from pt in pricingTemplates
                    group pt by new
                    {
                        pt.CustomerId,
                        pt.Default,
                        pt.Fboid,
                        pt.Margin,
                        pt.MarginType,
                        pt.Name,
                        pt.Notes,
                        pt.Oid,
                        pt.Type,
                        pt.Subject,
                        pt.Email,
                        pt.IsPricingExpired,
                        pt.IntoPlanePrice,
                        pt.TemplateId,
                    }
                    into groupedPt
                    select new PricingTemplatesGridViewModel
                    {
                        CustomerId = groupedPt.Key.CustomerId,
                        Default = groupedPt.Key.Default,
                        Fboid = groupedPt.Key.Fboid,
                        Margin = groupedPt.Key.Margin,
                        YourMargin = groupedPt.Key.Margin,
                        MarginType = groupedPt.Key.MarginType,
                        Name = groupedPt.Key.Name,
                        Notes = groupedPt.Key.Notes,
                        Oid = groupedPt.Key.Oid,
                        Type = groupedPt.Key.Type,
                        Subject = groupedPt.Key.Subject,
                        Email = groupedPt.Key.Email,
                        IsPricingExpired = groupedPt.Key.IsPricingExpired,
                        IntoPlanePrice = groupedPt.Key.IntoPlanePrice,
                        CustomersAssigned = customerAssignments.Sum(x => x.CustomerType == groupedPt.Key.TemplateId ? 1 : 0)
                    })
                .OrderBy(pt => pt.Name)
                .ToList();

            return result;
        }
        #endregion

    }
}
