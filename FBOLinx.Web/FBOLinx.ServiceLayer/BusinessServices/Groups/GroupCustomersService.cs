using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.Groups
{
    public interface IGroupCustomersService
    {
        Task StartAircraftTransfer(int groupId);
    }

    public class GroupCustomersService : IGroupCustomersService
    {
        private FboLinxContext _context;
        private FuelerLinxApiService _fuelerLinxApiService;
        private CustomerAircraftEntityService _CustomerAircraftEntityService;
        private CustomerInfoByGroupEntityService _CustomerInfoByGroupEntityService;

        #region Constructors
        public GroupCustomersService(FboLinxContext context, FuelerLinxApiService fuelerLinxApiService, CustomerAircraftEntityService customerAircraftEntityService, CustomerInfoByGroupEntityService customerInfoByGroupEntityService)
        {
            _CustomerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _CustomerAircraftEntityService = customerAircraftEntityService;
            _context = context;
            _fuelerLinxApiService = fuelerLinxApiService;
        }
        #endregion

        public async Task StartAircraftTransfer(int groupId)
        {
            try
            {
                var listWithCustomers = await _context.Customers.Where(s => s.FuelerlinxId > 0 && s.Company != null && s.GroupId == null).ToListAsync();
                var aircrafts = await _fuelerLinxApiService.GetAircraftsFromFuelerinx();
                var existingCustomerInfoByGroupRecords =
                    await _CustomerInfoByGroupEntityService.GetListBySpec(
                        new CustomerInfoByGroupByGroupIdSpecification(groupId));
                    
                List<CustomerInfoByGroup> customerInfoByGroupToInsert = new List<CustomerInfoByGroup>();

                foreach (var cust in listWithCustomers)
                {
                    if (existingCustomerInfoByGroupRecords.Any(x => x.CustomerId == cust.Oid))
                    {
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

                    customerInfoByGroupToInsert.Add(cibg);
                }

                if (customerInfoByGroupToInsert.Count > 0)
                    await _CustomerInfoByGroupEntityService.BulkInsert(customerInfoByGroupToInsert);

                var existingCustomerAircraftRecordsForGroup =
                    await _CustomerAircraftEntityService.GetListBySpec(
                        new CustomerAircraftsByGroupSpecification(groupId));
                List<CustomerAircrafts> customerAircraftsToInsert = new List<CustomerAircrafts>();

                foreach (var cust in listWithCustomers)
                {
                    var existingCustomerAircrafts = existingCustomerAircraftRecordsForGroup.Where(s => s.GroupId == groupId && s.CustomerId == cust.Oid);

                    var filteredAircraftsByCompany = aircrafts.Result.Where(s => s.CompanyId == cust.FuelerlinxId).ToList();

                    foreach (var aircraft in filteredAircraftsByCompany)
                    {
                        if (existingCustomerAircrafts.Any(x => x.TailNumber?.ToUpper() == aircraft.TailNumber?.ToUpper()))
                            continue;

                        CustomerAircrafts ca = new CustomerAircrafts();
                        ca.AircraftId = Convert.ToInt32(aircraft.AircraftTypeId);
                        ca.CustomerId = cust.Oid;
                        ca.GroupId = groupId;
                        ca.TailNumber = aircraft.TailNumber;

                        customerAircraftsToInsert.Add(ca);
                    }
                }

                if (customerAircraftsToInsert.Count > 0)
                    await _CustomerAircraftEntityService.BulkInsert(customerAircraftsToInsert);
            }
            catch (Exception ex)
            {
                var eee = ex.Message;
            }
        }
    }
}
