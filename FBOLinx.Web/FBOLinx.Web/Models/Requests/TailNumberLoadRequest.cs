using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class TailNumberLoadRequest
    {
        [Required]
        public int CompanyID { get; set; }
        public int FuelVolume { get; set; }
        public string TailNumber { get; set; }
        public string ICAO { get; set; }
    }
}
