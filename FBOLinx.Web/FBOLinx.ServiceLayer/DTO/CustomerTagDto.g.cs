namespace FBOLinx.Service.Mapping.Dto
{
    public partial class CustomerTagDto
    {
        public int Oid { get; set; }
        public int GroupId { get; set; }
        public int? CustomerId { get; set; }
        public string Name { get; set; }
    }
}