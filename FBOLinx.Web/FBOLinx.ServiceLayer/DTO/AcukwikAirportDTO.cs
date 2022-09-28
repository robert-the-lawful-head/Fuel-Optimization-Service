using System.Collections.Generic;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.Service.Mapping.Dto
{
    public class AcukwikAirportDTO : BaseEntityModelDTO<AcukwikAirport>, IEntityModelDTO<AcukwikAirport, int>
    {
        public int Oid { get; set; }
        public string Icao { get; set; }
        public string Iata { get; set; }
        public string Faa { get; set; }
        public string FullAirportName { get; set; }
        public string AirportCity { get; set; }
        public string StateSubdivision { get; set; }
        public string Country { get; set; }
        public string AirportType { get; set; }
        public string DistanceFromCity { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double? Elevation { get; set; }
        public string Variation { get; set; }
        public double? IntlTimeZone { get; set; }
        public string DaylightSavingsYn { get; set; }
        public string FuelType { get; set; }
        public string AirportOfEntry { get; set; }
        public string Customs { get; set; }
        public string HandlingMandatory { get; set; }
        public string SlotsRequired { get; set; }
        public string Open24Hours { get; set; }
        public string ControlTowerHours { get; set; }
        public string ApproachList { get; set; }
        public string PrimaryRunwayId { get; set; }
        public double? RunwayLength { get; set; }
        public double? RunwayWidth { get; set; }
        public string Lighting { get; set; }
        public string AirportNameShort { get; set; }
        public double? DistanceToSelectedAirport { get; set; }
        public bool IsUnitedStatesAirport
        {
            get
            {
                return (string.IsNullOrEmpty(Country) || Country.ToUpper() == "UNITED STATES" || Country.ToUpper() == "USA");
            }
        }

        public string ProperAirportIdentifier
        {
            get
            {
                if (!string.IsNullOrEmpty(Icao))
                    return Icao;
                if (IsUnitedStatesAirport && !string.IsNullOrEmpty(Faa))
                    return Faa;
                return Iata;
            }
        }
        public ICollection<AcukwikFbohandlerDetailDto> AcukwikFbohandlerDetailCollection { get; set; }
    }
}