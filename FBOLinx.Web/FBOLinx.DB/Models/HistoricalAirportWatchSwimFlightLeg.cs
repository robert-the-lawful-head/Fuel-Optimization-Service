using System;

namespace FBOLinx.DB.Models
{
    public partial class HistoricalAirportWatchSwimFlightLeg : FBOLinxBaseEntityModel<long>
    {
        public int AirportWatchHistoricalDataId { get; set; }
        public long SwimFlightLegId { get; set; }
    }
}
