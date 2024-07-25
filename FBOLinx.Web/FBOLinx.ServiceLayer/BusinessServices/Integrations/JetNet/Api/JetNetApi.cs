using Azure.Core;
using FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Client;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.JetNet;
using Fuelerlinx.SDK;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Api
{
    public interface IJetNetApi
    {
        Task<JetNetApiToken> GetJetNetToken(JetNetUser request);
        Task<JetNetDto> GetJetNetData(string tailNumber, JetNetApiToken token);
    }

    public class JetNetApi : Core.BaseModels.Api.BaseApi, IJetNetApi
    {
        public JetNetApi(string basePath)
        {
            ApiClient = new JetNetApiClient(basePath);
        }

        public JetNetApi(JetNetApiClient client)
        {
            ApiClient = client;
        }

        public async Task<JetNetApiToken> GetJetNetToken(JetNetUser request)
        {
            var path = "/Admin/APILogin";
            var body = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            return await CallApiPOSTAsync<JetNetApiToken>(path, body);
        }

        public async Task<JetNetDto> GetJetNetData(string tailNumber, JetNetApiToken token)
        {
            var path = "/CustomEndpoints/getFuelerLinxExport/" + tailNumber + "/" + token.apiToken.HtmlEncode();
            var headers = new Dictionary<string, string> { { "Authorization", "Bearer " + token.bearerToken.ToString() } };

            return await CallApiGETAsync<JetNetDto>(path, headers);
        }
    }
}
