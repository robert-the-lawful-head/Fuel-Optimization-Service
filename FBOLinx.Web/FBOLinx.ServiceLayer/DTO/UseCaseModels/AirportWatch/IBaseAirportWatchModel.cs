using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch
{
    public interface IBaseAirportWatchModel
    {
        [Required]
        [StringLength(10)]
        public string AircraftHexCode { get; set; }

        [StringLength(25)]
        public string TailNumber { get; set; }
    }
}
