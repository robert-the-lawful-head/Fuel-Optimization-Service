using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.AirportWatch
{
    public class AirportWatchIntegrityCheckResult
    {
        public int DistinctRecordsCount { get; set; }
        public int DuplicateRecordsCount { get; set; }
    }
}
