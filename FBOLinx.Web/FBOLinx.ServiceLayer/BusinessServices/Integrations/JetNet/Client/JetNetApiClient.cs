using FBOLinx.Core.BaseModels.Api;

namespace FBOLinx.ServiceLayer.BusinessServices.Integrations.JetNet.Client
{
    public class JetNetApiClient : BaseApiClient, IApiClient
    {

        public JetNetApiClient(string basePath = "https://customer.jetnetconnect.com/api/") : base(basePath)
        {

        }
    }
}
