using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class FboFavoriteAircraft : FBOLinxBaseEntityModel<int>
    {
        public int? AircraftId { get; set; }
        [ForeignKey("OID")]
        public int FboId { get; set; }
        [ForeignKey("OID")]
        public int GroupId { get; set; }
        public string TailNumber { get; set; }

    }
}
