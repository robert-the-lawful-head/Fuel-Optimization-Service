using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models.Responses;
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
            var query = await (from f in _context.Fbos
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
                }).ToListAsync();

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
            return result.ToList();
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
                    NeedsAttention = g.Max(a => a.IsDefaultPricingTemplate == true ? 1 : 0) == 1 ||
                                    g.Max(a => a.IsPricingTemplateRemoved == true ? 1 : 0) == 1 ? 1: 0
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

        public async Task<List<CustomerInfoByGroup>> GetCustomersByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0)
        {
            //Suspended at the "Customers" level means the customer has "HideInFBOLinx" enabled so should not be shown to any FBO/Group
            var customerInfoByGroup = await _context.CustomerInfoByGroup.Where(x => x.GroupId == groupId && (customerInfoByGroupId == 0 || x.Oid == customerInfoByGroupId))
                .Include(x => x.Customer)
                .Where(x => !x.Customer.Suspended.HasValue || !x.Customer.Suspended.Value)
                .Include(x => x.Customer.CustomCustomerType)
                .Where(x => x.Customer.CustomCustomerType.Fboid == fboId)
                .Include(x => x.Customer.CustomCustomerType.PricingTemplate)
                .Where(x => x.Customer.CustomCustomerType.PricingTemplate.Fboid == fboId)
                .Include(x => x.Customer.CustomerContacts)
                .ToListAsync();

            customerInfoByGroup.RemoveAll(x => x.Customer == null);

            return customerInfoByGroup;
        }

        public async Task<List<CustomerListResponse>> GetCustomersListByGroupAndFbo(int groupId, int fboId)
        {
            //Suspended at the "Customers" level means the customer has "HideInFBOLinx" enabled so should not be shown to any FBO/Group
            var customerInfoByGroup = await _context.CustomerInfoByGroup.Where(x => x.GroupId == groupId)
                .Include(x => x.Customer)
                .Where(x => !x.Customer.Suspended.HasValue || !x.Customer.Suspended.Value)
                .Include(x => x.Customer.CustomCustomerType)
                .Where(x => x.Customer.CustomCustomerType.Fboid == fboId)
                .Include(x => x.Customer.CustomCustomerType.PricingTemplate)
                .Where(x => x.Customer.CustomCustomerType.PricingTemplate.Fboid == fboId)
                .Select(x => new CustomerListResponse
                {
                    CustomerInfoByGroupID = x.Oid,
                    CompanyId = x.CustomerId,
                    Company = x.Company
                })
                .ToListAsync();

            return customerInfoByGroup;
        }


        public bool CompareCustomers(CustomerInfoByGroup customer1 , CustomerInfoByGroup customer2)
        {
            return customer1.ZipCode == customer2.ZipCode
               && customer1.Website == customer2.Website
               && customer1.Username == customer2.Username
               && customer1.Suspended == customer2.Suspended
               && customer1.State == customer2.State
               && customer1.ShowJetA == customer2.ShowJetA
               && customer1.Show100Ll == customer2.Show100Ll
               && customer1.Sfid == customer2.Sfid
               && customer1.PricingTemplateRemoved == customer2.PricingTemplateRemoved
               && customer1.Password == customer2.Password
               && customer1.Oid == customer2.Oid
               && customer1.Network == customer2.Network
               && customer1.MergeRejected == customer2.MergeRejected
               && customer1.MainPhone == customer2.MainPhone
               && customer1.Joined == customer2.Joined
               && customer1.GroupId == customer2.GroupId
               && customer1.EmailSubscription == customer2.EmailSubscription
               && customer1.Distribute == customer2.Distribute
               && customer1.DefaultTemplate == customer2.DefaultTemplate
               && customer1.CustomerType == customer2.CustomerType
               && customer1.CustomerId == customer2.CustomerId
               && customer1.CustomerCompanyType == customer2.CustomerCompanyType
               && customer1.Country == customer2.Country
               && customer1.Company == customer2.Company
               && customer1.City == customer2.City
               && customer1.CertificateType == customer2.CertificateType
               && customer1.Address == customer2.Address
               && customer1.Active == customer2.Active;               

        }


        public bool UpdateCustomerInfo (CustomerInfoByGroup newCustomer)
        {
            try
            {
                var oldCustomer = _context.CustomerInfoByGroup.FirstOrDefault(c => c.Oid == newCustomer.Oid);
                if(oldCustomer != null)
                {
                    oldCustomer.Active = newCustomer.Active;
                    oldCustomer.Address = newCustomer.Address;
                    oldCustomer.CertificateType = newCustomer.CertificateType;
                    oldCustomer.City = newCustomer.City;
                    oldCustomer.Company = newCustomer.Company;
                    oldCustomer.Country = newCustomer.Country;
                    oldCustomer.CustomerCompanyType = newCustomer.CustomerCompanyType;
                    oldCustomer.CustomerId = newCustomer.CustomerId;
                    oldCustomer.CustomerType = newCustomer.CustomerType;
                    oldCustomer.DefaultTemplate = newCustomer.DefaultTemplate;
                    oldCustomer.Distribute = newCustomer.Distribute;
                    oldCustomer.EmailSubscription = newCustomer.EmailSubscription;
                    oldCustomer.GroupId = newCustomer.GroupId;
                    oldCustomer.Joined = newCustomer.Joined;
                    oldCustomer.MainPhone = newCustomer.MainPhone;
                    oldCustomer.MergeRejected = newCustomer.MergeRejected;
                    oldCustomer.Network = newCustomer.Network;
                    oldCustomer.Password = newCustomer.Password;
                    oldCustomer.PricingTemplateRemoved = newCustomer.PricingTemplateRemoved;
                    oldCustomer.Sfid = newCustomer.Sfid;
                    oldCustomer.Show100Ll = newCustomer.Show100Ll;
                    oldCustomer.ShowJetA = newCustomer.ShowJetA;
                    oldCustomer.State = newCustomer.State;
                    oldCustomer.Suspended = newCustomer.Suspended;
                    oldCustomer.Username = newCustomer.Username;
                    oldCustomer.Website = newCustomer.Website;
                    oldCustomer.ZipCode = newCustomer.ZipCode;

                    _context.SaveChangesAsync();

                    return true;

                }

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
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
