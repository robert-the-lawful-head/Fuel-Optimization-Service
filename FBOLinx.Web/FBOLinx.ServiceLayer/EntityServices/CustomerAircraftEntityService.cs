﻿using System;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.Core.Extensions;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomerAircraftEntityService : IRepository<CustomerAircrafts, FboLinxContext>
    {
        Task<List<CustomerAircraftsViewModel>> GetTailNumbers(int groupId, int aircraftPricingTemplateId = 0, int customerId = 0);
        Task<IList<Tuple<int, string, string, string>>> GetAircraftsByFlightDepartments(IList<string> tailNumbers);
        Task<IEnumerable<Tuple<int, string, string>>> GetPricingTemplates(IList<string> tailNumbers);
        Task<List<AircraftLocation>> GetAircraftLocations(int fuelerlinxCustomerId);
    }
    public class CustomerAircraftEntityService : Repository<CustomerAircrafts, FboLinxContext>, ICustomerAircraftEntityService
    {
        private readonly FboLinxContext _context;
        public CustomerAircraftEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CustomerAircraftsViewModel>> GetTailNumbers(int groupId, int aircraftPricingTemplateId = 0, int customerId = 0)
        {
            var tailNumberList = await
                                (from ca in _context.CustomerAircrafts
                                 join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                                 where (aircraftPricingTemplateId == 0 || ap.PriceTemplateId == aircraftPricingTemplateId)
                                       && (customerId == 0 || ca.CustomerId == customerId)
                                       && ca.GroupId == groupId
                                       && !string.IsNullOrEmpty(ca.TailNumber)
                                 select new CustomerAircraftsViewModel { TailNumber = ca.TailNumber.Trim(), CustomerId = ca.CustomerId, PricingTemplateId = ap.PriceTemplateId }).ToListAsync();

            return tailNumberList;
        }

        public async Task<IList<Tuple<int, string, string, string>>> GetAircraftsByFlightDepartments(IList<string> tailNumbers)
        {
            var customerAircrafts = await _context.CustomerAircrafts.Where(x => tailNumbers.Contains(x.TailNumber)).Select(x => new { x.AircraftId, x.TailNumber, x.CustomerId }).ToListAsync();
            List<int> customerIds = customerAircrafts.Select(x => x.CustomerId).Distinct().ToList();
            var customerInfo = await _context.CustomerInfoByGroup.Where(x => customerIds.Contains(x.CustomerId)).Select(x => new { x.Company, x.MainPhone, x.CustomerId}).ToListAsync();
            
            var flightDepartmentList1 = from ca in customerAircrafts
                                        join cg in customerInfo on ca.CustomerId equals cg.CustomerId
                select new { ca.AircraftId, ca.TailNumber, cg.Company, cg.MainPhone };

            return flightDepartmentList1.Select(x => new Tuple<int, string, string, string>(x.AircraftId, x.TailNumber, x.Company, x.MainPhone)).ToList();

            //var flightDepartmentList = from ca in _context.CustomerAircrafts
            //                           join cg in _context.CustomerInfoByGroup on ca.CustomerId equals cg.CustomerId
            //                           where tailNumbers.Contains(ca.TailNumber)
            //                           select new { ca.AircraftId, ca.TailNumber, cg.Company, cg.MainPhone };
            //return (await flightDepartmentList.ToListAsync()).Select(x => new Tuple<int, string, string, string>(x.AircraftId, x.TailNumber, x.Company, x.MainPhone));
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

        public async Task<List<AircraftLocation>> GetAircraftLocations(int fuelerlinxCustomerId)
        {
            var aircraftLocationsQueryable = from c in _context.Customers
                join ca in _context.CustomerAircrafts on c.Oid equals ca.CustomerId
                where c.FuelerlinxId == fuelerlinxCustomerId
                select new AircraftLocation { AircraftId = ca.AircraftId, TailNumber = ca.TailNumber };
            return (await aircraftLocationsQueryable.ToListAsync()).DistinctBy(x => x.TailNumber).ToList();
        }

        public async Task<List<CustomerAircrafts>> GetCustomerAircraftsByGroupAndTail(List<int> groupIds, List<string> tailNumbers, int customerId)
        {
            var customerAircrafts = await (from ca in _context.CustomerAircrafts
                                           join groupId in context.AsTable(groupIds) on ca.GroupId.ToString() equals groupId.Value
                                           where ca.TailNumber != null && ca.TailNumber != "" && tailNumbers.Contains(ca.TailNumber) && ca.CustomerId == customerId
                                           select ca).ToListAsync();

            return customerAircrafts;
        }
    }
}
