using System.Collections.Generic;
using FBOLinx.Core.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class AuditEntryDto
    {
        public EntityEntry Entry { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValue { get; set; }
        public Dictionary<string, object> OldValues { get; set; }
        public Dictionary<string, object> NewValues { get; set; }
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; set; }
        public int CustomerId { get; set; }
        public int GroupId { get; set; }
        public int FboId { get; set; }
    }
}