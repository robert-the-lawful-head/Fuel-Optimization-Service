﻿using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Extensions;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.Dto.UseCaseModels;
using FBOLinx.ServiceLayer.DTO.Responses.Customers;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.PricingTemplate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IPricingTemplateEntityService : IRepository<PricingTemplate, FboLinxContext>
    {
        Task<List<PricingTemplateGrid>> GetPricingTemplateGrid(int fboId, int groupId);
        Task<List<PricingTemplateGrid>> GetDefualtPricingTemplateGridByFboId(int fboId);
        Task<List<PricingTemplateGrid>> GetCostPlusPricingTemplates(int fboId);
        Task<List<PricingTemplateGrid>> GetPricingTemplatesWithEmailContent(int fboId, int groupId);
        Task<PricingTemplate> CopyPricingTemplate(int? currentPricingTemplateId, string pricingTemplateName);
        Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroupDto customer, int fboId, int groupId, int pricingTemplateId = 0);
        Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroupDto customer, int fboId, int groupId, int pricingTemplateId = 0);
        Task<List<PricingTemplate>> GetStandardTemplatesForAllCustomers(int groupId, int fboId);
        Task<List<CustomerAircraftsViewModel>> GetCustomerAircrafts(int groupId, int fboId = 0);
        Task<List<PricingTemplate>> GetAllPricingTemplates();
        Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForAllCustomers(int fboId, int groupId);
    }
    
    public class PricingTemplateEntityService : Repository<PricingTemplate, FboLinxContext>, IPricingTemplateEntityService
    {
        private readonly FboLinxContext _context;
        private readonly CustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        private readonly IFboPricesEntityService _fboPricesEntityService;
        private readonly IFboPreferencesService _fboPreferencesService;

        public PricingTemplateEntityService(FboLinxContext context,
            CustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IFboPricesEntityService fboPricesEntityService,
            IFboPreferencesService fboPreferencesService) : base(context)
        {
            _context = context;
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _fboPricesEntityService = fboPricesEntityService;
            _fboPreferencesService = fboPreferencesService;
        }

        public async Task<PricingTemplate> CopyPricingTemplate(int? currentPricingTemplateId, string pricingTemplateName)
        {
            var existingTemplate = _context.PricingTemplate.FirstOrDefault(s => s.Oid == currentPricingTemplateId);

            if (existingTemplate == null) return null;

            DB.Models.PricingTemplate pt = new DB.Models.PricingTemplate
            {
                Name = pricingTemplateName,
                Fboid = existingTemplate.Fboid,
                Default = false,
                Notes = existingTemplate.Notes,
                MarginType = existingTemplate.MarginType,
                Email = existingTemplate.Email,
                Subject = existingTemplate.Subject,
                EmailContentId = existingTemplate.EmailContentId
            };

            _context.PricingTemplate.Add(pt);
            await _context.SaveChangesAsync();

            return pt;
        }

        public Task<List<PricingTemplateGrid>> GetCostPlusPricingTemplates(int fboId)
        {
            var templateCustomersCount = (
                  from tc in
                  (
                      from cig in _context.CustomerInfoByGroup
                      join cct in _context.CustomCustomerTypes on cig.CustomerId equals cct.CustomerId
                      join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                      where pt.Fboid == fboId && !string.IsNullOrEmpty(cct.CustomerType.ToString())
                      select new
                      {
                          pt.Oid,
                          cct.CustomerId
                      }
                  )
                  group tc by tc.Oid into resultsGroup
                  select new
                  {
                      Oid = resultsGroup.Key,
                      Count = resultsGroup.Count()
                  }).ToList();

            var result = (
                from p in _context.PricingTemplate
                join tcc in templateCustomersCount on p.Oid equals tcc.Oid
                into leftJoinTemplateCustomersCount
                from tcc in leftJoinTemplateCustomersCount.DefaultIfEmpty()
                where p.Fboid == fboId && p.MarginType == 0
                select new PricingTemplateGrid
                {
                    CustomersAssigned = tcc == null ? 0 : tcc.Count
                });

            return result.ToListAsync();
        }

        public async Task<List<PricingTemplateGrid>> GetDefualtPricingTemplateGridByFboId(int fboId)
        {
            return await (
                from p in _context.PricingTemplate
                join cm in (
                    from c in _context.CustomerMargins
                    group c by new { c.TemplateId }
                    into cmResults
                    select new CustomerMarginModel
                    {
                        TemplateId = cmResults.Key.TemplateId,
                        MaxPrice = cmResults.Max(x => (x.Amount ?? 0))
                    }) on p.Oid equals cm.TemplateId
                into leftJoinCustomerMargins
                from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                join fp in (
                    from f in _context.Fboprices
                    where f.EffectiveTo > DateTime.UtcNow && f.Fboid == fboId && f.Expired != true
                    select f) on p.MarginTypeProduct equals fp.Product into leftJoinFboPrices
                from fp in leftJoinFboPrices.DefaultIfEmpty()
                where p.Fboid == fboId
                    && (p.Default ?? false)
                select new PricingTemplateGrid
                {
                    CustomerId = p.CustomerId,
                    Default = p.Default,
                    Fboid = p.Fboid,
                    Margin = cm == null ? 0 : cm.MaxPrice,
                    MarginType = p.MarginType,
                    Name = p.Name,
                    Notes = p.Notes,
                    Subject = p.Subject,
                    Email = p.Email,
                    Oid = p.Oid,
                    Type = p.Type,
                    IntoPlanePrice = (fp == null ? 0 : (fp.Price ?? 0)) +
                                     (cm == null ? 0 : cm.MaxPrice)
                }).ToListAsync();
        }

        public async Task<List<PricingTemplateGrid>> GetPricingTemplateGrid(int fboId,int groupId)
        {
            //Load customer assignments by template ID
            var customerAssignments = await (from cibg in _context.CustomerInfoByGroup
                                             join cct in _context.CustomCustomerTypes on cibg.CustomerId equals cct.CustomerId
                                             join c in _context.Customers on cibg.CustomerId equals c.Oid
                                             where cct.Fboid == fboId && cibg.GroupId == groupId && (c.Suspended == null || c.Suspended == false)
                                             select new
                                             { CustomerType = cct.CustomerType }).ToListAsync();

            //Load customer aircrafts assignments
            var customerAircraftAssignments = await GetCustomerAircrafts(groupId, fboId);

            //Separate inner queries first for FBO Prices and Margin Tiers
            var oldPrices = await _context.Fboprices.Where(f => f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && (f.Expired == null || f.Expired != true)).ToListAsync();
            foreach (var p in oldPrices)
            {
                p.Expired = true;
                _context.Fboprices.Update(p);
            }
            await _context.SaveChangesAsync();

            var tempFboPrices = await _fboPricesEntityService.GetListBySpec(new CurrentFboPricesByFboIdSpecification(fboId));

            tempFboPrices = tempFboPrices.Where(fp => fp.Product.Contains("JetA")).OrderBy(f => f.Oid).ToList();

            var oldJetAPriceExists = tempFboPrices.Where(f => f.Product.Contains("JetA") && f.EffectiveFrom <= DateTime.UtcNow && f.EffectiveTo <= DateTime.UtcNow).ToList();
            if (oldJetAPriceExists.Count() > 0)
            {
                // Set old prices to expire, remove from collection
                for (int i = 0; i <= 1; i++)
                {
                    oldJetAPriceExists[i].Expired = true;
                    await _fboPricesEntityService.DeleteAsync(oldJetAPriceExists[i]);
                    await _context.SaveChangesAsync();

                    tempFboPrices.Remove(oldJetAPriceExists[i]);
                }
            }

            var tempPricingTemplates = await (_context.PricingTemplate.Where(x => x.Fboid == fboId).ToListAsync());

            var tempMarginTiers = await (from pt in _context.PricingTemplate
                                         join cm in _context.CustomerMargins on pt.Oid equals cm.TemplateId
                                              into leftJoinCm
                                         from cm in leftJoinCm.DefaultIfEmpty()
                                         join prt in _context.PriceTiers on
                                             new { PriceTierId = cm.PriceTierId, Min = 1.0 } equals
                                             new { PriceTierId = prt.Oid, Min = prt.Min.GetValueOrDefault() }
                                         into leftJoinPrt
                                         from prt in leftJoinPrt.DefaultIfEmpty()
                                         where pt.Fboid == fboId && prt != null
                                         select new { PricingTemplateId = pt.Oid, cm.Amount }).ToListAsync();

            var flightTypeClassifications = FlightTypeClassifications.Private;
            var departureType = ApplicableTaxFlights.DomesticOnly;
            var universalTime = DateTime.Today.ToUniversalTime();
            var addOnMargins = await (
                           from s in _context.TempAddOnMargin
                           where s.FboId == fboId && s.EffectiveTo >= universalTime
                           select s).ToListAsync();

            //Prepare fees/taxes based on the provided departure type and flight type
            var feesAndTaxes = await _context.FbofeesAndTaxes.Include(x => x.OmitsByCustomer).Include(x => x.OmitsByPricingTemplate).Where(x =>
                      x.Fboid == fboId && (x.FlightTypeClassification == FlightTypeClassifications.All ||
                                           x.FlightTypeClassification == FlightTypeClassifications.Private))
                      .AsNoTracking()
                      .ToListAsync();
            feesAndTaxes = feesAndTaxes.Where(x =>
                    (x.FlightTypeClassification == FlightTypeClassifications.All ||
                     x.FlightTypeClassification == flightTypeClassifications) &&
                    (x.DepartureType == departureType ||
                     departureType == FBOLinx.Core.Enums.ApplicableTaxFlights.All ||
                     x.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.All)).ToList();

                var customerPricingResults = (from pt in tempPricingTemplates 
                                          join ppt in tempMarginTiers on (pt != null ? pt.Oid : 0) equals ppt.PricingTemplateId
                                             into leftJoinPPT
                                         from ppt in leftJoinPPT.DefaultIfEmpty()
                                         join fp in tempFboPrices on new
                                          {
                                              Fboid = pt != null ? pt.Fboid : 0,
                                              Product = pt != null ? pt.MarginTypeProduct : ""
                                          } equals new
                                          {
                                              Fboid = fp.Fboid.GetValueOrDefault(),
                                              Product = fp.Product.Split(' ')[1]
                                          }
                                          into leftJoinFP
                                          from fp in leftJoinFP.DefaultIfEmpty()
                                          join am in addOnMargins on new { FboId = fboId } equals new { am.FboId }
                                          into leftJoinAM
                                          from am in leftJoinAM.DefaultIfEmpty()
                                          select new CustomerWithPricingResponse
                                          {
                                              PricingTemplateId = pt.Oid,
                                              MarginType = (pt == null ? 0 : pt.MarginType),
                                              DiscountType = (pt == null ? 0 : pt.DiscountType),
                                              FboPrice = (fp == null || fp.Oid == 0 ? 0 : fp.Price),
                                              CustomerMarginAmount = pt == null ? 0 : (pt.MarginTypeProduct == "Retail"
                                                  ? (ppt == null ? 0 : ppt.Amount) + (am == null || am.MarginJet == null ? 0 : (double)am.MarginJet)
                                                  : (ppt == null ? 0 : ppt.Amount)),
                                              amount = ppt == null ? 0 : ppt.Amount,
                                              PricingTemplateName = pt.Name
                                          }).ToList();

            customerPricingResults.ForEach(x =>
            {
                if (x.FboPrice > 0)
                {
                    x.AllInPrice = GetAllInPrice(x);

                    if (feesAndTaxes.Count > 0)
                    {
                        //Add domestic-departure-only price options
                        List<CustomerWithPricingResponse> domesticOptions = new List<CustomerWithPricingResponse>();
                        if ((feesAndTaxes.Any(y => y.DepartureType == ApplicableTaxFlights.DomesticOnly) &&
                            departureType == ApplicableTaxFlights.All) || departureType == ApplicableTaxFlights.DomesticOnly)
                        {
                            domesticOptions = customerPricingResults.Where(c => c.PricingTemplateId == x.PricingTemplateId).ToList();
                            domesticOptions.ForEach(y =>
                            {
                                y.Product = "Jet A (Domestic Departure)";
                                y.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                    fee.DepartureType == ApplicableTaxFlights.DomesticOnly ||
                                    fee.DepartureType == ApplicableTaxFlights.All)
                                    .ToList().Clone<FboFeesAndTaxes>().ToList();
                            });
                        }

                        //Add international-departure-only price options
                        List<CustomerWithPricingResponse> internationalOptions = new List<CustomerWithPricingResponse>();
                        if ((feesAndTaxes.Any(y => y.DepartureType == ApplicableTaxFlights.InternationalOnly) &&
                             departureType == ApplicableTaxFlights.All) ||
                            departureType == ApplicableTaxFlights.InternationalOnly)
                        {
                            internationalOptions = customerPricingResults.Where(c => c.PricingTemplateId == x.PricingTemplateId).ToList();
                            internationalOptions.ForEach(y =>
                            {
                                y.Product = "Jet A (International Departure)";
                                y.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                    fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                                    fee.DepartureType == ApplicableTaxFlights.All)
                                    .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != y.PricingTemplateId))
                                    .ToList();
                                y.FeesAndTaxes = feesAndTaxes.Where(fee =>
                                        fee.DepartureType == ApplicableTaxFlights.InternationalOnly ||
                                        fee.DepartureType == ApplicableTaxFlights.All)
                                    .ToList().Clone<FboFeesAndTaxes>().ToList();
                            });
                        }

                        //Add price options for all departure types
                        List<CustomerWithPricingResponse> allDepartureOptions = new List<CustomerWithPricingResponse>();
                        if ((feesAndTaxes.Any(y => y.DepartureType == ApplicableTaxFlights.All) &&
                           departureType == ApplicableTaxFlights.All) &&
                          (domesticOptions.Count == 0 || internationalOptions.Count == 0))
                        {
                            allDepartureOptions = customerPricingResults.Where(c => c.PricingTemplateId == x.PricingTemplateId).ToList();
                            allDepartureOptions.ForEach(y =>
                            {
                                var productName = y.Product;
                                if (domesticOptions.Count == 0)
                                    productName += " (Domestic Departure)";
                                y.Product = productName;
                                y.FeesAndTaxes = feesAndTaxes.Where(fee => fee.DepartureType == ApplicableTaxFlights.All)
                                    .Where(fee => fee.OmitsByPricingTemplate == null || fee.OmitsByPricingTemplate.All(o => o.PricingTemplateId != y.PricingTemplateId))
                                    .ToList().Clone<FboFeesAndTaxes>().ToList();
                            });
                        }

                        List<CustomerWithPricingResponse> resultsWithFees = new List<CustomerWithPricingResponse>();
                        resultsWithFees.AddRange(domesticOptions);
                        resultsWithFees.AddRange(internationalOptions);
                        resultsWithFees.AddRange(allDepartureOptions);

                        //Set the "IsOmitted" case for all fees that might be omitted from a pricing template or customer specifically
                        //Each collection of fees is cloned so updating the flag of one collection does not affect other pricing results where the template did not omit it
                        var isRetailMinus = false;
                        resultsWithFees.ForEach(y =>
                        {
                            y.FeesAndTaxes.ForEach(fee =>
                            {

                                    isRetailMinus = customerPricingResults.Where(x => x.PricingTemplateId == y.PricingTemplateId).FirstOrDefault().MarginType == MarginTypes.RetailMinus;
                                    fee.SetIsOmittedForPricing((int)y.PricingTemplateId,isRetailMinus);
                            });
                        });

                        x.AllInPrice = GetAllInPrice(resultsWithFees.FirstOrDefault());
                    }
                }
                else
                    x.AllInPrice = 0;
            });
                var decimalPrecisionFormat = await _fboPreferencesService.GetDecimalPrecisionStringFormat(fboId);
                //Join the inner queries on the pricing templates
                var pricingTemplates = (from p in tempPricingTemplates
                                    join c in customerPricingResults on p.Oid equals c.PricingTemplateId
                                    join cm in tempMarginTiers on p.Oid equals cm.PricingTemplateId
                                        into leftJoinCmTiers
                                    from cm in leftJoinCmTiers.DefaultIfEmpty()
                                    join fp in tempFboPrices on "JetA " + p.MarginTypeProduct equals fp.Product
                                        into leftJoinFp
                                    from fp in leftJoinFp.DefaultIfEmpty()
                                    where p.Fboid == fboId && (fp == null || fp.EffectiveFrom == null || fp.EffectiveFrom <= DateTime.UtcNow)
                                    select new PricingTemplateGrid
                                    {
                                        CustomerId = p.CustomerId,
                                        Default = p.Default,
                                        Fboid = p.Fboid,
                                        MarginType = p.MarginType,
                                        Name = p.Name,
                                        Notes = p.Notes,
                                        Oid = p.Oid,
                                        Type = p.Type,
                                        Subject = p.Subject,
                                        Email = p.Email,
                                        IsPricingExpired = fp == null && p.MarginType != MarginTypes.FlatFee ? true : false,// && (p.MarginType == null || p.MarginType == MarginTypes.FlatFee) 
                                        EmailContentId = p.EmailContentId,
                                        DiscountType = p.DiscountType,
                                        AllInPrice = c.AllInPrice,
                                        CustomersAssigned = customerAssignments.Sum(x => x.CustomerType == p.Oid ? 1 : 0),
                                        AircraftsAssigned = customerAircraftAssignments.Sum(y => y.PricingTemplateId == p.Oid ? 1 : 0),
                                        PricingFormula = (p.MarginType == MarginTypes.CostPlus ? "Cost + " : "Retail - ") + (p.DiscountType == DiscountTypes.Percentage ?
                                                    (cm != null ? cm.Amount.ToString() : "0") + "%"
                                                    : string.Format(decimalPrecisionFormat, (cm == null ? 0 : cm.Amount.GetValueOrDefault())))
                                    }).ToList();

            return pricingTemplates;
        }

        public async Task<List<PricingTemplateGrid>> GetPricingTemplatesWithEmailContent(int fboId, int groupId)
        {
            var templates = await GetPricingTemplateGrid(fboId, groupId);

            var emailContent = await _context.EmailContent.Where(e => e.FboId == fboId).ToListAsync();

            var lastSent = await (from dl in _context.DistributionLog
                                  join dl2 in (
                                     from distributionLog in _context.DistributionLog
                                     where distributionLog.Fboid == fboId
                                     group distributionLog by distributionLog.PricingTemplateId into d
                                     select new { PricingTemplateId = d.Key, DateMax = d.Max(x => x.DateSent) }
                                  ) on new { PricingTemplateId = dl.PricingTemplateId, DateSent = dl.DateSent } equals new { PricingTemplateId = dl2.PricingTemplateId, DateSent = dl2.DateMax }
                                  select new { PricingTemplateId = dl2.PricingTemplateId, LastSent = dl2.DateMax }).ToListAsync();

           var templatesWithEmailContent = (
            from t in templates
            join l in lastSent on t.Oid equals l.PricingTemplateId
            into leftJoinedL
            from l in leftJoinedL.DefaultIfEmpty()
            join ec in emailContent on t.EmailContentId equals ec.Oid
            into leftJoinedEC
            from ec in leftJoinedEC.DefaultIfEmpty()
            select new PricingTemplateGrid
            {
                CustomerId = t.CustomerId,
                Default = t.Default,
                Fboid = t.Fboid,
                Margin = t.Margin,
                YourMargin = t.Margin,
                MarginType = t.MarginType,
                Name = t.Name,
                Notes = t.Notes,
                Oid = t.Oid,
                Type = t.Type,
                Subject = t.Subject,
                Email = t.Email,
                IsPricingExpired = t.IsPricingExpired,
                //IntoPlanePrice = t.IntoPlanePrice,
                CustomersAssigned = t.CustomersAssigned,
                EmailContentId = t.EmailContentId,
                EmailContent = ec,
                LastSent= l == null ? "N/A" : l.LastSent.ToString("MM/dd/yyyy HH:mm")
            }
            ).ToList();

            foreach (var t in templatesWithEmailContent)
            {
                t.CustomerEmails = await(from cg in _context.CustomerInfoByGroup.Where((x => x.GroupId == groupId && x.Active.HasValue && x.Active.Value))
                                         join c in _context.Customers on cg.CustomerId equals c.Oid
                                         join cc in _context.CustomCustomerTypes.Where(x => x.Fboid == fboId && x.CustomerType == t.Oid) on cg.CustomerId equals cc.CustomerId
                                         join custc in _context.CustomerContacts on c.Oid equals custc.CustomerId
                                         join co in _context.Contacts on custc.ContactId equals co.Oid
                                         join cibg in _context.ContactInfoByGroup on new {ContactId = co.Oid, GroupId = groupId} equals new {ContactId = cibg.ContactId, GroupId = cibg.GroupId}
                                         join cibf in _context.ContactInfoByFbo on new { ContactId = (int?) co.Oid, FboId = (int?) fboId } equals new { ContactId = cibf.ContactId, FboId = cibf.FboId } into leftJoinCIBF
                                         from cibf in leftJoinCIBF.DefaultIfEmpty()
                                         where ((cibf.ContactId != null && (cibf.CopyAlerts ?? false)) || (cibf.ContactId == null && (cibg.CopyAlerts ?? false)))
                                         && !string.IsNullOrEmpty(cibg.Email)
                                         && (c.Suspended ?? false) == false
                                         select cibg.Email
                                ).ToListAsync();
            }
            return templatesWithEmailContent;
        }

        public async Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroupDto customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            var templates = await (from cg in _context.CustomerInfoByGroup
                                   join c in _context.Customers on cg.CustomerId equals c.Oid
                                   join cct in _context.CustomCustomerTypes on new
                                   {
                                       customerId = cg.CustomerId,
                                       fboId = fboId
                                   } equals new
                                   {
                                       customerId = cct.CustomerId,
                                       fboId = cct.Fboid
                                   }
                                   join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid

                                   where cg.GroupId == groupId
                                         && cg.CustomerId == customer.CustomerId
                                         && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
                                   select pt).ToListAsync();
            return templates;
        }

        public async Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroupDto customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            var aircraftPricesResult = await(from ap in _context.AircraftPrices
                                             join ca in _context.CustomerAircrafts on ap.CustomerAircraftId equals ca.Oid
                                             join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                             where ca.CustomerId == customer.CustomerId
                                                   && ca.GroupId == groupId
                                                   && pt.Fboid == fboId
                                                   && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
                                             group pt by new { pt.Oid, pt.Name, pt.Fboid, pt.CustomerId, pt.Notes, pt.Type, pt.MarginType }
                into groupedPrices
                                             select new DB.Models.PricingTemplate()
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

        public async Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForAllCustomers(int fboId, int groupId)
        {
            var aircraftPricesResult = await (from ap in _context.AircraftPrices
                                              join ca in _context.CustomerAircrafts on ap.CustomerAircraftId equals ca.Oid
                                              join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                              where ca.GroupId == groupId
                                                    && pt.Fboid == fboId
                                              group pt by new { pt.Oid, pt.Name, pt.Fboid, pt.CustomerId, pt.Notes, pt.Type, pt.MarginType }
                into groupedPrices
                                              select new DB.Models.PricingTemplate()
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

        public async Task<List<PricingTemplate>> GetStandardTemplatesForAllCustomers(int fboId, int groupId)
        {
            var result = await (from cg in _context.CustomerInfoByGroup
                                join c in _context.Customers on cg.CustomerId equals c.Oid
                                join cct in _context.CustomCustomerTypes on new
                                {
                                    customerId = cg.CustomerId,
                                    fboId = fboId
                                } equals new
                                {
                                    customerId = cct.CustomerId,
                                    fboId = cct.Fboid
                                }
                                join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid

                                where cg.GroupId == groupId && (!c.Suspended.HasValue || !c.Suspended.Value)
                                select new PricingTemplate { Oid = pt.Oid, CustomerId = cg.CustomerId, Name = pt.Name }).ToListAsync();

            return result;
        }

        public async Task<List<PricingTemplate>> GetAllPricingTemplates()
        {
            var templates = await (from p in _context.PricingTemplate
                                   select new PricingTemplate
                                   {
                                       Oid = p.Oid,
                                       Fboid = p.Fboid,
                                       Default = p.Default
                                   }).ToListAsync();

            return templates;
        }

        #region Private Methods
        private double GetAllInPrice(CustomerWithPricingResponse customerWithPricing)
        {
            //Start by calculating the total before the margin including pre-margin taxes and fees
            double result = GetPreMarginSubTotal(customerWithPricing);

            //Next apply the margin to the pre-margin subtotal
            result = GetSubtotalWithMargin(result, customerWithPricing);

            //Finally add the post-margin fees and taxes
            result = GetPostMarginTotal(result, customerWithPricing);

            return result;
        }

        private double GetPreMarginSubTotal(CustomerWithPricingResponse customerWithPricing)
        {
            if (!customerWithPricing.MarginType.HasValue || customerWithPricing.MarginType == MarginTypes.RetailMinus || customerWithPricing.MarginType == MarginTypes.FlatFee)
                return (customerWithPricing.FboPrice.GetValueOrDefault());
            double result = customerWithPricing.FboPrice.GetValueOrDefault();
            double basePrice = customerWithPricing.FboPrice.GetValueOrDefault();
            if (customerWithPricing.FeesAndTaxes == null)
                return result;

            foreach (var feeAndTax in customerWithPricing.FeesAndTaxes.Where(x => x.WhenToApply == FBOLinx.Core.Enums.FeeCalculationApplyingTypes.PreMargin && !x.IsOmitted).OrderBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.Percentage ? 1 : 2).ThenBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.FlatPerGallon ? 1 : 2))
            {
                result += feeAndTax.GetCalculatedValue(basePrice, result);
            }

            return result;
        }

        private double GetSubtotalWithMargin(double preMarginSubTotal, CustomerWithPricingResponse customerWithPricing)
        {
            double result = 0;
            double itp = 0;
            if (!customerWithPricing.MarginType.HasValue)
                result = 0;

            else if (customerWithPricing.MarginType.Value == MarginTypes.CostPlus)
            {
                if (customerWithPricing.DiscountType.GetValueOrDefault() == DiscountTypes.Percentage)
                {
                    itp = customerWithPricing.FboPrice.GetValueOrDefault() * (Math.Abs(customerWithPricing.CustomerMarginAmount.GetValueOrDefault()) / 100);
                    result = (preMarginSubTotal + itp);
                }
                else
                {
                    result = (preMarginSubTotal + Math.Abs(customerWithPricing.CustomerMarginAmount.GetValueOrDefault()));
                }
            }
            else if (customerWithPricing.MarginType.Value == MarginTypes.RetailMinus)
            {
                if (customerWithPricing.DiscountType.GetValueOrDefault() == DiscountTypes.Percentage)
                {

                    itp = customerWithPricing.FboPrice.GetValueOrDefault() * (Math.Abs(customerWithPricing.CustomerMarginAmount.GetValueOrDefault()) / 100);
                    result = (preMarginSubTotal - itp);
                }
                else
                {
                    result = (preMarginSubTotal - Math.Abs(customerWithPricing.CustomerMarginAmount.GetValueOrDefault()));
                }
            }
            return result;
        }

        private double GetPostMarginTotal(double subTotalWithMargin, CustomerWithPricingResponse customerWithPricing)
        {
            double result = subTotalWithMargin;

            if (customerWithPricing.FeesAndTaxes == null)
                return result;

            foreach (var feeAndTax in customerWithPricing.FeesAndTaxes.Where(x => x.WhenToApply == FBOLinx.Core.Enums.FeeCalculationApplyingTypes.PostMargin && !x.IsOmitted).OrderBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.Percentage ? 1 : 2).ThenBy(x => x.CalculationType == FBOLinx.Core.Enums.FeeCalculationTypes.FlatPerGallon ? 1 : 2))
            {
                result += feeAndTax.GetCalculatedValue(subTotalWithMargin, result);
            }

            return result;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetCustomerAircrafts(int groupId, int fboId = 0)
        {
            var pricingTemplates = await GetStandardPricingTemplatesForAllCustomers(fboId, groupId);

            var aircraftPricingTemplates = await GetCustomerAircraftTemplates(fboId, groupId);

            var defaultPricingTemplate = await _context.PricingTemplate.Where(p => p.Fboid == fboId && p.Default == true).FirstOrDefaultAsync();

            var result = await GetCustomerAircrafts(groupId);

            result.ForEach(x =>
            {
                if (x.Oid > 0)
                {
                    var aircraftPricingTemplate = aircraftPricingTemplates.FirstOrDefault(pt => pt.CustomerAircraftId == x.Oid);
                    if (aircraftPricingTemplate != null)
                    {
                        x.PricingTemplateId = aircraftPricingTemplate?.Oid;
                        x.PricingTemplateName = aircraftPricingTemplate?.Name;
                    }
                    else
                    {
                        var pricingTemplate = pricingTemplates.FirstOrDefault(pt => pt.CustomerId == x.CustomerId);
                        if (pricingTemplate != null)
                        {
                            x.PricingTemplateId = pricingTemplate?.Oid;
                            x.PricingTemplateName = pricingTemplate?.Name;
                        }
                        else
                        {
                            x.PricingTemplateId = defaultPricingTemplate?.Oid;
                            x.PricingTemplateName = defaultPricingTemplate?.Name;
                        }
                        x.IsCompanyPricing = true;
                    }
                }
            });

            return result;
        }

        private async Task<List<CustomerAircraftsPricingTemplatesModel>> GetCustomerAircraftTemplates(int fboId, int groupId)
        {
            var aircraftPricingTemplates = await (
                                    from ap in _context.AircraftPrices
                                    join ca in _context.CustomerAircrafts on ap.CustomerAircraftId equals ca.Oid
                                    join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                    into leftJoinPt
                                    from pt in leftJoinPt.DefaultIfEmpty()
                                    where ca.GroupId == groupId && pt.Fboid == fboId && fboId > 0
                                    select new CustomerAircraftsPricingTemplatesModel
                                    {
                                        Oid = ap == null ? 0 : pt.Oid,
                                        Name = ap == null ? "" : pt.Name,
                                        CustomerAircraftId = ap == null ? 0 : ap.CustomerAircraftId
                                    }).ToListAsync();

            return aircraftPricingTemplates;
        }

        public async Task<List<DB.Models.PricingTemplate>> GetStandardPricingTemplatesForAllCustomers(int fboId, int groupId)
        {
            List<DB.Models.PricingTemplate> result = new List<DB.Models.PricingTemplate>();

            var standardTemplates = await GetStandardTemplatesForAllCustomers(fboId, groupId);
            result.AddRange(standardTemplates);
            return result;
        }

        private async Task<List<CustomerAircraftsViewModel>> GetCustomerAircrafts(int groupId)
        {
            var customers = await _customerInfoByGroupEntityService.GetListBySpec(new CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(groupId));
            var customerAircrafts = customers.SelectMany(c => c.Customer.CustomerAircrafts).ToList();

            List<CustomerAircraftsViewModel> result = (from c in customers
                                                       join ca in customerAircrafts on c.CustomerId equals ca.CustomerId into leftJoinCA
                                                       from ca in leftJoinCA.DefaultIfEmpty()
                                                       select new CustomerAircraftsViewModel
                                                       {
                                                           Oid = ca == null ? 0 : ca.Oid,
                                                           GroupId = c.GroupId,
                                                           CustomerId = c.CustomerId,
                                                           Company = c.Company,
                                                           AircraftId = ca == null ? 0 : ca.AircraftId,
                                                           TailNumber = ca?.TailNumber,
                                                           Size = ca == null ? AircraftSizes.NotSet : ca.Size.HasValue && ca.Size != AircraftSizes.NotSet ? ca.Size : (AircraftSizes.NotSet),
                                                           BasedPaglocation = ca?.BasedPaglocation,
                                                           NetworkCode = ca?.NetworkCode,
                                                           AddedFrom = ca == null ? 0 : ca.AddedFrom,
                                                           IsFuelerlinxNetwork = c.Customer.FuelerlinxId > 0
                                                       })
               .OrderBy(x => x.TailNumber).ToList();

            return result;
        }
        #endregion
    }
}
