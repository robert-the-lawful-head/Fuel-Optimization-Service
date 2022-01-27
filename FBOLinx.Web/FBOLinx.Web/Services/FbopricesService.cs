using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class FbopricesService
    {
        private readonly FboLinxContext _context;

        public FbopricesService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task<List<FbopricesResult>> GetPrices(int fboId)
        {
            var products = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(Fboprices.FuelProductPriceTypes));
            var universalTime = DateTime.Today.ToUniversalTime();

            var fboprices = await (
                            from f in _context.Fboprices
                            where f.EffectiveTo > DateTime.UtcNow
                            && f.Fboid == fboId && f.Price != null && f.Expired != true
                            select f).ToListAsync();

            var oldPrices = await _context.Fboprices.Where(f => f.EffectiveTo <= DateTime.UtcNow && f.Fboid == fboId && f.Price != null && f.Expired != true).ToListAsync();
            foreach (var p in oldPrices)
            {
                p.Expired = true;
                _context.Fboprices.Update(p);
            }
            await _context.SaveChangesAsync();

            var addOnMargins = await (
                            from s in _context.TempAddOnMargin
                            where s.FboId == fboId && s.EffectiveTo >= universalTime
                            select s).ToListAsync();

            var result = (from p in products
                          join f in fboprices on
                                new { Product = p.Description, FboId = fboId }
                                equals
                                new { f.Product, FboId = f.Fboid.GetValueOrDefault() }
                          into leftJoinFBOPrices
                          from f in leftJoinFBOPrices.DefaultIfEmpty()
                          join s in addOnMargins on new { FboId = fboId } equals new { s.FboId }
                          into tmpJoin
                          from s in tmpJoin.DefaultIfEmpty()
                          where p.Description.StartsWith("JetA") || p.Description.StartsWith("SAF")
                          select new FbopricesResult
                          {
                              Oid = f?.Oid ?? 0,
                              Fboid = fboId,
                              Product = p.Description,
                              Price = f?.Price,
                              EffectiveFrom = f?.EffectiveFrom ?? DateTime.UtcNow,
                              EffectiveTo = f?.EffectiveTo ?? null,
                              TimeStamp = f?.Timestamp,
                              SalesTax = f?.SalesTax,
                              Currency = f?.Currency,
                              TempJet = s?.MarginJet,
                              TempAvg = s?.MarginAvgas,
                              TempId = s?.Id,
                              TempDateFrom = s?.EffectiveFrom,
                              TempDateTo = s?.EffectiveTo
                          }).ToList();

            return result;
        }
    }
}
