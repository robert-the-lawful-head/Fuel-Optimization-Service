using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Api;
using FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Client;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.JetNet;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations.AppPartnerSDKSettings;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet
{
    public interface IJetNetService
    {
        Task<JetNetDto> GetJetNetInformation(string tailNumber);
    }

    public class JetNetService : IJetNetService
    {
        private AppPartnerSDKSettings.JetNetAPISettings _jetNetApiSettings;
        private IMemoryCache _MemoryCache;
        private IAircraftService _AircraftService;

        public JetNetService(IOptions<AppPartnerSDKSettings> appPartnerSDKSettings, IMemoryCache memoryCache, IAircraftService aircraftService)
        {
            _jetNetApiSettings = appPartnerSDKSettings?.Value?.JetNet;
            _MemoryCache = memoryCache;
            _AircraftService = aircraftService;
        }

        public async Task<JetNetDto> GetJetNetInformation(string tailNumber)
        {
            var client = GetJetNetApiClient();
            var api = new JetNetApi(client);

            //var token = GetAuthorizationTokenFromCache(); // Doesn't look like the token expiration lasts 120 minutes
            //if (token == null || string.IsNullOrEmpty(token.bearerToken))
            //{
                var jetNetUser = new JetNetUser() { emailaddress = _jetNetApiSettings.Username, password = _jetNetApiSettings.Password };
                var token = await api.GetJetNetToken(jetNetUser);
            //var cacheEntryOptions =
            //    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(100));
            //if (token != null && !string.IsNullOrEmpty(token.bearerToken))
            //    _MemoryCache.Set("JetNet", token, cacheEntryOptions);
            //}

            try
            {
                var jetNetInformation = await api.GetJetNetData(tailNumber, token);

                if (jetNetInformation.aircraftresult != null)
                {
                    var aircrafts = _AircraftService.GetAllAircrafts(true);
                    var aircraft = aircrafts.Result.FirstOrDefault(x => x.Make == jetNetInformation.aircraftresult.make && x.Model == jetNetInformation.aircraftresult.model);
                    if (aircraft == null)
                    {
                        var fuelerlinxAircraft = await _AircraftService.GetAircraftTypeByIcao(jetNetInformation.aircraftresult.icaotype);
                        if (fuelerlinxAircraft != null)
                        {
                            jetNetInformation.aircraftresult.make = fuelerlinxAircraft.Make;
                            jetNetInformation.aircraftresult.model = fuelerlinxAircraft.Model;
                        }
                    }

                    jetNetInformation.aircraftresult.companies = jetNetInformation.aircraftresult.companyrelationships.GroupBy(x => x.companyname).Select(x => new Company
                    {
                        company = x.Key,
                        companyrelationships = new List<CompanyRelationship> { new CompanyRelationship
                        {
                            companyname = x.Key,
                            companyaddress1 = x.Select(c => c.companyaddress1).First(),
                            companyaddress2 = x.Select(c => c.companyaddress2).First(),
                            companycity = x.Select(c => c.companycity).First(),
                            companycountry = x.Select(c => c.companycountry).First(),
                            companypostcode = x.Select(c => c.companypostcode).First(),
                            companystateabbr = x.Select(c => c.companystateabbr).First(),
                            contactfirstname = x.Select(c => c.contactfirstname).First(),
                            contactlastname = x.Select(c => c.contactlastname).First(),
                            contacttitle = x.Select(c => c.contacttitle).First(),
                            contactbestphone = x.Select(c => c.contactbestphone).First(),
                            contactemail = x.Select(c => c.contactemail).First(),
                            contactid = x.Select(c=>c.contactid).First(),
                            companyrelation=x.Select(c=>c.companyrelation).First(),
                        }
                    }
                    }).ToList();

                    foreach (var companyContact in jetNetInformation.aircraftresult.companies)
                    {
                        var companyRelationshipsNotInList = (from cr in jetNetInformation.aircraftresult.companyrelationships
                                                             join c in companyContact.companyrelationships on new { cr.contactid, cr.companyrelation }  equals new { c.contactid, c.companyrelation}
                                                             into leftJoinedC
                                                             from c in leftJoinedC.DefaultIfEmpty()
                                                             where cr.companyname == companyContact.company && c == null
                                                             select cr).ToList();
                        companyContact.companyrelationships.AddRange(companyRelationshipsNotInList);
                    }
                    return jetNetInformation;
                }
                return new JetNetDto();
            }
            catch(Exception ex)
            {
                return new JetNetDto();
            }
        }

        private JetNetApiClient GetJetNetApiClient()
        {
            var client =
                new JetNetApiClient();
            return client;
        }

        private JetNetApiToken GetAuthorizationTokenFromCache()
        {
            try
            {
                JetNetApiToken result = null;
                if (_MemoryCache.TryGetValue("JetNet", out result) && result != null)
                    return result;

                return null;
            }
            catch (System.Exception exception)
            {
                return null;
            }
        }

    }
}
