namespace FBOLinx.DB.Models
{
    public class FuelReqConfirmation : FBOLinxBaseEntityModel<int>
    {
        public int SourceId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
