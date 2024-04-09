using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.SWIM
{
    public class SWIMFlightLegDataDTO : BaseEntityModelDTO<DB.Models.SWIMFlightLegData>, IEntityModelDTO<DB.Models.SWIMFlightLegData, long>
    {
        public long Oid { get; set; }
        public DateTime? ETA { get; set; }
        public double? ActualSpeed { get; set; }
        public double? Altitude { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public long SWIMFlightLegId { get; set; }
        public string RawXmlMessage { get; set; }
    }
}
