using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    [Table("ImportedFBOEmails")]
    public class ImportedFboEmails
    {
        [Key]
        public int Oid { get; set; }
        public string Icao { get; set; }

        public string Email { get; set; }

        public int AcukwikFBOHandlerId { get; set; }

        public string AcukwikFBOHandlerName { get; set; }
    }
}
