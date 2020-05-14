using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class PricingTemplateService
    {
        private readonly FboLinxContext _context;
        public PricingTemplateService(FboLinxContext context)
        {
            _context = context;
        }
        public async Task FixCustomCustomerTypes(int groupId, int fboId)
        {
            if (groupId == 0 || fboId == 0)
            {
                return;
            }

            PricingTemplate defaultPricingTemplate = await _context.PricingTemplate
                                                                        .Where(x => x.Fboid == fboId && x.Default == true)
                                                                        .FirstOrDefaultAsync();

            if (defaultPricingTemplate == null)
            {
                return;
            }

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
                        CustomerType = defaultPricingTemplate.Oid,
                        Fboid = fboId
                    };
                    _context.CustomCustomerTypes.Add(newcct);
                }
                else
                {
                    bool customerTypeExits = pricingTemplates.Any(p => p.Equals(c.cct.CustomerType));
                    if (!customerTypeExits)
                    {
                        c.cct.CustomerType = defaultPricingTemplate.Oid;
                        _context.CustomCustomerTypes.Update(c.cct);
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
