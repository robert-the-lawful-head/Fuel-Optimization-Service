using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerImportVM
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Activate { get; set; }
        public string Tail { get; set; }
        public string AircraftModel { get; set; }
        public string AircraftMake { get; set; }
        public string AircraftSize { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Distribution { get; set; }
        public string Note { get; set; }
        public int groupid { get; set; }
    }
}
