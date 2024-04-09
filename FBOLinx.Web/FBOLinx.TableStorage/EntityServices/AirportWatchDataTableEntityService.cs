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
    public class AirportWatchDataTableEntityService : BaseTableEntityService<AirportWatchDataTableEntity>
    {
        protected readonly BlobStorageService _BlobStorageService;

        public AirportWatchDataTableEntityService(IOptions<AzureTableStorageSettings> azureTableStorageSettings, BlobStorageService blobStorageService) : base(azureTableStorageSettings)
        {
            _BlobStorageService = blobStorageService;

            CreatePartitionKeyFunc = (date) => $"{date.Month}-{date.Day}-{date.Year}";
        }

        public Func<DateTime, string> CreatePartitionKeyFunc { get; set; }

        public async Task<IEnumerable<AirportWatchDataTableEntity>> GetAirportWatchDataRecords(DateTime startDate, DateTime endDate)
        {
            IList<string> partitionKeys = new List<string>();
            foreach (DateTime day in DateTimeHelper.EachDay(startDate, endDate))
            {
                string partitionKey = CreatePartitionKeyFunc(day);
                if (!partitionKeys.Contains(partitionKey))
                {
                    partitionKeys.Add(partitionKey);
                }
            }

            List<AirportWatchDataTableEntity> result = new List<AirportWatchDataTableEntity>();
            foreach (IEnumerable<string> partitionKeysBatch in partitionKeys.Batch(100))
            {
                string filter = GetTableQueryFilterForPartitionKeys(partitionKeysBatch);
                filter = AppendDateRangeFilter(filter, startDate, endDate);
                IEnumerable<AirportWatchDataTableEntity> airportWatchDataRecords = await GetAll(filter);
                if (airportWatchDataRecords != null)
                {
                    result.AddRange(airportWatchDataRecords);
                }
            }

            await RestoreReferencedData(result);

            return result.OrderByDescending(x => x.BoxTransmissionDateTimeUtc).ToList();
        }

        public async Task<IEnumerable<AirportWatchDataTableEntity>> GetAirportWatchDataRecords(DateTime day)
        {
            string partitionKey = CreatePartitionKeyFunc(day);
            string filter = GetTableQueryFilterForPartitionKeys(new List<string>(){ partitionKey });
            IEnumerable<AirportWatchDataTableEntity> result = await GetAll(filter);

            await RestoreReferencedData(result);

            return result;
        }

        public override async Task BatchInsert(IList<AirportWatchDataTableEntity> entities)
        {
            foreach (AirportWatchDataTableEntity entity in entities)
            {
                entity.PartitionKey = CreatePartitionKeyFunc(entity.BoxTransmissionDateTimeUtc);
                entity.RowKey = GetRowKey();

                await SetBlobStorageReferences(entity);
            }

            foreach (IGrouping<string, AirportWatchDataTableEntity> entitiesGroup in entities.GroupBy(x => x.PartitionKey))
            {
                await base.BatchInsert(entitiesGroup.ToList());
            }
        }

        private async Task SetBlobStorageReferences(AirportWatchDataTableEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.DataBlob))
            {
                string blobName = $"{entity.BoxTransmissionDateTimeUtc.Month}-{entity.BoxTransmissionDateTimeUtc.Day}-{entity.BoxTransmissionDateTimeUtc.Year}:{entity.BoxTransmissionDateTimeUtc.Hour}:{entity.BoxTransmissionDateTimeUtc.Minute}:{entity.BoxTransmissionDateTimeUtc.Second}.csv";
                await _BlobStorageService.Upload<AirportWatchDataTableEntity>(entity.PartitionKey, blobName, entity.DataBlob);
                entity.DataBlob = blobName;
            }
        }

        private async Task RestoreReferencedData(IEnumerable<AirportWatchDataTableEntity> entities)
        {
            foreach (AirportWatchDataTableEntity entity in entities)
            {
                if (!string.IsNullOrWhiteSpace(entity.DataBlob))
                {
                    entity.DataBlob = await _BlobStorageService.Download<AirportWatchDataTableEntity>(entity.PartitionKey, entity.DataBlob);
                }
            }
        }

        private string AppendDateRangeFilter(string filter, DateTime startDate, DateTime endDate)
        {
            return $"{filter} and (MinAircraftPositionDateTimeUtc ge datetime'{startDate:yyyy-MM-ddTHH:mm:ssZ}' and MaxAircraftPositionDateTimeUtc le datetime'{endDate:yyyy-MM-ddTHH:mm:ssZ}')";
        }
    }
}
