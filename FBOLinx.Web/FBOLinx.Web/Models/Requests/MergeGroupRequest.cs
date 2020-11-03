using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Requests
{
    public class MergeGroupRequest
    {
        public int BaseGroupId { get; set; }
        public List<Group> Groups { get; set; }
    }
}
