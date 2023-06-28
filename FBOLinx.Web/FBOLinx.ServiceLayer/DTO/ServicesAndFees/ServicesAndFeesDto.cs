using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.ServicesAndFees
{
    public class ServicesAndFeesDto
    {
        public string Oid { get; set; }
        public string Service { get; set; }
        public string ServiceType { get; set; }
    }
}
