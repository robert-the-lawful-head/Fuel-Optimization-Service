using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Customers;
using FBOLinx.DB.Specifications.Group;
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
        private CustomerEntityService _customerEntityService;
        private GroupEntityService _groupEntityService;
        private List<GroupDTO> _existingGroupRecords;
        private CustomerDTO _customerRecord;
        private CustomerAircraftEntityService _customerAircraftEntityService;
        private string _tailNumber;

        public FuelerLinxAircraftSyncingService(FuelerLinxApiService fuelerLinxApiService,
            CustomerEntityService customerEntityService,
            GroupEntityService groupEntityService,
            CustomerAircraftEntityService customerAircraftEntityService)
        {
            _customerAircraftEntityService = customerAircraftEntityService;
            _groupEntityService = groupEntityService;
            _customerEntityService = customerEntityService;
            _fuelerLinxApiService = fuelerLinxApiService;
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
            if (_fuelerlinxAircraft == null || _customerRecord == null)
                return;

            //Find all aircraft currently covering the FuelerLinx company fleet
            var coveredAircraft = await _customerAircraftEntityService.GetListBySpec(
                new CustomerAircraftByGroupAndTailSpecification(_existingGroupRecords.Select(x => x.Oid).ToList(),
                    new List<string>() {_tailNumber}));

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
                join c in coveredAircraft on new { GroupId = f.GroupId.GetValueOrDefault(), f.TailNumber } equals new
                    { GroupId = c.GroupId.GetValueOrDefault(), TailNumber = c.TailNumber }
                into leftJoinCoveredAircraft from c in leftJoinCoveredAircraft.DefaultIfEmpty()
                where (c?.Oid).GetValueOrDefault() == 0
                select f).ToList();

            coveredAircraft.ForEach(x => x.AircraftId = _fuelerlinxAircraft.AircraftId.GetValueOrDefault());
            coveredAircraft.AddRange(missingCoverage);

            //Add them to the CustomerAircrafts table and update covered aircraft with the make/model
            await _customerAircraftEntityService.BulkInsertOrUpdate(coveredAircraft);
        }

        private async Task PrepareDataForSync()
        {
            var client = await _fuelerLinxApiService.GetApiClient();

            var aircraftResponse = await client.FBOLinx_GetAircraftDataForCompanyAndTailNumberAsync(_fuelerLinxCompanyId, _tailNumber);
            if ((aircraftResponse?.Success).GetValueOrDefault() && aircraftResponse.Result != null)
                _fuelerlinxAircraft = aircraftResponse.Result;

            _existingGroupRecords = await _groupEntityService.GetListBySpec(new AllGroupsSpecification(false));

            _customerRecord = await _customerEntityService.GetSingleBySpec(
                new CustomerByFuelerLinxIdSpecification(_fuelerLinxCompanyId));

        }
    }
}
