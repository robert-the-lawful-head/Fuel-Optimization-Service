using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.Responses.Customers;
using FBOLinx.ServiceLayer.EntityServices;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerInfoByGroupService : IBaseDTOService<CustomerInfoByGroupDto, CustomerInfoByGroup>
    {
        Task<CustomerInfoByGroupDto> AddNewCustomerInfoByGroup(CustomerInfoByGroupDto customerInfoByGroup);
        Task<List<CustomerListResponse>> GetCustomersListByGroupAndFbo(int groupId, int fboId, int customerInfoByGroupId = 0);
        Task<List<CustomerInfoByGroupDto>> GetCustomers(int groupId, List<string> tailNumbers = null);
        Task<List<CustomerInfoByGroupDto>> GetCustomersByGroup(int groupId, int customerInfoByGroupId = 0);
        Task<CustomerInfoByGroupDto> GetById(int customerInfoByGroupId);
    }

    public class CustomerInfoByGroupService : BaseDTOService<CustomerInfoByGroupDto, DB.Models.CustomerInfoByGroup, FboLinxContext>, ICustomerInfoByGroupService
    {
        private ICustomerService _CustomerService;

        public CustomerInfoByGroupService(ICustomerInfoByGroupEntityService entityService, ICustomerService customerService) : base(entityService)
        {
            _CustomerService = customerService;
        }

        public async Task<CustomerInfoByGroupDto> AddNewCustomerInfoByGroup(CustomerInfoByGroupDto customerInfoByGroup)
        {
            if (customerInfoByGroup.CustomerId == 0)
            {
                var customer = customerInfoByGroup.Customer == null ? new CustomersDto() {Company = customerInfoByGroup.Company, Active = true, ShowJetA = true} : customerInfoByGroup.Customer;
                customerInfoByGroup.Customer = await _CustomerService.AddNewCustomer(customer);
                customerInfoByGroup.CustomerId = customerInfoByGroup.Customer.Oid;
            }

            customerInfoByGroup.Customer = null;
            return await AddAsync(customerInfoByGroup);
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

        public async Task<List<CustomerInfoByGroupDto>> GetCustomers(int groupId, List<string> tailNumbers = null)
        {
            List<CustomerInfoByGroupDto> result = new List<CustomerInfoByGroupDto>();
            if (tailNumbers?.Count == 0 || tailNumbers == null)
                result = await GetListbySpec(new CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(groupId));
            else
                result = await GetListbySpec(new CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(groupId, tailNumbers));

            return result;
        }
        public async Task<List<CustomerInfoByGroupDto>> GetCustomersByGroup(int groupId, int customerInfoByGroupId = 0)
        {
            //Suspended at the "Customers" level means the customer has "HideInFBOLinx" enabled so should not be shown to any FBO/Group
            var customers = new List<CustomerInfoByGroupDto>();
            if (customerInfoByGroupId == 0)
                customers = await GetListbySpec(new CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(groupId));
            else
                customers = await GetListbySpec(new CustomerInfoByGroupCustomerAircraftsByGroupIdSpecification(groupId, customerInfoByGroupId));
            customers.RemoveAll(x => x == null);

            return customers;
        }

        public async Task<CustomerInfoByGroupDto> GetById(int customerInfoByGroupId)
        {
           var result =  await _EntityService.Where(x => x.Oid == customerInfoByGroupId).Include(x => x.Customer.CustomerAircrafts).FirstOrDefaultAsync();

            return result.Adapt<CustomerInfoByGroupDto>();

        }
    }
}
