using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class FboContactsViewModel
    {
        public int Oid { get; set; }
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public bool? Primary { get; set; }
        public bool? CopyAlerts { get; set; }
    }
}
