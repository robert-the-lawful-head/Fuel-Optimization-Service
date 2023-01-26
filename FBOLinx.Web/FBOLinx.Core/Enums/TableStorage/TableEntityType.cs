using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Enums
{
    public enum TableEntityType : short
    {
        Undefined = 0,
        AirportWatchLiveData,
        FlightPlanningServiceLog,
        FuelOrderServiceLog,
        FuelPriceServiceLog,
        SchedulingIntegrationDispatchServiceLog,
        SchedulingIntegrationServiceLog,
    }
}
