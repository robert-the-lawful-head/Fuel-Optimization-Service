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
                    CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                    cibg.GroupId = groupId;
                    cibg.CustomerId = cust.Oid;
                    cibg.Company = cust.Company;
                    cibg.Username = cust.Username;
                    cibg.Password = cust.Password;
                    cibg.Joined = cust.Joined;
                    cibg.Active = cust.Active;
                    cibg.Distribute = cust.Distribute;
                    cibg.Network = cust.Network;
                    cibg.MainPhone = cust.MainPhone;
                    cibg.Address = cust.Address;
                    cibg.City = cust.City;
                    cibg.State = cust.State;
                    cibg.ZipCode = cust.ZipCode;
                    cibg.Country = cust.Country;
                    cibg.Website = cust.Website;
                    cibg.ShowJetA = cust.ShowJetA;
                    cibg.Show100Ll = cust.Show100Ll;
                    cibg.Suspended = cust.Suspended;

                    _context.CustomerInfoByGroup.Add(cibg);
                    _context.SaveChanges();
                }

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
