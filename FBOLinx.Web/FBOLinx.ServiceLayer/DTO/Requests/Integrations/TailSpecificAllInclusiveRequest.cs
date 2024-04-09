using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Requests.Integrations
{
    public class TailSpecificAllInclusiveRequest
    {
        [Required]
        public string TailNumber { get; set; }
        [Required]
        public double AllInPrice { get; set; }

        [Required]
        public string Product { get; set; }
        public DateTime? Expiration { get; set; }
        public TimeStandards TimeStandard { get; set; } = 0;
    }
}
