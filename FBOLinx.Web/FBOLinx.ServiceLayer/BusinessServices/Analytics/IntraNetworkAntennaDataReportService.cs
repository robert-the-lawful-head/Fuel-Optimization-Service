using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.AircraftHexTailMapping;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.Group;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.DTO.Requests.AirportWatch;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Analytics;
using FBOLinx.ServiceLayer.EntityServices.SWIM;

namespace FBOLinx.ServiceLayer.BusinessServices.Analytics
{
    public interface IIntraNetworkAntennaDataReportService
    {
        Task<List<IntraNetworkVisitsReportItem>> GenerateReportForNetwork(int groupId, DateTime startDateTimeUtc, DateTime endDateTimeUtc);
    }

    public class IntraNetworkAntennaDataReportService : IIntraNetworkAntennaDataReportService
    {
        private IAirportWatchHistoricalDataService _AirportWatchHistoricalDataService;
        private IFboService _FboService;
        private IAirportTimeService _AirportTimeService;
        private ICustomerInfoByGroupService _CustomerInfoByGroupService;
        private FAAAircraftMakeModelEntityService _FaaAircraftMakeModelEntityService;
        private AirportWatchService _AirportWatchService;
        private IGroupService _GroupService;

        public IntraNetworkAntennaDataReportService(IAirportWatchHistoricalDataService airportWatchHistoricalDataService,
            IFboService fboService,
            IAirportTimeService airportTimeService,
            ICustomerInfoByGroupService customerInfoByGroupService,
            FAAAircraftMakeModelEntityService faaAircraftMakeModelEntityService,
            AirportWatchService airportWatchService,
            IGroupService groupService)
        {
            _GroupService = groupService;
            _AirportWatchService = airportWatchService;
            _FaaAircraftMakeModelEntityService = faaAircraftMakeModelEntityService;
            _CustomerInfoByGroupService = customerInfoByGroupService;
            _AirportTimeService = airportTimeService;
            _FboService = fboService;
            _AirportWatchHistoricalDataService = airportWatchHistoricalDataService;
        }

        public async Task<List<IntraNetworkVisitsReportItem>> GenerateReportForNetwork(int groupId, DateTime startDateTimeUtc, DateTime endDateTimeUtc)
        {
            var group = await _GroupService.GetSingleBySpec(new GroupByGroupIdSpecification(groupId));
            var fbos = await _FboService.GetListbySpec(new AllFbosByGroupIdSpecification(groupId));
            var airportWatchData =
                await _AirportWatchService.GetArrivalsDeparturesRefactored(groupId, null, new AirportWatchHistoricalDataRequest()
                {
                    EndDateTime = endDateTimeUtc,
                    StartDateTime = startDateTimeUtc,
                    KeepParkingEvents = true
                });


            //Create a list of IntraNetworkAntennaDataReportItem for each grouped record in airportWatchData by TailNumber, Company, and CustomerId
            //For every airport in each group, create a IntraNetworkAntennaDataReportVisitsItem for each FBO at that airport
            List<IntraNetworkVisitsReportItem> result = new List<IntraNetworkVisitsReportItem>();

            var groupedData = airportWatchData.GroupBy(x => new { x.TailNumber, x.Company, x.CustomerInfoByGroupID, x.AirportIcao });
            foreach (var groupByIcaoAndTail in groupedData)
            {
                //First ensure we have an item for the current tail number
                var item = result.FirstOrDefault(x =>
                    x.TailNumber == groupByIcaoAndTail.Key.TailNumber &&
                    x.CustomerInfoByGroupId == groupByIcaoAndTail.Key.CustomerInfoByGroupID);
                if (item == null)
                {
                    //If not, add it to the result
                    item = new IntraNetworkVisitsReportItem()
                    {
                        GroupName = group.GroupName,
                        TailNumber = groupByIcaoAndTail.Key.TailNumber,
                        Company = groupByIcaoAndTail.Key.Company,
                        AircraftType = groupByIcaoAndTail.FirstOrDefault()?.AircraftType,
                        AircraftTypeCode = groupByIcaoAndTail.FirstOrDefault()?.AircraftTypeCode,
                        CustomerInfoByGroupId = groupByIcaoAndTail.Key.CustomerInfoByGroupID
                    };
                    result.Add(item);
                }

                //Grab all of the fbos at the current airport
                var fbosAtAirport = fbos.Where(x =>
                    x.FboAirport?.Icao?.ToUpper() == groupByIcaoAndTail.Key.AirportIcao?.ToUpper()).ToList();

                //For each one, add a IntraNetworkAntennaDataReportVisitsItem to the current IntraNetworkAntennaDataReportItem with the visit stats
                foreach (var fbo in fbosAtAirport)
                {
                    item.VisitsByAirport.Add(new IntraNetworkVisitsReportByAirportItem()
                    {
                        AcukwikFboHandlerId = fbo.AcukwikFBOHandlerId.GetValueOrDefault(),
                        FboName = fbo.Fbo,
                        Icao = groupByIcaoAndTail.Key.AirportIcao,
                        VisitsToAirport = groupByIcaoAndTail.Count(),
                        VisitsToFbo = (groupByIcaoAndTail.FirstOrDefault()?.VisitsToMyFbo).GetValueOrDefault(),
                    });
                }
            }

            return result;

        }
    }
}
