using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class CompaniesByGroup
    {
        [Column("GroupID")]
        public int GroupId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Key]
        [Column("OID")]
        public int Oid { get; set; }

        [InverseProperty("CompanyByGroup")]
        [ForeignKey("CustomerId")]
        public Customers Customer { get; set; }
    }
}
