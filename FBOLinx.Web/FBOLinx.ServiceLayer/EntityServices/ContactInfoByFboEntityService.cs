using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IContactInfoByFboEntityService : IRepository<ContactInfoByFbo, FboLinxContext>
    {
        Task<List<ContactInfoByFbo>> GetContactInfoByContactIdFboId(int groupId, int fboId);
    }
    public class ContactInfoByFboEntityService : Repository<ContactInfoByFbo, FboLinxContext>, IContactInfoByFboEntityService
    {
        private FboLinxContext _context;
        public ContactInfoByFboEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ContactInfoByFbo>> GetContactInfoByContactIdFboId(int groupId, int fboId)
        {
            var contactInfoByFboForAlerts =
                   await (from cibg in _context.ContactInfoByGroup
                          join c in _context.Contacts on cibg.ContactId equals c.Oid
                          join cibf in _context.Set<ContactInfoByFbo>() on new { ContactId = c.Oid, FboId = fboId } equals new { ContactId = cibf.ContactId.GetValueOrDefault(), FboId = cibf.FboId.GetValueOrDefault() } into leftJoinCIBF
                          from cibf in leftJoinCIBF.DefaultIfEmpty()
                          where cibg.GroupId == groupId
                          select cibf).ToListAsync();

            return contactInfoByFboForAlerts;
        }
    }
}
