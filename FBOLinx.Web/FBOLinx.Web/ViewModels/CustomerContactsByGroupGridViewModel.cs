using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerContactsByGroupGridViewModel
    {
        public int ContactInfoByGroupId { get; set; }
        public int CustomerContactId { get; set; }
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Extension { get; set; }
        public string Fax { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool? Primary { get; set; }
        public bool? CopyAlerts { get; set; }
        public string PrimaryContact { get; set; }
        public string CopyAlertsContact { get; set; }
        public int? GroupId { get; set; }
        public int? CustomerId { get; set; }
    }
}