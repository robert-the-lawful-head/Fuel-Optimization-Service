using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class AirportWatchDistinctBoxesDTO : BaseEntityModelDTO<DB.Models.AirportWatchDistinctBoxes>, IEntityModelDTO<DB.Models.AirportWatchDistinctBoxes, int>
    {
        public int Oid { get; set; }
        public string BoxName { get; set; }
        public DateTime? LastLiveDateTime { get; set; }
        public DateTime? LastHistoricDateTime { get; set; }
        public string AirportICAO { get; set; }
    }
}
