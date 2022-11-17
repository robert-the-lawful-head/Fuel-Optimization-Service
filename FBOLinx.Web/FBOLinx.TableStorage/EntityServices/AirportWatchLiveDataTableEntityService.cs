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

        public async Task<IEnumerable<AirportWatchLiveDataTableEntity>> GetAirportWatchLiveDataRecords(IEnumerable<string> boxNames, DateTime startDate, DateTime endDate)
        {
            IList<string> partitionKeys = new List<string>();
            foreach (DateTime day in DateTimeHelper.EachHour(startDate, endDate))
            {
                foreach (string boxName in boxNames)
                {
                    string partitionKey = CreatePartitionKeyFunc(day, boxName);
                    if (!partitionKeys.Contains(partitionKey))
                    {
                        partitionKeys.Add(partitionKey);
                    }
                }
            }

            List<AirportWatchLiveDataTableEntity> result = new List<AirportWatchLiveDataTableEntity>();
            foreach (IEnumerable<string> partitionKeysBatch in partitionKeys.Batch(100))
            {
                string filter = GetTableQueryFilterForPartitionKeys(partitionKeysBatch);
                filter = AppendDateRangeFilter(filter, startDate, endDate);
                IEnumerable<AirportWatchLiveDataTableEntity> airportWatchLiveDataRecords = await GetAll(filter);
                if (airportWatchLiveDataRecords != null)
                {
                    result.AddRange(airportWatchLiveDataRecords);
                }
            }

            return result.OrderByDescending(x => x.BoxTransmissionDateTimeUtc).ToList();
        }

        public override async Task BatchInsert(IList<AirportWatchLiveDataTableEntity> entities)
        {
            try
            {
                foreach (AirportWatchLiveDataTableEntity entity in entities)
                {
                    entity.PartitionKey = CreatePartitionKeyFunc(entity.BoxTransmissionDateTimeUtc, entity.BoxName);
                    entity.RowKey = GetRowKey();
                }

                foreach (IGrouping<string, AirportWatchLiveDataTableEntity> entitiesGroup in entities.GroupBy(x => x.PartitionKey))
                {
                    await base.BatchInsert(entitiesGroup.ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private string AppendDateRangeFilter(string filter, DateTime startDate, DateTime endDate)
        {
            return $"{filter} and (BoxTransmissionDateTimeUtc ge datetime'{startDate:yyyy-MM-ddTHH:mm:ssZ}' and BoxTransmissionDateTimeUtc le datetime'{endDate:yyyy-MM-ddTHH:mm:ssZ}')";
        }
    }
}
