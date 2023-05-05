using FBOLinx.DB.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerTypesEntityService : IRepository<CustomCustomerTypes, FboLinxContext>
    {
        Task FixAndSaveCustomCustomerTypes(int fboId, int groupId, int customerType);
        Task FixCustomCustomerTypesForAllGroups(int customerId);
    }
    public class CustomerTypesEntityService : Repository<CustomCustomerTypes, FboLinxContext>, ICustomerTypesEntityService
    {
        private readonly FboLinxContext _context;
        private readonly IPricingTemplateEntityService _PricingTemplateEntityService;

        public CustomerTypesEntityService(FboLinxContext context, IPricingTemplateEntityService pricingTemplateEntityService) : base(context)
        {
            _context = context;
            _PricingTemplateEntityService = pricingTemplateEntityService;
        }

        public async Task FixAndSaveCustomCustomerTypes(int fboId, int groupId, int customerType)
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

        public async Task FixCustomCustomerTypesForAllGroups(int customerId)
        {
            var customCustomerRecords = await (
                from g in _context.Group
                join cg in _context.CustomerInfoByGroup on g.Oid equals cg.GroupId
                join f in _context.Fbos on g.Oid equals f.GroupId
                join cct in _context.CustomCustomerTypes
                on new { customerId = cg.CustomerId, fboId = f.Oid }
                   equals
                   new
                   {
                       customerId = cct.CustomerId,
                       fboId = cct.Fboid
                   } into leftJoinCCT
                from cct in leftJoinCCT.DefaultIfEmpty()
                where g.Isfbonetwork == false && cg.CustomerId == customerId
                select new
                {
                    cg.CustomerId,
                    Fboid = f.Oid,
                    cct
                })
                .Distinct()
                .ToListAsync();

            var needsCustomCustomerType = customCustomerRecords.Where(c => c.cct == null).ToList();

            var allPricingTemplates = await _PricingTemplateEntityService.GetAllPricingTemplates();

            var insertCustomCustomerTypes = (from c in needsCustomCustomerType
                                             join p in allPricingTemplates on c.Fboid equals p.Fboid
                                             where p.Default == true
                                             select new
                                              CustomCustomerTypes
                                             {
                                                 CustomerId = c.CustomerId,
                                                 CustomerType = p.Oid,
                                                 Fboid = p.Fboid
                                             }).ToList();

            //Bulk insert
            await BulkInsert(insertCustomCustomerTypes);

            var currentCustomCustomerTypes = customCustomerRecords.Where(c => c.cct != null).ToList();
            var customCustomerTypesNeedingUpdates = (from c in currentCustomCustomerTypes
                                                     join p in allPricingTemplates on c.cct.CustomerType equals p.Oid into leftJoinPricingTemplates
                                                     from p in leftJoinPricingTemplates.DefaultIfEmpty()
                                                     where p is null
                                                     select new
                                                    CustomCustomerTypes
                                                     {
                                                         Oid = c.cct.Oid,
                                                         CustomerId = c.CustomerId,
                                                         Fboid = c.Fboid
                                                     }).ToList();

            var updateCustomCustomerTypes = (from c in customCustomerTypesNeedingUpdates
                                             join p in allPricingTemplates on c.Fboid equals p.Fboid
                                             where p.Default == true
                                             select new
                                              CustomCustomerTypes
                                             {
                                                 Oid = c.Oid,
                                                 CustomerId = c.CustomerId,
                                                 CustomerType = p.Oid,
                                                 Fboid = p.Fboid
                                             }).ToList();

            //Bulk update
            await BulkUpdate(updateCustomCustomerTypes);
        }
    }
}
