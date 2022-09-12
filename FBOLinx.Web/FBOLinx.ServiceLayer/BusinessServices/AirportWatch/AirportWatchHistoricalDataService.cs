using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Projections.AirportWatch;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface
        IAirportWatchHistoricalDataService : IBaseDTOService<AirportWatchHistoricalDataDto,
            DB.Models.AirportWatchHistoricalData>
    {
        Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int groupId,
            int? fboId, DateTime? startDateTimeUtc, DateTime? endDateTimeUtc, List<string> tailNumbers = null);
    }

    public class AirportWatchHistoricalDataService : BaseDTOService<AirportWatchHistoricalDataDto, DB.Models.AirportWatchHistoricalData, FboLinxContext>, IAirportWatchHistoricalDataService
    {
        private AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private AircraftService _AircraftService;

        public AirportWatchHistoricalDataService(IRepository<AirportWatchHistoricalData, FboLinxContext> entityService 
            //AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService,
            //AircraftService aircraftService
            ) : base(entityService)
        {
            //_AircraftService = aircraftService;
            //_AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
        }

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int groupId,
            int? fboId, DateTime? startDateTimeUtc, DateTime? endDateTimeUtc, List<string> tailNumbers = null)
        {
            return null;
            //var historicalData = await _AirportWatchHistoricalDataEntityService.GetHistoricalDataWithCustomerAndAircraftInfo(groupId, fboId, startDateTimeUtc, endDateTimeUtc, tailNumbers);
            //var aircraftFactoryData = await (_AircraftService.GetAllAircraftsOnlyAsQueryable()).ToListAsync();

            //var result = (from h in historicalData
            //    join a in aircraftFactoryData on h.AircraftId equals a.AircraftId
            //        into leftJoinedAircrafts
            //    from a in leftJoinedAircrafts.DefaultIfEmpty()
            //    orderby h.AircraftPositionDateTimeUtc descending
            //    select new FboHistoricalDataModel
            //    {
            //        CustomerId = h.CustomerId,
            //        Company = h.Company,
            //        AircraftPositionDateTimeUtc = h.AircraftPositionDateTimeUtc,
            //        TailNumber = h.TailNumber,
            //        AtcFlightNumber = h.AtcFlightNumber,
            //        AircraftHexCode = h.AircraftHexCode,
            //        AircraftTypeCode = h.AircraftTypeCode,
            //        Make = a?.Make,
            //        Model = a?.Model,
            //        AircraftStatus = h.AircraftStatus,
            //        AirportICAO = h.AirportICAO,
            //        CustomerInfoByGroupID = h.CustomerInfoByGroupID,
            //        Latitude = h.Latitude,
            //        Longitude = h.Longitude
            //    });
            //return result.ToList();
        }
    }
}
