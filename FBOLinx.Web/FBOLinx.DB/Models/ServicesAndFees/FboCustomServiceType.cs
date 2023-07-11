using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models.ServicesAndFees
{
    [Table("FboCustomServiceTypes")]
    public class FboCustomServiceType : FBOLinxBaseEntityModel<int>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [ForeignKey("OID")]
        public int? FboId { get; set; }
        public virtual Fbos Fbo { get; set; }
        public virtual ICollection<FboCustomServicesAndFees> FboCustomServicesAndFees { get; set; }
    }
}
