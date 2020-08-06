using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class TailNumberLoadResponse
    {
        public Double? PricePerGallon { get; set; }
        public Double? Fees { get; set; }
        public string Template { get; set; }
        public string Company { get; set; }
        public string MakeModel { get; set; }
    }
}
