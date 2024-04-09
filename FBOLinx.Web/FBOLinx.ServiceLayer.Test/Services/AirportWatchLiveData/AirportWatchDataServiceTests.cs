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
    public class AirportWatchDataServiceTests : BaseTestFixture<AirportWatchDataTableEntityService>
    {
        private List<AirportWatchDataTableEntity> testRecords;

        [Test]
        public async Task GetAirportWatchDataRecordsTest()
        {
            Arrange();
            
            string testData = "test,,test,test,test" + Environment.NewLine;
            testData += "test,test,test,test,test" + Environment.NewLine;
            testData += "test,test,test,test,test" + Environment.NewLine;

            testRecords = new List<AirportWatchDataTableEntity>();
            for (int i = 1; i < 10; i++)
            {
                testRecords.Add(new AirportWatchDataTableEntity()
                {
                    BoxTransmissionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    MinAircraftPositionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    MaxAircraftPositionDateTimeUtc = DateTime.UtcNow.AddHours(i),
                    DataBlob = testData,
                });
            }
        
            await subject.BatchInsert(testRecords);
        
            IEnumerable<AirportWatchDataTableEntity> result = await subject.GetAirportWatchDataRecords(testRecords[0].BoxTransmissionDateTimeUtc.AddMinutes(-1), testRecords[2].BoxTransmissionDateTimeUtc.AddMinutes(1));
            
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task SaveLiveDataToTableStorageTest()
        {
            Arrange();

            string testData = "test,test,test,test,test" + Environment.NewLine;
            testData += "test,test,test,test,test" + Environment.NewLine;
            testData += "test,test,test,test,test" + Environment.NewLine;

            testRecords = new List<AirportWatchDataTableEntity>()
            {
                new AirportWatchDataTableEntity()
                {
                    BoxTransmissionDateTimeUtc = DateTime.UtcNow,
                    MinAircraftPositionDateTimeUtc = DateTime.UtcNow,
                    MaxAircraftPositionDateTimeUtc = DateTime.UtcNow,
                    DataBlob = testData,
                }
            };
            await subject.BatchInsert(testRecords);
        }
        
        [TearDown]
        public async Task TearDownEachTest()
        {
            foreach (AirportWatchDataTableEntity airportWatchDataTableEntity in testRecords)
            {
                await subject.Delete(airportWatchDataTableEntity.PartitionKey, airportWatchDataTableEntity.RowKey);
            }
        }
    }
}
