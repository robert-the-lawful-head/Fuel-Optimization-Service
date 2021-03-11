using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class FuelerLinxAircraftRequest
    {
        [Required]
        public int FuelerlinxCompanyID { get; set; }

        public string TailNumbers { get; set; }
    }
}
