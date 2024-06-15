using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FBOLinx.Job.AirportWatch
{
    public class ModelMapperService
    {
        public List<AirportWatchLiveData> ConvertToDBModel(List<AirportWatchDataType> data)
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

            return GroupAndAirportWatchData(airportWatchData);

            //return airportWatchData
            //    .OrderByDescending(row => row.AircraftPositionDateTimeUtc)
            //    .GroupBy(row => new { row.AircraftHexCode, row.BoxName })
            //    .Select(grouped => grouped.First())
            //    .ToList();
        }

        public List<AirportWatchLiveData> GroupAndAirportWatchData(List<AirportWatchLiveData> airportWatchData)
        {
            var groupedData = airportWatchData
                .OrderByDescending(row => row.AircraftPositionDateTimeUtc)
                .GroupBy(row => new { row.AircraftHexCode, row.BoxName });

            var groupedAirportWatchDataList = new List<AirportWatchLiveData>();

            foreach (var group in groupedData)
            {
                var airportWatch = new AirportWatchLiveData()
                {
                    BoxTransmissionDateTimeUtc = FindGroupedNotNullOrDefaultValue(group, s => s.AircraftPositionDateTimeUtc),
                    AircraftHexCode = FindGroupedNotNullOrDefaultValue(group, s => s.AircraftHexCode),
                    AtcFlightNumber = FindGroupedNotNullOrDefaultValue(group, s => s.AtcFlightNumber),
                    AltitudeInStandardPressure = FindGroupedNotNullOrDefaultValue(group, s => s.AltitudeInStandardPressure),
                    GroundSpeedKts = FindGroupedNotNullOrDefaultValue(group, s => s.GroundSpeedKts),
                    TrackingDegree = FindGroupedNotNullOrDefaultValue(group, s => s.TrackingDegree),
                    Latitude = FindGroupedNotNullOrDefaultValue(group, s => s.Latitude),
                    Longitude = FindGroupedNotNullOrDefaultValue(group, s => s.Longitude),
                    VerticalSpeedKts = FindGroupedNotNullOrDefaultValue(group, s => s.VerticalSpeedKts),
                    TransponderCode = FindGroupedNotNullOrDefaultValue(group, s => s.TransponderCode),
                    BoxName = FindGroupedNotNullOrDefaultValue(group, s => s.BoxName),
                    AircraftPositionDateTimeUtc = FindGroupedNotNullOrDefaultValue(group, s => s.AircraftPositionDateTimeUtc),
                    AircraftTypeCode = FindGroupedNotNullOrDefaultValue(group, s => s.AircraftTypeCode),
                    GpsAltitude = FindGroupedNotNullOrDefaultValue(group, s => s.GpsAltitude),
                    IsAircraftOnGround = FindGroupedNotNullOrDefaultValue(group, s => s.IsAircraftOnGround)
                };
                groupedAirportWatchDataList.Add(airportWatch);
            }
            return groupedAirportWatchDataList;
        }
        public TProperty FindGroupedNotNullOrDefaultValue<T, TKey, TProperty>(
            IGrouping<TKey, T> group,
            Expression<Func<T, TProperty>> propertySelector)
        {
            var compiledSelector = propertySelector.Compile();
            return group
                .Select(compiledSelector)
                .FirstOrDefault(value => value != null);
        }
    }
}
