using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Enums.TableStorage;

namespace FBOLinx.DB.Models.ServiceLogs
{
    [Table("TableStorageLogs")]
    public class TableStorageLog : FBOLinxBaseEntityModel<int>
    {
        public TableEntityType TableEntityType { get; set; }
        public TableStorageLogType LogType { get; set; }
        public string RequestData { get; set; }
        public string AdditionalData { get; set; }
        public string PartitionKey { get; set; }
    }
}
