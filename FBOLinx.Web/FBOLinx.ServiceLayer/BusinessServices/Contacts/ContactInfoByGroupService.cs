using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Customers;

namespace FBOLinx.ServiceLayer.BusinessServices.Contacts
{
    public interface IContactInfoByGroupService
    {
        Task<List<CustomerContactsByGroupGridViewModel>> GetAllCustomerContactsByGroupFbo(int groupId, int fboId);
        Task<List<CustomerContactsByGroupGridViewModel>> GetNonFuelerLinxCustomersWithNoEmailByGroupFbo(int groupId, int fboId);
    }
    public class ContactInfoByGroupService: IContactInfoByGroupService
    {
        private FboLinxContext _context;
        private ICustomerInfoByGroupService _customerInfoByGroupService;

        public ContactInfoByGroupService(FboLinxContext context, ICustomerInfoByGroupService customerInfoByGroupService)
        {
            _context = context;
            _customerInfoByGroupService = customerInfoByGroupService;
        }

        public async Task<List<CustomerContactsByGroupGridViewModel>> GetNonFuelerLinxCustomersWithNoEmailByGroupFbo(int groupId, int fboId)
        {
            var customers = await _customerInfoByGroupService.GetCustomers(groupId);

            var customerContacts = await GetAllCustomerContactsByGroupFbo(groupId, fboId);

            var customerContactInfoByGroupVM = (from c in customers
                                                join cc in customerContacts on c.CustomerId equals cc.CustomerId
                                                into leftJoinCC
                                                from cc in leftJoinCC.DefaultIfEmpty()
                                                where c.Customer.FuelerlinxId is null || c.Customer.FuelerlinxId == 0 || c.Customer.FuelerlinxId < 0
                                                where cc == null
                                                select new CustomerContactsByGroupGridViewModel()
                                                {
                                                    CustomerId = c.CustomerId
                                                }).ToList();
            return customerContactInfoByGroupVM;
        }

        public async Task<List<CustomerContactsByGroupGridViewModel>> GetAllCustomerContactsByGroupFbo(int groupId, int fboId)
        {
            var customerContactInfoByGroupVM = new List<CustomerContactsByGroupGridViewModel>();
            try
            {
                customerContactInfoByGroupVM = await (from cc in _context.CustomerContacts
                                                      join cuibg in _context.CustomerInfoByGroup on new { CustomerId = cc.CustomerId, GroupId = groupId } equals new { CustomerId = cuibg.CustomerId, GroupId = cuibg.GroupId }
                                                      join cu in _context.Customers on cuibg.CustomerId equals cu.Oid
                                                      join cibg in _context.ContactInfoByGroup on cc.ContactId equals cibg.ContactId
                                                      join cibf in _context.ContactInfoByFbo on new { ContactId = cc.ContactId, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() }
                                                      into leftJoinCIBF
                                                      from cibf in leftJoinCIBF.DefaultIfEmpty()
                                                      where cibg.GroupId == groupId
                                                      select new CustomerContactsByGroupGridViewModel()
                                                      {
                                                          ContactInfoByGroupId = cibg.Oid,
                                                          ContactInfoByFboId= cibf.Oid,
                                                          IsFuelerLinxCustomer = cu.FuelerlinxId > 0 ? true : false,
                                                          CustomerId = cc.CustomerId,
                                                          ContactId = cc.ContactId,
                                                          Email = cibg.Email,
                                                          FirstName = cibg.FirstName,
                                                          LastName = cibg.LastName
                                                      }).ToListAsync();
            }
            catch (System.Exception ex)
            {

            }
            return customerContactInfoByGroupVM;
        }
    }
}
