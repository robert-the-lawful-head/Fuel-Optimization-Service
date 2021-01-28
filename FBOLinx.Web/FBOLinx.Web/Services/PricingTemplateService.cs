using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;

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

        public async Task FixDefaultPricingTemplate(int fboId)
        {
            var existingPricingTemplates =
                   await _context.PricingTemplate.Where(x => x.Fboid == fboId).ToListAsync();
            if (existingPricingTemplates != null && existingPricingTemplates.Count != 0)
                return;

            //Add a default pricing template - project #1c5383
            var newTemplate = new PricingTemplate()
            {
                Default = true,
                Fboid = fboId,
                Name = "Posted Retail",
                MarginType = PricingTemplate.MarginTypes.RetailMinus,
                Notes = ""
            };

            await _context.PricingTemplate.AddAsync(newTemplate);
            await _context.SaveChangesAsync();

            await AddDefaultCustomerMargins(newTemplate.Oid, 1, 500);
            await AddDefaultCustomerMargins(newTemplate.Oid, 501, 750);
            await AddDefaultCustomerMargins(newTemplate.Oid, 751, 1000);
            await AddDefaultCustomerMargins(newTemplate.Oid, 1001, 99999);
        }

        private async Task AddDefaultCustomerMargins(int priceTemplateId, double min, double max)
        {
            var newPriceTier = new PriceTiers() { Min = min, Max = max, MaxEntered = max };
            await _context.PriceTiers.AddAsync(newPriceTier);
            await _context.SaveChangesAsync();

            var newCustomerMargin = new CustomerMargins()
            {
                Amount = 0,
                TemplateId = priceTemplateId,
                PriceTierId = newPriceTier.Oid
            };
            await _context.CustomerMargins.AddAsync(newCustomerMargin);
            await _context.SaveChangesAsync();

        }
    }
}
