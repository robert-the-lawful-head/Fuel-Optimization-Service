using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerContactLogDataVM
    {
        public CustomerContactLogData customerContactLogData { get; set; }

        public CustomerContacts customerContact { get; set; }

        public Contacts NewContact { get; set; }
        public Contacts OldContact { get; set; }
    }
}
