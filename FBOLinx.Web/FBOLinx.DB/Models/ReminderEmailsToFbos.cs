using System;

namespace FBOLinx.DB.Models
{
    public partial class ReminderEmailsToFbos
    {
        public int? Fboid { get; set; }
        public DateTime? Date { get; set; }
        public bool? Success { get; set; }
        public string Message { get; set; }
        public int Oid { get; set; }
    }
}
