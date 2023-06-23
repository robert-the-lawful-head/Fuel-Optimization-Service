using System.ComponentModel.DataAnnotations;
namespace FBOLinx.DB.Models.ServicesAndFees
{
    public class FboCustomServiceType : FBOLinxBaseEntityModel<int>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
