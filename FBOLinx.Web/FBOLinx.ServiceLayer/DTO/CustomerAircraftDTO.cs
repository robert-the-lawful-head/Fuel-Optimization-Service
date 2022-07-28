using System.ComponentModel.DataAnnotations;
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.DTO
{
    public class CustomerAircraftDTO : BaseEntityModelDTO<DB.Models.CustomerAircrafts>, IEntityModelDTO<DB.Models.CustomerAircrafts, int>
    {
        public int Oid { get; set; }
        public int? GroupId { get; set; }
        public int CustomerId { get; set; }
        public int AircraftId { get; set; }
        [StringLength(25)]
        public string TailNumber { get; set; }
        public AircraftSizes? Size { get; set; }
        [StringLength(50)]
        public string BasedPaglocation { get; set; }
        [StringLength(50)]
        public string NetworkCode { get; set; }
        public int? AddedFrom { get; set; }
        public CustomerDTO Customer { get; set; }
        
    }
}
