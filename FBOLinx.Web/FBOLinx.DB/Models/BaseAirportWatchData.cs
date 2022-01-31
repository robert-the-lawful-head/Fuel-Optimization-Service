using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models
{
    public class BaseAirportWatchData
    {
        [Required]
        [StringLength(10)]
        public string AircraftHexCode { get; set; }

        [StringLength(25)]
        public string TailNumber { get; set; }
    }
}
