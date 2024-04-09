using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class GroupCustomerContact
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
    public class GroupCustomerWithContactsResponse
    {
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public List<GroupCustomerContact> Contacts { get; set; }
    }
}
