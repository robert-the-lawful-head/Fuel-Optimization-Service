using System.Collections.Generic;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO.Requests.Groups
{
    public class MergeGroupRequest
    {
        public int BaseGroupId { get; set; }
        public List<Group> Groups { get; set; }
    }
}
