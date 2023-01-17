using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Models.ServiceLogs;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.EntityServices.SWIM
{
    public class TableStorageLogEntityService : Repository<TableStorageLog, ServiceLogsContext>
    {
        public TableStorageLogEntityService(ServiceLogsContext context) : base(context)
        {
        }
    }
}
