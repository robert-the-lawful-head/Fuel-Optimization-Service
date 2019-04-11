using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelerLinxResponseContentBase
    {
        public Int16 Error { get; set; }
        public int ErrorID { get; set; }
        public string ErrorMessage { get; set; }
    }
}
