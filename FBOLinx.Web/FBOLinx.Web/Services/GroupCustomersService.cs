using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using Fuelerlinx.SDK;
using Microsoft.EntityFrameworkCore;

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
                var aircrafts = await _fuelerLinxService.GetAircraftsFromFuelerinx();

                System.Collections.ArrayList customerExistsList = new System.Collections.ArrayList();
                foreach (var cust in listWithCustomers)
                {
                    var customerExists = await _context.CustomerInfoByGroup.CountAsync(s => s.CustomerId == cust.Oid && s.GroupId == groupId);

                    if (customerExists > 0)
                    {
                        customerExistsList.Add(cust.Oid);
                        continue;
                    }

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
                    cibg.CertificateType = cust.CertificateType;

                    _context.CustomerInfoByGroup.Add(cibg);
                    await _context.SaveChangesAsync();
                }

                foreach (var cust in listWithCustomers)
                {
                    var customerAircrafts = await _context.CustomerAircrafts.CountAsync(s => s.GroupId == groupId && s.CustomerId == cust.Oid);

                    if (customerExistsList.Contains(cust.Oid) || customerAircrafts > 0)
                        continue;

                    var filteredAircraftsByCompany = aircrafts.Result.Where(s => s.CompanyId == cust.FuelerlinxId).ToList();
                    
                    foreach(var aircraft in filteredAircraftsByCompany)
                    {
                        CustomerAircrafts ca = new CustomerAircrafts();
                        ca.AircraftId = Convert.ToInt32(aircraft.AircraftTypeId);
                        ca.CustomerId = cust.Oid;
                        ca.GroupId = groupId;
                        ca.TailNumber = aircraft.TailNumber;
                        
                        _context.CustomerAircrafts.Add(ca);
                        await _context.SaveChangesAsync();
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
