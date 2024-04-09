namespace FBOLinx.Service.Mapping.Dto
{
    public partial class ImportedFboEmailsDto
    {
        public int Oid { get; set; }
        public string Icao { get; set; }
        public string Email { get; set; }
        public int AcukwikFBOHandlerId { get; set; }
        public string AcukwikFBOHandlerName { get; set; }
    }
}