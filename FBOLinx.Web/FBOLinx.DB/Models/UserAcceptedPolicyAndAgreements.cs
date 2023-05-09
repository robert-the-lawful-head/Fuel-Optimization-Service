using System;
using System.Collections.Generic;

namespace FBOLinx.DB.Models
{
    public class UserAcceptedPolicyAndAgreements : FBOLinxBaseEntityModel<int>
    {
        public int UserId { get; set; }
        public int DocumentId { get; set; }
        public DateTime AcceptedDateTime { get; set; }
        public ICollection<PolicyAndAgreementDocuments> Documents { get; set; }
        public User User { get; set; }
    }
}
