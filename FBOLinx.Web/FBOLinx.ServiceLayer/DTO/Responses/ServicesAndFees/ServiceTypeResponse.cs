using FBOLinx.DB.Models.ServicesAndFees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServicesAndFees
{
    public class ServiceTypeResponse : FboCustomServiceType
    {
        public bool IsCustom { get; set; }

    }
}
