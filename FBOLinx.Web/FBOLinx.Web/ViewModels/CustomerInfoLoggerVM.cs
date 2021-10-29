using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerInfoLoggerVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public int PrimaryKey { get; set; }

        //for OldDate
        public CustomerInfoByGroup OldCustomerInfoByGroup { get; set; }
        public CustomerAircrafts OldCustomerAircrafts { get; set; }
        public Contacts OldContact { get; set; }
       public PricingTemplate OldPricingTemplate { get; set; }



        //for NewDate
        public CustomerInfoByGroup NewCustomerInfoByGroup { get; set; }
        public CustomerAircrafts NewCustomerAircrafts { get; set; }
        public Contacts NewContact { get; set; }
        public PricingTemplate NewPricingTemplate { get; set; }

    }
}
