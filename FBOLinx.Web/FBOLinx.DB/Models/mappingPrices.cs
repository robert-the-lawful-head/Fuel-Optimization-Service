using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("MappingPrices")]
    public partial class MappingPrices
    {
        [Key]
        [Column("oid")]
        public int Oid { get; set; }
        [Column("groupId")]
        public int GroupId { get; set; }
        [Column("fboPriceId")]
        public int FboPriceId { get; set; }
    }
}
