using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class CustomerListResponse
    {
        public int CustomerInfoByGroupID { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
    }
}
