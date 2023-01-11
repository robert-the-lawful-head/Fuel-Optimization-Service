namespace FBOLinx.ServiceLayer.DTO.Responses.Analitics
{
    public class ValidCustomersProjection
    {
        public int Oid { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public FBOLinx.DB.Models.Customers Customer { get; set; }
    }
}
