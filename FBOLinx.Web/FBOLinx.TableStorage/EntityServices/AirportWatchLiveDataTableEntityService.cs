using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.TableStorage.Entities;
using FBOLinx.TableStorage.Utils;
using Microsoft.Extensions.Options;

namespace FBOLinx.TableStorage.EntityServices
{
    public class AirportWatchLiveDataTableEntityService: BaseTableEntityService<AirportWatchLiveDataTableEntity>
    {
        public AirportWatchLiveDataTableEntityService(IOptions<AzureTableStorageSettings> azureTableStorageSettings) : base(azureTableStorageSettings)
        {
            CreatePartitionKeyFunc = (date, boxName) => $"{boxName}-{date.Month}-{date.Day}-{date.Year}:{date.Hour}";
        }

        public Func<DateTime, string, string> CreatePartitionKeyFunc { get; set; }
        
        public override async Task BatchInsert(IEnumerable<AirportWatchLiveDataTableEntity> entities)
        {
            try
            {
                foreach (AirportWatchLiveDataTableEntity entity in entities)
                {
                    entity.PartitionKey = CreatePartitionKeyFunc(entity.BoxTransmissionDateTimeUtc, entity.BoxName);
                    entity.RowKey = GetRowKey();
                }

                await base.BatchInsert(entities);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
