using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class FaaAircraftMakeModelReferenceDto
    {
        public int Oid { get; set; }
        public int? DegaAircraftID { get; set; }

        [StringLength(255)]
        public string CODE { get; set; }

        [StringLength(255)]
        public string MFR { get; set; }

        [StringLength(255)]
        public string MODEL { get; set; }

        [StringLength(255)]
        public string AC_WEIGHT { get; set; }

        [StringLength(255)]
        public string TC_DATA_SHEET { get; set; }

        [StringLength(255)]
        public string TC_DATA_HOLDER { get; set; }

        public double? TYPE_ACFT { get; set; }

        public double? TYPE_ENG { get; set; }

        public double? AC_CAT { get; set; }

        public double? BUILD_CERT_IND { get; set; }

        public double? NO_ENG { get; set; }

        
        public double? NO_SEATS { get; set; }

        public double? SPEED { get; set; }

        public string MakeModel
        {
            get
            {
                return $"{MFR} {MODEL}";
            }
        }
    }
}
