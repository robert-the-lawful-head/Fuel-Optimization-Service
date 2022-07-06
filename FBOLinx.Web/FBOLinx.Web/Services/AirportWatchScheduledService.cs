using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBOLinx.Web.Services
{
    public class AirportWatchScheduledService : IHostedService
    {
        private Timer _Timer;
        private IServiceScopeFactory _ScopeFactory;
        private readonly AirportWatchService _AirportWatchService;
        private bool _IsDebugging = false;

        public AirportWatchScheduledService(IServiceScopeFactory scopeFactory)
        {
            _ScopeFactory = scopeFactory;
            CheckForDebugMode();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_IsDebugging)
            {
                TimeSpan interval = TimeSpan.FromMinutes(1);

                Task.Run(() =>
                {
                    var t1 = Task.Delay(interval);
                    t1.Wait();
                    _Timer = new Timer(GetAirportWatchLiveTrips, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
                });
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void GetAirportWatchLiveTrips(object state)
        {
            CheckForDebugMode();
            if (_IsDebugging)
            {
                using (var scope = _ScopeFactory.CreateScope())
                {
                    var airportWatchService = scope.ServiceProvider.GetRequiredService<AirportWatchService>();
                    await airportWatchService.GetAirportWatchTestData();
                }
            }
        }

        [Conditional("DEBUG")]
        private void CheckForDebugMode()
        {
            _IsDebugging = true;
        }
    }
}
