using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.TableStorage.Entities;
using FBOLinx.TableStorage.EntityServices;
using NUnit.Framework;

namespace FBOLinx.ServiceLayer.Test.Services.AirportWatchLiveData
{
    public class AirportWatchLiveDataServiceTests : BaseTestFixture<AirportWatchLiveDataTableEntityService>
    {
        private List<AirportWatchLiveDataTableEntity> testRecords;

        [Test]
        public async Task GetAirportWatchLiveDataRecordsTest()
        {
            Arrange();

            testRecords = new List<AirportWatchLiveDataTableEntity>();
            for (int i = 1; i < 10; i++)
            {
                testRecords.Add(new AirportWatchLiveDataTableEntity()
                {
                    BoxTransmissionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    AircraftPositionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    BoxName = "test",
                    AircraftHexCode = "test",
                });
            }

            await subject.BatchInsert(testRecords);

            IEnumerable<AirportWatchLiveDataTableEntity> result = await subject.GetAirportWatchLiveDataRecords(new List<string>() { "test" }, testRecords[0].BoxTransmissionDateTimeUtc.AddMinutes(-1), testRecords[2].BoxTransmissionDateTimeUtc.AddMinutes(1));
            
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task SaveLiveDataToTableStorageTest()
        {
            Arrange();

            testRecords = new List<AirportWatchLiveDataTableEntity>()
            {
                new AirportWatchLiveDataTableEntity()
                {
                    BoxTransmissionDateTimeUtc = DateTime.UtcNow,
                    AircraftPositionDateTimeUtc = DateTime.UtcNow,
                    BoxName = "test",
                    AircraftHexCode = "test",
                }
            };
            await subject.BatchInsert(testRecords);
        }
        
        [TearDown]
        public async Task TearDownEachTest()
        {
            foreach (AirportWatchLiveDataTableEntity airportWatchLiveDataTableEntity in testRecords)
            {
                await subject.Delete(airportWatchLiveDataTableEntity.PartitionKey, airportWatchLiveDataTableEntity.RowKey);
            }
        }
    }
}
