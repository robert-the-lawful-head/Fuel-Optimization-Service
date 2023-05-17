using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Customers;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Responses.Customers;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerInfoByGroupService : IBaseDTOService<CustomerInfoByGroupDto, CustomerInfoByGroup>
    {
        Task<List<CustomerListResponse>> GetCustomersListByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0);
    }

    public class CustomerInfoByGroupService : BaseDTOService<CustomerInfoByGroupDto, DB.Models.CustomerInfoByGroup, FboLinxContext>, ICustomerInfoByGroupService
    {
        public CustomerInfoByGroupService(IRepository<CustomerInfoByGroup, FboLinxContext> entityService) : base(entityService)
        {
        }

        public async Task<List<CustomerListResponse>> GetCustomersListByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0)
        {
            //Suspended at the "Customers" level means the customer has "HideInFBOLinx" enabled so should not be shown to any FBO/Group
            var customerInfoByGroup = await GetListbySpec(new CustomerInfoByGroupByFboIdSpecification(groupId, fboId, customerInfoByGroupId));
            customerInfoByGroup.RemoveAll(x => x == null);

            var customerInfoByGroupList = customerInfoByGroup.Select(x => new CustomerListResponse
                {
                    CustomerInfoByGroupID = x.Oid,
                    CompanyId = x.CustomerId,
                    Company = x.Company
                })
                .ToList();

            return customerInfoByGroupList;
        }
    }
}
