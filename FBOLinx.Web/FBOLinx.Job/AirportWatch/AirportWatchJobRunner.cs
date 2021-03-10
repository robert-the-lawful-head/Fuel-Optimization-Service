using FBOLinx.DB.Models;
using FBOLinx.Job.Base;
using FBOLinx.Job.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FBOLinx.Job.AirportWatch
{
    public class AirportWatchJobRunner : IJobRunner
    {
        private string _lastWatchedFile = "";
        private int _lastWatchedFileRecordIndex = 0;
        private readonly IConfiguration _config;
        private readonly ApiClient _apiClient;
        private bool _isPostingData = false;
        private DateTime? _LastPostDateTimeUTC;

        public AirportWatchJobRunner(IConfiguration config)
        {
            _config = config;
            _apiClient = new ApiClient(config["FBOLinxApiUrl"]);
        }

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

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //Don't push more often than every 9 seconds to prevent consistent spikes
            if (_LastPostDateTimeUTC.HasValue && DateTime.UtcNow < _LastPostDateTimeUTC.GetValueOrDefault().AddSeconds(9))
                return;
            
            using var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(_config["AirportWatchJobLog"])
                .CreateLogger();

            logger.Information($"File: {e.FullPath} {e.Name} {e.ChangeType}");

            if (_isPostingData)
            {
                logger.Information("Fbolinx api call delayed - previous POST still in progress.");
                return;
            }

            _isPostingData = true;
            List<AirportWatchDataType> data = GetCSVRecords(e.FullPath, e.Name);
            List<AirportWatchLiveData> airportWatchData = ConvertToDBModel(data);

            if (airportWatchData.Count > 0)
            {
                try
                {
                    _apiClient.PostAsync("airportwatch/list", airportWatchData).Wait();
                    logger.Information("Fbolinx api call succeed!");
                    _LastPostDateTimeUTC = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Failed to call Fbolinx api!");
                }
            }

            _isPostingData = false;
        }
    
        private List<AirportWatchDataType> GetCSVRecords(string filePath, string fileName)
        {
            if (_lastWatchedFile != fileName)
            {
                _lastWatchedFileRecordIndex = 0;
            }

            var data = new List<AirportWatchDataType>();

            try
            {
                AirportWatchCsvParser csvParser = new AirportWatchCsvParser(filePath);

                data = csvParser.GetRecords(_lastWatchedFileRecordIndex);

                _lastWatchedFileRecordIndex += data.Count;
                _lastWatchedFile = fileName;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    
        private List<AirportWatchLiveData> ConvertToDBModel(List<AirportWatchDataType> data)
        {
            List<AirportWatchLiveData> airportWatchData = new List<AirportWatchLiveData>();

            foreach (var record in data)
            {
                if (record == null)
                {
                    continue;
                }

                var airportWatchRow = new AirportWatchLiveData { };

                if (string.IsNullOrEmpty(record.AircraftHexCode) ||
                    string.IsNullOrEmpty(record.AtcFlightNumber))
                {
                    continue;
                }
                airportWatchRow.AircraftHexCode = record.AircraftHexCode;
                airportWatchRow.AtcFlightNumber = record.AtcFlightNumber;
                airportWatchRow.BoxName = record.BoxName;
                airportWatchRow.AircraftTypeCode = record.AircraftTypeCode;

                if (!int.TryParse(record.BoxTransmissionDateTimeUtc, out int BoxTransmissionDateTimeUtc))
                {
                    continue;
                }
                airportWatchRow.BoxTransmissionDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(BoxTransmissionDateTimeUtc).DateTime;

                if (!int.TryParse(record.AircraftPositionDateTimeUtc, out int AircraftPositionDateTimeUtc))
                {
                    continue;
                }
                airportWatchRow.AircraftPositionDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(AircraftPositionDateTimeUtc).DateTime;

                if (!string.IsNullOrEmpty(record.AltitudeInStandardPressure))
                {
                    if (!int.TryParse(record.AltitudeInStandardPressure, out int AltitudeInStandardPressure))
                    {
                        continue;
                    }
                    else
                    {
                        airportWatchRow.AltitudeInStandardPressure = AltitudeInStandardPressure;
                    }
                }

                if (!string.IsNullOrEmpty(record.GroundSpeedKts))
                {
                    if (!int.TryParse(record.GroundSpeedKts, out int GroundSpeedKts))
                    {
                        continue;
                    }
                    else
                    {
                        airportWatchRow.GroundSpeedKts = GroundSpeedKts;
                    }
                }

                if (!string.IsNullOrEmpty(record.TrackingDegree))
                {
                    if (!double.TryParse(record.TrackingDegree, out double TrackingDegree))
                    {
                        continue;
                    }
                    else
                    {
                        airportWatchRow.TrackingDegree = TrackingDegree;
                    }
                }

                if (string.IsNullOrEmpty(record.Latitude) || !double.TryParse(record.Latitude, out double Latitude))
                {
                    continue;
                }
                airportWatchRow.Latitude = Latitude;

                if (string.IsNullOrEmpty(record.Longitude) || !double.TryParse(record.Longitude, out double Longitude))
                {
                    continue;
                }
                airportWatchRow.Longitude = Longitude;

                if (!string.IsNullOrEmpty(record.VerticalSpeedKts))
                {
                    if (!int.TryParse(record.VerticalSpeedKts, out int VerticalSpeedKts))
                    {
                        continue;
                    }
                    else
                    {
                        airportWatchRow.VerticalSpeedKts = VerticalSpeedKts;
                    }
                }

                if (!string.IsNullOrEmpty(record.TransponderCode))
                {
                    if (!int.TryParse(record.TransponderCode, out int TransponderCode))
                    {
                        continue;
                    }
                    else
                    {
                        airportWatchRow.TransponderCode = TransponderCode;
                    }
                }

                if (!string.IsNullOrEmpty(record.GpsAltitude))
                {
                    if (!int.TryParse(record.GpsAltitude, out int GpsAltitude))
                    {
                        continue;
                    }
                    else
                    {
                        airportWatchRow.GpsAltitude = GpsAltitude;
                    }
                }

                if (record.IsAircraftOnGround != "0" && record.IsAircraftOnGround != "1")
                {
                    continue;
                }
                airportWatchRow.IsAircraftOnGround = record.IsAircraftOnGround == "1" ? true : false;

                airportWatchData.Add(airportWatchRow);
            }

            return airportWatchData
                .OrderByDescending(row => row.AircraftPositionDateTimeUtc)
                .GroupBy(row => new { row.AircraftHexCode, row.AtcFlightNumber })
                .Select(grouped => grouped.First())
                .ToList();
        }
    }
}
