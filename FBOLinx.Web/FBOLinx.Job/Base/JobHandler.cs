using FBOLinx.Job.AirportWatch;
using FBOLinx.Job.EngagementEmails;
using FBOLinx.Job.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FBOLinx.Job.Base
{
    public class JobHandler
    {
        private readonly string _AirportWatchJobCmd = "-a";
        private readonly string _EngagementEmailsJobCmd = "-e";
        private IJobRunner _JobRunner;
        private IConfiguration _config;

        public JobHandler(IConfiguration config)
        {
            _config = config;
        }

        public async Task RunJob(string jobCommand)
        {
            if (jobCommand == _AirportWatchJobCmd)
            {
                _JobRunner = new AirportWatchJobRunner(_config);
            }
            else if(jobCommand== _EngagementEmailsJobCmd)
            {
                _JobRunner = new EngagementEmailsStrategy(_config);
            }

            await _JobRunner.Run();
        }
    }
}
