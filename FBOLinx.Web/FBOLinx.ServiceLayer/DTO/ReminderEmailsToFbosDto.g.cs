using System;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class ReminderEmailsToFbosDto
    {
        public int? Fboid { get; set; }
        public DateTime? Date { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public int Oid { get; set; }
    }
}