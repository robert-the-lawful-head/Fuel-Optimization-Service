using FBOLinx.DB.Models;
using FBOLinx.Job.Base;
using FBOLinx.Job.Interfaces;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services;
using FBOLinx.Web.ViewModels;
using IO.Swagger.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Job.EngagementEmails
{
    public class EngagementEmailsStrategy : IJobRunner
    {
        private readonly IConfiguration _config;
        private readonly ApiClient _apiClient;

        public EngagementEmailsStrategy(IConfiguration config)
        {
            _config = config;
            _apiClient = new ApiClient(config["FBOLinxApiUrl"]);
        }

        public async Task Run()
        {
            //login as a conductor user
            LandingSiteLoginRequest landingSiteLoginRequest = new LandingSiteLoginRequest();
            landingSiteLoginRequest.Username = "consolejobs@fuelerlinx.com";
            landingSiteLoginRequest.Password = "Filus123";

            var responseUser = await _apiClient.PostAsync("users/authenticate", landingSiteLoginRequest);
            var conductorUser = Newtonsoft.Json.JsonConvert.DeserializeObject<DB.Models.User>(responseUser);

            //get all active fbos with expiredpricing
            var responseFbos = await _apiClient.GetAsync("fboprices/getallfboswithexpiredretailpricing", conductorUser.Token);
            var fbos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FbosGridViewModel>>(responseFbos);

            foreach (var fbo in fbos)
            {
                var responseAcukwikAirport = await _apiClient.GetAsync("acukwikairports/byicao/" + fbo.Icao, conductorUser.Token);
                var acukwikAirport = Newtonsoft.Json.JsonConvert.DeserializeObject<AcukwikAirports>(responseAcukwikAirport);

                //send email if it's 9am local airport time
                var airportDateTime = DateTime.UtcNow.AddHours(acukwikAirport.IntlTimeZone.GetValueOrDefault());
                if (acukwikAirport.DaylightSavingsYn == "Y")
                    airportDateTime = airportDateTime.AddHours(1);

                if (airportDateTime.Hour == 9)
                {
                    List<string> toEmails = new List<string>();

                    var responseFbo = await _apiClient.GetAsync("fbos/" + fbo.Oid, conductorUser.Token);
                    var fboInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Fbos>(responseFbo);

                    if (fboInfo.FuelDeskEmail != "")
                        toEmails.Add(fboInfo.FuelDeskEmail);

                    var responseFboContacts = await _apiClient.GetAsync("fbocontacts/fbo/" + fbo.Oid, conductorUser.Token);
                    var fboContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FboContactsViewModel>>(responseFboContacts);

                    foreach (FboContactsViewModel fboContact in fboContacts)
                    {
                        if (fboContact.CopyAlerts.GetValueOrDefault())
                            toEmails.Add(fboContact.Email);
                    }

                    if (toEmails.Count > 0)
                        await GenerateExpiredPricesEmail(toEmails, fbo.Fbo, conductorUser.Token);
                }
            }

            //Ramp fee emails, only goes out at 9am Pacific
            await CheckRampFees(conductorUser);
        }

        private async Task CheckRampFees(User conductorUser)
        {
            var responseKVNYAirport = await _apiClient.GetAsync("acukwikairports/byicao/KVNY", conductorUser.Token);
            var KVNYAirport = Newtonsoft.Json.JsonConvert.DeserializeObject<AcukwikAirports>(responseKVNYAirport);
            var KVNYDateTime = DateTime.UtcNow.AddHours(KVNYAirport.IntlTimeZone.GetValueOrDefault());
            if (KVNYAirport.DaylightSavingsYn == "Y")
                KVNYDateTime = KVNYDateTime.AddHours(1);

            if (KVNYDateTime.Hour == 9)
            {
                //go through each active fbo icao and call fuelerlinx to pull the latest pullhistory for that icao within the past 24 hours
                var responseActiveFbos = await _apiClient.GetAsync("fbos", conductorUser.Token);
                var activeFbos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FbosGridViewModel>>(responseActiveFbos);

                foreach (var fbo in activeFbos)
                {
                    if (fbo.GroupId > 1)
                    {
                        //check if there's ramp fees
                        var rampFeesResponse = await _apiClient.GetAsync("rampfees/fbo/" + fbo.Oid, conductorUser.Token);
                        var rampFees = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RampFees>>(rampFeesResponse);
                        var noRampFees = true;

                        foreach (RampFees rampFee in rampFees)
                        {
                            if (rampFee.Price > 0)
                            {
                                noRampFees = false;
                                break;
                            }
                        }

                        //if not, find the latest pull history record from fuelerlinx and send email
                        if (noRampFees)
                        {
                            var fuelerLinxCustomerIdResponse = await _apiClient.PostAsync("fboprices/get-latest-flight-dept-pullhistory-for-icao/", new FBOLinxGetLatestFlightDeptPullHistoryByIcaoRequest() { Icao = fbo.Icao }, conductorUser.Token);
                            var fuelerLinxCustomerId = 0;
                            if (fuelerLinxCustomerIdResponse != "" && int.TryParse(fuelerLinxCustomerIdResponse, out fuelerLinxCustomerId))
                                fuelerLinxCustomerId = int.Parse(fuelerLinxCustomerIdResponse);

                            if (fuelerLinxCustomerId > 0)
                            {
                                var customerResponse = await _apiClient.GetAsync("customers/getbyfuelerlinxid/" + fuelerLinxCustomerId, conductorUser.Token);
                                var customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customers>(customerResponse);

                                List<string> toEmails = new List<string>();

                                var responseFbo = await _apiClient.GetAsync("fbos/" + fbo.Oid, conductorUser.Token);
                                var fboInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Fbos>(responseFbo);

                                if (fboInfo.FuelDeskEmail != "")
                                    toEmails.Add(fboInfo.FuelDeskEmail);

                                var responseFboContacts = await _apiClient.GetAsync("fbocontacts/fbo/" + fbo.Oid, conductorUser.Token);
                                var fboContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FboContactsViewModel>>(responseFboContacts);

                                foreach (FboContactsViewModel fboContact in fboContacts)
                                {
                                    if (fboContact.CopyAlerts.GetValueOrDefault())
                                        toEmails.Add(fboContact.Email);
                                }

                                if (toEmails.Count > 0)
                                    await GenerateNoRampFeesEmail(toEmails, fbo.Fbo, customer.Company, fbo.Icao, conductorUser.Token);
                            }
                        }
                    }
                }
            }
        }

        private async Task GenerateExpiredPricesEmail(List<string> toEmails, string fboName, string token)
        {
            var requestObject = new NotifyFboNoRampFeesRequest();
            requestObject.ToEmails = toEmails;
            requestObject.FBO = fboName;
            var result = await _apiClient.PostAsync("fboprices/notify-fbo-expired-prices", requestObject, token);
        }
        private async Task GenerateNoRampFeesEmail(List<string> toEmails, string fboName, string flightDepartment, string icao, string token)
        {
            var requestObject = new NotifyFboNoRampFeesRequest();
            requestObject.ToEmails = toEmails;
            requestObject.FBO = fboName;
            requestObject.CustomerName = flightDepartment;
            requestObject.ICAO = icao;
            var result = await _apiClient.PostAsync("rampfees/notify-fbo-no-rampfees", requestObject, token);
        }
    }
}
