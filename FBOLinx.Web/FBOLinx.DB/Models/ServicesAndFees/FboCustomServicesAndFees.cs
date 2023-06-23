using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.DB.Models.ServicesAndFees
{
    public class FboCustomServicesAndFees : FBOLinxBaseEntityModel<int>
    {
        [Required]
        public short ServiceActionType { get; set; }

        [Required]
        public int FboId { get; set; }

        [Required]
        public int HandlerID { get; set; }

        [Required]
        [StringLength(100)]
        public string Service { get; set; }

        [Required]
        public int FboCustomServiceTypeId { get; set; }

        [ForeignKey("FboCustomServiceTypeId")]
        public FboCustomServiceType FboCustomServiceType { get; set; }
    }
}
