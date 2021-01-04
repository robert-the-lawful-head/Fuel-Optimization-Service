using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models;

namespace FBOLinx.Web.ViewModels
{
    public class FuelReqsTotalOrdersByAircraftSizeViewModel
    {
        public AirCrafts.AircraftSizes? Size { get; set; }
        public int TotalOrders { get; set; }

        public string AircraftSizeDescription
        {
            get { return FBOLinx.Core.Utilities.Enum.GetDescription(Size ?? AirCrafts.AircraftSizes.NotSet); }
        }
    }
}
