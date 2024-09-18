using FBOLinx.DB.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.BusinessServices.Contacts
{
    public interface IContactInfoByGroupService
    {
        Task<List<CustomerContactsByGroupGridViewModel>> GetNonFuelerLinxCustomersWithNoEmailByGroupFbo(int groupId, int fboId);
    }
    public class ContactInfoByGroupService: IContactInfoByGroupService
    {
        private FboLinxContext _context;

        public ContactInfoByGroupService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerContactsByGroupGridViewModel>> GetNonFuelerLinxCustomersWithNoEmailByGroupFbo(int groupId, int fboId)
        {
            var customerContactInfoByGroupVM = await(from cuibg in _context.CustomerInfoByGroup
                                                     join cu in _context.Customers on new { cuibg.CustomerId, cuibg.GroupId} equals new { CustomerId = cu.Oid, GroupId = groupId}
                                                     join cc in _context.CustomerContacts on cu.Oid equals cc.CustomerId into leftJoinCC
                                                     from cc in leftJoinCC.DefaultIfEmpty()
                                                     join c in _context.Contacts on cc.ContactId equals c.Oid into leftJoinC
                                                     from c in leftJoinC.DefaultIfEmpty()
                                                     join cibg in _context.ContactInfoByGroup on c.Oid equals cibg.ContactId into leftJoinCibg
                                                     from cibg in leftJoinCibg.DefaultIfEmpty()
                                                     join cibf in _context.ContactInfoByFbo on new { ContactId = c.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                                                     from cibf in leftJoinCIBF.DefaultIfEmpty()
                                                     where cu.FuelerlinxId < 1 && cuibg.GroupId == groupId && cibg == null
                                                     select new CustomerContactsByGroupGridViewModel()
                                                     {
                                                         CustomerId = cc.CustomerId
                                                     }).ToListAsync();
            return customerContactInfoByGroupVM;
        }
    }
}
