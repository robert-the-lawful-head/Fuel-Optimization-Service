﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Customers;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Responses.Customers;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerService : IBaseDTOService<CustomersDto, DB.Models.Customers>
    {
        Task<CustomersDto> AddNewCustomer(CustomersDto customer);
        Task<List<CustomerNeedsAttentionModel>> GetCustomersNeedingAttentionByGroupFbo(int groupId, int fboId);
        Task<List<NeedsAttentionCustomersCountModel>> GetNeedsAttentionCustomersCountByGroupFbo();
        Task<CustomersDto> GetCustomerByFuelerLinxId(int fuelerLinxId);
        bool CompareCustomers(CustomerInfoByGroupDto oldCustomer, CustomerInfoByGroupDto newCustomer);
        Task<List<CustomersViewedByFbo>> GetCustomersViewedByFbo(int fboId);
        Task<List<CustomerCompanyTypes>> GetCustomerCompanyTypes(int groupId, int fboId);
        Task<List<CustomCustomerTypes>> GetCustomCustomerTypes(int fboId);
    }

        public class CustomerService : BaseDTOService<CustomersDto, DB.Models.Customers, FboLinxContext>, ICustomerService
    {
        private FboLinxContext _context;

        public CustomerService(FboLinxContext context, ICustomersEntityService customerEntityService) : base(customerEntityService)
        {
            _context = context;
        }

        #region Public Methods

        public async Task<CustomersDto> AddNewCustomer(CustomersDto customer)
        {
            CustomersDto record = null;
            if (customer.FuelerlinxId.GetValueOrDefault() != 0)
                record =
                    await GetSingleBySpec(
                        new CustomerByFuelerLinxIdSpecification(customer.FuelerlinxId.GetValueOrDefault()));
            if (record == null || record.Oid == 0)
                record = await GetSingleBySpec(new CustomerByCompanyName(customer.Company));
            if (record != null && record.Oid != 0)
            {
                record.Company = customer.Company;
                await UpdateAsync(record);
                return record;
            }
            else
            {
                return await AddAsync(customer);
            }
        }

        public async Task<List<CustomerNeedsAttentionModel>> GetCustomersNeedingAttentionByGroupFbo(int groupId, int fboId)
        {
            var query = await (from f in _context.Fbos
                join cg in _context.CustomerInfoByGroup on new {GroupId = f.GroupId, Active = true} equals new
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
                    GroupId = f.GroupId,
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
                    join cg in _context.CustomerInfoByGroup on new { GroupId = f.GroupId, Active = true } equals new { cg.GroupId, Active = cg.Active ?? false }
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
                        GroupId = f.GroupId,
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

        public bool CompareCustomers (CustomerInfoByGroupDto oldCustomer , CustomerInfoByGroupDto newCustomer)
        {
            return
            oldCustomer.Active == newCustomer.Active &&
            oldCustomer.Address == newCustomer.Address &&
            oldCustomer.CertificateType == newCustomer.CertificateType &&
            oldCustomer.City == newCustomer.City &&
            oldCustomer.Company == newCustomer.Company &&
            oldCustomer.Country == newCustomer.Country &&
            oldCustomer.CustomerCompanyType == newCustomer.CustomerCompanyType &&
            oldCustomer.Distribute == newCustomer.Distribute &&
            oldCustomer.EmailSubscription == newCustomer.EmailSubscription &&
            oldCustomer.MainPhone == newCustomer.MainPhone &&
            oldCustomer.Show100Ll == newCustomer.Show100Ll &&
            oldCustomer.ShowJetA == newCustomer.ShowJetA &&
            oldCustomer.State == newCustomer.State &&
            oldCustomer.Website == newCustomer.Website;
        }

        public async Task<CustomersDto> GetCustomerByFuelerLinxId(int fuelerLinxId)
        {
            var result =
                await GetSingleBySpec(new CustomerByFuelerLinxIdSpecification(fuelerLinxId));
            return result;
        }

        public async Task<List<CustomersViewedByFbo>> GetCustomersViewedByFbo(int fboId)
        {
            var result= await _context.CustomersViewedByFbo.Where(x => x.Fboid == fboId).ToListAsync();
            return result;
        }

        public async Task<List<CustomerCompanyTypes>> GetCustomerCompanyTypes(int groupId, int fboId)
        {
            var result = await _context.CustomerCompanyTypes
                    .Where(x => x.GroupId == groupId && x.Fboid == fboId).ToListAsync();
            return result;
        }

        public async Task<List<CustomCustomerTypes>> GetCustomCustomerTypes(int fboId)
        {
            var result = await _context.CustomCustomerTypes
                    .Where(x => x.Fboid == fboId).ToListAsync();
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
