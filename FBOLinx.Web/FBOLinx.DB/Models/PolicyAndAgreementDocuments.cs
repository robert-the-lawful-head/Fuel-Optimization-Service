using FBOLinx.Core.Enums;
using System.Collections.Generic;

namespace FBOLinx.DB.Models
{
    public class PolicyAndAgreementDocuments : FBOLinxBaseEntityModel<int>
    {
        public DocumentTypeEnum DocumentType { get; set; }
        public string DocumentVersion { get; set; }
        public string Document { get; set; }
        public DocumentAcceptanceFlag AcceptanceFlag { get; set; }
        public bool IsEnabled { get; set; }
        public List<PolicyAndAgreementGroupExemptions> PolicyAndAgreementGroupExemptions { get; set; }
        public List<UserAcceptedPolicyAndAgreements> UserAcceptedPolicyAndAgreements { get; set; }


    }
}
