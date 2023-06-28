using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Models.ServicesAndFees
{
    public class FboCustomServicesAndFees : FBOLinxBaseEntityModel<int>
    {
        [Required]
        public ServiceActionType ServiceActionType { get; set; }

        [Required]
        public int FboId { get; set; }
        public int? AcukwikServicesOfferedId { get; set; }
        [StringLength(100)]
        public string Service { get; set; }
        [Required]
        [StringLength(100)]
        public string ServiceType { get; set; }
    }
}
