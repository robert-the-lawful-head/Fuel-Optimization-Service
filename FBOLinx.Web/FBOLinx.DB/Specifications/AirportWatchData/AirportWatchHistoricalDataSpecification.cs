﻿using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.DB.Models;

namespace FBOLinx.DB.Specifications.AirportWatchData
{
    public sealed class AirportWatchHistoricalDataSpecification : Specification<AirportWatchHistoricalData>
    {
        public AirportWatchHistoricalDataSpecification(string atcFlightNumber, DateTime startDate)
            : base(x => x.AtcFlightNumber == atcFlightNumber && x.AircraftPositionDateTimeUtc >= startDate)
        {
        }
    }
}
