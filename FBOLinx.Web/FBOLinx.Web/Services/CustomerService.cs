using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Data;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Remotion.Linq.Clauses;

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
            var result = await (
                    from f in _context.Fbos
                    join cg in _context.CustomerInfoByGroup on new { GroupId = f.GroupId ?? 0, Active = true } equals new { cg.GroupId, Active = cg.Active ?? false }
                    into leftJoinCg
                    from cg in leftJoinCg.DefaultIfEmpty()
                    join c in _context.Customers on cg.CustomerId equals c.Oid
                    into leftJoinCustomer
                    from c in leftJoinCustomer.DefaultIfEmpty()
                    join cct in _context.CustomCustomerTypes on new { cg.CustomerId, Fboid = fboId } equals new { cct.CustomerId, cct.Fboid }
                    into leftJoinCCT
                    from cct in leftJoinCCT.DefaultIfEmpty()
                    join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                    into leftJoinPT
                    from pt in leftJoinPT.DefaultIfEmpty()
                    join cc in _context.CustomerContacts on cg.CustomerId equals cc.CustomerId
                    into leftJoinCC
                    from cc in leftJoinCC.DefaultIfEmpty()
                    join cibg in _context.ContactInfoByGroup on new { cc.ContactId, cg.GroupId, CopyAlerts = true } equals new { cibg.ContactId, cibg.GroupId, CopyAlerts = cibg.CopyAlerts ?? false }
                    into leftJoinCibg
                    from cibg in leftJoinCibg.DefaultIfEmpty()
                    join cvf in _context.CustomersViewedByFbo on new { Fboid = fboId, cg.CustomerId } equals new { cvf.Fboid, cvf.CustomerId }
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
                        ContactInfoByGroupId = cibg == null ? 0 : cibg.Oid,
                        FuelerlinxId = c == null ? 0 : c.FuelerlinxId ?? 0,
                        CustomersViewedByFboId = cvf == null ? 0 : cvf.Oid
                    }
                )
                .GroupBy(g => new { g.CustomerInfoByGroupID , g.Company})
                .Select(g => new CustomerNeedsAttentionModel
                {
                    Oid = g.Key.CustomerInfoByGroupID,
                    Company = g.Key.Company,
                    NeedsAttention = g.Max(a => a.PricingTemplateId) == 0 ||
                                    (g.Max(a => a.ContactInfoByGroupId) == 0 && g.Max(a => a.FuelerlinxId) <= 0) ||
                                    g.Max(a => a.CustomersViewedByFboId) == 0 ? true : false
                })
                .Where(g => g.NeedsAttention == true)
                .ToListAsync();
            return result;
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
                    join cibg in _context.ContactInfoByGroup on new { cc.ContactId, cg.GroupId, CopyAlerts = true } equals new { cibg.ContactId, cibg.GroupId, CopyAlerts = cibg.CopyAlerts ?? false }
                    into leftJoinCibg
                    from cibg in leftJoinCibg.DefaultIfEmpty()
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
                        ContactInfoByGroupId = cibg == null ? 0 : cibg.Oid,
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
                    NeedsAttention = g.Max(a => a.PricingTemplateId) == 0 ||
                                    (g.Max(a => a.ContactInfoByGroupId) == 0 && g.Max(a => a.FuelerlinxId) <= 0) ||
                                    g.Max(a => a.CustomersViewedByFboId) == 0 ? 1 : 0
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
    }

    public class CustomerNeedsAttentionModel
    {
        public int Oid { get; set; }
        public string Company { get; set; }
        public bool NeedsAttention { get; set; }
    }

    public class NeedsAttentionCustomersCountModel
    {
        public int GroupId { get; set; }
        public int FboId { get; set; }
        public int CustomersNeedingAttention { get; set; }
        public int Customers { get; set; }
    }
}
