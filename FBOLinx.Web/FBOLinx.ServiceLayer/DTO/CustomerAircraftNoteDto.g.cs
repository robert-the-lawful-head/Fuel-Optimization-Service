using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class CustomerAircraftNoteDto
    {
        public int Oid { get; set; }
        public int CustomerAircraftId { get; set; }
        public int? Fboid { get; set; }
        public string Notes { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }
        public int? LastUpdatedByUserId { get; set; }
    }
}
