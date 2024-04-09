using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using Microsoft.Extensions.Hosting;

namespace FBOLinx.ServiceLayer.ScheduledService
{
    public class AirportWatchLiveDataReprocessScheduledService : IHostedService
    {
        private IServiceScopeFactory _ScopeFactory;

        public AirportWatchLiveDataReprocessScheduledService(IServiceScopeFactory scopeFactory)
        {
            _ScopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            TimeSpan interval = TimeSpan.FromSeconds(10);
            var t1 = Task.Delay(interval);
            t1.Wait();
            Task.Run(RunTest);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async void RunTest()
        {
            using (var scope = _ScopeFactory.CreateScope())
            {
                var airportWatchService = scope.ServiceProvider.GetRequiredService<AirportWatchService>();
                await airportWatchService.ReProcessLatestRecords();
            }
        }
    }
}
