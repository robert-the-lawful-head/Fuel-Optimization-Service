using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class FboFavoriteCompany : FBOLinxBaseEntityModel<int>
    {
        [ForeignKey("OID")]
        public int CustomerInfoByGroupId { get; set; }
        [ForeignKey("OID")]
        public int FboId { get; set; }
    }
}
