using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models;

namespace FBOLinx.Web.ViewModels
{
    public class FuelReqsTotalOrdersByAircraftSizeViewModel
    {
        public AirCrafts.AircraftSizes? Size { get; set; }
        public int TotalOrders { get; set; }

        public string AircraftSizeDescription
        {
            get { return Utilities.Enum.GetDescription(Size ?? AirCrafts.AircraftSizes.NotSet); }
        }
    }
}
