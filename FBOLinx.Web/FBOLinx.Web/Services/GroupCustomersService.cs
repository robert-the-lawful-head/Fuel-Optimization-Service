using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class GroupCustomersService
    {
        private FboLinxContext _context;

        #region Constructors
        public GroupCustomersService(FboLinxContext context)
        {
            _context = context;
        }
        #endregion

        public static async Task BeginCustomerAircraftsImport(FboLinxContext context, int groupId)
        {
            GroupCustomersService service = new GroupCustomersService(context);
            await service.StartAircraftTransfer(groupId);
        }

        public async Task StartAircraftTransfer(int groupId)
        {
            try
            {
                var listWithCustomers = _context.Customers.Where(s => s.FuelerlinxId > 0 && s.Company != null).ToList();

                foreach (var cust in listWithCustomers)
                {
                    var listOfAirplanes = _context.CustomerAircrafts.Where(s => s.CustomerId == cust.Oid).GroupBy(s => s.AircraftId).ToList();

                    foreach (var airplane in listOfAirplanes)
                    {
                        var singleAirplane = airplane;
                        CustomerAircrafts ca = new CustomerAircrafts();
                        ca.AircraftId = airplane.Key;
                        ca.CustomerId = cust.Oid;
                        ca.GroupId = groupId;
                        ca.TailNumber = airplane.First().TailNumber;
                        ca.Size = airplane.First().Size;
                        ca.NetworkCode = airplane.First().NetworkCode;
                        ca.AddedFrom = airplane.First().AddedFrom;

                        _context.CustomerAircrafts.Add(ca);
                        _context.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                var eee = ex.Message;
            }


            
        }
    }
}
