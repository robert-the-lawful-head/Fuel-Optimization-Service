using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models
{
    [Table("FBOFeeAndTaxOmitsByCustomer")]
    public class FboFeeAndTaxOmitsByCustomer
    {
        [Column("OID")]
        public int Oid { get; set; }
        [Column("FBOFeeAndTaxID")]
        [Required]
        public int FboFeeAndTaxId { get; set; }
        [Column("CustomerID")]
        [Required]
        public int CustomerId { get; set; }

        #region Relationships
        [ForeignKey("FboFeeAndTaxId")]
        [InverseProperty("OmitsByCustomer")]
        public FboFeesAndTaxes FboFeeAndTax { get; set; }
        #endregion
    }
}
