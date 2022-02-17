namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FboLinxImageFileDataDto
    {
        public int Oid { get; set; }
        public byte[] FileData { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int FboId { get; set; }
        public int GroupId { get; set; }
    }
}