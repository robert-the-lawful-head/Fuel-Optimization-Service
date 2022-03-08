using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.Dto.Responses;
using FBOLinx.ServiceLayer.Dto.UseCaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IPricingTemplateEntityService : IRepository<PricingTemplate>
    {
        Task<List<PricingTemplateGrid>> GetPricingTemplateGrid(int fboId, int groupId);
        Task<List<PricingTemplateGrid>> GetDefualtPricingTemplateGridByFboId(int fboId);
        Task<List<PricingTemplateGrid>> GetCostPlusPricingTemplates(int fboId);
        Task<List<PricingTemplateGrid>> GetPricingTemplatesWithEmailContent(int fboId, int groupId);
        Task<PricingTemplate> CopyPricingTemplate(int? currentPricingTemplateId, string pricingTemplateName);
        Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0);
        Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0);
    }
    
    public class PricingTemplateEntityService : Repository<PricingTemplate>, IPricingTemplateEntityService
    {
        private readonly FboLinxContext _context;
        public PricingTemplateEntityService(FboLinxContext context,
            IFboPricesEntityService fboPricesEntityService,
            ICustomerMarginsEntityService customerMarginEntityService,
            CustomerInfoByGroupEntityService customerInfoByGroupEntityService) : base(context)
        {
            _context = context;
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
                                    join fp in tempFboPrices on "JetA " + p.MarginTypeProduct equals fp.Product
                                        into leftJoinFp
                                    from fp in leftJoinFp.DefaultIfEmpty()
                                    where p.Fboid == fboId && (fp == null || fp.EffectiveFrom == null || fp.EffectiveFrom <= DateTime.UtcNow)
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
                                        TemplateId = cm == null ? 0 : cm.TemplateId,
                                        p.EmailContentId
                                    }).ToList();


            //Group the final result
            return (from pt in pricingTemplates
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
                              pt.EmailContentId,
                          }
                    into groupedPt
                          select new PricingTemplateGrid()
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
                              CustomersAssigned = customerAssignments.Sum(x => x.CustomerType == groupedPt.Key.TemplateId ? 1 : 0),
                              EmailContentId = groupedPt.Key.EmailContentId
                          })
                .OrderBy(pt => pt.Name)
                .ToList();
        }

        public async Task<List<PricingTemplateGrid>> GetPricingTemplatesWithEmailContent(int fboId, int groupId)
        {
            var templates = await GetPricingTemplateGrid(fboId, groupId);
            var templatesWithEmailContent = (
            from t in templates
            join ec in _context.EmailContent on t.EmailContentId equals ec.Oid
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
                IntoPlanePrice = t.IntoPlanePrice,
                CustomersAssigned = t.CustomersAssigned,
                EmailContentId = t.EmailContentId,
                EmailContent = ec
            }
            ).ToList();

            foreach (var t in templatesWithEmailContent)
            {
                t.CustomerEmails = await(from cg in _context.CustomerInfoByGroup.Where((x => x.GroupId == groupId))
                                         join c in _context.Customers on cg.CustomerId equals c.Oid
                                         join cc in _context.CustomCustomerTypes.Where(x => x.Fboid == fboId) on cg.CustomerId equals cc.CustomerId
                                         join custc in _context.CustomerContacts on c.Oid equals custc.CustomerId
                                         join co in _context.Contacts on custc.ContactId equals co.Oid
                                         join cibg in _context.ContactInfoByGroup on co.Oid equals cibg.ContactId
                                         join cibf in _context.Set<ContactInfoByFbo>() on new { ContactId = co.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                                         from cibf in leftJoinCIBF.DefaultIfEmpty()
                                         where (cg.Active ?? false)
                                         && cc.CustomerType == t.Oid
                                         && ((cibf.ContactId != null && (cibf.CopyAlerts ?? false)) || (cibf.ContactId == null && (cibg.CopyAlerts ?? false)))
                                         && !string.IsNullOrEmpty(cibg.Email)
                                         && cibg.GroupId == groupId
                                         && (c.Suspended ?? false) == false
                                         select cibg.Email
                                ).ToListAsync();
            }
            return templatesWithEmailContent;
        }

        public async Task<List<PricingTemplate>> GetStandardPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
        {
            return await(from cg in _context.CustomerInfoByGroup
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
        }

        public async Task<List<PricingTemplate>> GetTailSpecificPricingTemplatesForCustomerAsync(CustomerInfoByGroup customer, int fboId, int groupId, int pricingTemplateId = 0)
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
    }
}
