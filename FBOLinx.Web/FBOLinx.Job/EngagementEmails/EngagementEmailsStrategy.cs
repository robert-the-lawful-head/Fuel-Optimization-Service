using FBOLinx.Job.Base;
using FBOLinx.Job.Interfaces;
using Microsoft.Extensions.Configuration;
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
            var responseUser = await _apiClient.PostAsync("users/authenticate", new LandingSiteLoginRequest { Username = "consolejobs@fuelerlinx.com", Password = "Filus123" });
            var conductorUser = Newtonsoft.Json.JsonConvert.DeserializeObject<DB.Models.User>(responseUser.Content.ReadAsStringAsync().Result);

            //run job
            _apiClient.GetAsync("fbos/sendengagementemails", conductorUser.Token);
        }
    }

    public partial class LandingSiteLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
