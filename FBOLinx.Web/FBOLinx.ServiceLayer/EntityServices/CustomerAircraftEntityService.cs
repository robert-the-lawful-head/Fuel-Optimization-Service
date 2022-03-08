using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerAircraftEntityService : IRepository<CustomerAircrafts>
    {
        Task<List<string>> GetTailNumbers(int aircraftPricingTemplateId, int customerId, int groupId);
    }
    public class CustomerAircraftEntityService : Repository<CustomerAircrafts>, ICustomerAircraftEntityService
    {
        private readonly FboLinxContext _context;
        public CustomerAircraftEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetTailNumbers(int aircraftPricingTemplateId, int customerId, int groupId)
        {
            var tailNumberList = from ca in _context.CustomerAircrafts
                                 join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                                 where ap.PriceTemplateId == aircraftPricingTemplateId
                                       && ca.CustomerId == customerId
                                       && ca.GroupId == groupId
                                       && !string.IsNullOrEmpty(ca.TailNumber)
                                 select ca.TailNumber.Trim();
            return await tailNumberList.ToListAsync();
        }
    }
}
