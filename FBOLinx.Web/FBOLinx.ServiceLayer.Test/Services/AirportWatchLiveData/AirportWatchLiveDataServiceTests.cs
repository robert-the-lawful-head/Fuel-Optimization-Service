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
        public async Task SaveLiveDataToTableStorage()
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
                    AircraftICAO = "test",
                }
            });

            await subject.DeleteAllEntitiesAsync();
        }
    }
}
