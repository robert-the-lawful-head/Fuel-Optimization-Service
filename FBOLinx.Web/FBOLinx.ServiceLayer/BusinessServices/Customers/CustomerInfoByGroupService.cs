using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Responses.Customers;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerInfoByGroupService : IBaseDTOService<CustomerInfoByGroupDTO, CustomerInfoByGroup>
    {
        Task<CustomerInfoByGroupDTO> AddNewCustomerInfoByGroup(CustomerInfoByGroupDTO customerInfoByGroup);
        Task<List<CustomerInfoByGroupDTO>> GetCustomersByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0);
        Task<List<CustomerListResponse>> GetCustomersListByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0);
    }

    public class CustomerInfoByGroupService : BaseDTOService<CustomerInfoByGroupDTO, DB.Models.CustomerInfoByGroup, FboLinxContext>, ICustomerInfoByGroupService
    {
        private ICustomerService _CustomerService;

        public CustomerInfoByGroupService(ICustomerInfoByGroupEntityService entityService, ICustomerService customerService) : base(entityService)
        {
            _CustomerService = customerService;
        }

        public async Task<CustomerInfoByGroupDTO> AddNewCustomerInfoByGroup(CustomerInfoByGroupDTO customerInfoByGroup)
        {
            if (customerInfoByGroup.CustomerId == 0)
            {
                var customer = customerInfoByGroup.Customer == null ? new CustomerDTO() {Company = customerInfoByGroup.Company, Active = true, ShowJetA = true} : customerInfoByGroup.Customer;
                customerInfoByGroup.Customer = await _CustomerService.AddNewCustomer(customer);
                customerInfoByGroup.CustomerId = customerInfoByGroup.Customer.Oid;
            }

            return await AddAsync(customerInfoByGroup);
        }

        public async Task<List<CustomerInfoByGroupDTO>> GetCustomersByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0)
        {
            //Suspended at the "Customers" level means the customer has "HideInFBOLinx" enabled so should not be shown to any FBO/Group
            var customerInfoByGroup = await GetListbySpec(new CustomerInfoByGroupByFboIdSpecification(groupId, fboId, customerInfoByGroupId));
            customerInfoByGroup.RemoveAll(x => x == null);

            return customerInfoByGroup;
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
