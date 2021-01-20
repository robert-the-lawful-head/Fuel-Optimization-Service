using CsvHelper;
using FBOLinx.DB.Models;
using FBOLinx.Job.Base;
using FBOLinx.Job.Types;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace FBOLinx.Job.Jobs
{
    public class AirportWatchJobRunner : IJobRunner
    {
        private string _lastWatchedFile = "";
        private int _lastWatchedFileRecordIndex = 0;
        private readonly IConfiguration _config;
        private readonly ApiClient _apiClient;

        public AirportWatchJobRunner(IConfiguration config)
        {
            _config = config;
            _apiClient = new ApiClient(config["FBOLinxApiUrl"]);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = _config["AirportWatchDataLocation"];

                // Watch for changes in File Size
                watcher.NotifyFilter = NotifyFilters.Size;

                // Only watch text files.
                watcher.Filter = "*.csv";

                // Add event handlers.
                watcher.Changed += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the job.");
                while (Console.Read() != 'q') ;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e) {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.Name} {e.ChangeType}");
            if (_lastWatchedFile != e.Name)
            {
                _lastWatchedFileRecordIndex = 0;
            }

            var data = new List<AirportWatchDataType>();

            while (true)
            {
                try
                {
                    using var reader = new StreamReader(e.FullPath);
                    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                    csv.Configuration.HasHeaderRecord = false;
                    int rowIndex = 0;

                    while (csv.Read())
                    {
                        if (rowIndex < _lastWatchedFileRecordIndex)
                        {
                            rowIndex++;
                        }
                        else
                        {
                            try
                            {
                                var row = csv.GetRecord<AirportWatchDataType>();
                                if (!string.IsNullOrEmpty(row.AircraftHexCode) && !string.IsNullOrEmpty(row.AtcFlightNumber))
                                {
                                    data.Add(row);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }

                            rowIndex++;
                            _lastWatchedFileRecordIndex++;
                        }
                    }
                    _lastWatchedFile = e.Name;
                    break;
                }
                catch {}
            }

            var airportWatchData = data
                .Select(row => new AirportWatchLiveData
                {
                    BoxTransmissionDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(row.BoxTransmissionDateTimeUtc).DateTime,
                    AircraftHexCode = row.AircraftHexCode,
                    AtcFlightNumber = row.AtcFlightNumber,
                    AltitudeInStandardPressure = row.AltitudeInStandardPressure,
                    GroundSpeedKts = row.GroundSpeedKts,
                    TrackingDegree = row.TrackingDegree,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    VerticalSpeedKts = row.VerticalSpeedKts,
                    TransponderCode = row.TransponderCode,
                    BoxName = row.BoxName,
                    AircraftPositionDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(row.AircraftPositionDateTimeUtc).DateTime,
                    AircraftTypeCode = row.AircraftTypeCode,
                    GpsAltitude = row.GpsAltitude,
                    IsAircraftOnGround = row.IsAircraftOnGround,
                })
                .OrderByDescending(row => row.BoxTransmissionDateTimeUtc)
                .GroupBy(row => new { row.AircraftHexCode, row.AtcFlightNumber })
                .Select(grouped => grouped.First())
                .ToList();

             
            if (airportWatchData.Count > 0)
            {
                try
                {
                    _apiClient.PostAsync("airportwatch/list", airportWatchData).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
