using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerMarginService
    {
        Task CreateCustomerMargins(int? pricingTemplateId, int copiedPricingTemplateId);
    }
    public class CustomerMarginService : ICustomerMarginService
    {
        private readonly FboLinxContext _context;
        public CustomerMarginService(FboLinxContext context)
        {
            _context = context;
        }
        public async Task CreateCustomerMargins(int? pricingTemplateId, int copiedPricingTemplateId)
        {
            var listMargins = _context.CustomerMargins.Where(s => s.TemplateId == pricingTemplateId && s.PriceTierId != 0).ToList();

            if (listMargins.Count == 0) return;
            
            foreach (var margin in listMargins)
            {
                CustomerMargins cm = new CustomerMargins
                {
                    TemplateId = copiedPricingTemplateId,
                    Amount = margin.Amount
                };

                _context.CustomerMargins.Add(cm);
                _context.SaveChanges();

                var priceTier = _context.PriceTiers.Where(s => s.Oid == margin.PriceTierId).ToList();

                foreach (var pricet in priceTier)
                {
                    PriceTiers ptNew = new PriceTiers
                    {
                        Min = pricet.Min,
                        Max = pricet.Max
                    };

                    _context.PriceTiers.Add(ptNew);
                    await _context.SaveChangesAsync();

                    if (ptNew.Oid != 0)
                    {

                        cm.PriceTierId = ptNew.Oid;

                        _context.CustomerMargins.Update(cm);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
