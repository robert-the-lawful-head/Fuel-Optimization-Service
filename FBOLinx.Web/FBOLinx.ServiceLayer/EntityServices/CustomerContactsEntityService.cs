using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerContactsEntityService : IRepository<ContactInfoByGroup, FboLinxContext>
    {
        Task<List<ContactInfoByGroup>> GetRecipientsForCustomer(CustomerInfoByGroupDto customer, int fboId, int groupId);
    }
    public class CustomerContactsEntityService : Repository<ContactInfoByGroup, FboLinxContext>, ICustomerContactsEntityService
    {
        public FboLinxContext _context { get; }

        public CustomerContactsEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ContactInfoByGroup>> GetRecipientsForCustomer(CustomerInfoByGroupDto customer, int fboId, int groupId)
        {
            var result = await (from cc in _context.Set<CustomerContacts>()
                                join c in _context.Set<Contacts>() on cc.ContactId equals c.Oid
                                join cibg in _context.Set<ContactInfoByGroup>() on c.Oid equals cibg.ContactId
                                join cibf in _context.Set<ContactInfoByFbo>() on new { ContactId = c.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                                from cibf in leftJoinCIBF.DefaultIfEmpty()
                                where cibg.GroupId == groupId
                                      && cc.CustomerId == customer.CustomerId
                                      && ((cibf.ContactId != null && (cibf.CopyAlerts ?? false)) || (cibf.ContactId == null && (cibg.CopyAlerts ?? false)))
                                      && !string.IsNullOrEmpty(cibg.Email)
                                select cibg).ToListAsync();
            return result;
        }
    }
}
