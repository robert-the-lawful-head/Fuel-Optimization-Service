using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Customers;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Responses.FuelPricing;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog
{
    public interface IMissedQuoteLogService : IBaseDTOService<MissedQuoteLogDTO, DB.Models.MissedQuoteLog>
    {
        Task<List<MissedQuotesLogViewModel>> GetMissedQuotesList(int fboId);
        Task LogMissedQuote(string icaos, List<FuelPriceResponse> result, CustomersDto customer);
    }

    public class MissedQuoteLogService : BaseDTOService<MissedQuoteLogDTO, DB.Models.MissedQuoteLog, FboLinxContext>, IMissedQuoteLogService
    {
        private IFboService _FboService;
        private IFboEntityService _FboEntityService;
        private readonly CustomerInfoByGroupEntityService _CustomerInfoByGroupEntityService;
        private AppPartnerSDKSettings.FuelerlinxSDKSettings _FuelerlinxSdkSettings;
        private IServiceScopeFactory _ScopeFactory;
        private readonly ICustomerService _CustomerService;
        private readonly ICustomerInfoByGroupService _customerInfoByGroupService;

        public MissedQuoteLogService(IMissedQuoteLogEntityService entityService, IFboService fboService, IFboEntityService fboEntityService, IOptions<AppPartnerSDKSettings> appPartnerSDKSettings, CustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IServiceScopeFactory scopeFactory, ICustomerService customerService, ICustomerInfoByGroupService customerInfoByGroupService) : base(entityService)
        {
            _FboService = fboService;
            _FboEntityService = fboEntityService;
            _CustomerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _FuelerlinxSdkSettings = appPartnerSDKSettings.Value.FuelerLinx;
            _ScopeFactory = scopeFactory;
            _CustomerService = customerService;
            _customerInfoByGroupService = customerInfoByGroupService;
        }

        public async Task<List<MissedQuotesLogViewModel>> GetMissedQuotesList(int fboId)
        {
            var recentMissedQuotes = await GetRecentMissedQuotes(fboId, true);

            var fbo = await _FboEntityService.GetSingleBySpec(new FboByIdSpecification(fboId));
            var customersList = await _customerInfoByGroupService.GetCustomersListByGroupAndFbo(fbo.GroupId, fboId);

            var recentMissedQuotesGroupedList = recentMissedQuotes.GroupBy(r => r.CustomerId).Select(g => new
            {
                CustomerId = g.Key,
                MissedQuotesCount = g.Count(x => x.CustomerId > 0)
            }).ToList();

            var missedQuotesLogList = new List<MissedQuotesLogViewModel>();
            foreach (MissedQuoteLogDTO missedQuoteLogDto in recentMissedQuotes)
            {
                if (missedQuotesLogList.Count == 5)
                    break;

                var customerName = customersList.Where(c => c.CompanyId == missedQuoteLogDto.CustomerId).Select(x => x.Company).FirstOrDefault();
                if (customerName != null && !missedQuotesLogList.Any(x => x.CustomerName == customerName))
                {
                    MissedQuotesLogViewModel missedQuotesLogViewModel = new MissedQuotesLogViewModel();
                    missedQuotesLogViewModel.CustomerName = customerName;
                    missedQuotesLogViewModel.CreatedDate = missedQuoteLogDto.CreatedDateString;
                    missedQuotesLogViewModel.MissedQuotesCount = recentMissedQuotesGroupedList.Where(g => g.CustomerId == missedQuoteLogDto.CustomerId).Select(m => m.MissedQuotesCount).FirstOrDefault();
                    missedQuotesLogList.Add(missedQuotesLogViewModel);
                }
            }

            return missedQuotesLogList.OrderBy(m => m.CustomerName).ToList();
        }

        public async Task LogMissedQuote(string icaos, List<FuelPriceResponse> result, CustomersDto  customer)
        {
            foreach (var icao in icaos.Split(',').Select(x => x.Trim()))
            {
                var fbos = await _FboEntityService.GetFbosByIcaos(icao);

                foreach (var fbo in fbos.Where(f => f.AccountType == Core.Enums.AccountTypes.RevFbo).ToList())
                {
                    if (!result.Any(r => r.Icao == icao && r.Fbo == fbo.Fbo))
                    {
                        List<MissedQuoteLogDTO> missedQuotesToLog = new List<MissedQuoteLogDTO>();
                        var customerGroup = await _CustomerInfoByGroupEntityService.GetSingleBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(customer.Oid, fbo.GroupId));

                        if (customerGroup != null && customerGroup.Oid > 0)
                        {
                            //var debugging = await GetMissedQuotesDebuggingInformation(customer, fbo, validPricing);

                            var missedQuoteLog = await GetRecentMissedQuotes(fbo.Oid);
                            var recentMissedQuote = missedQuoteLog.Where(m => m.Emailed.HasValue && m.Emailed.Value == true).ToList();
                            var isEmailed = false;

                            if (recentMissedQuote.Count == 0)
                            {
                                try
                                {
                                    if (!_FuelerlinxSdkSettings.APIEndpoint.Contains("-"))
                                    {
                                        var toEmails = await _FboService.GetToEmailsForEngagementEmails(fbo.Oid);

                                        if (toEmails.Count > 0)
                                            await _FboService.NotifyFboNoPrices(toEmails, fbo.Fbo, customer.Company);

                                        isEmailed = true;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            var missedQuote = new MissedQuoteLogDTO();
                            missedQuote.CreatedDate = DateTime.UtcNow;
                            missedQuote.FboId = fbo.Oid;
                            missedQuote.CustomerId = customer.Oid;
                            missedQuote.Emailed = isEmailed;
                            //missedQuote.Debugs = debugging;
                            missedQuotesToLog.Add(missedQuote);
                        }
                        await SaveMissedQuotes(missedQuotesToLog);
                    }
                }
            }
        }

        private async Task SaveMissedQuotes(List<MissedQuoteLogDTO> missedQuoteLogs)
        {
            if (missedQuoteLogs?.Count == 0)
                return;

            using (var scope = _ScopeFactory.CreateScope())
            {
                var missedQuoteLogService = scope.ServiceProvider.GetRequiredService<IMissedQuoteLogService>();
                foreach (var missedQuoteLog in missedQuoteLogs)
                {
                    await missedQuoteLogService.AddAsync(missedQuoteLog);
                }
            }
        }

        private async Task<List<MissedQuoteLogDTO>> GetRecentMissedQuotes(int fboId, bool isGridView = false)
        {
            var daysBefore = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0));
            var recentMissedQuotes = await GetListbySpec(new MissedQuoteLogSpecification(fboId, daysBefore));
            recentMissedQuotes = recentMissedQuotes.OrderByDescending(r => r.CreatedDate).ToList();
            var localTimeZone = await _FboService.GetAirportTimeZoneByFboId(fboId);

            foreach (MissedQuoteLogDTO missedQuoteLog in recentMissedQuotes)
            {
                var localDateTime = DateTime.UtcNow;

                if (isGridView)
                {
                    localDateTime = await _FboService.GetAirportLocalDateTimeByUtcFboId(missedQuoteLog.CreatedDate.Value, fboId);
                    missedQuoteLog.CreatedDateString = localDateTime + " " + localTimeZone;
                }
            }

            return recentMissedQuotes;
        }
    }
}
