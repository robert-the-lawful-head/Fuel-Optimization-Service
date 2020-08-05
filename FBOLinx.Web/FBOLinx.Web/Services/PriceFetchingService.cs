using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Data;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Services
{
    public class PriceFetchingService
    {
        private FboLinxContext _context;
        private int _FboId;
        private int _GroupId;

        public PriceFetchingService(FboLinxContext context)
        {
            _context = context;
        }

        #region Public Methods

        public async Task<List<CustomerWithPricing>> GetCustomerPricingByLocationAsync(string icao, int customerId)
        {
            List<string> airports = icao.Split(',').Select(x => x.Trim()).ToList();
            List<CustomerWithPricing> result = new List<CustomerWithPricing>();
            List<Fboairports> fboAirports = await _context.Fboairports
                                                            .Include(x => x.Fbo)
                                                            .ThenInclude(x => x.Group)
                                                            .Where(x => airports.Any(a => a == x.Icao) && x.Fbo != null && x.Fbo.Active == true && x.Fbo.Group != null && x.Fbo.Group.Active == true)
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
                    await GetCustomerPricingAsync(fbo.Oid, fbo.GroupId.GetValueOrDefault(), customerInfoByGroup.Oid, templates.Select(x => x.Oid).ToList());

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

        public async Task<List<CustomerWithPricing>> GetCustomerPricingAsync(int fboId, int groupId, int customerInfoByGroupId, List<int> pricingTemplateIds)
        {
            _FboId = fboId;
            _GroupId = groupId;

            try
            {
                var defaultPricingTemplate = await _context.PricingTemplate
                    .Where(x => x.Fboid == fboId && x.Default.GetValueOrDefault()).FirstOrDefaultAsync();
                int defaultPricingTemplateId = 0;
                if (defaultPricingTemplate != null)
                    defaultPricingTemplateId = defaultPricingTemplate.Oid;

                var customerPricingResults = await (from cg in _context.CustomerInfoByGroup
                    join c in _context.Customers on cg.CustomerId equals c.Oid
                    join pt in _context.PricingTemplate on fboId equals pt.Fboid into leftJoinPT
                    from pt in leftJoinPT.DefaultIfEmpty()
                    join ppt in _context.PriceTiers.Include("CustomerMargin") on pt.Oid equals ppt.CustomerMargin
                            .TemplateId
                        into leftJoinPPT
                    from ppt in leftJoinPPT.DefaultIfEmpty()
                    join ff in _context.Fbofees on new {fboId = _FboId, feeType = 8} equals new
                    {
                        fboId = ff.Fboid,
                        feeType = ff.FeeType
                    } into leftJoinFF
                    from ff in leftJoinFF.DefaultIfEmpty()
                    join fp in _context.Fboprices.Where(x =>
                        (!x.EffectiveTo.HasValue || x.EffectiveTo > DateTime.UtcNow) &&
                        x.Expired != true) on new
                    {
                        fboId = (pt != null ? pt.Fboid : 0),
                        product = (pt != null ? pt.MarginTypeProduct : "")
                    } equals new
                    {
                        fboId = fp.Fboid.GetValueOrDefault(),
                        product = fp.Product
                    } 
                    join tmp in _context.TempAddOnMargin.Where((x =>
                                                        x.EffectiveFrom < DateTime.Now &&
                                                         x.EffectiveTo > DateTime.Now)) on new
                                                        {
                                                            fboId = (pt != null ? pt.Fboid : 0)
                                                        } equals new
                                                        {
                                                            fboId = tmp.FboId
                                                        } into leftJoinTMP
                                                    from tmp in leftJoinTMP.DefaultIfEmpty()                                
                    join cvf in _context.CustomersViewedByFbo on new {cg.CustomerId, Fboid = _FboId} equals new
                    {
                        cvf.CustomerId,
                        cvf.Fboid
                    } into letJoinCVF
                    from cvf in letJoinCVF.DefaultIfEmpty()
                    join ccot in _context.CustomerCompanyTypes on new { CustomerCompanyType = cg.CustomerCompanyType.GetValueOrDefault(), cg.GroupId} equals new {CustomerCompanyType = ccot.Oid, GroupId = ccot.GroupId == 0 ? groupId : ccot.GroupId } 
                    into leftJoinCCOT
                    from ccot in leftJoinCCOT.DefaultIfEmpty()
                    join fbo in _context.Fbos on pt.Fboid equals fbo.Oid
                    join fboAirport in _context.Fboairports on fbo.Oid equals fboAirport.Fboid
                    join groups in _context.Group on fbo.GroupId equals groups.Oid
                    where cg.GroupId == _GroupId
                          && (customerInfoByGroupId == 0 || cg.Oid == customerInfoByGroupId)
                          && (pricingTemplateIds.Any(x => x == pt.Oid))
                    select new CustomerWithPricing()
                    {
                        CustomerId = cg.CustomerId,
                        CustomerInfoByGroupId = cg.Oid,
                        Company = cg.Company,
                        PricingTemplateId = pt.Oid,
                        DefaultCustomerType = cg.CustomerType,
                        MarginType = pt.MarginType,
                        FboPrice = fp.Price,
                        CustomerMarginAmount = pt.MarginTypeProduct == "JetA Retail" && tmp.MarginJet.HasValue ? ppt.CustomerMargin.Amount + (double)tmp.MarginJet.Value ?? 0 : ppt.CustomerMargin.Amount,
                        FboFeeAmount = ff.FeeAmount,
                        Suspended = cg.Suspended,
                        FuelerLinxId = c.FuelerlinxId,
                        Network = cg.Network,
                        GroupId = cg.GroupId,
                        FboId = pt.Fboid,
                        NeedsAttention = !(cvf != null && cvf.Oid > 0),
                        HasBeenViewed = (cvf != null && cvf.Oid > 0),
                        PricingTemplateName = pt == null ? "" : pt.Name,
                        MinGallons = ppt.Min,
                        MaxGallons = ppt.Max,
                        CustomerCompanyType = cg.CustomerCompanyType,
                        CustomerCompanyTypeName = ccot == null || string.IsNullOrEmpty(ccot.Name) ? "" : ccot.Name,
                        IsPricingExpired = (fp == null && (pt == null || pt.MarginType == null || pt.MarginType != PricingTemplate.MarginTypes.FlatFee)),
                        ExpirationDate = fp.EffectiveTo,
                        Icao = fboAirport.Icao,
                        Iata = fboAirport.Iata,
                        Notes = pt.Notes,
                        Fbo = fbo.Fbo,
                        Group = groups.GroupName
                    }).OrderBy(x => x.Company).ThenBy(x => x.PricingTemplateId).ThenBy(x => x.MinGallons).ToListAsync();

                return customerPricingResults;
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
            var customerAircrafts = _context.CustomerAircrafts.Where(x => x.CustomerId == customer.CustomerId && x.GroupId == groupId);

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

        public async Task<List<PricingTemplatesGridViewModel>> GetPricingTemplates(int fboId, int? groupId)
        {
            Fboprices jetaACostRecord = await _context.Fboprices.Where(x => x.Fboid == fboId && x.Product == "JetA Cost" && x.Expired != true).FirstOrDefaultAsync();
            IEnumerable<Utilities.Enum.EnumDescriptionValue> products = Utilities.Enum.GetDescriptions(typeof(Models.Fboprices.FuelProductPriceTypes));

            var fboPrices = await (from f in _context.Fboprices
                                   where f.EffectiveTo > DateTime.UtcNow && f.Fboid == fboId && f.Expired != true
                                   select f).ToListAsync();

            var templateCustomersCount = await (
                from tc in
                (
                    from cig in _context.CustomerInfoByGroup
                    join cct in _context.CustomCustomerTypes on cig.CustomerId equals cct.CustomerId
                    join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                    where cig.GroupId == groupId && pt.Fboid == fboId && !string.IsNullOrEmpty(cct.CustomerType.ToString())
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
                }).ToListAsync();

            var resultPrices =
                          from p in products
                          join f in fboPrices on new { Product = p.Description, FboId = fboId } equals new { f.Product, FboId = f.Fboid.GetValueOrDefault() }
                          into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in (from s in _context.TempAddOnMargin
                                     where s.FboId == fboId && s.EffectiveTo >= DateTime.Today.ToUniversalTime()
                                     select s) on fboId equals s.FboId
                              into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          select new
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.Now,
                              f?.EffectiveTo,
                              TimeStamp = f?.Timestamp,
                              f?.SalesTax,
                              f?.Currency,
                              tempJet = s?.MarginJet,
                              tempAvg = s?.MarginAvgas,
                              tempId = s?.Id,
                              tempDateFrom = s?.EffectiveFrom,
                              tempDateTo = s?.EffectiveTo
                          };

            double? jetACost = resultPrices.FirstOrDefault(s => s.Product == "JetA Cost").Price;
            double? jetARetail = resultPrices.FirstOrDefault(s => s.Product == "JetA Retail").Price;

            List<PricingTemplatesGridViewModel> result =
                    (from p in
                        (
                        from p in _context.PricingTemplate
                        join f in (_context.Fbos.Include("Preferences")) on p.Fboid equals f.Oid
                        join cm in (
                            from c in _context.CustomerMargins
                            join tm in (_context.PriceTiers)
                            on c.PriceTierId equals tm.Oid
                            group c by new { c.TemplateId }
                            into cmResults
                            select new CustomerMarginModel
                            {
                                TemplateId = cmResults.Key.TemplateId,
                                MaxPrice = cmResults.FirstOrDefault().Amount.GetValueOrDefault()
                            }
                        ) on p.Oid equals cm.TemplateId
                        into leftJoinCustomerMargins
                        from cm in leftJoinCustomerMargins.DefaultIfEmpty()
                        join fp in fboPrices on p.MarginTypeProduct equals fp.Product
                        into leftJoinFboPrices
                        from fp in leftJoinFboPrices.DefaultIfEmpty()
                        join tcc in templateCustomersCount on p.Oid equals tcc.Oid
                        into leftJoinTemplateCustomersCount
                        from tcc in leftJoinTemplateCustomersCount.DefaultIfEmpty()
                        where p.Fboid == fboId
                        select new
                        {
                            CustomerId = p.CustomerId.GetValueOrDefault(),
                            Default = p.Default.GetValueOrDefault(),
                            p.Fboid,
                            Margin = cm == null ? 0 : cm.MaxPrice,
                            MarginType = p.MarginType.GetValueOrDefault(),
                            p.Name,
                            p.Notes,
                            p.Oid,
                            Type = p.Type.GetValueOrDefault(),
                            p.Subject,
                            p.Email,
                            IntoPlanePrice = (jetaACostRecord == null ? 0 : jetaACostRecord.Price.GetValueOrDefault()) + (cm == null ? 0 : cm.MaxPrice),
                            IsInvalid = (f != null && f.Preferences != null && ((f.Preferences.OmitJetACost.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == Models.PricingTemplate.MarginTypes.CostPlus) || f.Preferences.OmitJetARetail.GetValueOrDefault() && p.MarginType.GetValueOrDefault() == Models.PricingTemplate.MarginTypes.RetailMinus)) ? true : false,
                            IsPricingExpired = (fp == null && (p.MarginType == null || p.MarginType != PricingTemplate.MarginTypes.FlatFee)),
                            YourMargin = GetMargin(jetaACostRecord, fp, cm),
                            CustomerMargin = cm,
                            CustomersAssigned = tcc == null ? 0 : tcc.Count
                        })
                     select new PricingTemplatesGridViewModel
                     {
                         CustomerId = p.CustomerId,
                         Default = p.Default,
                         Fboid = p.Fboid,
                         Margin = p.Margin,
                         MarginType = p.MarginType,
                         Name = p.Name,
                         Notes = p.Notes,
                         Oid = p.Oid,
                         Type = p.Type,
                         Subject = p.Subject,
                         Email = p.Email,
                         IsInvalid = p.IsInvalid,
                         IsPricingExpired = p.IsPricingExpired,
                         IntoPlanePrice = getPrices(p.IntoPlanePrice, p.YourMargin, p.Oid, p.CustomerMargin, p.Margin, p.MarginType, jetARetail, jetACost).Item1,
                         YourMargin = getPrices(p.IntoPlanePrice, p.YourMargin, p.Oid, p.CustomerMargin, p.Margin, p.MarginType, jetARetail, jetACost).Item2,
                         CustomersAssigned = p.CustomersAssigned
                     }
                    )
                    .GroupBy(x => x.Oid)
                    .Select(x => x.FirstOrDefault())
                    .ToList();

            return result;
        }
        #endregion


        #region Private Methods
        private double GetMargin(Fboprices jetaACostRecord, Fboprices price, CustomerMarginModel cm)
        {
            if (jetaACostRecord == null || jetaACostRecord.Price.GetValueOrDefault() <= 0)
            {
                return 0;
            }
            double result = 0;
            if (price != null)
            {
                result = price.Price.GetValueOrDefault();
            }

            if (cm == null)
            {
                return result - jetaACostRecord.Price.GetValueOrDefault();
            }
            return result + cm.MaxPrice - jetaACostRecord.Price.GetValueOrDefault();
        }

        private Tuple<double, double> getPrices(double prevIntoPlanePrice, double? prevYourMargin, int oid, CustomerMarginModel margins, double margin, PricingTemplate.MarginTypes? marginType, double? jetARetail, double? jetACost)
        {
            double intoPlanePrice = prevIntoPlanePrice;
            double? yourMargin = prevYourMargin;

            if (oid != 0)
            {
                if (margins != null)
                {
                    string marginTypeDescription = Utilities.Enum.GetDescription(marginType ?? PricingTemplate.MarginTypes.CostPlus);
                    if (marginTypeDescription == "Retail -")
                    {
                        if (jetARetail != null)
                        {
                            intoPlanePrice = jetARetail.Value - margins.MaxPrice;
                        }
                        else
                        {
                            intoPlanePrice = margins.MaxPrice;
                        }

                        if (jetACost != null)
                        {
                            yourMargin = margin;
                        }
                        else
                        {
                            yourMargin = intoPlanePrice - 0;
                        }
                    }
                    else if (marginTypeDescription == "Cost +")
                    {
                        if (jetACost != null)
                        {
                            intoPlanePrice = margins.MaxPrice + jetACost.Value;
                            yourMargin = intoPlanePrice - jetACost.Value;
                        }
                        else
                        {
                            intoPlanePrice = margins.MaxPrice + 0;
                            yourMargin = intoPlanePrice - 0;
                        }
                    }
                }
            }
            return Tuple.Create(intoPlanePrice, yourMargin.GetValueOrDefault());
        }
        #endregion
    }
}
