using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.Web.DTO
{
    public class GroupFboViewModel
    {
        public List<GroupViewModel> Groups { get; set; }
        public List<FbosGridViewModel> Fbos { get; set; }
    }
}
