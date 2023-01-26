using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models.ServiceLogs;

namespace FBOLinx.DB.Specifications.ServiceLogs
{
    public class TableStorageLogSpecification :
        Specification<TableStorageLog>
    {
        public TableStorageLogSpecification(string partitionKey)
            : base(x => x.PartitionKey == partitionKey)
        {
        }
    }
}
