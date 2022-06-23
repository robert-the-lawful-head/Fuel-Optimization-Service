using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Core.BaseModels.Entities;

namespace FBOLinx.DB.Models
{
    public class FBOLinxBaseEntityModel<T> : BaseEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OID")]
        public virtual T Id { get; set; }
    }
}
