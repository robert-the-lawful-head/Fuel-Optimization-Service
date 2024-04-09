using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.Core.Enums
{
    public enum FlightLegStatus : short
    {
        EnRoute = 0,
        Landing,
        TaxiingDestination,
        Arrived,
        Departing,
        TaxiingOrigin,
    }
}
