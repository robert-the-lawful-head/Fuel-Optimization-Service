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
        private FuelerLinxApiService _fuelerLinxApiService;
        #region Constructors
        public GroupCustomersService(FboLinxContext context, FuelerLinxApiService fuelerLinxApiService)
        {
            _context = context;
            _fuelerLinxApiService = fuelerLinxApiService;
        }
        #endregion

        public static async Task BeginCustomerAircraftsImport(FboLinxContext context, int groupId, FuelerLinxApiService fuelerLinxApiService)
        {
            GroupCustomersService service = new GroupCustomersService(context, fuelerLinxApiService);
            await service.StartAircraftTransfer(groupId);
        }

        public async Task StartAircraftTransfer(int groupId)
        {
            try
            {
                var listWithCustomers = _context.Customers.Where(s => s.FuelerlinxId > 0 && s.Company != null && s.GroupId == null).ToList();
                var aircrafts = await _fuelerLinxApiService.GetAircraftsFromFuelerinx();

                System.Collections.ArrayList customerExistsList = new System.Collections.ArrayList();
                foreach (var cust in listWithCustomers)
                {
                    var customerExists = await _context.CustomerInfoByGroup.CountAsync(s => s.CustomerId == cust.Id && s.GroupId == groupId);

                    if (customerExists > 0)
                    {
                        customerExistsList.Add(cust.Id);
                        continue;
                    }

                    CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                    cibg.GroupId = groupId;
                    cibg.CustomerId = cust.Id;
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
                    var customerAircrafts = await _context.CustomerAircrafts.CountAsync(s => s.GroupId == groupId && s.CustomerId == cust.Id);

                    if (customerExistsList.Contains(cust.Id) || customerAircrafts > 0)
                        continue;

                    var filteredAircraftsByCompany = aircrafts.Result.Where(s => s.CompanyId == cust.FuelerlinxId).ToList();
                    
                    foreach(var aircraft in filteredAircraftsByCompany)
                    {
                        CustomerAircrafts ca = new CustomerAircrafts();
                        ca.AircraftId = Convert.ToInt32(aircraft.AircraftTypeId);
                        ca.CustomerId = cust.Id;
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
