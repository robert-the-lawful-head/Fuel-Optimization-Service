using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.DB.Models;

namespace FBOLinx.DB
{
    public class FAAAircraftMakeModelReference : FBOLinxBaseEntityModel<int>
    {
        public int? DegaAircraftID { get; set; }

        [StringLength(255)]
        public string CODE { get; set; }

        [StringLength(255)]
        public string MFR { get; set; }

        [StringLength(255)]
        public string MODEL { get; set; }

        [StringLength(255)]
        [Column("AC-WEIGHT")]
        public string AC_WEIGHT { get; set; }

        [StringLength(255)]
        [Column("TC-DATA-SHEET")]
        public string TC_DATA_SHEET { get; set; }

        [StringLength(255)]
        [Column("TC-DATA-HOLDER")]
        public string TC_DATA_HOLDER { get; set; }

        [Column("TYPE-ACFT")]
        public double? TYPE_ACFT { get; set; }

        [Column("TYPE-ENG")]
        public double? TYPE_ENG { get; set; }

        [Column("AC-CAT")]
        public double? AC_CAT { get; set; }

        [Column("BUILD-CERT-IND")]
        public double? BUILD_CERT_IND { get; set; }

        [Column("NO-ENG")]
        public double? NO_ENG { get; set; }

        [Column("NO-SEATS")]
        public double? NO_SEATS { get; set; }

        [Column("SPEED")]
        public double? SPEED { get; set; }
    }
}