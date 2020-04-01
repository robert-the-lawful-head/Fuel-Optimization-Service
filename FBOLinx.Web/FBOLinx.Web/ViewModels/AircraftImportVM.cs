using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class AircraftImportVM
    {
        public int CustomerId { get; set; }
        public int GroupId { get; set; }
        public int AircraftID { get; set; }
        public string TailNumber { get; set; }
        public string Model { get; set; }
        public string AircraftMake { get; set; }
        public int? Size { get; set; }
        public bool? IsImported { get; set; }
        public List<string> OtherOptions { get; set; }
        public string selectedModel { get; set; }
    }
}
