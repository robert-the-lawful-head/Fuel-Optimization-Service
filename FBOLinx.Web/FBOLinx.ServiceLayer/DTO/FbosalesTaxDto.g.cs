namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbosalesTaxDto
    {
        public int Fboid { get; set; }
        public double SalesTaxDecimal { get; set; }
        public double? SalesTax100Lldecimal { get; set; }
        public int Oid { get; set; }
    }
}