using FBOLinx.DB.Models;
using FBOLinx.Job.Base;
using FBOLinx.Job.Types;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using TinyCsvParser;

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

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            using var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(_config["AirportWatchJobLog"])
                .CreateLogger();

            logger.Information($"File: {e.FullPath} {e.Name} {e.ChangeType}");

            List<AirportWatchDataType> data = GetCSVRecords(e.FullPath, e.Name);
            List<AirportWatchLiveData> airportWatchData = ConvertToDBModel(data);

            if (airportWatchData.Count > 0)
            {
                try
                {
                    _apiClient.PostAsync("airportwatch/list", airportWatchData).Wait();
                    logger.Information("Fbolinx api call succeed!");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Failed to call Fbolinx api!");
                }
            }
        }
    
        private List<AirportWatchDataType> GetCSVRecords(string filePath, string fileName)
        {
            if (_lastWatchedFile != fileName)
            {
                _lastWatchedFileRecordIndex = 0;
            }

            var data = new List<AirportWatchDataType>();

            while (true)
            {
                try
                {
                    CsvParserOptions csvParserOptions = new CsvParserOptions(false, ',');
                    CsvAirportWatchDataTypeMapping csvMapper = new CsvAirportWatchDataTypeMapping();
                    CsvParser<AirportWatchDataType> csvParser = new CsvParser<AirportWatchDataType>(csvParserOptions, csvMapper);

                    var records = csvParser
                        .ReadFromFile(filePath, Encoding.ASCII);

                    var recordsCount = records.Count();

                    data = records.Select(r => r.Result).Skip(_lastWatchedFileRecordIndex).ToList();

                    _lastWatchedFileRecordIndex = recordsCount;
                    _lastWatchedFile = fileName;
                    break;
                }
                catch { }
            }

            return data;
        }
    
        private List<AirportWatchLiveData> ConvertToDBModel(List<AirportWatchDataType> data)
        {
            List<AirportWatchLiveData> airportWatchData = new List<AirportWatchLiveData>();

            foreach (var record in data)
            {
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
                .OrderByDescending(row => row.BoxTransmissionDateTimeUtc)
                .GroupBy(row => new { row.AircraftHexCode, row.AtcFlightNumber })
                .Select(grouped => grouped.First())
                .ToList();
        }
    }
}
