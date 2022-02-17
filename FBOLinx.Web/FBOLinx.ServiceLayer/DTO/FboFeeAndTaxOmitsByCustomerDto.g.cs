using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FboFeeAndTaxOmitsByCustomerDto
    {
        public int Oid { get; set; }
        public int FboFeeAndTaxId { get; set; }
        public int CustomerId { get; set; }
        public FboFeesAndTaxesDto FboFeeAndTax { get; set; }
    }
}