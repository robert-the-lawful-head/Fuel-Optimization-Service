using FBOLinx.DB.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using FBOLinx.ServiceLayer.EntityServices.EntityViewModels;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerMarginsEntityService : IRepository<DB.Models.CustomerMargins>
    {
        Task<IEnumerable<MarginTierView>> GetMarginTiers();
        Task AddDefaultCustomerMargins(int priceTemplateId, double min, double max);
    }
    public class CustomerMarginsEntityService : Repository<DB.Models.CustomerMargins>, ICustomerMarginsEntityService
    {
        private readonly FboLinxContext _context;
        public CustomerMarginsEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddDefaultCustomerMargins(int priceTemplateId, double min, double max)
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

        public async Task<IEnumerable<MarginTierView>> GetMarginTiers()
        {
            return await (from c in _context.CustomerMargins
                   join tm in _context.PriceTiers on c.PriceTierId equals tm.Oid
                   group c by c.TemplateId into cmGroupResult
                   select new MarginTierView()
                   {
                       TemplateId = cmGroupResult.Key,
                       MaxPriceTierValue = cmGroupResult.Max(c => Math.Abs(c.Amount.HasValue ? c.Amount.Value : 0)),
                       MinPriceTierValue = cmGroupResult.Min(c => Math.Abs(c.Amount.HasValue ? c.Amount.Value : 0))
                   }).ToListAsync();
        }
    }
}
