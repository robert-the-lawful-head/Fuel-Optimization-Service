using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Enums.TableStorage
{
    public enum TableStorageLogType : short
    {
        FailedInsert = 0,
        FailedBatchInsert,
        Statistics
    }
}
