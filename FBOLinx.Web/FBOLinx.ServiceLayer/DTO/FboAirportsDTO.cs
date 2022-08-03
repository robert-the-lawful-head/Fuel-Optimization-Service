using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class FboAirportsDTO : BaseEntityModelDTO<DB.Models.Fboairports>, IEntityModelDTO<DB.Models.Fboairports, int>
    {
        public int Oid { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public int Fboid { get; set; }
        public bool? DefaultTemplate { get; set; }
    }
}
