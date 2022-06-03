using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Services.Interfaces;
using FBOLinx.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using static FBOLinx.Core.Utilities.Extensions.ListExtensions;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Web.Services
{
    public class PriceFetchingService: IPriceFetchingService
    {
        private FboLinxContext _context;
        private int _FboId;
        private int _GroupId;
        private CustomerService _CustomerService;
        private FboService _FboService;
        private IMailService _MailService;
        private IPricingTemplateService _PricingTemplateService;

        public PriceFetchingService(FboLinxContext context, CustomerService customerService, FboService fboService, IMailService mailService, FuelerLinxApiService fuelerLinxApiService, IPricingTemplateService pricingTemplateService)
        {
            _FboService = fboService;
            _CustomerService = customerService;
            _context = context;
            _MailService = mailService;
            _PricingTemplateService = pricingTemplateService;
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
                                            .FirstOrDefaultAsync();
                CustomerInfoByGroup customerInfoByGroup = await _context.CustomerInfoByGroup
                                                                            .Where(x => x.CustomerId == customerId && x.GroupId == fbo.GroupId)
                                                                            .FirstOrDefaultAsync();
                if (customerInfoByGroup == null || customerInfoByGroup.Active != true)
                    continue;

                List<PricingTemplate> templates = await _PricingTemplateService.GetAllPricingTemplatesForCustomerAsync(customerInfoByGroup, fbo.Oid, fbo.GroupId.GetValueOrDefault(), 0, isAnalytics);
                if (templates == null)
                    continue;

                List<CustomerWithPricing> pricing =
                    await GetCustomerPricingAsync(fbo.Oid, fbo.GroupId.GetValueOrDefault(), customerInfoByGroup.Oid, templates.Select(x => x.Oid).ToList(), flightTypeClassifications, departureType, feesAndTaxes);

                List<string> alertEmailAddresses = await _context.Fbocontacts.Where(x => x.Fboid == fbo.Oid).Include(x => x.Contact).Where(x => x.Contact != null && x.Contact.CopyOrders.HasValue && x.Contact.CopyOrders.Value).Select(x => x.Contact.Email).ToListAsync();

                List<string> alertEmailAddressesUsers = await _context.User.Where(x => x.GroupId == fbo.GroupId && (x.FboId == 0 || x.FboId == fbo.Oid) && x.CopyAlerts.HasValue && x.CopyOrders.Value).Select(x => x.Username).ToListAsync();

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
                    if(alertEmailAddressesUsers!=null)
                    {
                        foreach(var email in alertEmailAddressesUsers)
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
                var customerInfoByGroup = await _CustomerService.GetCustomersByGroupAndFbo(groupId, fboId, customerInfoByGroupId);
                //#17nvxpq: Fill-in missing template IDs if one isn't properly provided
                if (!pricingTemplateIds.Any(x => x > 0))
                {
                    List<PricingTemplate> templates = await _PricingTemplateService.GetAllPricingTemplatesForCustomerAsync(customerInfoByGroup.FirstOrDefault(), fboId, groupId);
                    pricingTemplateIds = templates.Select(x => x.Oid).ToList();
                }
                var fboPrices = await _context.Fboprices.Where(x =>
                    (!x.EffectiveTo.HasValue || x.EffectiveTo > universalTime) &&
                    (!x.EffectiveFrom.HasValue || x.EffectiveFrom <= universalTime) &&
                    x.Expired != true && x.Fboid == fboId).ToListAsync();
                var pricingTemplates = await _context.PricingTemplate.Where(x => x.Fboid == fboId && pricingTemplateIds.Contains(x.Oid))
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
                var priceBreakdownDisplayType = await GetPriceBreakdownDisplayType(fboId);

                //Prepare fees/taxes based on the provided departure type and flight type
                if (feesAndTaxes == null)
                {
                    feesAndTaxes = await _context.FbofeesAndTaxes.Include(x => x.OmitsByCustomer).Include(x => x.OmitsByPricingTemplate).Where(x =>
                        x.Fboid == fboId && (x.FlightTypeClassification == FlightTypeClassifications.All ||
                                             x.FlightTypeClassification == flightTypeClassifications ||
                                             flightTypeClassifications == FlightTypeClassifications.All)).ToListAsync();
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
                var customerPricingResults = (from cg in customerInfoByGroup
                                              join pt in pricingTemplates on fboId equals pt.Fboid into leftJoinPT
                                              from pt in leftJoinPT.DefaultIfEmpty()
                                              join ppt in customerMargins on (pt != null ? pt.Oid : 0) equals ppt.TemplateId
                                                  into leftJoinPPT
                                              from ppt in leftJoinPPT.DefaultIfEmpty()
                                              join fp in fboPrices on new
                                              {
                                                  fboId = (pt != null ? pt.Fboid : 0),
                                                  product = (pt != null ? (feesAndTaxesPassedIn == null ? pt.MarginTypeProduct : "JetA " + pt.MarginTypeProduct) : "")
                                              } equals new
                                              {
                                                  fboId = fp.Fboid ?? 0,
                                                  product = feesAndTaxesPassedIn == null ? fp.GenericProduct : fp.Product
                                              }
                                              join tmp in tempAddonMargin on new
                                              {
                                                  fboId = (pt != null ? pt.Fboid : 0)
                                              } equals new
                                              {
                                                  fboId = tmp.FboId
                                              } into leftJoinTMP
                                              from tmp in leftJoinTMP.DefaultIfEmpty()
                                              join cvf in customersViewedByFbo on new { cg.CustomerId, Fboid = _FboId } equals new
                                              {
                                                  cvf.CustomerId,
                                                  cvf.Fboid
                                              } into letJoinCVF
                                              from cvf in letJoinCVF.DefaultIfEmpty()
                                              join ccot in customerCompanyTypes on new
                                              { CustomerCompanyType = cg.CustomerCompanyType ?? 0, cg.GroupId } equals new
                                              { CustomerCompanyType = ccot.Oid, GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId }
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
                                                  DiscountType = (pt == null ? 0 : pt.DiscountType),
                                                  FboPrice = (fp == null ? 0 : fp.Price),
                                                  CustomerMarginAmount = (pt.MarginTypeProduct == "Retail" && tmp != null &&
                                                                          (tmp.MarginJet.HasValue)
                                                      ? (ppt == null ? 0 : ppt.Amount) + (double)tmp.MarginJet ?? 0
                                                      : (ppt == null ? 0 : ppt.Amount)),
                                                  amount = ppt == null ? 0 : ppt.Amount ,
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
                                                                                     pt.MarginType != MarginTypes.FlatFee)),
                                                  ExpirationDate = fp?.EffectiveTo,
                                                  Icao = (fbo.fboAirport == null ? "" : fbo.fboAirport.Icao),
                                                  Iata = (fbo.fboAirport == null ? "" : fbo.fboAirport.Iata),
                                                  Notes = (pt == null ? "" : pt.Notes),
                                                  Fbo = (fbo == null ? "" : fbo.Fbo),
                                                  Group = (fbo.Group == null ? "" : fbo.Group.GroupName),
                                                  PriceBreakdownDisplayType = priceBreakdownDisplayType,
                                                  Product = fp.Product.Replace(" Cost", "").Replace(" Retail", "").Replace("JetA", "Jet A")
                                              }).OrderBy(x => x.Company).ThenBy(x => x.PricingTemplateId).ThenBy(x => x.Product).ThenBy(x => x.MinGallons).ToList();

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
                        x.Product = "Jet A (Domestic Departure)";
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
                        x.Product = "Jet A (International Departure)";
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
                            productName += " (Domestic Departure)";
                        else if (domesticOptions.Count > 0 && internationalOptions.Count == 0)
                            productName += " (International Departure)";
                        x.Product = productName;
                        x.FeesAndTaxes = feesAndTaxes.Where(fee => fee.DepartureType == ApplicableTaxFlights.All)
                            .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != x.PricingTemplateId))
                            .ToList().Clone<FboFeesAndTaxes>().ToList();
                    });
                }

                List<CustomerWithPricing> resultsWithFees = new List<CustomerWithPricing>();
                resultsWithFees.AddRange(domesticOptions);
                resultsWithFees.AddRange(internationalOptions);
                resultsWithFees.AddRange(allDepartureOptions);
                
                //Set the "IsOmitted" case for all fees that might be omitted from a pricing template or customer specifically
                //Each collection of fees is cloned so updating the flag of one collection does not affect other pricing results where the template did not omit it
                resultsWithFees.ForEach(x =>
                {
                    x.FeesAndTaxes.ForEach(fee =>
                    {
                        if (fee.OmitsByPricingTemplate != null &&
                            fee.OmitsByPricingTemplate.Any(o =>
                                o.PricingTemplateId == x.PricingTemplateId))
                        {
                            fee.IsOmitted = true;
                            fee.OmittedFor = "P";
                        }

                        if ((fee.OmitsByCustomer != null && fee.OmitsByCustomer.Any(o =>
                            o.CustomerId == customerInfoByGroup.FirstOrDefault()?.CustomerId)))
                        {
                            fee.IsOmitted = true;
                            fee.OmittedFor = "C";
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

        public async Task<PriceDistributionService.PriceBreakdownDisplayTypes> GetPriceBreakdownDisplayType(int fboId)
        {
            bool hasDepartureTypeRule = false;
            bool hasFlightTypeRule = false;
            var priceBreakdownDisplayType = PriceDistributionService.PriceBreakdownDisplayTypes.SingleColumnAllFlights;

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
                priceBreakdownDisplayType = PriceDistributionService.PriceBreakdownDisplayTypes.SingleColumnAllFlights;
            }
            else if (!hasDepartureTypeRule && hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceDistributionService.PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly;
            }
            else if (hasDepartureTypeRule && !hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceDistributionService.PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly;
            }
            else if (hasDepartureTypeRule && hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceDistributionService.PriceBreakdownDisplayTypes.FourColumnsAllRules;
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
            var fbos = await _context.Fbos.Where(f => f.Active == true).Include(f => f.Users).Include("fboAirport").Where(x => x.GroupId > 1).ToListAsync();

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
            var oldPrices = await _context.Fboprices.Where(f =>
                    f.EffectiveTo <= DateTime.UtcNow && (f.Fboid.HasValue && fboIds.Contains(f.Fboid.Value)) && f.Price != null && f.Expired != true)
                .ToListAsync();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.BulkUpdateAsync(oldPrices);
            await transaction.CommitAsync();
        }
        #endregion
    }
}
