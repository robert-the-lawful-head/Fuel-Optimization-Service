using System;
using System.Collections.Generic;
using System.Linq;
using FBOLinx.DB.Models;
using FBOLinx.Job.AirportWatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FBOLinx.Job.Test
{
    [TestClass]
    public class ModelMapperServiceTest
    {
        [TestMethod]
        public void ConvertToDBModel_CorrectlyConvertsAndFiltersData()
        {
            // Arrange
            var inputData = new List<AirportWatchDataType>
            {
                new AirportWatchDataType
                {
                    AircraftHexCode = "ABC123",
                    AtcFlightNumber = "FL123",
                    BoxName = "Box1",
                    AircraftTypeCode = "B737",
                    BoxTransmissionDateTimeUtc = "1609459200",
                    AircraftPositionDateTimeUtc = "1609459200",
                    AltitudeInStandardPressure = "30000",
                    GroundSpeedKts = "450",
                    TrackingDegree = "180",
                    Latitude = "34.5",
                    Longitude = "-117.5",
                    VerticalSpeedKts = "0",
                    TransponderCode = "7500",
                    GpsAltitude = "31000",
                    IsAircraftOnGround = "0"
                },
                new AirportWatchDataType
                {
                    AircraftHexCode = "",
                    AtcFlightNumber = "FL456",
                    BoxName = "Box2",
                    AircraftTypeCode = "A320",
                    BoxTransmissionDateTimeUtc = "1609459300",
                    AircraftPositionDateTimeUtc = "1609459300",
                    AltitudeInStandardPressure = "25000",
                    GroundSpeedKts = "400",
                    TrackingDegree = "90",
                    Latitude = "35.5",
                    Longitude = "-118.5",
                    VerticalSpeedKts = "100",
                    TransponderCode = "7600",
                    GpsAltitude = "26000",
                    IsAircraftOnGround = "1"
                }, // This record should be filtered out due to empty AircraftHexCode
                new AirportWatchDataType
                {
                    AircraftHexCode = "DEF456",
                    AtcFlightNumber = "FL789",
                    BoxName = "Box3",
                    AircraftTypeCode = "B777",
                    BoxTransmissionDateTimeUtc = "invalid",
                    AircraftPositionDateTimeUtc = "1609459400",
                    AltitudeInStandardPressure = "35000",
                    GroundSpeedKts = "500",
                    TrackingDegree = "270",
                    Latitude = "36.5",
                    Longitude = "-119.5",
                    VerticalSpeedKts = "200",
                    TransponderCode = "7700",
                    GpsAltitude = "36000",
                    IsAircraftOnGround = "0"
                } // This record should be filtered out due to invalid BoxTransmissionDateTimeUtc
            };

            ModelMapperService mapperService = new ModelMapperService();

            // Act
            var result = mapperService.ConvertToDBModel(inputData);

            // Assert
            Assert.AreEqual(
                1,
                result.Count,
                "Only one record should pass the filtering and be converted."
            );

            var convertedData = result.First();
            Assert.AreEqual("ABC123", convertedData.AircraftHexCode);
            Assert.AreEqual("FL123", convertedData.AtcFlightNumber);
            Assert.AreEqual("Box1", convertedData.BoxName);
            Assert.AreEqual("B737", convertedData.AircraftTypeCode);
            Assert.AreEqual(
                new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                convertedData.BoxTransmissionDateTimeUtc
            );
            Assert.AreEqual(
                new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                convertedData.AircraftPositionDateTimeUtc
            );
            Assert.AreEqual(30000, convertedData.AltitudeInStandardPressure);
            Assert.AreEqual(450, convertedData.GroundSpeedKts);
            Assert.AreEqual(180, convertedData.TrackingDegree);
            Assert.AreEqual(34.5, convertedData.Latitude);
            Assert.AreEqual(-117.5, convertedData.Longitude);
            Assert.AreEqual(0, convertedData.VerticalSpeedKts);
            Assert.AreEqual(7500, convertedData.TransponderCode);
            Assert.AreEqual(31000, convertedData.GpsAltitude);
            Assert.IsFalse(convertedData.IsAircraftOnGround);
        }

        [TestMethod]
        public void GroupAndAirportWatchData_CorrectlyGroupsAndSelectsFirstNonNullValues()
        {
            DateTime utcDateTime = DateTime.UtcNow;


            // Arrange
            var inputData = new List<AirportWatchLiveData>
            {
                new AirportWatchLiveData
                {
                    AircraftPositionDateTimeUtc = utcDateTime.AddMinutes(-2),
                    AircraftHexCode = "ABC123",
                    BoxName = "Box1",
                    AltitudeInStandardPressure = null,
                    Latitude = 34.5
                },
                new AirportWatchLiveData
                {
                    AircraftPositionDateTimeUtc = utcDateTime.AddMinutes(-1),
                    AircraftHexCode = "ABC123",
                    BoxName = "Box1",
                    AltitudeInStandardPressure = 30000,
                    Latitude = 26.0
                },
                new AirportWatchLiveData
                {
                    AircraftPositionDateTimeUtc = utcDateTime,
                    AircraftHexCode = "DEF456",
                    BoxName = "Box2",
                    AltitudeInStandardPressure = 25000,
                    Latitude = 36.7
                }
            };

            ModelMapperService service = new ModelMapperService();

            // Act
            var result = service.GroupAndGetAirportWatchDataList(inputData);

            // Assert
            Assert.AreEqual(
                2,
                result.Count,
                "Should group into 2 distinct groups based on AircraftHexCode and BoxName."
            );

            var group1 = result.FirstOrDefault(r =>
                r.AircraftHexCode == "ABC123" && r.BoxName == "Box1"
            );
            Assert.IsNotNull(group1, "Group 1 should exist.");
            Assert.AreEqual(
                30000,
                group1.AltitudeInStandardPressure,
                "Should select the first non-null AltitudeInStandardPressure for group 1."
            );
            Assert.AreEqual(
                26.0,
                group1.Latitude,
                "Should select the first non-null Latitude for group 1."
            );

            var group2 = result.FirstOrDefault(r =>
                r.AircraftHexCode == "DEF456" && r.BoxName == "Box2"
            );
            Assert.IsNotNull(group2, "Group 2 should exist.");
            Assert.AreEqual(
                25000,
                group2.AltitudeInStandardPressure,
                "Should correctly retain AltitudeInStandardPressure for group 2."
            );
            Assert.AreEqual(36.7, group2.Latitude, "Should correctly retain Latitude for group 2.");
        }
        class SampleData
        {
            public int GroupKey { get; set; }
            public string Value { get; set; }
        }
        [TestMethod]
        public void FindGroupedNotNullOrDefaultValue_ReturnsFirstNonNullValue()
        {
            // Arrange
            var sampleData = new List<SampleData>
            {
                new SampleData { GroupKey = 1, Value = null },
                new SampleData { GroupKey = 1, Value = "First Non-Null Value" },
                new SampleData { GroupKey = 1, Value = "Second Non-Null Value" }
            };

            var groupedData = sampleData.GroupBy(x => x.GroupKey);
            var targetGroup = groupedData.First();

            ModelMapperService service = new ModelMapperService();

            // Act
            var result = service.FindGroupedNotNullOrDefaultValue(targetGroup, x => x.Value);

            // Assert
            Assert.AreEqual("First Non-Null Value", result);
        }
    }
}
