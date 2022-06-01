using FBOLinx.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class CustomerAircrafts : FBOLinxBaseEntityModel<int>
    {
        [Column("GroupID")]
        public int? GroupId { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("AircraftID")]
        public int AircraftId { get; set; }
        [StringLength(25)]
        public string TailNumber { get; set; }
        public AircraftSizes? Size { get; set; }
        [Column("BasedPAGLocation")]
        [StringLength(50)]
        public string BasedPaglocation { get; set; }
        [StringLength(50)]
        public string NetworkCode { get; set; }
        public int? AddedFrom { get; set; }

        [InverseProperty("CustomerAircraft")]
        public ICollection<FuelReq> FuelReqs { get; set; }

        [InverseProperty("CustomerAircraft")]
        public AircraftPrices AircraftPrices { get; set; }

        [ForeignKey("AircraftId")]
        public AirCrafts Aircraft { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }
    }
}
