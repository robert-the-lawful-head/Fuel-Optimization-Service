namespace FBOLinx.Service.Mapping.Dto
{
    public partial class DistributionEmailsBodyDto
    {
        public int Oid { get; set; }
        public int? Fboid { get; set; }
        public string BodyOfEmail { get; set; }
        public string UnsubscribeLink { get; set; }
    }
}