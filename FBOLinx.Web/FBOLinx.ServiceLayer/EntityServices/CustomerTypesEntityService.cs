using FBOLinx.DB.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerTypesEntityService : IRepository<CustomerMargins, FboLinxContext>
    {
        Task FixAndSaveCustomCustomerTypes(int fboId, int groupId, int customerType);
    }
    public class CustomerTypesEntityService : Repository<CustomerMargins, FboLinxContext>, ICustomerTypesEntityService
    {
        private readonly FboLinxContext _context;
        public CustomerTypesEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }


        public async Task FixAndSaveCustomCustomerTypes(int fboId,int groupId,int customerType)
        {
            List<int> pricingTemplates = await _context.PricingTemplate
                                                .Where(p => p.Fboid.Equals(fboId))
                                                .Select(p => p.Oid)
                                                .ToListAsync();

            var filteredList = await (
                from cg in _context.CustomerInfoByGroup
                join cct in _context.CustomCustomerTypes
                on new { customerId = cg.CustomerId, fboId }
                   equals
                   new
                   {
                       customerId = cct.CustomerId,
                       fboId = cct.Fboid
                   } into leftJoinCCT
                from cct in leftJoinCCT.DefaultIfEmpty()
                where cg.GroupId.Equals(groupId)
                select new
                {
                    cg.CustomerId,
                    cct
                })
                .Distinct()
                .ToListAsync();

            filteredList.ForEach(c =>
            {
                if (c.cct == null)
                {
                    CustomCustomerTypes newcct = new CustomCustomerTypes
                    {
                        CustomerId = c.CustomerId,
                        CustomerType = customerType,
                        Fboid = fboId
                    };
                    _context.CustomCustomerTypes.Add(newcct);
                }
                else
                {
                    bool customerTypeExits = pricingTemplates.Any(p => p.Equals(c.cct.CustomerType));
                    if (!customerTypeExits)
                    {
                        c.cct.CustomerType = customerType;
                        _context.CustomCustomerTypes.Update(c.cct);
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
