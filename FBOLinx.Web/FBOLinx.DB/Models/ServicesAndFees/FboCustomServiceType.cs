using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models.ServicesAndFees
{
    [Table("FboCustomServiceTypes")]
    public class FboCustomServiceType : FBOLinxBaseEntityModel<int>
    {
        public FboCustomServiceType()
        {
            CreatedDate = DateTime.UtcNow;
        }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [ForeignKey("OID")]
        public int? FboId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        [ForeignKey("OID")]
        public int CreatedByUserId { get; set; }
        public virtual Fbos Fbo { get; set; }
        public virtual ICollection<FboCustomServicesAndFees> FboCustomServicesAndFees { get; set; }
        public virtual User CreatedByUser { get; set; }
    }
}
