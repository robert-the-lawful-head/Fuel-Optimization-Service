using System;
using System.Collections.Generic;

namespace FBOLinx.Web.Models
{
    public partial class AdminEmails
    {
        public int Oid { get; set; }
        public int GroupId { get; set; }
        public string Page { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string EmailBody { get; set; }
    }
}
