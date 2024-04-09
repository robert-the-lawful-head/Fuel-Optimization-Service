using FBOLinx.DB.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FBOLinx.ServiceLayer.EntityServices.EntityViewModels;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerInfoByGroupEntityService : IRepository<CustomerInfoByGroup, FboLinxContext>
    {
        Task<List<CustomerTypeView>> GetCustomerAssignmentsByTemplateId(int fboId, int groupId);
    }
    public class CustomerInfoByGroupEntityService : Repository<CustomerInfoByGroup, FboLinxContext>, ICustomerInfoByGroupEntityService
    {
        private readonly FboLinxContext _context;
        public CustomerInfoByGroupEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CustomerTypeView>> GetCustomerAssignmentsByTemplateId(int fboId, int groupId)
        {
            return await (from cibg in _context.CustomerInfoByGroup
                          join cct in _context.CustomCustomerTypes on cibg.CustomerId equals cct.CustomerId
                          join c in _context.Customers on cibg.CustomerId equals c.Oid
                          where cct.Fboid == fboId && cibg.GroupId == groupId && (c.Suspended == null || c.Suspended == false)
                          select new CustomerTypeView()
                          { CustomerType = cct.CustomerType }).ToListAsync();
        }
    }
}
