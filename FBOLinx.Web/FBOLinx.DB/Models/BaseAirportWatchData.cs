using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class BaseAirportWatchData: FBOLinxBaseEntityModel<int>
    {
        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string AircraftHexCode { get; set; }

        [StringLength(25)]
        [Column(TypeName = "varchar")]
        public string TailNumber { get; set; }
    }
}
