using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications.CompanyPricingLog;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.CompanyPricingLog;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.CompanyPricingLog
{
    public interface ICompanyPricingLogService : IBaseDTOService<CompanyPricingLogDto, DB.Models.CompanyPricingLog>
    {
        Task<List<CompanyPricingLogDto>> GetCompanyPricingLogs(int fuelerlinxCompanyId, DateTime startDate,
            DateTime endDate);

        Task<List<CompanyPricingLogMostRecentQuoteModel>> GetMostRecentQuoteDatesForAirport(string icao,
            bool useCache = true);
        public Task AddCompanyPricingLogs(string ICAO, int FuelerlinxCompanyID);

        Task<List<CompanyPricingLogCountByDateRange>> GetCompanyPricingLogCountByDateRange(DateTime startDate,
            DateTime endDate, int? fuelerlinxCompanyId = null);
    }

    public class CompanyPricingLogService : BaseDTOService<CompanyPricingLogDto, DB.Models.CompanyPricingLog, FboLinxContext>, ICompanyPricingLogService
    {
        private int _CacheLifeSpanInMinutes = 10;
        private string _MostRecentQuotesCacheKey = "CompanyPricingLog_MostRecentQuotes_";
        private ICompanyPricingLogEntityService _CompanyPricingLogEntityService;
        private IMemoryCache _MemoryCache;

        public CompanyPricingLogService(ICompanyPricingLogEntityService companyPricingLogEntityService, IMemoryCache memoryCache) : base(companyPricingLogEntityService)
        {
            _MemoryCache = memoryCache;
            _CompanyPricingLogEntityService = companyPricingLogEntityService;
        }

        public async Task<List<CompanyPricingLogDto>> GetCompanyPricingLogs(int fuelerlinxCompanyId, DateTime startDate,
            DateTime endDate)
        {
            var result =
                await GetListbySpec(
                    new CompanyPricingLogSpecification(fuelerlinxCompanyId, startDate, endDate));
            return result;
        }

        public async Task<List<CompanyPricingLogMostRecentQuoteModel>> GetMostRecentQuoteDatesForAirport(string icao, bool useCache = true)
        {
            List<CompanyPricingLogMostRecentQuoteModel> result = null;

            if (useCache)
                result = await GetMostRecentQuotesForAirportFromCache(icao);

            if (result == null)
            {
                result = await _CompanyPricingLogEntityService.GetMostRecentQuotesByAirport(icao);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheLifeSpanInMinutes));
                _MemoryCache.Set(_MostRecentQuotesCacheKey + icao, result, cacheEntryOptions);
            }
            
            return result;
        }

        public async Task AddCompanyPricingLogs(string ICAO, int FuelerlinxCompanyID)
        {
            //TODO: Refactoring: move this to a bulk insert method once we establish a service for CompanyPricingLog
            List<string> icaoList = ICAO.Split(',').Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<CompanyPricingLogDto> companyPricingLogs = new List<CompanyPricingLogDto>();
            foreach (string icao in icaoList)
            {
                companyPricingLogs.Add(new CompanyPricingLogDto
                {
                    CompanyId = FuelerlinxCompanyID,
                    ICAO = icao,
                    CreatedDate = DateTime.Now
                });
            }

            await BulkInsert(companyPricingLogs);
        }

        public async Task<List<CompanyPricingLogCountByDateRange>> GetCompanyPricingLogCountByDateRange(DateTime startDate, DateTime endDate, int? fuelerlinxCompanyId = null)
        {
            return await _CompanyPricingLogEntityService.GetCompanyPricingLogCountByDateRange(startDate, endDate, fuelerlinxCompanyId);
        }

        private async Task<List<CompanyPricingLogMostRecentQuoteModel>> GetMostRecentQuotesForAirportFromCache(
            string icao)
        {
            try
            {
                List<CompanyPricingLogMostRecentQuoteModel> result = null;
                if (_MemoryCache.TryGetValue(_MostRecentQuotesCacheKey + icao, out result))
                    return result;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
