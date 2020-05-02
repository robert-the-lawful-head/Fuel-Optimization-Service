using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerAircraftsGridViewModel
    {
        public int Oid { get; set; }
        public int? GroupId { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public int AircraftId { get; set; }
        public string TailNumber { get; set; }
        public AirCrafts.AircraftSizes? Size { get; set; }
        public string BasedPaglocation { get; set; }
        public string NetworkCode { get; set; }
        public int? AddedFrom { get; set; }
        public int? PricingTemplateId { get; set; }
        public string PricingTemplateName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool IsFuelerlinxNetwork { get; set; }

        public string AircraftSizeDescription
        {
            get { return Utilities.Enum.GetDescription(Size ?? AirCrafts.AircraftSizes.NotSet); }
        }
    }
}
