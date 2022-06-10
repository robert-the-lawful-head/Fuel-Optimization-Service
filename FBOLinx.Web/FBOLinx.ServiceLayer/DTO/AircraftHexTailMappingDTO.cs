using FBOLinx.DB;

namespace FBOLinx.ServiceLayer.DTO
{
    public class AircraftHexTailMappingDTO : BaseEntityModelDTO<AircraftHexTailMapping>, IEntityModelDTO<AircraftHexTailMapping, int>
    {
        public int Oid { get; set; }
        public string AircraftHexCode { get; set; }
        public string TailNumber { get; set; }
    }
}