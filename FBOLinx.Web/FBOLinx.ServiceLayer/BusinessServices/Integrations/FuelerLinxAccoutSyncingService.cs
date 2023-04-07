using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Customers;
using FBOLinx.DB.Specifications.Group;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Mapping;
using Fuelerlinx.SDK;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations
{
    public interface IFuelerLinxAccoutSyncingService
    {
        Task SyncFuelerLinxAccount(int fuelerLinxCompanyId);
    }

    public class FuelerLinxAccoutSyncingService : IFuelerLinxAccoutSyncingService
    {
        private FuelerLinxApiService _fuelerLinxApiService;
        private int _fuelerLinxCompanyId;
        private CompanyDTO _fuelerlinxCompany;
        private ICustomersEntityService _customerEntityService;
        private IGroupService _groupService;
        private List<GroupDTO> _existingGroupRecords;
        private CustomerDTO _customerRecord;
        private CustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        private List<CustomerInfoByGroupDTO> _customerInfoByGroupRecords;
        private ICollection<AircraftDataDTO> _fuelerlinxAircraftList;
        private CustomerAircraftEntityService _customerAircraftEntityService;
        private ICustomerService _customerService;
        private readonly IPricingTemplateService _pricingTemplateService;
        private readonly IFboService _fboService;

        public FuelerLinxAccoutSyncingService(FuelerLinxApiService fuelerLinxApiService,
            ICustomersEntityService customerEntityService,
            IGroupService groupService,
            CustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            CustomerAircraftEntityService customerAircraftEntityService,
            ICustomerService customerService,
            IPricingTemplateService pricingTemplateService,
            IFboService fboService
            )
        {
            _customerAircraftEntityService = customerAircraftEntityService;
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _groupService = groupService;
            _customerEntityService = customerEntityService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _customerService = customerService;
            _pricingTemplateService = pricingTemplateService;
            _fboService = fboService;
        }

        public async Task SyncFuelerLinxAccount(int fuelerLinxCompanyId)
        {
            _fuelerLinxCompanyId = fuelerLinxCompanyId;
            await PrepareDataForSync();
            await SyncCompany();
        }

        private async Task SyncCompany()
        {
            if (_fuelerlinxCompany == null)
                return;

            await UpdateCustomerRecord();

            await UpdateCustomerInfoByGroupRecords();

            await UpdateFleetStatus();

            var fbos = await _fboService.GetListbySpec(new AllFbosFromAllGroupsSpecification());

            foreach (var fbo in fbos)
            {
                await _pricingTemplateService.FixCustomCustomerTypes(fbo.GroupId, fbo.Oid);
            }
        }

        private async Task UpdateCustomerRecord()
        {
            //Set the FuelerLinxId equal to the FuelerLinx company's primary key.  Use a negative value if it has become de-activated.
            _customerRecord.FuelerlinxId = (_fuelerlinxCompany.Active.GetValueOrDefault()
                ? _fuelerlinxCompany.Id
                : -(_fuelerlinxCompany.Id));
            _customerRecord.Suspended = _fuelerlinxCompany.HideInFboLinx.GetValueOrDefault();
            _customerRecord.Company = _fuelerlinxCompany.CompanyName;

            await _customerService.UpdateAsync(_customerRecord);
        }

        private async Task UpdateCustomerInfoByGroupRecords()
        {
            //Insert CustomerInfoByGroup records for each group where the FuelerLinx company doesn't exist yet.
            var customerInfoByGroupRecordsToInsert = (from g in _existingGroupRecords
                    join cg in _customerInfoByGroupRecords on g.Oid equals cg.GroupId
                        into leftJoinCustomerInfoByGroup
                    from cg in leftJoinCustomerInfoByGroup.DefaultIfEmpty()
                    select new {GroupId = g.Oid, CustomerInfoByGroup = cg})
                .Where(x => x.CustomerInfoByGroup == null
                && _fuelerlinxCompany.Active.GetValueOrDefault())
                .Select(x =>
                    new CustomerInfoByGroupDTO()
                    {
                        GroupId = x.GroupId,
                        CustomerId = _customerRecord.Oid,
                        Company = _fuelerlinxCompany.CompanyName,
                        Active = true,
                        Distribute = true,
                        Network = false,
                        ShowJetA = true,
                        Show100Ll = false,
                        Suspended = false,
                        CustomerType = (_fuelerlinxCompany.Active.GetValueOrDefault() ? 1 : 3),
                        CertificateType = ((_fuelerlinxCompany.CertificateType?.ToLower()?.Contains("charter"))
                            .GetValueOrDefault()
                                ? CertificateTypes.Part135
                                : CertificateTypes.Part91)
                    })
                .ToList();

            //Set the company type to "1" for an active FuelerLinx account.  Otherwise, use 3.
            _customerInfoByGroupRecords.ForEach(x =>
            {
                x.CustomerType = (_fuelerlinxCompany.Active.GetValueOrDefault() ? 1 : 3);
                x.Company = _fuelerlinxCompany.CompanyName;
            });

            _customerInfoByGroupRecords.AddRange(customerInfoByGroupRecordsToInsert);

            //Update/Insert all records from above
            var customerInfoByGroupRecords = _customerInfoByGroupRecords.Select(x => x.Map<CustomerInfoByGroup>()).ToList();
            await _customerInfoByGroupEntityService.BulkUpdate(customerInfoByGroupRecords.Where(x => x.Oid > 0).ToList());
            await _customerInfoByGroupEntityService.BulkInsert(customerInfoByGroupRecords.Where(x => x.Oid <= 0).ToList(), new BulkConfig()
            {
                BatchSize = 500,
                SetOutputIdentity = false,
                BulkCopyTimeout = 0,
                WithHoldlock = false
            });
        }

        private async Task UpdateFleetStatus()
        {
            if (_fuelerlinxAircraftList == null || _fuelerlinxAircraftList.Count == 0)
                return;

            //Update all customer aircraft records' "AddedFrom" flag based on the customer's status.
            //This will allow FuelerLinx accounts that have been de-activated to have their tails removed or edited in FBOLinx accounts.
            List<int> groupIds = _customerInfoByGroupRecords.Select(x => x.GroupId).ToList();
            List<string> tailNumbers = _fuelerlinxAircraftList.Select(x => x.TailNumber).ToList();

            //Find aircrafts that don't have any customer associations to add in
            var nonCustomerAircraftList =
               await _customerAircraftEntityService.GetListBySpec(
                   new CustomerAircraftByGroupAndTailSpecification(groupIds, 0));

            var newAircraftToAdd = (from t in tailNumbers
                                    join g in groupIds on 1 equals 1
                                    join ca in nonCustomerAircraftList on new { TailNumber = t.ToLower(), GroupId = g } equals new
                                    { TailNumber = ca.TailNumber.ToLower(), GroupId = ca.GroupId }
                                    //    into leftJoinCustomerAircrafts
                                    //from ca in leftJoinCustomerAircrafts.DefaultIfEmpty()
                                    select new CustomerAircrafts()
                                    {
                                        AddedFrom = (_customerRecord.FuelerlinxId > 0 ? 1 : 0),
                                        AircraftId = _fuelerlinxAircraftList.FirstOrDefault(x => x.TailNumber == t).AircraftId
                                            .GetValueOrDefault(),
                                        GroupId = g,
                                        CustomerId = _customerRecord.Oid,
                                        TailNumber = t,
                                        Size = (Core.Enums.AircraftSizes?)((short)(_fuelerlinxAircraftList
                                            .FirstOrDefault(x => x.TailNumber == t).Size)),
                                        Oid = ca.Oid
                                    }).ToList();

            if (newAircraftToAdd?.Count > 0)
                await _customerAircraftEntityService.BulkUpdate(newAircraftToAdd.GroupBy(x => x.Oid).Select(x => x.First()).ToList());

            var customerAircraftList =
                await _customerAircraftEntityService.GetListBySpec(
                    new CustomerAircraftByGroupAndTailSpecification(groupIds, _customerRecord.Oid));

            //Find any missing customer aircraft records that need to be added
            var aircraftToAdd = (from t in tailNumbers
                join g in groupIds on 1 equals 1
                join ca in customerAircraftList on new { TailNumber = t.ToLower(), GroupId = g, CustomerId = _customerRecord.Oid } equals new
                        { TailNumber = ca.TailNumber.ToLower(), GroupId = ca.GroupId, CustomerId = ca.CustomerId }
                    into leftJoinCustomerAircrafts
                from ca in leftJoinCustomerAircrafts.DefaultIfEmpty()
                where ca is null
                select new CustomerAircrafts()
                {
                    AddedFrom = (_customerRecord.FuelerlinxId > 0 ? 1 : 0),
                    AircraftId = _fuelerlinxAircraftList.FirstOrDefault(x => x.TailNumber == t).AircraftId
                        .GetValueOrDefault(),
                    GroupId = g,
                    CustomerId = _customerRecord.Oid,
                    TailNumber = t,
                    Size = (Core.Enums.AircraftSizes?)((short)(_fuelerlinxAircraftList
                        .FirstOrDefault(x => x.TailNumber == t).Size)),
                })
                .ToList();

            if (aircraftToAdd?.Count > 0)
                await _customerAircraftEntityService.BulkInsert(aircraftToAdd, new BulkConfig()
                {
                    BatchSize = 500,
                    SetOutputIdentity = false,
                    BulkCopyTimeout = 0,
                    WithHoldlock = false
                });

            //Find aircrafts to remove customer association
            var aircraftToRemove = (from ca in customerAircraftList
                                    join t in tailNumbers on ca.TailNumber.ToLower() equals t.ToLower()
                                    into leftJoinTailNumbers
                                    from t in leftJoinTailNumbers.DefaultIfEmpty()
                                    where t is null
                                    select new CustomerAircrafts()
                                    {
                                        Oid = ca.Oid,
                                        CustomerId = 0,
                                        GroupId = ca.GroupId,
                                        AircraftId = ca.AircraftId,
                                        TailNumber = ca.TailNumber,
                                        Size = ca.Size
                                    }).ToList();
          
            if (aircraftToRemove?.Count > 0)
            {
                await _customerAircraftEntityService.BulkUpdate(aircraftToRemove);
                customerAircraftList =
                await _customerAircraftEntityService.GetListBySpec(
                    new CustomerAircraftByGroupAndTailSpecification(groupIds, _customerRecord.Oid));
            }

            if (customerAircraftList == null || customerAircraftList.Count == 0)
                return;
            
            var aircraftNeedingUpdates = customerAircraftList.Where(x => x.AddedFrom != (_customerRecord.FuelerlinxId > 0 ? 1 : 0)).ToList();
            if (aircraftNeedingUpdates?.Count == 0)
                return;

            aircraftNeedingUpdates.ForEach(x => x.AddedFrom = (_customerRecord.FuelerlinxId > 0 ? 1 : 0));
            await _customerAircraftEntityService.BulkUpdate(aircraftNeedingUpdates);
        }

        private async Task PrepareDataForSync()
        {
            var client = await _fuelerLinxApiService.GetApiClient();
            
            var companyResponse = await client.FBOLinx_GetCompanyAsync(_fuelerLinxCompanyId);
            if ((companyResponse?.Success).GetValueOrDefault())
                _fuelerlinxCompany = companyResponse.Result;

            var aircraftResponse = await client.FBOLinx_GetAircraftDataForCompanyAsync(_fuelerLinxCompanyId);
            if ((aircraftResponse?.Success).GetValueOrDefault())
                _fuelerlinxAircraftList = aircraftResponse.Result;

            _existingGroupRecords = await _groupService.GetListbySpec(new AllGroupsSpecification(false));

            _customerRecord = await _customerService.GetSingleBySpec(
                new CustomerByFuelerLinxIdSpecification(_fuelerLinxCompanyId));

            //If a customer record doesn't yet exist then create one for the FuelerLinx flight department
            if (_customerRecord == null)
                _customerRecord = await _customerService.AddAsync(new CustomerDTO()
                {
                    Action = false,
                    Margin = 0,
                    Company = _fuelerlinxCompany.CompanyName,
                    Active = (_fuelerlinxCompany.Active),
                    Distribute = true,
                    FuelerlinxId = (_fuelerlinxCompany.Active.GetValueOrDefault() ? _fuelerlinxCompany.Id : -(_fuelerlinxCompany.Id)),
                    ShowJetA = true,
                    CertificateType = ((_fuelerlinxCompany.CertificateType?.ToLower()?.Contains("charter")).GetValueOrDefault() ? CertificateTypes.Part135 : CertificateTypes.Part91),
                    Suspended = _fuelerlinxCompany.HideInFboLinx.GetValueOrDefault()
                });

            var result = await _customerInfoByGroupEntityService.GetListBySpec(
                new CustomerInfoByGroupByCustomerIdSpecification(_customerRecord.Oid));

            result = (from r in result
                      join g in _existingGroupRecords on r.GroupId equals g.Oid
                      select r).ToList();

            _customerInfoByGroupRecords = result.Map<List<CustomerInfoByGroupDTO>>();
        }
    }
}
