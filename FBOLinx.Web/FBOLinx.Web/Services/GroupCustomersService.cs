using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.Model;
using IO.Swagger.Api;

namespace FBOLinx.Web.Services
{
    public class GroupCustomersService
    {
        private FboLinxContext _context;
        private FuelerLinxService _fuelerLinxService;
        #region Constructors
        public GroupCustomersService(FboLinxContext context, FuelerLinxService fuelerLinxService)
        {
            _context = context;
            _fuelerLinxService = fuelerLinxService;
        }
        #endregion

        public static async Task BeginCustomerAircraftsImport(FboLinxContext context, int groupId, FuelerLinxService fuelerLinxService)
        {
            GroupCustomersService service = new GroupCustomersService(context, fuelerLinxService);
            await service.StartAircraftTransfer(groupId);
        }

        public async Task StartAircraftTransfer(int groupId)
        {
            try
            {
                var listWithCustomers = _context.Customers.Where(s => s.FuelerlinxId > 0 && s.Company != null && s.GroupId == null).ToList();
                var aircrafts = _fuelerLinxService.GetAircraftsFromFuelerinx();
                foreach (var cust in listWithCustomers)
                {
                    CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                    cibg.GroupId = groupId;
                    cibg.CustomerId = cust.Oid;
                    cibg.Company = cust.Company;
                    cibg.Username = "";
                    cibg.Password = "";
                    cibg.Joined = DateTime.Today;
                    cibg.Active = true;
                    cibg.Distribute = false;
                    cibg.Network = false;
                    cibg.MainPhone = "";
                    cibg.Address = "";
                    cibg.City = "";
                    cibg.State = "";
                    cibg.ZipCode = "";
                    cibg.Country = "";
                    cibg.Website = "";
                    cibg.ShowJetA = true;
                    cibg.Show100Ll = false;
                    cibg.Suspended = false;

                    _context.CustomerInfoByGroup.Add(cibg);
                    _context.SaveChanges();
                }

                foreach (var cust in listWithCustomers)
                {
                    var filteredAircraftsByCompany = aircrafts.Result.Where(s => s.CompanyId == cust.FuelerlinxId).ToList();
                    
                    foreach(var aircraft in filteredAircraftsByCompany)
                    {
                        CustomerAircrafts ca = new CustomerAircrafts();
                        ca.AircraftId = Convert.ToInt32(aircraft.AircraftTypeId);
                        ca.CustomerId = cust.Oid;
                        ca.GroupId = groupId;
                        ca.TailNumber = aircraft.TailNumber;
                        
                        _context.CustomerAircrafts.Add(ca);
                        _context.SaveChanges();
                    }

                    //foreach (var airplane in listOfAirplanes)
                    //{
                    //    var singleAirplane = airplane;
                    //    CustomerAircrafts ca = new CustomerAircrafts();
                    //    ca.AircraftId = airplane.Key;
                    //    ca.CustomerId = cust.Oid;
                    //    ca.GroupId = groupId;
                    //    ca.TailNumber = airplane.First().TailNumber;
                    //    ca.Size = airplane.First().Size;
                    //    ca.NetworkCode = airplane.First().NetworkCode;
                    //    ca.AddedFrom = airplane.First().AddedFrom;

                    //    _context.CustomerAircrafts.Add(ca);
                    //    _context.SaveChanges();
                    //}
                }
            }
            catch(Exception ex)
            {
                var eee = ex.Message;
            }


            
        }
    }
}
