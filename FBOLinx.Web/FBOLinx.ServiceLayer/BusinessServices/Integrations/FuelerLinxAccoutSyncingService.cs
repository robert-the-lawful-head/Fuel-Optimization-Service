using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Customers;
using FBOLinx.DB.Specifications.Group;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Mapster;

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
        private CustomerEntityService _customerEntityService;
        private GroupEntityService _groupEntityService;
        private List<GroupDTO> _existingGroupRecords;
        private CustomerDTO _customerRecord;
        private CustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        private List<CustomerInfoByGroupDTO> _customerInfoByGroupRecords;
        private ICollection<AircraftDataDTO> _fuelerlinxAircraftList;
        private CustomerAircraftEntityService _customerAircraftEntityService;

        public FuelerLinxAccoutSyncingService(FuelerLinxApiService fuelerLinxApiService, 
            CustomerEntityService customerEntityService, 
            GroupEntityService groupEntityService, 
            CustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            CustomerAircraftEntityService customerAircraftEntityService)
        {
            _customerAircraftEntityService = customerAircraftEntityService;
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _groupEntityService = groupEntityService;
            _customerEntityService = customerEntityService;
            _fuelerLinxApiService = fuelerLinxApiService;
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
        }

        private async Task UpdateCustomerRecord()
        {
            //Set the FuelerLinxId equal to the FuelerLinx company's primary key.  Use a negative value if it has become de-activated.
            _customerRecord.FuelerlinxId = (_fuelerlinxCompany.Active.GetValueOrDefault()
                ? _fuelerlinxCompany.Id
                : -(_fuelerlinxCompany.Id));
            _customerRecord.Suspended = _fuelerlinxCompany.HideInFboLinx.GetValueOrDefault();
            _customerRecord.Company = _fuelerlinxCompany.CompanyName;

            await _customerEntityService.Update(_customerRecord);
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
            var customerInfoByGroupRecords = _customerInfoByGroupRecords.Select(x => x.Adapt<CustomerInfoByGroup>()).ToList();
            await _customerInfoByGroupEntityService.BulkInsertOrUpdate(customerInfoByGroupRecords);
        }

        private async Task UpdateFleetStatus()
        {
            if (_fuelerlinxAircraftList == null || _fuelerlinxAircraftList.Count == 0)
                return;

            //Update all customer aircraft records' "AddedFrom" flag based on the customer's status.
            //This will allow FuelerLinx accounts that have been de-activated to have their tails removed or edited in FBOLinx accounts.
            List<int> groupIds = _customerInfoByGroupRecords.Select(x => x.GroupId).ToList();
            List<string> tailNumbers = _fuelerlinxAircraftList.Select(x => x.TailNumber).ToList();

            var customerAircraftList =
                await _customerAircraftEntityService.GetListBySpec(
                    new CustomerAircraftByGroupAndTailSpecification(groupIds, tailNumbers));

            if (customerAircraftList == null || customerAircraftList.Count == 0)
                return;
            customerAircraftList.ForEach(x => x.AddedFrom = (_customerRecord.FuelerlinxId > 0 ? 1 : 0));
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

            _existingGroupRecords = await _groupEntityService.GetListBySpec(new AllGroupsSpecification(false));

            _customerRecord = await _customerEntityService.GetSingleBySpec(
                new CustomerByFuelerLinxIdSpecification(_fuelerLinxCompanyId));

            //If a customer record doesn't yet exist then create one for the FuelerLinx flight department
            if (_customerRecord == null)
                _customerRecord = await _customerEntityService.Add(new CustomerDTO()
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
            _customerInfoByGroupRecords = result.Adapt<List<CustomerInfoByGroupDTO>>();
        }
    }
}
