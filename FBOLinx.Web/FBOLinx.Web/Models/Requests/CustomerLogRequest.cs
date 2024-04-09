using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class CustomerLogRequest
    {
        public int userId { get; set; }

        public int Role { get; set; }
    }
}
