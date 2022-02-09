using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FBOLinx.Core.BaseModels.Entities;

namespace FBOLinx.DB.Models
{
    public class FBOLinxBaseEntityModel<T> : BaseEntityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OID")]
        public virtual T Oid { get; set; }
    }
}
