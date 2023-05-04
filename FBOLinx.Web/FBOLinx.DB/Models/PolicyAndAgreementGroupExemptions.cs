using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class PolicyAndAgreementGroupExemptions : FBOLinxBaseEntityModel<int>
    {
        public int DocumentId { get; set; }
        public DateTime DateTimeExempted { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
        [ForeignKey("Oid")]
        public PolicyAndAgreementDocuments PolicyAndAgreementDocuments { get; set; }
    }
}
