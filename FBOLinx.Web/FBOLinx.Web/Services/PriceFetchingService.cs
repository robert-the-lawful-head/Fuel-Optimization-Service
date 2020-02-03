using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Data;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models;
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
        public async Task<List<CustomerWithPricing>> GetCustomerPricingAsync(int fboId, int groupId, int customerInfoByGroupId = 0, int pricingTemplateId = 0)
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
                    join cct in _context.CustomCustomerTypes on new {customerId = cg.CustomerId, fboId = _FboId} equals
                        new
                        {
                            customerId = cct.CustomerId,
                            fboId = cct.Fboid
                        } into leftJoinCCT
                    from cct in leftJoinCCT.DefaultIfEmpty()
                    join pt in _context.PricingTemplate on (cct == null || cct.CustomerType == 0 ? defaultPricingTemplateId : cct.CustomerType) equals pt.Oid into leftJoinPT
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
                    join fp in _context.Fboprices.Where((x =>
                        x.EffectiveFrom.Value < DateTime.Now &&
                        (!x.EffectiveTo.HasValue || x.EffectiveTo > DateTime.Now))) on new
                    {
                        fboId = (pt != null ? pt.Fboid : 0),
                        product = (pt != null ? pt.MarginTypeProduct : "")
                    } equals new
                    {
                        fboId = fp.Fboid.GetValueOrDefault(),
                        product = fp.Product
                    } into leftJoinFP
                    from fp in leftJoinFP.DefaultIfEmpty()
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
                    where cg.GroupId == _GroupId
                          && (customerInfoByGroupId == 0 || cg.Oid == customerInfoByGroupId)
                          && (pricingTemplateId == 0 || pt.Oid == pricingTemplateId)
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
                        NeedsAttention = !(cvf != null && cvf.Oid > 0),
                        HasBeenViewed = (cvf != null && cvf.Oid > 0),
                        PricingTemplateName = pt == null ? "" : pt.Name,
                        MinGallons = ppt.Min,
                        MaxGallons = ppt.Max,
                        CustomerCompanyType = cg.CustomerCompanyType,
                        CustomerCompanyTypeName = ccot == null || string.IsNullOrEmpty(ccot.Name) ? "" : ccot.Name,
                        IsPricingExpired = (fp == null && (pt == null || pt.MarginType == null || pt.MarginType != PricingTemplate.MarginTypes.FlatFee))
                    }).OrderBy(x => x.Company).ToListAsync();

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

            var result = await GetStandardPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);
            var aircraftPricesResult = await GetTailSpecificPricingTemplatesForCustomerAsync(customer, fboId, groupId, pricingTemplateId);

            foreach (PricingTemplate aircraftPricingTemplate in aircraftPricesResult)
            {
                if (result.Any(x => x.Oid == aircraftPricingTemplate.Oid))
                    continue;
                var tailNumbers = string.Join(",", (from ca in _context.CustomerAircrafts
                    join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                    where ap.PriceTemplateId == aircraftPricingTemplate.Oid
                          && ca.CustomerId == customer.CustomerId
                          && ca.GroupId == _GroupId
                    select ca.TailNumber).ToList());
                if (!string.IsNullOrEmpty(tailNumbers))
                    aircraftPricingTemplate.Name += " - " + tailNumbers;
            }

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
        #endregion
    }
}
