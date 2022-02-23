namespace FBOLinx.Service.Mapping.Dto
{
    public partial class ContactInfoByFboDto
    {
        public int Oid { get; set; }
        public int? ContactId { get; set; }
        public int? FboId { get; set; }
        public bool? CopyAlerts { get; set; }
    }
}