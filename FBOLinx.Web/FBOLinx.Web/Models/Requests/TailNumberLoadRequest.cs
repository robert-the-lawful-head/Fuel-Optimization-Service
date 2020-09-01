using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class TailNumberLoadRequest
    {
        public int FuelVolume { get; set; }
        [Required]
        public string TailNumber { get; set; }
        public string ICAO { get; set; }
        [Required]
        public int FBOID { get; set; }
        [Required]
        public int GroupID { get; set; }
    }
}
