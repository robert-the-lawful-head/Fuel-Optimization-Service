using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class CustomerTagRequest
    {
        public int GroupId { get; set; }
        public int? CustomerId { get; set; }
        public bool IsFuelerLinx { get; set; }
    }
}
