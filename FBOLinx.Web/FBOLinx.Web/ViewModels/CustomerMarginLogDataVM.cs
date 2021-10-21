using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerMarginLogDataVM
    {
        public CustomCustomerTypesLogData customerTypesLogData { get; set; }

        public CustomCustomerTypes customerTypes { get; set; }

        public string oldMarginName { get; set; }

        public string NewMarginName { get; set; }
    }
}
