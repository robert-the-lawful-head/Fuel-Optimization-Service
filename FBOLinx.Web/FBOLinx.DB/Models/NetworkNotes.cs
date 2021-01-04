namespace FBOLinx.DB.Models
{
    public partial class NetworkNotes
    {
        public int Oid { get; set; }
        public int? GroupId { get; set; }
        public int? Fboid { get; set; }
        public int? CustomerId { get; set; }
        public string Notes { get; set; }
    }
}
