using FBOLinx.Job.Jobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.Job.Base
{
    public class JobHandler
    {
        private readonly string _AirportWatchJobCmd = "-a";
        private IJobRunner _JobRunner;
        private IConfiguration _config;

        public JobHandler(IConfiguration config)
        {
            _config = config;
        }

        public void RunJob(string jobCommand)
        {
            if (jobCommand == _AirportWatchJobCmd)
            {
                _JobRunner = new AirportWatchJobRunner(_config);
            }

            _JobRunner.Run();
        }
    }
}
