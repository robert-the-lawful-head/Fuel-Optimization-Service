namespace FBOLinx.DB.Models
{
    public partial class CustomerSchedulingSoftwareByGroup
    {
        public int CustomerId { get; set; }
        public int GroupId { get; set; }
        public string SchedulingSoftware { get; set; }
        public int Oid { get; set; }
    }
}
