﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using static FBOLinx.Core.Utilities.Extensions.ListExtensions;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.ServiceLayer.Dto.Responses;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelPricing
{
    public class PriceFetchingService : IPriceFetchingService
    {
        private FboLinxContext _context;
        private int _FboId;
        private int _GroupId;
        private ICustomerService _CustomerService;
        private IFboService _FboService;
        private IMailService _MailService;
        private IPricingTemplateService _PricingTemplateService;
        private IFboPricesEntityService _FboPricesEntityService;
        private ICustomerInfoByGroupService _CustomerInfoByGroupService;
        private IRepository<Fboprices, FboLinxContext> _fboPricesRepo;

        public PriceFetchingService(FboLinxContext context, ICustomerService customerService, IFboService fboService, IMailService mailService, FuelerLinxApiService fuelerLinxApiService, IPricingTemplateService pricingTemplateService, IFboPricesEntityService fboPricesEntityService, ICustomerInfoByGroupService customerInfoByGroupService, IRepository<Fboprices, FboLinxContext> fboPricesRepo)
        {
            _FboService = fboService;
            _CustomerService = customerService;
            _context = context;
            _MailService = mailService;
            _PricingTemplateService = pricingTemplateService;
            _FboPricesEntityService = fboPricesEntityService;
            _CustomerInfoByGroupService = customerInfoByGroupService;
            _fboPricesRepo = fboPricesRepo;
        }

        #region Public Methods

        public async Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(
            string icao, int customerId, FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null, int fboId = 0)
        {
            return await GetCustomerPricingByLocationAsync(icao, customerId, flightTypeClassifications, departureType, feesAndTaxes, fboId, 0, false);
        }
        public async Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(
            string icao, int customerId, FlightTypeClassifications flightTypeClassifications,
            ApplicableTaxFlights departureType = ApplicableTaxFlights.All, List<FboFeesAndTaxes> feesAndTaxes = null, int fboId = 0, int groupId = 0, bool isAnalytics = false)
        {
            List<string> airports = icao.Split(',').Select(x => x.Trim()).ToList();
            List<CustomerWithPricing> result = new List<CustomerWithPricing>();
            List<Fboairports> fboAirports = await _context.Fboairports
                                                            .Include(x => x.Fbo)
                                                            .ThenInclude(x => x.Group)
                                                            .Where(x => airports.Any(a => a == x.Icao) && x.Fbo != null && x.Fbo.Active == true && x.Fbo.Group != null && x.Fbo.Group.Active == true && (fboId == 0 || x.Fbo.Oid == fboId) && (groupId == 0 || x.Fbo.GroupId == groupId))
                                                            .ToListAsync();
            if (fboAirports == null)
                return result;

            await UpdateExpiredPrices(fboAirports.Select(x => x.Fboid).ToList());

            foreach (var fboAirport in fboAirports)
            {
                //Do not include Paragon results from the Paragon Network
                //Do not include "Legacy" accounts that are still using the old system
                if (fboAirport.Fbo.Group.Isfbonetwork.GetValueOrDefault() || fboAirport.Fbo.Group.IsLegacyAccount.GetValueOrDefault())
                    continue;
                Fbos fbo = await _context.Fbos
                                            .Include(x => x.Group)
                                            .Where(x => x.Oid == fboAirport.Fboid)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
                var customerInfoByGroup = await _CustomerInfoByGroupService.GetSingleBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(customerId, fbo.GroupId));
                if (customerInfoByGroup == null || customerInfoByGroup.Active != true)
                    continue;

                List<DB.Models.PricingTemplate> templates = await _PricingTemplateService.GetAllPricingTemplatesForCustomerAsync(customerInfoByGroup, fbo.Oid, fbo.GroupId, 0, isAnalytics);
                if (templates == null)
                    continue;

                bool hasFlightTypeSpecificFeesAndTaxes = false;
                if (feesAndTaxes == null)
                    hasFlightTypeSpecificFeesAndTaxes = await _context.FbofeesAndTaxes
                        .Where(x => x.Fboid == fbo.Oid &&
                                    (x.FlightTypeClassification == FlightTypeClassifications.Commercial ||
                                     x.FlightTypeClassification == FlightTypeClassifications.Private)).AsNoTracking()
                        .AnyAsync();
                else
                    hasFlightTypeSpecificFeesAndTaxes = feesAndTaxes.Where(x =>
                        (x.FlightTypeClassification == FlightTypeClassifications.Commercial ||
                         x.FlightTypeClassification == FlightTypeClassifications.Private)).Any();

                List<CustomerWithPricing> pricing = new List<CustomerWithPricing>();

                //If the user did not specify a flight type and the FBO has a separation of both, then we need to get pricing for both private and commercial flights
                if ((flightTypeClassifications == FlightTypeClassifications.All ||
                     flightTypeClassifications == FlightTypeClassifications.NotSet) &&
                    hasFlightTypeSpecificFeesAndTaxes)
                {
                    var privatePricing = await GetCustomerPricingAsync(fbo.Oid, fbo.GroupId, customerInfoByGroup.Oid, templates.Select(x => x.Oid).ToList(), FlightTypeClassifications.Private, departureType, feesAndTaxes);
                    var commercialPricing = await GetCustomerPricingAsync(fbo.Oid, fbo.GroupId, customerInfoByGroup.Oid, templates.Select(x => x.Oid).ToList(), FlightTypeClassifications.Commercial, departureType, feesAndTaxes);
                    pricing.AddRange(privatePricing);
                    pricing.AddRange(commercialPricing);
                }
                else
                {
                    pricing = await GetCustomerPricingAsync(fbo.Oid, fbo.GroupId, customerInfoByGroup.Oid, templates.Select(x => x.Oid).ToList(), flightTypeClassifications, departureType, feesAndTaxes);
                }

                List<string> alertEmailAddresses = await _context.Fbocontacts.Where(x => x.Fboid == fbo.Oid).Include(x => x.Contact).Where(x => x.Contact != null && x.Contact.CopyOrders.HasValue && x.Contact.CopyOrders.Value).Select(x => x.Contact.Email).AsNoTracking().ToListAsync();

                List<string> alertEmailAddressesUsers = await _context.User.Where(x => x.GroupId == fbo.GroupId && (x.FboId == 0 || x.FboId == fbo.Oid) && x.CopyAlerts.HasValue && x.CopyOrders.Value).Select(x => x.Username).AsNoTracking().ToListAsync();

                foreach (var price in pricing)
                {
                    var template = templates.FirstOrDefault(x => x.Oid == price.PricingTemplateId);
                    if (template != null && template.TailNumbers != null)
                    {
                        template.TailNumbers.Sort();
                        price.TailNumbers = string.Join(",", template.TailNumbers);
                    }
                    if (alertEmailAddresses != null)
                        price.CopyEmails = string.Join(";", alertEmailAddresses);
                    if (alertEmailAddressesUsers != null)
                    {
                        foreach (var email in alertEmailAddressesUsers)
                        {
                            if (email.Contains('@'))
                                price.CopyEmails += ";" + email;
                        }
                    }
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
            var feesAndTaxesPassedIn = feesAndTaxes;
            var flightTypePassedIn = flightTypeClassifications;

            try
            {
                //Prepare default values
                if (flightTypeClassifications == FlightTypeClassifications.NotSet ||
                    flightTypeClassifications == FlightTypeClassifications.All)
                    flightTypeClassifications = FlightTypeClassifications.Private;
                if (departureType == ApplicableTaxFlights.Never)
                    departureType = ApplicableTaxFlights.All;

                //Load all of the required information to get the quote
                var universalTime = DateTime.UtcNow;
                var customer = await _CustomerInfoByGroupService.GetCustomersByGroup(groupId, customerInfoByGroupId);
                var customCustomerTypes = await _CustomerService.GetCustomCustomerTypes(fboId);

                if (customer.Count > 1)
                    customer = (from c in customer
                               join cct in customCustomerTypes on c.CustomerId equals cct.CustomerId
                               select c).ToList();

                var customerInfoByGroup = customer[0];
                //var customerInfoByGroup = await _CustomerInfoByGroupService.GetListbySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(customerInfoByGroupId, groupId));
                //#17nvxpq: Fill-in missing template IDs if one isn't properly provided
                if (!pricingTemplateIds.Any(x => x > 0))
                {
                    List<DB.Models.PricingTemplate> templates = await _PricingTemplateService.GetAllPricingTemplatesForCustomerAsync(customerInfoByGroup, fboId, groupId);
                    pricingTemplateIds = templates.Select(x => x.Oid).ToList();
                }
                var fboPrices = await _FboPricesEntityService.GetListBySpec(new CurrentFboPricesByFboIdSpecification(fboId));
                fboPrices = await RemovePricesNoLongerEffective(fboPrices);
                var pricingTemplates = await _context.PricingTemplate.Where(x => x.Fboid == fboId && pricingTemplateIds.Contains(x.Oid))
                    .Include(x => x.CustomerMargins)
                    .AsNoTracking()
                    .ToListAsync();
                var customerMargins = await _context.CustomerMargins.Include(x => x.PricingTemplate)
                    .Where(x => x.PricingTemplate != null && x.PricingTemplate.Fboid == fboId).Include(x => x.PriceTier)
                    .AsNoTracking()
                    .ToListAsync();
                var tempAddonMargin = await _context.TempAddOnMargin.Where((x =>
                    x.EffectiveFrom < universalTime &&
                    x.EffectiveTo > universalTime))
                    .AsNoTracking()
                    .ToListAsync();
                var customersViewedByFbo =
                    await _context.CustomersViewedByFbo.Where(x => x.Fboid == fboId)
                    .AsNoTracking()
                    .ToListAsync();
                var customerCompanyTypes = await _context.CustomerCompanyTypes
                    .Where(x => x.GroupId == groupId && x.Fboid == fboId)
                    .AsNoTracking()
                    .ToListAsync();
                var fbo = await _FboService.GetFbo(fboId);
                var priceBreakdownDisplayType = await GetPriceBreakdownDisplayType(fboId);

                //Prepare fees/taxes based on the provided departure type and flight type
                if (feesAndTaxes == null)
                {
                    feesAndTaxes = await _context.FbofeesAndTaxes.Include(x => x.OmitsByCustomer).Include(x => x.OmitsByPricingTemplate).Where(x =>
                        x.Fboid == fboId && (x.FlightTypeClassification == FlightTypeClassifications.All ||
                                             x.FlightTypeClassification == flightTypeClassifications ||
                                             flightTypeClassifications == FlightTypeClassifications.All))
                        .AsNoTracking()
                        .ToListAsync();
                    if (departureType != ApplicableTaxFlights.All)
                        feesAndTaxes = feesAndTaxes.Where(x =>
                            x.DepartureType == departureType || x.DepartureType == ApplicableTaxFlights.All).ToList();
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
                var customerPricingResults = (from  c in customer
                                              join pt in pricingTemplates on fboId equals pt.Fboid
                                              join ppt in customerMargins on pt.Oid equals ppt.TemplateId
                                                  into leftJoinPPT
                                              from ppt in leftJoinPPT.DefaultIfEmpty()
                                              join cct in customCustomerTypes on c.CustomerId equals cct.CustomerId into leftJoinCCT
                                              from cct in leftJoinCCT.DefaultIfEmpty()
                                              join fp in fboPrices on new
                                              {
                                                  fboId = (pt != null ? pt.Fboid : 0),
                                                  product = (pt != null ? (feesAndTaxesPassedIn == null ? pt.MarginTypeProduct : "JetA " + pt.MarginTypeProduct) : "")
                                              } equals new
                                              {
                                                  fboId = fp.Fboid ?? 0,
                                                  product = feesAndTaxesPassedIn == null ? fp.Product.Split(' ')[1] : fp.Product
                                              }
                                              join tmp in tempAddonMargin on new
                                              {
                                                  fboId = (pt != null ? pt.Fboid : 0)
                                              } equals new
                                              {
                                                  fboId = tmp.FboId
                                              } into leftJoinTMP
                                              from tmp in leftJoinTMP.DefaultIfEmpty()
                                              join cvf in customersViewedByFbo on new { CustomerId = c.CustomerId, Fboid = _FboId } equals new
                                              {
                                                  cvf.CustomerId,
                                                  cvf.Fboid
                                              } into letJoinCVF
                                              from cvf in letJoinCVF.DefaultIfEmpty()
                                              join ccot in customerCompanyTypes on new
                                              { CustomerCompanyType = c.CustomerCompanyType.GetValueOrDefault(), GroupId = c.GroupId } equals new
                                              { CustomerCompanyType = ccot.Oid, GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId }
                                                  into leftJoinCCOT
                                              from ccot in leftJoinCCOT.DefaultIfEmpty()
                                              where fp.Price > 0 && ((c.Oid == customerInfoByGroupId) || (customerInfoByGroupId == 0))
                                              select new CustomerWithPricing()
                                              {
                                                  CustomerId = c.Customer.Oid,
                                                  CustomerInfoByGroupId = c.Oid,
                                                  Company = c.Company,
                                                  PricingTemplateId = (pt == null ? 0 : pt.Oid),
                                                  DefaultCustomerType = cct?.CustomerType,
                                                  MarginType = (pt == null ? 0 : pt.MarginType),
                                                  DiscountType = (pt == null ? 0 : pt.DiscountType),
                                                  FboPrice = (fp == null ? 0 : fp.Price),
                                                  CustomerMarginAmount = (pt.MarginTypeProduct == "Retail" && tmp != null &&
                                                                          (tmp.MarginJet.HasValue)
                                                      ? (ppt == null ? 0 : ppt.Amount) + (double)tmp.MarginJet ?? 0
                                                      : (ppt == null ? 0 : ppt.Amount)),
                                                  amount = ppt == null ? 0 : ppt.Amount,
                                                  Suspended = c.Customer.Suspended,
                                                  FuelerLinxId = (c == null ? 0 : c.Customer.FuelerlinxId),
                                                  Network = c.Customer.Network,
                                                  GroupId = c.GroupId,
                                                  FboId = (pt == null ? 0 : pt.Fboid),
                                                  NeedsAttention = !(cvf != null && cvf.Oid > 0),
                                                  HasBeenViewed = (cvf != null && cvf.Oid > 0),
                                                  PricingTemplateName = pt == null ? "" : pt.Name,
                                                  MinGallons = (ppt == null ? 1 : ppt.PriceTier?.Min ?? 1),
                                                  MaxGallons = (ppt == null ? 99999 : ppt.PriceTier?.Max ?? 99999),
                                                  CustomerCompanyType = c.CustomerCompanyType,
                                                  CustomerCompanyTypeName = ccot == null || string.IsNullOrEmpty(ccot.Name) ? "" : ccot.Name,
                                                  IsPricingExpired = (fp == null && (pt == null || pt.MarginType == null ||
                                                                                     pt.MarginType != MarginTypes.FlatFee)),
                                                  ExpirationDate = fp?.EffectiveTo,
                                                  Icao = (fbo.FboAirport == null ? "" : fbo.FboAirport.Icao),
                                                  Iata = (fbo.FboAirport == null ? "" : fbo.FboAirport.Iata),
                                                  Notes = (pt == null ? "" : pt.Notes),
                                                  Fbo = (fbo == null ? "" : fbo.Fbo),
                                                  Group = (fbo.Group == null ? "" : fbo.Group.GroupName),
                                                  PriceBreakdownDisplayType = priceBreakdownDisplayType,
                                                  Product = fp.Product.Replace(" Cost", "").Replace(" Retail", "").Replace("JetA", "Jet A") + ((flightTypePassedIn == FlightTypeClassifications.Private || flightTypePassedIn == FlightTypeClassifications.Commercial) ? " (" + FBOLinx.Core.Utilities.Enums.EnumHelper.GetDescription(flightTypePassedIn) + ")" : "")
                                              }).OrderBy(x => x.Company).ThenBy(x => x.PricingTemplateId).ThenBy(x => x.Product).ThenBy(x => x.MinGallons).ToList();

                // If getting pricing by template for price checker, just use the first customer
                if (customerInfoByGroupId == 0 && customerPricingResults.Count > 0)
                {
                    var customerId = customerPricingResults[0].CustomerId;
                    customerPricingResults = customerPricingResults.Where(c => c.CustomerId == customerId).ToList();
                }

                if (feesAndTaxes.Count == 0)
                {
                    return customerPricingResults;
                }

                //Add domestic-departure-only price options
                List<CustomerWithPricing> domesticOptions = new List<CustomerWithPricing>();
                if ((feesAndTaxes.Any(x => x.DepartureType == ApplicableTaxFlights.DomesticOnly) &&
                     departureType == ApplicableTaxFlights.All) || departureType == ApplicableTaxFlights.DomesticOnly)
                {
                    domesticOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                    domesticOptions.ForEach(x =>
                    {
                        x.Product = x.Product + " (Domestic Departure)";
                        x.FeesAndTaxes = feesAndTaxes.Where(fee =>
                            fee.DepartureType == ApplicableTaxFlights.DomesticOnly ||
                            fee.DepartureType == ApplicableTaxFlights.All)
                            .ToList().Clone<FboFeesAndTaxes>().ToList();

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
                        x.Product = x.Product + " (International)";
                        x.FeesAndTaxes = feesAndTaxes.Where(fee =>
                            fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                            fee.DepartureType == ApplicableTaxFlights.All)
                            .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != x.PricingTemplateId))
                            .ToList();
                        x.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                                fee.DepartureType == ApplicableTaxFlights.All)
                            .ToList().Clone<FboFeesAndTaxes>().ToList();
                    });
                }

                //Add price options for "never" type
                List<CustomerWithPricing> neverOptions = new List<CustomerWithPricing>();
                if ((feesAndTaxes.Any(x => x.DepartureType == ApplicableTaxFlights.Never) &&
                     departureType == ApplicableTaxFlights.All))
                {
                    neverOptions = customerPricingResults.Clone<CustomerWithPricing>().ToList();
                    neverOptions.ForEach(x =>
                    {
                        var productName = x.Product;
                        if (internationalOptions.Count > 0 && domesticOptions.Count == 0)
                            productName += " (Domestic)";
                        else if (domesticOptions.Count > 0 && internationalOptions.Count == 0)
                            productName += " (International)";
                        x.Product = productName;
                        x.FeesAndTaxes = feesAndTaxes.Where(fee => fee.DepartureType == ApplicableTaxFlights.Never || fee.DepartureType == ApplicableTaxFlights.All)
                            .ToList().Clone<FboFeesAndTaxes>().ToList();
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
                        var productName = x.Product;
                        if (internationalOptions.Count > 0 && domesticOptions.Count == 0)
                            productName += " (Domestic)";
                        else if (domesticOptions.Count > 0 && internationalOptions.Count == 0)
                            productName += " (International)";
                        x.Product = productName;
                        x.FeesAndTaxes = feesAndTaxes.Where(fee => fee.DepartureType == ApplicableTaxFlights.All)
                            .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != x.PricingTemplateId))
                            .ToList().Clone<FboFeesAndTaxes>().ToList();
                    });
                }

                List<CustomerWithPricing> resultsWithFees = new List<CustomerWithPricing>();
                resultsWithFees.AddRange(domesticOptions);
                resultsWithFees.AddRange(internationalOptions);
                resultsWithFees.AddRange(neverOptions);
                resultsWithFees.AddRange(allDepartureOptions);

                //Set the "IsOmitted" case for all fees that might be omitted from a pricing template or customer specifically
                //Each collection of fees is cloned so updating the flag of one collection does not affect other pricing results where the template did not omit it
                var isRetailMinus = false;
                resultsWithFees.ForEach(x =>
                {
                    x.FeesAndTaxes.ForEach(fee =>
                    {
                        if (fee.OmitsByPricingTemplate != null &&
                            fee.OmitsByPricingTemplate.Any(o =>
                                o.PricingTemplateId == x.PricingTemplateId))
                        {
                            isRetailMinus = customerPricingResults.Where(z => z.PricingTemplateId == x.PricingTemplateId).FirstOrDefault().MarginType == MarginTypes.RetailMinus;
                            fee.SetIsOmittedForPricing((int)x.PricingTemplateId,isRetailMinus);
                        }

                        if ((fee.OmitsByCustomer != null && fee.OmitsByCustomer.Any(o =>
                            o.CustomerId == customerInfoByGroup.CustomerId)))
                        {
                            isRetailMinus = customerPricingResults.Where(x => x.CustomerId == x.CustomerId).FirstOrDefault().MarginType == MarginTypes.RetailMinus;
                            fee.SetIsomittedForCustomer(customerInfoByGroup.CustomerId, isRetailMinus);
                        }
                    });
                    
                });
                return resultsWithFees;
            }
            catch (System.Exception exception)
            {
                var test = exception;
                return new List<CustomerWithPricing>();
            }
        }

        public async Task<PriceBreakdownDisplayTypes> GetPriceBreakdownDisplayType(int fboId)
        {
            bool hasDepartureTypeRule = false;
            bool hasFlightTypeRule = false;
            var priceBreakdownDisplayType = PriceBreakdownDisplayTypes.SingleColumnAllFlights;

            var taxesAndFees = await _context.FbofeesAndTaxes.Where(x => x.Fboid == fboId).ToListAsync();
            foreach (FboFeesAndTaxes fee in taxesAndFees)
            {
                if (fee.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly || fee.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly)
                {
                    hasDepartureTypeRule = true;
                }
                if (fee.FlightTypeClassification == FBOLinx.Core.Enums.FlightTypeClassifications.Private || fee.FlightTypeClassification == FBOLinx.Core.Enums.FlightTypeClassifications.Commercial)
                {
                    hasFlightTypeRule = true;
                }
            }

            if (!hasDepartureTypeRule && !hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.SingleColumnAllFlights;
            }
            else if (!hasDepartureTypeRule && hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly;
            }
            else if (hasDepartureTypeRule && !hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly;
            }
            else if (hasDepartureTypeRule && hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.FourColumnsAllRules;
            }

            return priceBreakdownDisplayType;
        }

        public async Task<double> GetCurrentPostedRetail(int fboId)
        {
            var postedRetail = await _context.Fboprices
                                                .Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo > DateTime.UtcNow && fp.Fboid == fboId && fp.Expired != true && fp.Product == "JetA Retail").ToListAsync();
            return postedRetail.FirstOrDefault().Price.GetValueOrDefault();
        }

        public async Task<List<FbosGridViewModel>> GetAllFbosWithExpiredPricing()
        {
            var fboIdsWithExpiredPrice = new List<int>();

            var fbos = await (from f in _context.Fbos
                              join fa in _context.Fboairports on f.Oid equals fa.Fboid
                              where f.Active == true && f.GroupId > 1 && f.AccountType == AccountTypes.RevFbo
                              select new
                              {
                                  Active = f.Active,
                                  Oid = f.Oid,
                                  Fbo = f.Fbo,
                                  Icao = fa.Icao,
                                  GroupId = f.GroupId
                              }).ToListAsync();

            //var fbos = await _context.Fbos.Where(f => f.Active == true).Include(f => f.Users).Include("fboAirport").Where(x => x.GroupId > 1).ToListAsync();

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
                Icao = f.Icao,
                Oid = f.Oid,
                GroupId = f.GroupId
            }).ToList();

            return fbosWithExpiredPricing;
        }

        public async Task NotifyFboExpiredPrices(List<string> toEmails, string fbo)
        {
            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
            mailMessage.From = new MailAddress("donotreply@fbolinx.com");
            foreach (string email in toEmails)
            {
                if (_MailService.IsValidEmailRecipient(email))
                    mailMessage.To.Add(email);
            }

            var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridEngagementTemplateData
            {
                fboName = fbo,
                subject = "FBOLinx reminder - expired pricing!"
            };

            mailMessage.SendGridEngagementTemplateData = dynamicTemplateData;

            //Send email
            var result = await _MailService.SendAsync(mailMessage);
        }
        #endregion

        #region Private Methods
        private async Task UpdateExpiredPrices(List<int> fboIds)
        {
            //Mark old prices as expired
            var oldPrices = await _fboPricesRepo.Where(f =>
                    f.EffectiveTo <= DateTime.UtcNow && (f.Fboid.HasValue && fboIds.Contains(f.Fboid.Value)) && (f.Expired == null || f.Expired != true))
                .ToListAsync();

            await _fboPricesRepo.BulkUpdate(oldPrices);
        }

        private async Task<List<Fboprices>> RemovePricesNoLongerEffective(List<Fboprices> fboprices)
        {
            var oldJetAPriceExists = fboprices.Where(f => f.Product.Contains("JetA") && f.EffectiveFrom <= DateTime.UtcNow).OrderBy(f => f.Oid).ToList();
            if (oldJetAPriceExists.Count() > 2)
            {
                // Set old prices to expire, remove from collection
                for (int i = 0; i <= 1; i++)
                {
                    oldJetAPriceExists[i].Expired = true;
                    await _fboPricesRepo.UpdateAsync(oldJetAPriceExists[i]);
                    //_context.Fboprices.Update(oldJetAPriceExists[i]);
                    //await _context.SaveChangesAsync();

                    fboprices.Remove(oldJetAPriceExists[i]);
                }
            }

            var oldSafPriceExists = fboprices.Where(f => f.Product.Contains("SAF") && f.EffectiveFrom <= DateTime.UtcNow).OrderBy(f => f.Oid).ToList();
            if (oldSafPriceExists.Count() > 2)
            {
                // Set old prices to expire, remove from collection
                for (int i = 0; i <= 1; i++)
                {
                    oldSafPriceExists[i].Expired = true;
                    await _fboPricesRepo.UpdateAsync(oldSafPriceExists[i]);
                    //_context.Fboprices.Update(oldSafPriceExists[i]);
                    //await _context.SaveChangesAsync();

                    fboprices.Remove(oldSafPriceExists[i]);
                }
            }
            return fboprices;
        }
        #endregion
    }
}
