using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("FBOContacts")]
    public partial class Fbocontacts : FBOLinxBaseEntityModel<int>
    {
        [Column("FBOID")]
        public int Fboid { get; set; }
        [Column("ContactID")]
        public int ContactId { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("FboContact")]
        public Contacts Contact { get; set; }

        [ForeignKey("Fboid")]
        [InverseProperty("Contacts")]
        public Fbos Fbo { get; set; }
    }
}
