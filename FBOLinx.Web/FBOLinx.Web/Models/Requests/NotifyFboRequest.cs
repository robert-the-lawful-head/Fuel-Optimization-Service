using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class NotifyFboRequest
    {
        public int FboHandlerId { get; set; }
        public int FuelerLinxCompanyId { get; set; }
    }
}
