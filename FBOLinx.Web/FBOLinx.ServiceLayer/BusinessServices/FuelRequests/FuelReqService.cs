using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.FboAirport;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using Fuelerlinx.SDK;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.FuelRequests
{
    public interface IFuelReqService : IBaseDTOService<FuelReqDto, FuelReq>
    {
        Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = false);
        Task<List<FuelReqDto>> GetDirectOrdersForFbo(int fboId, DateTime? startDateTime = null,
            DateTime? endDateTime = null);

        Task<List<FuelReqDto>> GetDirectAndContractOrdersByGroupAndFbo(int groupId, int fboId,
            DateTime startDateTime, DateTime endDateTime);
    }

    public class FuelReqService : BaseDTOService<FuelReqDto, DB.Models.FuelReq, FboLinxContext>, IFuelReqService
    {
        private string _UpcomingOrdersCacheKeyPrefix = "UpcomingOrders_ByGroupAndFbo_";
        private FuelReqEntityService _FuelReqEntityService;
        private readonly FuelerLinxApiService _fuelerLinxService;
        private readonly FboLinxContext _context;
        private IFboEntityService _FboEntityService;
        private ICustomerInfoByGroupEntityService _CustomerInfoByGroupEntityService;
        private IMemoryCache _MemoryCache;

        public FuelReqService(FuelReqEntityService fuelReqEntityService, FuelerLinxApiService fuelerLinxService, FboLinxContext context,
            IFboEntityService fboEntityService,
            ICustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IMemoryCache memoryCache
            ) : base(fuelReqEntityService)
        {
            _MemoryCache = memoryCache;
            _CustomerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _FboEntityService = fboEntityService;
            _FuelReqEntityService = fuelReqEntityService;
            _fuelerLinxService = fuelerLinxService;
            _context = context;
        }

        public async Task<List<FuelReqDto>> GetUpcomingDirectAndContractOrders(int groupId, int fboId,
            bool useCache = false)
        {
            List<FuelReqDto> result = null;
            if (useCache)
            {
                result = await GetUpcomingOrdersFromCache(groupId, fboId);
            }

            if (result == null)
            {
                result = await GetDirectAndContractOrdersByGroupAndFbo(groupId, fboId, DateTime.UtcNow.AddHours(-1),
                    DateTime.UtcNow.AddHours(12));
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
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


            foreach (TransactionDTO transaction in fuelerlinxContractFuelOrders.Result)
            {
                var fuelRequest = FuelReqDto.Cast(transaction, customers.Where(x => x.Customer?.FuelerlinxId == transaction.CompanyId).Select(x => x.Company).FirstOrDefault());
                fuelReqsFromFuelerLinx.Add(fuelRequest);
            }

            var directOrders = await GetDirectOrdersFromDatabase(fboId, startDateTime, endDateTime, customers);
            

            result.AddRange(fuelReqsFromFuelerLinx);
            result.AddRange(directOrders);

            return result;
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
    }
}
