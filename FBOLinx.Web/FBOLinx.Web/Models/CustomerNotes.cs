using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class CustomerNotes
    {
        public int CustomerId { get; set; }
        public int GroupId { get; set; }
        public string CustomerNotes1 { get; set; }
        public int Oid { get; set; }
    }
}
