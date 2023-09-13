using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class FboFavoriteAircraft : FBOLinxBaseEntityModel<int>
    {
        [ForeignKey("OID")]
        public int FboId { get; set; }
        [ForeignKey("OID")]
        [Column("CustomerAircraftId")]
        public int CustomerAircraftsId { get; set; }
    }
}
