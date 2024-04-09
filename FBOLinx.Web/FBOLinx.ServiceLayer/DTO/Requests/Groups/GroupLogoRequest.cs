using System.ComponentModel.DataAnnotations;

namespace FBOLinx.ServiceLayer.DTO.Requests.Groups
{
    public class GroupLogoRequest
    {
        [Required]
        public int GroupId { get; set; }
        public string FileData { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
    }
}
