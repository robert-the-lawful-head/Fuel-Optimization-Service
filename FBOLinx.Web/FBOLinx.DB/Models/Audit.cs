﻿using System;

namespace FBOLinx.DB.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
        public int CustomerId { get; set; }
        public int GroupId { get; set; }
        public int FboId { get; set; }
    }
}
