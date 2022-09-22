using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.AirportWatch
{
    public class FboHistoricalDataModel
    {
        public int AirportWatchHistoricalDataID { get; set; }
        public string AircraftHexCode { get; set; }
        public string AtcFlightNumber { get; set; }
        public DateTime AircraftPositionDateTimeUtc { get; set; }
        public AircraftStatusType AircraftStatus { get; set; }
        public string AircraftTypeCode { get; set; }
        public string Company { get; set; }
        public int CustomerId { get; set; }
        public string TailNumber { get; set; }
        public int AircraftId { get; set; }
        public string AirportICAO { get; set; }
        public int CustomerInfoByGroupID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string AircraftStatusDescription
        {
            get
            {
                if (string.IsNullOrEmpty(AtcFlightNumber))
                    return "";
                return FBOLinx.Core.Utilities.Enum.GetDescription(AircraftStatus);
            }
        }
    }
}
