using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerByGroupLogVM
    {
        public enum LogType : int
        {
            CustomerInfo = 0,

            CustoemrContact = 1,

            CustomerAircarft = 2,

            CustomerItpMargin = 3

        }

        public int Oid { get; set; }
        public DateTime Time { get; set; }

        public string Location { get; set; }

        public string Action { get; set; }

        public string username { get; set; }

        public string Role { get; set; }


        public LogType logType { get; set; }
    }

    }
