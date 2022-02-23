using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FboairportsDto
    {
        public int Oid { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public int Fboid { get; set; }
        public bool? DefaultTemplate { get; set; }
        public FbosDto Fbo { get; set; }
    }
}