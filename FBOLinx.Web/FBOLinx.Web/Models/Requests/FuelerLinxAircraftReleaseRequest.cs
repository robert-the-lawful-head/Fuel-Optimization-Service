using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelerLinxAircraftReleaseRequest
    {
        [Required]
        public int FuelerlinxCompanyID { get; set; }
        [Required]
        public string TailNumber { get; set; }
    }
}
