using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Specifications;
using FBOLinx.DB.Specifications.CustomerInfoByGroup;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.FuelPrices;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog
{
    public class MissedQuoteLogService : BaseDTOService<MissedQuoteLogDto, DB.Models.MissedQuoteLog, FboLinxContext>
    {
        private IFboService _iFboService;
        private readonly CustomerInfoByGroupEntityService _customerInfoByGroupEntityService;
        private AppPartnerSDKSettings.FuelerlinxSDKSettings _fuelerlinxSdkSettings;
        private IServiceScopeFactory _ScopeFactory;

        public MissedQuoteLogService(MissedQuoteLogEntityService entityService, IFboService iFboService, IOptions<AppPartnerSDKSettings> appPartnerSDKSettings, CustomerInfoByGroupEntityService customerInfoByGroupEntityService,
            IServiceScopeFactory scopeFactory) : base(entityService)
        {
            _iFboService = iFboService;
            _customerInfoByGroupEntityService = customerInfoByGroupEntityService;
            _fuelerlinxSdkSettings = appPartnerSDKSettings.Value.FuelerLinx;
            _ScopeFactory = scopeFactory;
        }

        public async Task<List<MissedQuoteLogDto>> GetRecentMissedQuotes(int fboId, bool isGridView = false)
        {
            var daysBefore = DateTime.UtcNow.Add(new TimeSpan(-3, 0, 0, 0));
            var recentMissedQuotes = await _EntityService.GetListBySpec(new MissedQuoteLogSpecification(fboId, daysBefore));

            var recentMissedQuotedList = new List<MissedQuoteLogDto>();
            var localTimeZone = "";
            localTimeZone = await _iFboService.GetAirportTimeZoneByFboId(fboId);

            foreach (DB.Models.MissedQuoteLog missedQuoteLog in recentMissedQuotes)
            {
                var localDateTime = DateTime.UtcNow;

                if (isGridView)
                {
                    localDateTime = await _iFboService.GetAirportLocalDateTimeByUtcFboId(missedQuoteLog.CreatedDate.GetValueOrDefault(), fboId);
                }

                var missedQuoteLogDto = new MissedQuoteLogDto();
                missedQuoteLogDto.CreatedDateString = localDateTime.ToString("MM/dd/yyyy, HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
                missedQuoteLogDto.CustomerId = missedQuoteLog.CustomerId.GetValueOrDefault();
                missedQuoteLogDto.Emailed = missedQuoteLog.Emailed.GetValueOrDefault();
                missedQuoteLogDto.FboId = missedQuoteLog.FboId;
                missedQuoteLogDto.Oid = missedQuoteLog.Oid;
                recentMissedQuotedList.Add(missedQuoteLogDto);
            }

            return recentMissedQuotedList;
        }

        public async Task LogMissedQuote(string icaos, List<FuelPriceResponse> result, DB.Models.Customers customer)
        {
            foreach (var icao in icaos.Split(',').Select(x => x.Trim()))
            {
                var fbos = await _iFboService.GetFbosByIcaos(icao);

                foreach (var fbo in fbos)
                {
                    if (!result.Any(r => r.Icao == icao && r.Fbo == fbo.Fbo))
                    {
                        List<MissedQuoteLogDto> missedQuotesToLog = new List<MissedQuoteLogDto>();
                        var customerGroup = await _customerInfoByGroupEntityService.GetSingleBySpec(new CustomerInfoByGroupCustomerIdGroupIdSpecification(customer.Oid, fbo.GroupId.GetValueOrDefault()));

                        if (customerGroup != null && customerGroup.Oid > 0)
                        {
                            //var debugging = await GetMissedQuotesDebuggingInformation(customer, fbo, validPricing);

                            var missedQuoteLog = await GetRecentMissedQuotes(fbo.Oid);
                            var recentMissedQuote = missedQuoteLog.Where(m => m.Emailed.GetValueOrDefault() == true).ToList();
                            var isEmailed = false;

                            if (recentMissedQuote.Count == 0)
                            {
                                try
                                {
                                    if (!_fuelerlinxSdkSettings.APIEndpoint.Contains("-"))
                                    {
                                        var toEmails = await _iFboService.GetToEmailsForEngagementEmails(fbo.Oid);

                                        if (toEmails.Count > 0)
                                            await _iFboService.NotifyFboNoPrices(toEmails, fbo.Fbo, customer.Company);

                                        isEmailed = true;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            var missedQuote = new MissedQuoteLogDto();
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

        private async Task SaveMissedQuotes(List<MissedQuoteLogDto> missedQuoteLogs)
        {
            if (missedQuoteLogs?.Count == 0)
                return;

            using (var scope = _ScopeFactory.CreateScope())
            {
                var missedQuoteLogService = scope.ServiceProvider.GetRequiredService<MissedQuoteLogService>();
                foreach (var missedQuoteLog in missedQuoteLogs)
                {
                    await missedQuoteLogService.AddAsync(missedQuoteLog);
                }
            }
        }
    }
}
