using FBOLinx.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel;

namespace FBOLinx.DB.Models
{
    public class GroupPolicyAndAgreementDocuments
    {
        public int Oid { get;set; }
        public DocumentTypeEnum DocumentType { get; set; }
        public string DocumentVersion { get; set; }
        public string Document { get; set; }
        public DocumentAcceptanceFlag AcceptanceFlag { get; set; }
        public bool IsExempted { get; set; }
    }
}
