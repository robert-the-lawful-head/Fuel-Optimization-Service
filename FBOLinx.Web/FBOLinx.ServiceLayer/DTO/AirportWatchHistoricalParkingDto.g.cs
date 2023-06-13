using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class AirportWatchHistoricalParkingDto
    {
        public int Oid { get; set; }
        public int AirportWatchHistoricalDataId { get; set; }
        public int AcukwikFbohandlerId { get; set; }
        public DateTime? DateCalculatedUtc { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
