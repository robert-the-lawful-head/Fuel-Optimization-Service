namespace FBOLinx.DB.Models
{
    public partial class DistributionEmailsBody
    {
        public int Oid { get; set; }
        public int? Fboid { get; set; }
        public string BodyOfEmail { get; set; }
        public string UnsubscribeLink { get; set; }
    }
}
