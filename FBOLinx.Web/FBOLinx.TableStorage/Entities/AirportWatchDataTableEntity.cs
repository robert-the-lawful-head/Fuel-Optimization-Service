using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.TableStorage.Entities
{   
    public class AirportWatchDataTableEntity: BaseTableEntity
    {
        public DateTime BoxTransmissionDateTimeUtc { get; set; }
        public DateTime MinAircraftPositionDateTimeUtc { get; set; }
        public DateTime MaxAircraftPositionDateTimeUtc { get; set; }

        public string DataBlob { get; set; }
    }
}
