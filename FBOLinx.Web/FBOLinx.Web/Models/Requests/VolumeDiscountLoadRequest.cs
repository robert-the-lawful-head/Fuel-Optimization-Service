using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class VolumeDiscountLoadRequest
    {
        [Required]
        public int FuelerlinxCompanyID { get; set; }
        [Required]
        public string ICAO { get; set; }
        
        public FBOLinx.Core.Enums.StrictFlightTypeClassifications? FlightTypeClassification { get; set; }
    }
}
