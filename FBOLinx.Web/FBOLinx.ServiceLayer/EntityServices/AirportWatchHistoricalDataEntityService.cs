using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public class AirportWatchHistoricalDataEntityService : Repository<AirportWatchHistoricalData, FboLinxContext>
    {
        public AirportWatchHistoricalDataEntityService(FboLinxContext context) : base(context)
        {
        }

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int groupId,
            string airportIcao, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null)
        {
            var result = await (from hd in context.AirportWatchHistoricalData
                                join cad in context.CustomerAircrafts on new { TailNumber = hd.TailNumber, GroupId = groupId } equals new { cad.TailNumber, GroupId = (cad.GroupId.HasValue ? cad.GroupId.Value : 0) }
                                                                  into leftJoinedCustomerAircrafts
                                from cad in leftJoinedCustomerAircrafts.DefaultIfEmpty()
                                join cig in context.CustomerInfoByGroup on new { CustomerId = cad.CustomerId, GroupID = (cad.GroupId.HasValue ? cad.GroupId.Value : 0) } equals new { CustomerId = cig.CustomerId, GroupID = cig.GroupId }
                                    into leftJoinCustomerInfoByGroup
                                from cig in leftJoinCustomerInfoByGroup.DefaultIfEmpty()
                                where hd.AirportICAO == airportIcao
                                    && hd.AircraftPositionDateTimeUtc >= startDateTimeUtc
                                        && hd.AircraftPositionDateTimeUtc <= endDateTimeUtc
                                    && (tailNumbers == null || tailNumbers.Count == 0 || tailNumbers.Contains(hd.TailNumber))
                                group hd by new
                                {
                                    AirportWatchHistoricalDataID = hd.Oid,
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
                                    CustomerInfoByGroupID =
          (cig == null ? 0 : cig.Oid),
                                }
                into groupedResult
                                select new FboHistoricalDataModel
                                {
                                    AirportWatchHistoricalDataID = groupedResult.Key.AirportWatchHistoricalDataID,
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
                                    CustomerInfoByGroupID = groupedResult.Key.CustomerInfoByGroupID,
                                    Latitude = groupedResult.Key.Latitude,
                                    Longitude = groupedResult.Key.Longitude
                                })
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task<List<FboHistoricalDataModel>> GetHistoricalDataWithCustomerAndAircraftInfo(int groupId, int? fboId, DateTime startDateTimeUtc, DateTime endDateTimeUtc, List<string> tailNumbers = null)
        {
            string airportIcao =
                (await context.Fboairports.FirstOrDefaultAsync(x => fboId.HasValue && x.Fboid == fboId.Value))?.Icao;

            return await GetHistoricalDataWithCustomerAndAircraftInfo(groupId, airportIcao, startDateTimeUtc, endDateTimeUtc,
                tailNumbers);
        }
    }
}
