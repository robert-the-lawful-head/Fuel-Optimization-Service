using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Services
{
    public class CustomerService
    {
        private FboLinxContext _context;

        public CustomerService(FboLinxContext context)
        {
            _context = context;
        }

        #region Public Methods

        public async Task<List<CustomerNeedsAttentionModel>> GetCustomersNeedingAttentionByGroupFbo(int groupId, int fboId)
        {
            var query = from f in _context.Fbos
                join cg in _context.CustomerInfoByGroup on new {GroupId = f.GroupId ?? 0, Active = true} equals new
                        {cg.GroupId, Active = cg.Active ?? false}
                    into leftJoinCg
                from cg in leftJoinCg.DefaultIfEmpty()
                join c in _context.Customers on cg.CustomerId equals c.Oid
                    into leftJoinCustomer
                from c in leftJoinCustomer.DefaultIfEmpty()
                join cct in _context.CustomCustomerTypes on new {cg.CustomerId, Fboid = fboId} equals new
                        {cct.CustomerId, cct.Fboid}
                    into leftJoinCCT
                from cct in leftJoinCCT.DefaultIfEmpty()
                join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                    into leftJoinPT
                from pt in leftJoinPT.DefaultIfEmpty()
                join cc in _context.CustomerContacts on cg.CustomerId equals cc.CustomerId
                    into leftJoinCC
                from cc in leftJoinCC.DefaultIfEmpty()
                //join cibg in _context.ContactInfoByGroup on new { cc.ContactId, cg.GroupId, CopyAlerts = true } equals new { cibg.ContactId, cibg.GroupId, CopyAlerts = cibg.CopyAlerts ?? false }
                //into leftJoinCibg
                //from cibg in leftJoinCibg.DefaultIfEmpty()
                join cvf in _context.CustomersViewedByFbo on new {Fboid = fboId, cg.CustomerId} equals new
                        {cvf.Fboid, cvf.CustomerId}
                    into leftJoinCvf
                from cvf in leftJoinCvf.DefaultIfEmpty()
                where cg.GroupId == groupId && f.Oid == fboId && c.Suspended != true
                select new
                {
                    GroupId = f.GroupId ?? 0,
                    FboId = f.Oid,
                    CustomerInfoByGroupID = cg == null ? 0 : cg.Oid,
                    Company = cg == null ? null : cg.Company,
                    PricingTemplateId = pt == null ? 0 : pt.Oid,
                    IsDefaultPricingTemplate = pt == null ? true : pt.Default,
                    IsPricingTemplateRemoved = cg.PricingTemplateRemoved,
                    //ContactInfoByGroupId = cibg == null ? 0 : cibg.Oid,
                    FuelerlinxId = c == null ? 0 : c.FuelerlinxId ?? 0,
                    CustomersViewedByFboId = cvf == null ? 0 : cvf.Oid
                };

            var result = (query
                .GroupBy(g => new { g.CustomerInfoByGroupID , g.Company})
                .Select(g => new CustomerNeedsAttentionModel
                {
                    Oid = g.Key.CustomerInfoByGroupID,
                    Company = g.Key.Company,
                    NeedsAttention = g.Max(a => a.IsDefaultPricingTemplate) == true ||
                                    //(g.Max(a => a.ContactInfoByGroupId) == 0 && g.Max(a => a.FuelerlinxId) <= 0) ||
                                    g.Max(a => a.IsPricingTemplateRemoved) == true,
                    NeedsAttentionReason = GetNeedsAttentionReason(
                        g.Max(a => a.IsDefaultPricingTemplate) == true,
                        g.Max(a => a.IsPricingTemplateRemoved) == true,
                        g.Max(a => a.CustomersViewedByFboId) == 0 && g.Max(a => a.FuelerlinxId) > 0
                    )
                })
                .Where(g => g.NeedsAttention == true));
            return await result.ToListAsync();
        }

        public async Task<List<NeedsAttentionCustomersCountModel>> GetNeedsAttentionCustomersCountByGroupFbo()
        {
            var result = await (
                    from f in _context.Fbos
                    join cg in _context.CustomerInfoByGroup on new { GroupId = f.GroupId ?? 0, Active = true } equals new { cg.GroupId, Active = cg.Active ?? false }
                    into leftJoinCg
                    from cg in leftJoinCg.DefaultIfEmpty()
                    join c in _context.Customers on cg.CustomerId equals c.Oid
                    into leftJoinCustomer
                    from c in leftJoinCustomer.DefaultIfEmpty()
                    join cct in _context.CustomCustomerTypes on new { cg.CustomerId, Fboid = f.Oid } equals new { cct.CustomerId, cct.Fboid }
                    into leftJoinCCT
                    from cct in leftJoinCCT.DefaultIfEmpty()
                    join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                    into leftJoinPT
                    from pt in leftJoinPT.DefaultIfEmpty()
                    join cc in _context.CustomerContacts on cg.CustomerId equals cc.CustomerId
                    into leftJoinCC
                    from cc in leftJoinCC.DefaultIfEmpty()
                    //join cibg in _context.ContactInfoByGroup on new { cc.ContactId, cg.GroupId, CopyAlerts = true } equals new { cibg.ContactId, cibg.GroupId, CopyAlerts = cibg.CopyAlerts ?? false }
                    //into leftJoinCibg
                    //from cibg in leftJoinCibg.DefaultIfEmpty()
                    join cvf in _context.CustomersViewedByFbo on new { Fboid = f.Oid, cg.CustomerId } equals new { cvf.Fboid, cvf.CustomerId }
                    into leftJoinCvf
                    from cvf in leftJoinCvf.DefaultIfEmpty()
                    where c.Suspended != true
                    select new
                    {
                        GroupId = f.GroupId ?? 0,
                        FboId = f.Oid,
                        CustomerInfoByGroupID = cg == null ? 0 : cg.Oid,
                        PricingTemplateId = pt == null ? 0 : pt.Oid,
                        IsDefaultPricingTemplate = pt == null ? true : pt.Default,
                        IsPricingTemplateRemoved = cg.PricingTemplateRemoved,
                        //ContactInfoByGroupId = cibg == null ? 0 : cibg.Oid,
                        FuelerlinxId = c == null ? 0 : c.FuelerlinxId ?? 0,
                        CustomersViewedByFboId = cvf == null ? 0 : cvf.Oid
                    }
                )
                .GroupBy(g => new { g.GroupId, g.FboId, g.CustomerInfoByGroupID })
                .Select(g => new
                {
                    g.Key.GroupId,
                    g.Key.FboId,
                    g.Key.CustomerInfoByGroupID,
                    NeedsAttention = g.Max(a => a.IsDefaultPricingTemplate) == true ||
                                    //(g.Max(a => a.ContactInfoByGroupId) == 0 && g.Max(a => a.FuelerlinxId) <= 0) ||
                                    g.Max(a => a.IsPricingTemplateRemoved) == true ? 1: 0
                })
                .GroupBy(g => new { g.GroupId, g.FboId })
                .Select(g => new NeedsAttentionCustomersCountModel
                {
                    GroupId = g.Key.GroupId,
                    FboId = g.Key.FboId,
                    CustomersNeedingAttention = g.Sum(a => a.NeedsAttention),
                    Customers = g.Count()
                })
                .OrderBy(g => g.GroupId)
                .ThenBy(g => g.FboId)
                .ToListAsync();
            return result;
        }

        #endregion

        #region Private Methods
        private string GetNeedsAttentionReason(bool isDefaultPricingTemplate, bool isPricingTemplateRemoved, bool isNewFuelerlinxCustomer)
        {
            if (isPricingTemplateRemoved)
            {
                return "The ITP Margin Template for this customer was deleted and the customer has been assigned to the default template.";
            }
            if (isDefaultPricingTemplate && isNewFuelerlinxCustomer)
            {
                return "This FuelerLinx customer was added and set to your default pricing.  Please set to the appropriate pricing template.";
            }
            if (isDefaultPricingTemplate)
            {
                return "Customer was assigned to the default template and has not been changed yet.";
            }
            return null;
        }
        #endregion
    }

    public class CustomerNeedsAttentionModel
    {
        public int Oid { get; set; }
        public string Company { get; set; }
        public bool NeedsAttention { get; set; }
        public string NeedsAttentionReason { get; set; }
    }

    public class NeedsAttentionCustomersCountModel
    {
        public int GroupId { get; set; }
        public int FboId { get; set; }
        public int CustomersNeedingAttention { get; set; }
        public int Customers { get; set; }
    }
}
