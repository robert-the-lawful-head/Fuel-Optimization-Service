using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using FBOLinx.TableStorage.Utils;
using Microsoft.Extensions.Options;

namespace FBOLinx.TableStorage.EntityServices
{
    public abstract class BaseTableEntityService<TTableEntity> where TTableEntity : class, ITableEntity, new()
    {
        protected readonly TableClient tableClient;

        protected BaseTableEntityService(IOptions<AzureTableStorageSettings> azureTableStorageSettings)
        {
            if (!string.IsNullOrWhiteSpace(azureTableStorageSettings?.Value?.ConnectionString))
            {
                var options = new TableClientOptions();
                //options.Retry.NetworkTimeout = TimeSpan.FromSeconds(30);
                tableClient = new TableClient(azureTableStorageSettings.Value.ConnectionString, typeof(TTableEntity).Name, options);
            }
        }

        public async Task<TTableEntity> Get(string partitionKey, string rowKey)
        {
            if (!await AssertTableExists())
            {
                return null;
            }

            TTableEntity entity = await tableClient.GetEntityAsync<TTableEntity>(partitionKey, rowKey);

            return entity;
        }

        public async Task<IEnumerable<TTableEntity>> GetAll(IEnumerable<string> partitionKeys, IEnumerable<string> propertiesToSelect = null)
        {
            if (!await AssertTableExists())
            {
                return null;
            }

            string queryFilter = GetTableQueryFilterForPartitionKeys(partitionKeys);
            IEnumerable<TTableEntity> entities = await GetAll(queryFilter, propertiesToSelect);

            return entities;
        }

        public async Task<IEnumerable<TTableEntity>> GetAll(string queryFilter, IEnumerable<string> propertiesToSelect = null)
        {
            if (!await AssertTableExists())
            {
                return null;
            }

            AsyncPageable<TTableEntity> queryResultsMaxPerPage = tableClient.QueryAsync<TTableEntity>(queryFilter, @select: propertiesToSelect);
            IList<TTableEntity> entities = new List<TTableEntity>();
            await foreach (Page<TTableEntity> page in queryResultsMaxPerPage.AsPages())
            {
                foreach (TTableEntity entity in page.Values)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public virtual async Task Insert(TTableEntity entity)
        {
            if (entity == null || !await AssertTableExists())
            {
                return;
            }

            try
            {
                await tableClient.AddEntityAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public virtual async Task BatchInsert(IEnumerable<TTableEntity> entities)
        {
            if (entities == null || !await AssertTableExists())
            {
                return;
            }

            List<TableTransactionAction> tableTransactionActions = new List<TableTransactionAction>();

            tableTransactionActions.AddRange(entities.Select(e => new TableTransactionAction(TableTransactionActionType.Add, e)));

            try
            {
                foreach (IEnumerable<TableTransactionAction> batchToInsert in tableTransactionActions.Batch(100))
                {
                    await tableClient.SubmitTransactionAsync(batchToInsert).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Delete(string partitionKey, string rawKey)
        {
            if (!await AssertTableExists())
            {
                return;
            }

            try
            {
                await tableClient.DeleteEntityAsync(partitionKey, rawKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Deletes all rows from the table
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAllEntitiesAsync()
        {
            // Only the PartitionKey & RowKey fields are required for deletion
            AsyncPageable<TableEntity> entities = tableClient
                .QueryAsync<TableEntity>(select: new List<string>() { "PartitionKey", "RowKey" }, maxPerPage: 1000);

            // await entities.AsPages().ForEachAwaitAsync(async page => {
            //     // Since we don't know how many rows the table has and the results are ordered by PartitonKey+RowKey
            //     // we'll delete each page immediately and not cache the whole table in memory
            //     await BatchManipulateEntities(page.Values, TableTransactionActionType.Delete).ConfigureAwait(false);
            // });

            await foreach (var page in entities.AsPages())
            {
                // Since we don't know how many rows the table has and the results are ordered by PartitonKey+RowKey
                // we'll delete each page immediately and not cache the whole table in memory
                await BatchManipulateEntities(page.Values, TableTransactionActionType.Delete).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Groups entities by PartitionKey into batches of max 100 for valid transactions
        /// </summary>
        /// <returns>List of Azure Responses for Transactions</returns>
        public async Task<List<Response<IReadOnlyList<Response>>>> BatchManipulateEntities<T>(IEnumerable<T> entities, TableTransactionActionType tableTransactionActionType) where T : class, ITableEntity, new()
        {
            var groups = entities.GroupBy(x => x.PartitionKey);
            var responses = new List<Response<IReadOnlyList<Response>>>();
            foreach (var group in groups)
            {
                List<TableTransactionAction> actions;
                var items = group.AsEnumerable();
                while (items.Any())
                {
                    var batch = items.Take(100);
                    items = items.Skip(100);

                    actions = new List<TableTransactionAction>();
                    actions.AddRange(batch.Select(e => new TableTransactionAction(tableTransactionActionType, e)));
                    var response = await tableClient.SubmitTransactionAsync(actions).ConfigureAwait(false);
                    responses.Add(response);
                }
            }
            return responses;
        }

        public async Task Update(TTableEntity entity)
        {
            if (!await AssertTableExists())
            {
                return;
            }

            await tableClient.UpdateEntityAsync(entity, entity.ETag);
        }

        public string GetRowKey()
        {
            // Azure Table Storage stores Entities in ascending order based on the Row Key. In logs and historical data it can be useful to inverse the sort order. 
            var inverseTimeKey = DateTime
                .MaxValue
                .Subtract(DateTime.UtcNow)
                .TotalMilliseconds
                .ToString(CultureInfo.InvariantCulture);
            return $"{inverseTimeKey}-{Guid.NewGuid()}"; //combined a time-based key with a guid in order to create a unique key
        }
        
        protected string GetTableQueryFilterForPartitionKeys(IEnumerable<string> partitionKeys)
        {
            var result = "";
            foreach (var partitionKey in partitionKeys)
            {
                if (string.IsNullOrEmpty(result))
                    result = "(PartitionKey eq '" + partitionKey + "'";
                else
                    result += " or PartitionKey eq '" + partitionKey + "'";
            }
            result += ")";

            return result;
        }
        
        private async Task<bool> AssertTableExists()
        {
            if (tableClient == null)
            {
                return false;
            }

            await tableClient.CreateIfNotExistsAsync();

            return true;
        }
    }
}
