using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("SWIMFlightLegDataErrors")]
    public class SWIMFlightLegDataError : FBOLinxBaseEntityModel<int>
    {
        public string XmlMessage { get; set; }

        public long SWIMFlightLegDataId { get; set; }
        public virtual SWIMFlightLegData SWIMFlightLegData { get; set; }
    }
}
