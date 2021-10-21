using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class CustomerAircraftLogDataVM
    {
        public CustomerAircraftLogData customerAircraftLogData { get; set; }

        public CustomerAircrafts customerAircrafts { get; set; }

        public AirCrafts oldAircraft { get; set; }

        public AirCrafts NewAircraft { get; set; }

    }
}
