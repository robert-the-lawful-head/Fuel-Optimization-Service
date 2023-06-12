using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Projections.AirportWatch;
using FBOLinx.DB.Specifications.AirportWatchData;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.BusinessServices.AirportWatch
{
    public interface
        IAirportWatchHistoricalDataService : IBaseDTOService<AirportWatchHistoricalDataDto,
            DB.Models.AirportWatchHistoricalData>
    {
        Task<List<AirportWatchHistoricalDataDto>> GetHistoricalData(DateTime aircraftPositionStateDateUtc,
            DateTime aircraftPositionEndDateUtc,
            List<string> airportIcaos = null, List<string> aircraftHexCodes = null, List<string> tailNumbers = null, List<string> atcFlightNumbers = null);
        Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int? groupId,
            List<string> airportIcaos, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null);
        Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int groupId,
            int? fboId, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null);
    }

    public class AirportWatchHistoricalDataService : BaseDTOService<AirportWatchHistoricalDataDto, DB.Models.AirportWatchHistoricalData, FboLinxContext>, IAirportWatchHistoricalDataService
    {
        private AirportWatchHistoricalDataEntityService _AirportWatchHistoricalDataEntityService;
        private AircraftService _AircraftService;
        private IFboEntityService _FboEntityService;
        private CustomerAircraftEntityService _CustomerAircraftEntityService;
        private readonly ICustomerInfoByGroupService _CustomerInfoByGroupService;

        public AirportWatchHistoricalDataService(AirportWatchHistoricalDataEntityService airportWatchHistoricalDataEntityService,
            AircraftService aircraftService,
            IFboEntityService fboEntityService,
            CustomerAircraftEntityService customerAircraftEntityService,
            ICustomerInfoByGroupService customerInfoByGroupService
            ) : base(airportWatchHistoricalDataEntityService)
        {
            _CustomerAircraftEntityService = customerAircraftEntityService;
            _CustomerInfoByGroupService = customerInfoByGroupService;
            _FboEntityService = fboEntityService;
            _AircraftService = aircraftService;
            _AirportWatchHistoricalDataEntityService = airportWatchHistoricalDataEntityService;
        }

        public async Task<List<AirportWatchHistoricalDataDto>> GetHistoricalData(DateTime aircraftPositionStateDateUtc,
            DateTime aircraftPositionEndDateUtc,
            List<string> airportIcaos = null, List<string> aircraftHexCodes = null, List<string> tailNumbers = null, List<string> atcFlightNumbers = null)
        {
            var result = await _AirportWatchHistoricalDataEntityService.GetHistoricalData(aircraftPositionStateDateUtc,
                aircraftPositionEndDateUtc, airportIcaos, aircraftHexCodes, tailNumbers, atcFlightNumbers);
            return result == null ? null : result.Adapt<List<AirportWatchHistoricalDataDto>>();
        }

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int groupId,
            int? fboId, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null)
        {
            List<string> airportIcaos = null;
            if (fboId.HasValue)
            {
                var fbo = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId.GetValueOrDefault()));
                airportIcaos = new List<string>() { fbo?.FboAirport?.Icao };
            }
            else
            {
                var allFbos = await _FboEntityService.GetListBySpec(new AllFbosByGroupIdSpecification(groupId));
                airportIcaos = allFbos.Select(x => x.FboAirport.Icao).Distinct().ToList();
            }
            
            return await GetHistoricalDataWithCustomerAndAircraftInfo(groupId, airportIcaos, startDateTimeUtc, endDateTimeUtc, tailNumbers);
        }

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int? groupId,
            List<string> airportIcaos, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null)
        {
            //Fetch historical data for the provided tails
            var historicalData = await _AirportWatchHistoricalDataEntityService.GetHistoricalData(startDateTimeUtc, endDateTimeUtc, airportIcaos, null, tailNumbers) ;
            
            //Retrieve relevant customer aircraft information
            var distinctTailsFromHistoricalData = historicalData.Where(x => !string.IsNullOrEmpty(x.TailNumber)).Select(x => x.TailNumber)
                .Distinct().ToList();

            var customerInfoByGroupCollection = await _CustomerInfoByGroupService.GetCustomers(groupId.GetValueOrDefault(), distinctTailsFromHistoricalData);
            var customerAircrafts = customerInfoByGroupCollection.SelectMany(ca => ca.Customer.CustomerAircrafts).ToList();


            //var customerAircrafts =
            //    await _CustomerAircraftEntityService.GetListBySpec(
            //        new CustomerAircraftsByGroupSpecification(groupId.GetValueOrDefault(), distinctTailsFromHistoricalData));



            //Join the historical and customer aircraft data together
            var result = (from hd in historicalData
                                join cad in customerAircrafts on new { TailNumber = hd.TailNumber, GroupId = groupId.GetValueOrDefault() } equals new { cad.TailNumber, GroupId =  cad.GroupId }
                                                                  into leftJoinedCustomerAircrafts
                                from cad in leftJoinedCustomerAircrafts.DefaultIfEmpty()
                                group hd by new
                                {
                                    hd.AircraftHexCode,
                                    hd.AtcFlightNumber,
                                    hd.AircraftPositionDateTimeUtc,
                                    hd.AircraftStatus,
                                    hd.AirportICAO,
                                    hd.AircraftTypeCode,
                                    hd.Latitude,
                                    hd.Longitude,
                                    Company = cad == null ? "" : cad.Customer.Company,
                                    CustomerId = cad == null ? 0 : cad.CustomerId,
                                    TailNumber = cad == null ? hd.TailNumber : cad.TailNumber,
                                    AircraftId = cad == null ? 0 : cad.AircraftId,
                                    //We are allowed to use the nullable ? reference here because we are not executing this LINQ query against the DB.
                                    CustomerInfoByGroupID = cad?.Customer?.CustomerInfoByGroup?.FirstOrDefault()?.Oid,
                                }
                into groupedResult
                                select new FboHistoricalDataModel
                                {
                                    AirportWatchHistoricalDataID = groupedResult.Max(x => x.Oid),
                                    AircraftHexCode = groupedResult.Key.AircraftHexCode,
                                    AtcFlightNumber = groupedResult.Key.AtcFlightNumber,
                                    AircraftPositionDateTimeUtc = groupedResult.Key.AircraftPositionDateTimeUtc,
                                    AircraftStatus = groupedResult.Key.AircraftStatus,
                                    AircraftTypeCode = groupedResult.Key.AircraftTypeCode,
                                    Company = groupedResult.Key.Company,
                                    CustomerId = groupedResult.Key.CustomerId,
                                    TailNumber = groupedResult.Key.TailNumber,
                                    AircraftId = groupedResult.Key.AircraftId,
                                    AirportICAO = groupedResult.Key.AirportICAO,
                                    CustomerInfoByGroupID = groupedResult.Key.CustomerInfoByGroupID.GetValueOrDefault(),
                                    Latitude = groupedResult.Key.Latitude,
                                    Longitude = groupedResult.Key.Longitude,
                                    AirportWatchHistoricalParking = groupedResult.FirstOrDefault(x => x.AirportWatchHistoricalParking != null)?.AirportWatchHistoricalParking.Adapt<AirportWatchHistoricalParkingDto>()
                                })
                .ToList();




            //Populate aircraft info with factory data
            return await PopulateHistoricalDataWithAircraftFactoryData(result);
        }

        private async Task<List<FboHistoricalDataModel>> PopulateHistoricalDataWithAircraftFactoryData(List<FboHistoricalDataModel> historicalData)
        {
            var aircraftFactoryData = await (_AircraftService.GetAllAircraftsOnlyAsQueryable()).ToListAsync();

            var result = (from h in historicalData
                join a in aircraftFactoryData on h.AircraftId equals a.AircraftId
                    into leftJoinedAircrafts
                from a in leftJoinedAircrafts.DefaultIfEmpty()
                orderby h.AircraftPositionDateTimeUtc descending
                select new FboHistoricalDataModel
                {
                    AirportWatchHistoricalDataID = h.AirportWatchHistoricalDataID,
                    CustomerId = h.CustomerId,
                    Company = h.Company,
                    AircraftPositionDateTimeUtc = h.AircraftPositionDateTimeUtc,
                    TailNumber = h.TailNumber,
                    AtcFlightNumber = h.AtcFlightNumber,
                    AircraftHexCode = h.AircraftHexCode,
                    AircraftTypeCode = h.AircraftTypeCode,
                    Make = a?.Make,
                    Model = a?.Model,
                    AircraftStatus = h.AircraftStatus,
                    AirportICAO = h.AirportICAO,
                    CustomerInfoByGroupID = h.CustomerInfoByGroupID,
                    Latitude = h.Latitude,
                    Longitude = h.Longitude,
                    AirportWatchHistoricalParking = h.AirportWatchHistoricalParking
                });
            return result.ToList();
        }
    }
}
