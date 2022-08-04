using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IFboContactsEntityService : IRepository<Fbos, FboLinxContext>
    {
        Task<List<ContactsDTO>> GetFboContactsByFboId(int fboId);
    }

    public class FboContactsEntityService : Repository<Fbos, FboLinxContext>, IFboContactsEntityService
    {
        private FboLinxContext _context;
        public FboContactsEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ContactsDTO>> GetFboContactsByFboId(int fboId)
        {
            var fboContacts = await _context.Fbocontacts
                               .Include("Contact")
                               .Where(x => x.Fboid == fboId && !string.IsNullOrEmpty(x.Contact.Email))
                               .Select(f => new ContactsDTO
                               {
                                   FirstName = f.Contact.FirstName,
                                   LastName = f.Contact.LastName,
                                   Title = f.Contact.Title,
                                   Oid = f.Oid,
                                   Email = f.Contact.Email,
                                   Primary = f.Contact.Primary,
                                   CopyAlerts = f.Contact.CopyAlerts,
                                   CopyOrders = f.Contact.CopyOrders
                               })
                               .ToListAsync();

            return fboContacts;
        }
    }
}
