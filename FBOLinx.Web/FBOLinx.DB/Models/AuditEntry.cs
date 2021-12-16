using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static FBOLinx.DB.Models.Audit;

namespace FBOLinx.DB.Models
{
     public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public enum AuditEntryType : int
        {
            [Description("Deactivate")]
            Deactivated = 0 ,
            [Description("Activated")]
            Acitviated = 1 ,
            [Description("Contact Added")]
            ContactAdded = 2 ,
            [Description("Contact Deleted")]
            ContactDeleted = 3,
            [Description("Created")]
            Created = 4 ,
            [Description("Edited")]
            Edited = 5 ,
            [Description("Itp Template Assigned")]
            ItpTemplateAssigned = 6 ,
            [Description("Aircaft Added")]
            AircaftAdded = 7 ,
            [Description("Aircraft Deleted")]
            AircraftDeleted = 8 ,
            [Description("Aircraft Edited")]
            AircraftEdited = 9
        }

        public EntityEntry Entry { get; }
        public int UserId { get; set; }
        public string TableName { get; set; }

        public Dictionary<string, object> KeyValue { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public int CustomerId { get; set; }
        public int GroupId { get; set; }
        public int FboId { get; set; }

        public Audit ToAudit()
        {
            var audit = new Audit();
            audit.UserId = UserId;
            audit.CustomerId = CustomerId;
            audit.FboId = FboId;
            audit.GroupId = GroupId;
            audit.Type = AuditType.ToString();
            audit.TableName = TableName;
            audit.DateTime = DateTime.Now;
            audit.PrimaryKey =  JsonConvert.SerializeObject(KeyValue);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return audit;
        }
    }
}
