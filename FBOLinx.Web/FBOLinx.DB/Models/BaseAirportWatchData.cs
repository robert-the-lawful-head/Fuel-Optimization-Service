using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models
{
    public class BaseAirportWatchData: FBOLinxBaseEntityModel<int>
    {
        [Required]
        [StringLength(10)]
        public string AircraftHexCode { get; set; }

        [StringLength(25)]
        public string TailNumber { get; set; }
    }
}
