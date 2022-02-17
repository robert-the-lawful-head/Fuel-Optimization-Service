using FBOLinx.Core.Enums;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class EmailContentDto
    {
        public int Oid { get; set; }
        public string EmailContentHtml { get; set; }
        public int? FboId { get; set; }
        public int? GroupId { get; set; }
        public string FromAddress { get; set; }
        public string ReplyTo { get; set; }
        public EmailContentTypes EmailContentType { get; set; }
        public string EmailContentTypeDescription { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}