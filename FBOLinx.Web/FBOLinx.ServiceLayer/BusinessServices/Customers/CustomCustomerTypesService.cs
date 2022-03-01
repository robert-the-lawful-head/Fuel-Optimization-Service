using FBOLinx.DB.Context;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomCustomerTypeService
    {
        Task SaveCustomersTypes(int fboId, int oid, int defaultPricingTemplateId);
    }
    public class CustomCustomerTypeService : ICustomCustomerTypeService
    {
        private readonly FboLinxContext _context;
        public CustomCustomerTypeService(FboLinxContext context)
        {
            _context = context;
        }

        public async Task SaveCustomersTypes(int fboId, int oid, int defaultPricingTemplateId)
        {
            var customers = _context.CustomCustomerTypes
            .Where(c => c.Fboid.Equals(fboId) && c.CustomerType.Equals(oid))
            .Select(s => s.CustomerId)
            .ToList();          

            var groupInfo = _context.Fbos.FirstOrDefault(s => s.Oid == fboId).GroupId;
            _context.CustomerInfoByGroup.Where(s => customers.Contains(s.CustomerId) && s.GroupId == groupInfo).ToList().ForEach(s => s.PricingTemplateRemoved = true);

            var customCustomerTypes = _context.CustomCustomerTypes
                .Where(c => c.Fboid.Equals(fboId) && c.CustomerType.Equals(oid))
                .ToList();

            customCustomerTypes.ForEach(c => c.CustomerType = defaultPricingTemplateId);

            await _context.SaveChangesAsync();
        }
    }
}
