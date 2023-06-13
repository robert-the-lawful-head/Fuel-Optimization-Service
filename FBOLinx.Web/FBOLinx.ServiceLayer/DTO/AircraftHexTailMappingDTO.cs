using FBOLinx.DB;
using FBOLinx.DB.Models;
using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO
{
    public class AircraftHexTailMappingDTO : BaseEntityModelDTO<AircraftHexTailMapping>, IEntityModelDTO<AircraftHexTailMapping, int>
    {
        public int Oid { get; set; }
        public string AircraftHexCode { get; set; }
        public string TailNumber { get; set; }
        [StringLength(50)]
        public string FAAAircraftMakeModelCode { get; set; }

        [StringLength(100)]
        public string FAARegisteredOwner { get; set; }
        public FaaAircraftMakeModelReferenceDto FaaAircraftMakeModelReference { get; set; }
    }
}