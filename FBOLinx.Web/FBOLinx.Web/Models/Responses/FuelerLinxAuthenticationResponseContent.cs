using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class FuelerLinxAuthenticationResponseContent : FuelerLinxResponseContentBase
    {
        public string Token { get; set; } = string.Empty;
    }
}
