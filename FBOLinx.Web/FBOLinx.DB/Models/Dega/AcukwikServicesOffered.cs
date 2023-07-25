using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models.Dega
{
    public class AcukwikServicesOffered
    {
        [Key,Column("Handler_ID", Order = 0)]
        public int HandlerId { get; set; }

        [Key,Column("ServiceOfferedID", Order = 1)]
        public int ServiceOfferedId { get; set; }

        [StringLength(255)]
        public string Service { get; set; }

        [StringLength(255)]
        [Column("ServiceType")]
        public string ServiceType { get; set; }
    }
}
