using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Microsoft.Extensions.Caching.Memory;
using Azure.Core;
using FBOLinx.ServiceLayer.DTO.Responses.Analitics;
using FBOLinx.Core.Utilities.DatesAndTimes;
using Microsoft.Extensions.Logging;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public interface IFuelReqService : IBaseDTOService<FuelReqDto, FuelReq>
    {
        Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrdersForTailNumber(int groupId, int fboId,
            string tailNumber,
            bool useCache = false);
        Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = false);
        Task<List<FuelReqDto>> GetDirectOrdersForFbo(int fboId, DateTime? startDateTime = null,
            DateTime? endDateTime = null);

        Task<List<FuelReqDto>> GetDirectAndContractOrdersByGroupAndFbo(int groupId, int fboId,
            DateTime startDateTime, DateTime endDateTime);
        Task<List<ChartDataResponse>> GetCustomersBreakdown(int fboId, int groupId, int? customerId, DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<FuelReqTotalsProjection>> GetValidFuelRequestTotals(int fboId, DateTime startDateTime, DateTime endDateTime);
        IQueryable<ValidCustomersProjection> GetValidCustomers(int groupId, int? customeridval);
        Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetCustomerTransactionsCountForAirport(string icao, DateTime startDateTime, DateTime endDateTime);
        Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetfuelerlinxCustomerFBOOrdersCount(string fbo, string icao, DateTime startDateTime, DateTime endDateTime);
        int GetairportTotalOrders(int fuelerLinxCustomerID, ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount);
    }

    public class FuelReqService : BaseDTOService<FuelReqDto, DB.Models.FuelReq, FboLinxContext>, IFuelReqService
    {
        private int _CacheLifeSpanInMinutes = 10;
        private string _UpcomingOrdersCacheKeyPrefix = "UpcomingOrders_ByGroupAndFbo_";
        private int _HoursToLookBackForUpcomingOrders = 12;
        private int _HoursToLookForwardForUpcomingOrders = 48;

        private FuelReqEntityService _FuelReqEntityService;
        private readonly FuelerLinxApiService _fuelerLinxService;
        private readonly FboLinxContext _context;
        private IFboEntityService _FboEntityService;
        private ICustomerInfoByGroupEntityService _CustomerInfoByGroupEntityService;
        private IMemoryCache _MemoryCache;
        private IAirportService _AirportService;
        private IAirportTimeService _AirportTimeService;
        private readonly AcukwikAirportEntityService _AcukwikAirportEntityService;
        private readonly ILogger _logger;

        public FuelReqService(FuelReqEntityService fuelReqEntityService, FuelerLinxApiService fuelerLinxService, FboLinxContext context,
            IFboEntityService fboEntityService,
            ICustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IMemoryCache memoryCache, 
            IAirportTimeService airportTimeService,
            AcukwikAirportEntityService acukwikAirportEntityService,
            ILogger<FuelReqService> logger
            ) : base(fuelReqEntityService)
        {
            _logger = logger;
            _AcukwikAirportEntityService = acukwikAirportEntityService;
            _AirportTimeService = airportTimeService;
            _MemoryCache = memoryCache;
            _CustomerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _FboEntityService = fboEntityService;
            _FuelReqEntityService = fuelReqEntityService;
            _fuelerLinxService = fuelerLinxService;
            _context = context;
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrdersForTailNumber(int groupId, int fboId,
            string tailNumber,
            bool useCache = true)
        {
            tailNumber = tailNumber.Trim().ToUpper();
            var upcomingOrders = await GetUpcomingDirectAndContractOrders(groupId, fboId, useCache);
            return upcomingOrders?.Where(x => x.TailNumber?.ToUpper() == tailNumber).ToList();
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = true)
        {
            List<FuelReqDto> result = null;
            if (useCache)
            {
                result = await GetUpcomingOrdersFromCache(groupId, fboId);
            }

            if (result == null)
            {
                var startDateTime = await _AirportTimeService.GetAirportLocalDateTime(fboId, DateTime.UtcNow.AddHours(-_HoursToLookBackForUpcomingOrders));
                var endDateTime = await _AirportTimeService.GetAirportLocalDateTime(fboId,
                    DateTime.UtcNow.AddHours(_HoursToLookForwardForUpcomingOrders));
                result = await GetDirectAndContractOrdersByGroupAndFbo(groupId, fboId, startDateTime, endDateTime);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheLifeSpanInMinutes));
                _MemoryCache.Set(_UpcomingOrdersCacheKeyPrefix + groupId + "_" + fboId, result, cacheEntryOptions);
            }

            result?.RemoveAll(x => x.Cancelled.GetValueOrDefault());

            return result;
        }

        public async Task<List<FuelReqDto>> GetDirectOrdersForFbo(int fboId, DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            var startDate = startDateTime == null ? DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0)) : startDateTime;
            var endDate = endDateTime == null ? DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0)) : endDateTime;

            var fboRecord = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            if (fboRecord == null)
                return new List<FuelReqDto>();

            var customers =
                await _CustomerInfoByGroupEntityService.GetListBySpec(
                    new CustomerInfoByGroupByGroupIdSpecification(fboRecord.GroupId.GetValueOrDefault()));

            return await GetDirectOrdersFromDatabase(fboId, startDate, endDate, customers);
        }

        public async Task<List<FuelReqDto>> GetDirectAndContractOrdersByGroupAndFbo(int groupId, int fboId, DateTime startDateTime, DateTime endDateTime)
        {
            var result = new List<FuelReqDto>();
            var fboRecord = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            if (fboRecord == null)
                return result;

            var customers =
                await _CustomerInfoByGroupEntityService.GetListBySpec(
                    new CustomerInfoByGroupByGroupIdSpecification(groupId));
            

            FBOLinxContractFuelOrdersResponse fuelerlinxContractFuelOrders = await _fuelerLinxService.GetContractFuelRequests(new FBOLinxOrdersRequest()
            { EndDateTime = endDateTime, StartDateTime = startDateTime, Icao = fboRecord.FboAirport?.Icao, Fbo = fboRecord.Fbo, IsEtaOnly = true});

            List<FuelReqDto> fuelReqsFromFuelerLinx = new List<FuelReqDto>();


            foreach(TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
            {
                var fuelRequest = FuelReqDto.Cast(transaction, customers.Where(x => x.Customer?.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault());

                fuelRequest.Eta = await GetAirportLocalTime(fuelRequest.Eta.GetValueOrDefault(),transaction.AirportId.GetValueOrDefault());
                fuelRequest.DateCreated = await GetAirportLocalTime(fuelRequest.DateCreated.GetValueOrDefault(),transaction.AirportId.GetValueOrDefault());

                fuelReqsFromFuelerLinx.Add(fuelRequest);
            }

            var directOrders = await GetDirectOrdersFromDatabase(fboId, startDateTime, endDateTime, customers);

            foreach(FuelReqDto order in directOrders)
            {
                AcukwikAirport airport = await _AcukwikAirportEntityService.Where(x => x.Icao == order.Icao).FirstOrDefaultAsync();
                order.Eta = await GetAirportLocalTime(order.Eta.GetValueOrDefault(), airport.Oid);
                order.DateCreated = await GetAirportLocalTime(order.DateCreated.GetValueOrDefault(), airport.Oid);
            }

            result.AddRange(fuelReqsFromFuelerLinx);
            result.AddRange(directOrders);

            return result;
        }
        private async Task<DateTime?> GetAirportLocalTime(DateTime date, int airportId)
        {
            try
            {
                AcukwikAirport airport = await _AcukwikAirportEntityService.GetAsync(airportId);

                return DateTimeHelper.GetLocalTime(
                    date, airport.IntlTimeZone, airport.DaylightSavingsYn?.ToLower() == "y");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting airport local time for airport id {airportId} returning default UTC default dateTime", ex.InnerException);
                return date;
            }   
        }
        private async Task<List<FuelReqDto>> GetUpcomingOrdersFromCache(int groupId, int fboId)
        {
            try
            {
                List<FuelReqDto> result = null;
                if (_MemoryCache.TryGetValue(_UpcomingOrdersCacheKeyPrefix + groupId + "_" + fboId, out result))
                    return result;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<FuelReqDto>> GetDirectOrdersFromDatabase(int fboId, DateTime? startDate, DateTime? endDate, List<CustomerInfoByGroup> customers)
        {
            var requests = await GetListbySpec(new FuelReqByFboAndDateSpecification(fboId, startDate.Value, endDate.Value));

            requests.ForEach(x =>
                x.CustomerName = customers.FirstOrDefault(c => c.CustomerId == x.CustomerId.GetValueOrDefault())?.Company);
            return requests;
        }

        public async Task<IEnumerable<FuelReqTotalsProjection>> GetValidFuelRequestTotals(int fboId,DateTime startDateTime, DateTime endDateTime)
        {
            var validTransactions = await _context.FuelReq.Where(fr =>
            (fr.Cancelled == null || fr.Cancelled == false) && fr.Etd >= startDateTime && fr.Etd <= endDateTime && fr.Fboid.HasValue && fr.Fboid.Value == fboId).ToListAsync();

            return (from fr in validTransactions
                    group fr by fr.CustomerId
            into groupedFuelReqs
                    select new FuelReqTotalsProjection()
                    {
                        CustomerId = groupedFuelReqs.Key,
                        TotalOrders = groupedFuelReqs.Count(),
                        TotalVolume = groupedFuelReqs.Sum(fr => (fr.ActualVolume ?? 0) * (fr.ActualPpg ?? 0) > 0 ?
                                     fr.ActualVolume * fr.ActualPpg :
                                     (fr.QuotedVolume ?? 0) * (fr.QuotedPpg ?? 0)) ?? 0
                    });
        }
        public IQueryable<ValidCustomersProjection> GetValidCustomers(int groupId, int? customeridval)
        {
            return _context.CustomerInfoByGroup
                                   .Where(c => c.GroupId.Equals(groupId))
                                   .Include(x => x.Customer)
                                   .Where(x => (x.Customer != null && x.Customer.Suspended != true) && (x.CustomerId == customeridval
                                   || customeridval == null)).Select(c => new ValidCustomersProjection()
                                   {
                                       Oid = c.Oid,
                                       CustomerId = c.CustomerId,
                                       Company = c.Company.Trim(),
                                       Customer = c.Customer
                                   })
                                  .Distinct();
        }
        public async Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetCustomerTransactionsCountForAirport(string icao, DateTime startDateTime, DateTime endDateTime)
        {
            FBOLinxOrdersRequest fbolinxOrdersRequest = new FBOLinxOrdersRequest();
            fbolinxOrdersRequest.StartDateTime = startDateTime;
            fbolinxOrdersRequest.EndDateTime = endDateTime;
            fbolinxOrdersRequest.Icao = icao;

            FboLinxCustomerTransactionsCountAtAirportResponse response = await _fuelerLinxService.GetCustomerTransactionsCountForAirport(fbolinxOrdersRequest);
            return response.Result;
        }
        public async Task<ICollection<FbolinxCustomerTransactionsCountAtAirport>> GetfuelerlinxCustomerFBOOrdersCount(string fbo,string icao, DateTime startDateTime, DateTime endDateTime)
        {
            FBOLinxOrdersRequest fbolinxOrdersRequest = new FBOLinxOrdersRequest();
            fbolinxOrdersRequest.StartDateTime = startDateTime;
            fbolinxOrdersRequest.EndDateTime = endDateTime;
            fbolinxOrdersRequest.Icao = icao;
            fbolinxOrdersRequest.Fbo = fbo;

            var response = await _fuelerLinxService.GetCustomerFBOTransactionsCount(fbolinxOrdersRequest);

            return response.Result;
        }
        public int GetairportTotalOrders(int fuelerLinxCustomerID,ICollection<FbolinxCustomerTransactionsCountAtAirport> fuelerlinxCustomerOrdersCount)
        {
            var airportTotalOrders = 0;
            if (fuelerlinxCustomerOrdersCount != null)
                airportTotalOrders = fuelerlinxCustomerOrdersCount.Where(c => c.FuelerLinxCustomerId == fuelerLinxCustomerID).Select(f => f.TransactionsCount).FirstOrDefault();
            return airportTotalOrders;
        }
        public async Task<List<ChartDataResponse>> GetCustomersBreakdown(int fboId, int groupId, int? customerId, DateTime startDateTime, DateTime endDateTime)
        {
            string icao = await _context.Fboairports.Where(f => f.Fboid.Equals(fboId)).Select(f => f.Icao).FirstOrDefaultAsync();

            var fuelReqs = await GetValidFuelRequestTotals(fboId, startDateTime, endDateTime);

            var customers = await GetValidCustomers(groupId, customerId).ToListAsync();

            var fuelerlinxCustomerOrdersCount = await GetCustomerTransactionsCountForAirport(icao, startDateTime, endDateTime);

            List<ChartDataResponse> chartData = new List<ChartDataResponse>();
            foreach (var customer in customers)
            {
                var fuelerLinxCustomerID = Math.Abs((customer.Customer?.FuelerlinxId).GetValueOrDefault());
                var selectedCompanyFuelReqs = fuelReqs.Where(f => f.CustomerId.Equals(customer.CustomerId)).FirstOrDefault();

                chartData.Add(new ChartDataResponse()
                {
                    Name = customer.Company,
                    Orders = GetairportTotalOrders(fuelerLinxCustomerID, fuelerlinxCustomerOrdersCount),
                    Volume = selectedCompanyFuelReqs?.TotalVolume ?? 0
                });
            }
            return chartData;
        }
    }
}
