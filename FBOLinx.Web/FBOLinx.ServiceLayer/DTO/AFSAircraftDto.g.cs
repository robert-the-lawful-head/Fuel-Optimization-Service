using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AFSAircraftDto
    {
        public int Oid { get; set; }
        public string Icao { get; set; }
        public string AircraftTypeName { get; set; }
        public string AircraftTypeEngineName { get; set; }
        public int DegaAircraftID { get; set; }
        public AirCraftsDto AirCrafts { get; set; }
    }
}