using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Requests.Integrations
{
    public class IntegrationStatusRequest
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
