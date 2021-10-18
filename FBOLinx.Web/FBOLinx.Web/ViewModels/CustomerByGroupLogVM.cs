using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerByGroupLogVM
    {
        public DateTime Time { get; set; }

        public string Location { get; set; }

        public string  Action { get; set; }

        public string username { get; set; }

        public string Role { get; set; }

             
    }
}
