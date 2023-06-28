using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models.Dega
{
    public class AcukwikServicesOffered
    {
        [Column("Handler_ID")]
        public float? HandlerId { get; set; }

        [Column("ServiceOfferedID")]
        public float? ServiceOfferedId { get; set; }

        [StringLength(255)]
        public string Service { get; set; }

        [StringLength(255)]
        [Column("ServiceType")]
        public string ServiceType { get; set; }
    }
}
