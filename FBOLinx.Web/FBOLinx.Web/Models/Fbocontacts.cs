using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.Web.Models
{
    [Table("FBOContacts")]
    public partial class Fbocontacts
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
    }
}
