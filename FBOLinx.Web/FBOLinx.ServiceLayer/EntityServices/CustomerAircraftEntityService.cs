using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerAircraftEntityService : IRepository<CustomerAircrafts, FboLinxContext>
    {
        Task<List<string>> GetTailNumbers(int aircraftPricingTemplateId, int customerId, int groupId);
        Task<IEnumerable<Tuple<int, string, string, string>>> GetAircraftsByFlightDepartments(IList<string> tailNumbers);
        Task<IEnumerable<Tuple<int, string, string>>> GetPricingTemplates(IList<string> tailNumbers);
    }
    public class CustomerAircraftEntityService : Repository<CustomerAircrafts, FboLinxContext>, ICustomerAircraftEntityService
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

        public async Task<IEnumerable<Tuple<int, string, string, string>>> GetAircraftsByFlightDepartments(IList<string> tailNumbers)
        {
            var flightDepartmentList = from ca in _context.CustomerAircrafts
                                       join cg in _context.CustomerInfoByGroup on ca.CustomerId equals cg.CustomerId
                                       where tailNumbers.Contains(ca.TailNumber)
                                       select new { ca.AircraftId, ca.TailNumber, cg.Company, cg.MainPhone };
            return (await flightDepartmentList.ToListAsync()).Select(x => new Tuple<int, string, string, string>(x.AircraftId, x.TailNumber, x.Company, x.MainPhone));
        }

        public async Task<IEnumerable<Tuple<int, string, string>>> GetPricingTemplates(IList<string> tailNumbers)
        {
            var pricingTemplateList = from ca in _context.CustomerAircrafts
                                      join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                                      join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                      where tailNumbers.Contains(ca.TailNumber)
                                      select new { ca.AircraftId, ca.TailNumber, pt.Name };
            return (await pricingTemplateList.ToListAsync()).Select(x => new Tuple<int, string, string>(x.AircraftId, x.TailNumber, x.Name));
        }
    }
}
