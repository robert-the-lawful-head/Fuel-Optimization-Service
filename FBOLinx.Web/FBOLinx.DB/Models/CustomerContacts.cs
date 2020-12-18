using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class CustomerContacts
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("ContactID")]
        public int ContactId { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [InverseProperty("CustomerContacts")]
        [ForeignKey("ContactId")]
        public Contacts Contact { get; set; }

        [InverseProperty("CustomerContacts")]
        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }
    }
}
