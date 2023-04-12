using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Customers;
using FBOLinx.DB.Specifications.Group;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using AircraftSizes = FBOLinx.Core.Enums.AircraftSizes;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations
{
    public interface IFuelerLinxAircraftSyncingService
    {
        Task SyncFuelerlinxAircraft(int fuelerLinxCompanyId, string tailNumber);
    }

    public class FuelerLinxAircraftSyncingService : IFuelerLinxAircraftSyncingService
    {
        private FuelerLinxApiService _fuelerLinxApiService;
        private int _fuelerLinxCompanyId;
        private AircraftDataDTO _fuelerlinxAircraft;
        private ICustomersEntityService _customerEntityService;
        private IGroupService _groupService;
        private List<GroupDTO> _existingGroupRecords;
        private CustomerDTO _customerRecord;
        private CustomerAircraftEntityService _customerAircraftEntityService;
        private string _tailNumber;
        private ICustomerService _customerService;

        public FuelerLinxAircraftSyncingService(FuelerLinxApiService fuelerLinxApiService,
            ICustomersEntityService customerEntityService,
            IGroupService groupService,
            CustomerAircraftEntityService customerAircraftEntityService,
            ICustomerService customerService)
        {
            _customerAircraftEntityService = customerAircraftEntityService;
            _groupService = groupService;
            _customerEntityService = customerEntityService;
            _fuelerLinxApiService = fuelerLinxApiService;
            _customerService = customerService;
        }

        public async Task SyncFuelerlinxAircraft(int fuelerLinxCompanyId, string tailNumber)
        {
            _fuelerLinxCompanyId = fuelerLinxCompanyId;
            _tailNumber = tailNumber;
            await PrepareDataForSync();
            await SyncAircraft();
        }

        private async Task SyncAircraft()
        {
            if (_customerRecord == null)
                return;

            //Find all aircraft currently covering the FuelerLinx company fleet
            var coveredAircraft = await _customerAircraftEntityService.GetListBySpec(
                new CustomerAircraftByGroupAndTailSpecification(_existingGroupRecords.Select(x => x.Oid).ToList(),
                    new List<string>() { _tailNumber }, _customerRecord.Oid));

            if (_fuelerlinxAircraft == null)
            {
                if (coveredAircraft?.Count > 0)
                {
                    coveredAircraft = (from c in coveredAircraft
                                      select new CustomerAircrafts()
                                      {
                                          Oid = c.Oid,
                                          CustomerId = 0,
                                          GroupId = c.GroupId,
                                          AircraftId = c.AircraftId,
                                          TailNumber = c.TailNumber,
                                          Size = c.Size
                                      }).ToList();
                    await _customerAircraftEntityService.BulkUpdate(coveredAircraft, new BulkConfig()
                    {
                        BatchSize = 500,
                        SetOutputIdentity = false,
                        BulkCopyTimeout = 0,
                        WithHoldlock = false
                    });
                }

                return;
            }

            if (coveredAircraft?.Count == 0)
            {
                //Find aircrafts that don't have any customer associations to add in
                List<int> groupIds = _existingGroupRecords.Select(x => x.Oid).ToList();
                var nonCustomerAircraftList =
                   await _customerAircraftEntityService.GetListBySpec(
                       new CustomerAircraftByGroupAndTailSpecification(groupIds, new List<string>() { _tailNumber }, 0));

                var newAircraftToAdd = (from g in groupIds
                                        join ca in nonCustomerAircraftList on g equals ca.GroupId
                                        select new CustomerAircrafts()
                                        {
                                            AddedFrom = (_customerRecord.FuelerlinxId > 0 ? 1 : 0),
                                            AircraftId = _fuelerlinxAircraft.AircraftId.Value,
                                            GroupId = g,
                                            CustomerId = _customerRecord.Oid,
                                            TailNumber = _tailNumber,
                                            Size = (AircraftSizes?)_fuelerlinxAircraft.Size,
                                            Oid = ca.Oid
                                        }).ToList();

                if (newAircraftToAdd?.Count > 0)
                    await _customerAircraftEntityService.BulkUpdate(newAircraftToAdd.GroupBy(x => x.Oid).Select(x => x.First()).ToList(), new BulkConfig()
                    {
                        BatchSize = 500,
                        SetOutputIdentity = false,
                        BulkCopyTimeout = 0,
                        WithHoldlock = false
                    });

                coveredAircraft = newAircraftToAdd;
            }

            //Create a list of what full coverage should look like
            var fullCoverage = _existingGroupRecords.Select(x => new DB.Models.CustomerAircrafts()
            {
                AddedFrom = (_customerRecord.FuelerlinxId > 0 ? 1 : 0),
                AircraftId = _fuelerlinxAircraft.AircraftId.GetValueOrDefault(),
                TailNumber = _fuelerlinxAircraft.TailNumber,
                CustomerId = _customerRecord.Oid,
                Size = (AircraftSizes) ((short)_fuelerlinxAircraft.Size.GetValueOrDefault()),
                GroupId = x.Oid
            }).ToList();

            //Find the aircraft/groups missing records within FBOLinx
            var missingCoverage = (from f in fullCoverage
                join c in coveredAircraft on new { GroupId = f.GroupId, f.TailNumber, CustomerId = f.CustomerId } equals new
                    { GroupId = c.GroupId, TailNumber = c.TailNumber, CustomerId = c.CustomerId }
                into leftJoinCoveredAircraft from c in leftJoinCoveredAircraft.DefaultIfEmpty()

                where (c?.Oid).GetValueOrDefault() == 0
                select f).ToList();

            coveredAircraft.ForEach(x => x.AircraftId = _fuelerlinxAircraft.AircraftId.GetValueOrDefault());
            coveredAircraft.AddRange(missingCoverage);

            //Add them to the CustomerAircrafts table and update covered aircraft with the make/model
            await _customerAircraftEntityService.BulkInsert(coveredAircraft);
        }

        private async Task PrepareDataForSync()
        {
            var client = await _fuelerLinxApiService.GetApiClient();

            var aircraftResponse = await client.FBOLinx_GetAircraftDataForCompanyAndTailNumberAsync(_fuelerLinxCompanyId, _tailNumber);
            if ((aircraftResponse?.Success).GetValueOrDefault() && aircraftResponse.Result != null)
                _fuelerlinxAircraft = aircraftResponse.Result;

            _existingGroupRecords = await _groupService.GetListbySpec(new AllGroupsSpecification(false));

            _customerRecord = await _customerService.GetSingleBySpec(
                new CustomerByFuelerLinxIdSpecification(_fuelerLinxCompanyId));
        }
    }
}
