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
        [Test]
        public async Task GetAirportWatchLiveDataRecordsTest()
        {
            Arrange();

            var recordsToInsert = new List<AirportWatchLiveDataTableEntity>();
            for (int i = 1; i < 10; i++)
            {
                recordsToInsert.Add(new AirportWatchLiveDataTableEntity()
                {
                    BoxTransmissionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    AircraftPositionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    BoxName = "test",
                    AircraftHexCode = "test",
                });
            }

            await subject.BatchInsert(recordsToInsert);

            IEnumerable<AirportWatchLiveDataTableEntity> result = await subject.GetAirportWatchLiveDataRecords(new List<string>() { "test" }, recordsToInsert[0].BoxTransmissionDateTimeUtc.AddMinutes(-1), recordsToInsert[2].BoxTransmissionDateTimeUtc.AddMinutes(1));

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task SaveLiveDataToTableStorageTest()
        {
            Arrange();

            await subject.BatchInsert(new List<AirportWatchLiveDataTableEntity>()
            {
                new AirportWatchLiveDataTableEntity()
                {
                    BoxTransmissionDateTimeUtc = DateTime.UtcNow,
                    AircraftPositionDateTimeUtc = DateTime.UtcNow,
                    BoxName = "test",
                    AircraftHexCode = "test",
                }
            });

            await subject.DeleteAllEntitiesAsync();
        }
        
        [TearDown]
        public async Task TearDownEachTest()
        {
            await subject.DeleteAllEntitiesAsync();
        }
    }
}
