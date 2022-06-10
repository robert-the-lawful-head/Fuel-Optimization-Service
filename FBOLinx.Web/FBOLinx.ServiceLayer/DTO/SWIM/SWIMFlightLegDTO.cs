using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO.SWIM
{
    public class SWIMFlightLegDTO : BaseEntityModelDTO<SWIMFlightLegs>, IEntityModelDTO<SWIMFlightLegs, int>
    {
        public int Oid { get; set; }
        public string AircraftIdentification { get; set; }
        public string DepartureICAO { get; set; }
        public string ArrivalICAO { get; set; }
        public DateTime ATD { get; set; }
        public DateTime ETA { get; set; }

        public virtual IEnumerable<SWIMFlightLegDataDTO> SWIMFlightLegDataMessages { get; set; }
    }
}
