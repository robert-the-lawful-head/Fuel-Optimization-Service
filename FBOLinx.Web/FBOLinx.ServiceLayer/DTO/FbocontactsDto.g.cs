using FBOLinx.Service.Mapping.Dto;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FbocontactsDto
    {
        public int Fboid { get; set; }
        public int ContactId { get; set; }
        public int Oid { get; set; }
        public ContactsDto Contact { get; set; }
        public FbosDto Fbo { get; set; }
    }
}