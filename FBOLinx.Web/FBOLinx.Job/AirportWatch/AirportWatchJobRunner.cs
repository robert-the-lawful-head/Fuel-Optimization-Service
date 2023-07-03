using FBOLinx.DB.Models;
using FBOLinx.Job.Base;
using FBOLinx.Job.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Job.AirportWatch
{
    public class AirportWatchJobRunner : IJobRunner
    {
        private bool _isNewInstance  = true;
        private string _lastWatchedFile = "";
        private int _lastWatchedFileRecordIndex = 0;
        private long _lastWatchedFileNumberOfBytesRead = 0;
        private readonly IConfiguration _config;
        private bool _isPostingData = false;
        private DateTime? _LastPostDateTimeUTC;
        private List<string> _apiClientUrls;
        private int _NewRowThreshold = 30000;
        private Serilog.Core.Logger logger;
        private AirportWatchCsvParser _CsvParser;

        public AirportWatchJobRunner(IConfiguration config)
        {
            _config = config;
            _apiClientUrls = config["FBOLinxApiUrls"].ToString().Split(";").ToList();
            logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .WriteTo.File(_config["AirportWatchJobLog"])
                                .CreateLogger();
        }

        public async Task Run()
        {
            if (_isNewInstance)
            {
                logger.Information($"Job has been deployed so new instance has been created variables current values are  _isNewInstance:{_isNewInstance}, _lastWatchedFile: {_lastWatchedFile}, {_lastWatchedFileRecordIndex}, _isPostingData:{_isPostingData} _LastPostDateTimeUTC:{_LastPostDateTimeUTC.ToString()}, _apiClientUrls:{string.Join(", ", _apiClientUrls)}");
            }

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

        private async void OnChanged(object source, FileSystemEventArgs e)
        {
            //If server time is ahead for some reason and returns to normal, set the last post time to now so that it can post again
            if (_LastPostDateTimeUTC.HasValue && DateTime.UtcNow.AddHours(1) < _LastPostDateTimeUTC.GetValueOrDefault())
            {
                logger.Information($"_LastPostDateTimeUTC is ahead for more than one hour, set _LastPostDateTimeUTC to server time {DateTime.UtcNow} ");
                _LastPostDateTimeUTC = DateTime.UtcNow;
            }

            //Don't push more often than every 9 seconds to prevent consistent spikes
            if (_LastPostDateTimeUTC.HasValue && DateTime.UtcNow < _LastPostDateTimeUTC.GetValueOrDefault().AddSeconds(6))
                return;

            if (_isPostingData && (!_LastPostDateTimeUTC.HasValue || _LastPostDateTimeUTC > DateTime.UtcNow.AddMinutes(-2)))
            {
                return;
            }

            logger.Information($"File: {e.FullPath} {e.Name} {e.ChangeType}");

            if (_LastPostDateTimeUTC.HasValue && _LastPostDateTimeUTC < DateTime.UtcNow.AddMinutes(-2))
                logger.Information("Fbolinx api call was delayed due to previous POST in progress.  Re-trying since 2 minutes have passed."); 

            _isPostingData = true;
            List<AirportWatchDataType> data = GetCSVRecords(e.FullPath, e.Name);

            List<AirportWatchLiveData> airportWatchData = ConvertToDBModel(data);
            logger.Information($"csv records ({data.Count}) converted to DB model ({airportWatchData.Count}) !");

            if (airportWatchData.Count > 0)
            {
                if (airportWatchData.Count > _NewRowThreshold)
                {
                    logger.Information($"Amount of records to POST exceeded {string.Format("{0:N}", _NewRowThreshold)} (records count {string.Format("{0:N}", airportWatchData.Count)}) ( data count{string.Format("{0:N}", data.Count)}) ( last watch file record index{string.Format("{0:N}", _lastWatchedFileRecordIndex)}).  Jumping to end-of-file for next POST and skipping the current one.");
                    _isPostingData = false;
                    return;
                }

                List<Task> tasks = new List<Task>();
                foreach (var apiClientUrl in _apiClientUrls)
                {
                    try
                    {

                        tasks.Add(
                            Task.Run(async () =>
                            {
                                await PostAirportWatchData(apiClientUrl.Trim(), airportWatchData);
                            }));
                        logger.Information($"set LastPostDateTimeUTC  to {DateTime.UtcNow.ToString()}");
                        _LastPostDateTimeUTC = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Failed async task Fbolinx api for client {apiClientUrl}");
                    }
                }
                await Task.WhenAll(tasks);
                logger.Information("Fbolinx api call succeed!");
            }
            _isPostingData = false;
            airportWatchData = null;
        }

        private async Task PostAirportWatchData(string apiClientUrl, List<AirportWatchLiveData> airportWatchLiveData)
        {
            try
            {
                var apiClient = new ApiClient(apiClientUrl.Trim());

                var result = await apiClient.PostAsync("airportwatch/list", airportWatchLiveData);
                if (result.IsSuccessStatusCode)
                {
                    logger.Information($"Fbolinx api call to {apiClientUrl} completed.  Passed " + airportWatchLiveData.Count + " records. ");
                }
                else
                {
                    logger.Error($"Fbolinx api call to {apiClientUrl} failed. response : {result.Content.ReadAsStringAsync().Result}");
                }
                    
            }
            catch (System.Exception exception)
            {
                logger.Error(exception, $"Failed to call Fbolinx api for client {apiClientUrl}!");
            }
        }
    
        private List<AirportWatchDataType> GetCSVRecords(string filePath, string fileName)
        {
            if (_isNewInstance)
            {
                _CsvParser = new AirportWatchCsvParser(filePath);
                _CsvParser.JumpToEnd();
                logger.Information($"Running new instance of job, jumping to end of file to prevent memory overflow for big size files. Set _isNewInstance flag to {!_isNewInstance}");
                _isNewInstance = !_isNewInstance;
            }
            else if (_lastWatchedFile != fileName )
            {
                _CsvParser = new AirportWatchCsvParser(filePath);
                _CsvParser.JumpToEnd();
                logger.Information($"file to read changed from {_lastWatchedFile} to {fileName} {_lastWatchedFileRecordIndex} set last watch file record index to {_lastWatchedFileRecordIndex}");
            }

            var data = new List<AirportWatchDataType>();

            try
            {
                if (_CsvParser == null)
                    _CsvParser = new AirportWatchCsvParser(filePath);
                
                data = _CsvParser.GetRecords();
                logger.Information($"records read for {filePath} are {data.Count} ");
                
                _lastWatchedFile = fileName;
                
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                _CsvParser.JumpToEnd();
                logger.Error(ex,$"Error getting records from {filePath}.  Jumping to end of file.");
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

                if (string.IsNullOrEmpty(record.AircraftHexCode))
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
                .GroupBy(row => new { row.AircraftHexCode, row.BoxName })
                .Select(grouped => grouped.First())
                .ToList();
        }
    }
}
