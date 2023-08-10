using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class FboFavoriteAircrafts : FBOLinxBaseEntityModel<int>
    {
        
        [ForeignKey("AircraftID")]
        public int AircraftId { get; set; }
        [ForeignKey("OID")]
        public int FboId { get; set; }
        public virtual Fbos fbo { get; set; }
        public virtual AirCrafts Aircraft { get; set; }
    }
}
