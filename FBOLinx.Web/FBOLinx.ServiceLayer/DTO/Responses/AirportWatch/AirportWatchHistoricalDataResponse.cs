﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace FBOLinx.ServiceLayer.DTO.Responses.AirportWatch
{
    public class AirportWatchHistoricalDataResponse
    {
        public int AirportWatchHistoricalDataId { get; set; }
        public int CustomerInfoByGroupID { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public DateTime DateTime { get; set; }
        public string TailNumber { get; set; }
        public string FlightNumber { get; set; }
        public string HexCode { get; set; }
        public string AircraftType { get; set; }
        public string AircraftTypeCode { get; set; }
        public string Status { get; set; }
        public int? PastVisits { get; set; }
        public string Originated { get; set; }
        public string AirportIcao { get; set; }
        public int? VisitsToMyFbo { get; set; }
        public double? PercentOfVisits { get; set; }
        public int? ParkingAcukwikFBOHandlerId { get; set; }

        public bool IsConfirmedVisit
        {
            get
            {
                if (AirportWatchHistoricalParking == null)
                    return false;
                return AirportWatchHistoricalParking.IsConfirmed.GetValueOrDefault(true);
            }
        }

        public bool CustomerActionStatusEmailRequired { get; set; }
        public bool CustomerActionStatusSetupRequired { get; set; }
        public bool CustomerActionStatusTopCustomer { get; set; }
        public bool MoreThan2Badges { get; set; }
        public string ToolTipEmailRequired { get; set; }
        public string ToolTipSetupRequired { get; set; }
        public string ToolTipTopCustomer { get; set; }
        public string CustomerNeedsAttention { get; set; }

        public AirportWatchHistoricalParkingDto AirportWatchHistoricalParking { get; set; }
    }
}
