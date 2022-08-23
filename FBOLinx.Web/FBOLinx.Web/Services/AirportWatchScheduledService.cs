using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;

namespace FBOLinx.Web.Services
{
    public class AirportWatchScheduledService : IHostedService
    {
        private Timer _Timer;
        private IServiceScopeFactory _ScopeFactory;
        private readonly AirportWatchService _AirportWatchService;
        private bool _IsDebugging = false;
        private bool _IsCurrentlyRunning = false;

        public AirportWatchScheduledService(IServiceScopeFactory scopeFactory)
        {
            _ScopeFactory = scopeFactory;
            CheckForDebugMode();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_IsDebugging)
            {
                Task.Run(() =>
                {
                    _Timer = new Timer(GetAirportWatchLiveTrips, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
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
            return;
            //From Mike: something looks to be wrong with this and is causing crashes on the application.
            //Preventing this from running as of 7/26/2022.

            CheckForDebugMode();
            if (_IsDebugging && !_IsCurrentlyRunning)
            {
                using (var scope = _ScopeFactory.CreateScope())
                {
                    _IsCurrentlyRunning = true;
                    var airportWatchService = scope.ServiceProvider.GetRequiredService<AirportWatchService>();
                    await airportWatchService.GetAirportWatchTestData();
                    _IsCurrentlyRunning = false;
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
