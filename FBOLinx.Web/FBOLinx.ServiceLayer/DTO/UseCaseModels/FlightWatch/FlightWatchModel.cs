using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.Extensions.Aircraft;
using Geolocation;
using SQLitePCL;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch
{
    public class FlightWatchModel
    {
        
        private List<AirportWatchHistoricalDataDto> _AirportWatchHistoricalDataCollection;
        private AirportPosition _AirportPosition;
        private List<FuelReqDto> _UpcomingFuelOrderCollection;
        private CustomerAircraftsViewModel _CustomerAircraft;
        private SWIMFlightLegDTO _SwimFlightLeg;
        private AirportWatchLiveDataDto _AirportWatchLiveData;
        private FlightLegStatus? _Status;
        private bool _DoesSWIMFlightLegNeedUpdate = false;

        public FlightWatchModel(AirportWatchLiveDataDto airportWatchLiveData, List<AirportWatchHistoricalDataDto> airportWatchHistoricalDataCollection, SWIMFlightLegDTO swimFlightLeg)
        {
            _SwimFlightLeg = swimFlightLeg;
            _AirportWatchHistoricalDataCollection = airportWatchHistoricalDataCollection;
            _AirportWatchLiveData = airportWatchLiveData;
        }

        public int? AirportWatchLiveDataId => _AirportWatchLiveData?.Oid;
        public int? SWIMFlightLegId => _SwimFlightLeg?.Oid;

        public int VisitsToMyFBO { get; set; }
        public int Arrivals { get; set; }
        public int Departures { get; set; }
        public string FocusedAirportICAO { get; set; }
        public DateTime? BoxTransmissionDateTimeUtc => _AirportWatchLiveData?.BoxTransmissionDateTimeUtc;

        public string AtcFlightNumber => string.IsNullOrEmpty(_AirportWatchLiveData?.AtcFlightNumber)
            ? _SwimFlightLeg?.AircraftIdentification
            : _AirportWatchLiveData.AtcFlightNumber;

        public int? AltitudeInStandardPressure => _AirportWatchLiveData?.AltitudeInStandardPressure;
        public int? GroundSpeedKts => _AirportWatchLiveData?.GroundSpeedKts;
        public double? TrackingDegree => _AirportWatchLiveData?.TrackingDegree;
        public int? VerticalSpeedKts => _AirportWatchLiveData?.VerticalSpeedKts;
        public int? TransponderCode => _AirportWatchLiveData?.TransponderCode;
        public string BoxName => _AirportWatchLiveData?.BoxName;
        public DateTime? AircraftPositionDateTimeUtc => _AirportWatchLiveData?.AircraftPositionDateTimeUtc;
        public string AircraftTypeCode => _AirportWatchLiveData?.AircraftTypeCode;
        public int? GpsAltitude => _AirportWatchLiveData?.GpsAltitude;
        public string AircraftHexCode => _AirportWatchLiveData?.AircraftHexCode;
        public string TailNumber => string.IsNullOrEmpty(_AirportWatchLiveData?.TailNumber) ? _SwimFlightLeg?.AircraftIdentification : _AirportWatchLiveData.TailNumber;

        public string FlightDepartment => string.IsNullOrEmpty(_SwimFlightLeg?.FlightDepartment) ? _CustomerAircraft?.Company : _SwimFlightLeg?.FlightDepartment;

        public string Make => string.IsNullOrEmpty(_CustomerAircraft?.Make) ? _SwimFlightLeg?.FAAMake : _CustomerAircraft?.Make;
        public string Model => string.IsNullOrEmpty(_CustomerAircraft?.Model) ? _SwimFlightLeg?.FAAModel : _CustomerAircraft?.Model;
        public string FAAMake => _SwimFlightLeg?.FAAMake;
        public string FAAModel => _SwimFlightLeg?.FAAModel;
        public double? FuelCapacityGal => _CustomerAircraft?.FuelCapacityGal;

        public string Origin
        {
            get
            {
                if (string.IsNullOrEmpty(FocusedAirportICAO))
                    return "";
                return FocusedAirportICAO == DepartureICAO ? ArrivalICAO : DepartureICAO;
            }
        }

        public string City
        {
            get
            {
                if (string.IsNullOrEmpty(FocusedAirportICAO))
                    return "";
                return FocusedAirportICAO == DepartureICAO ? ArrivalCity : DepartureCity;
            }
        }

        public string DepartureICAO => _SwimFlightLeg?.DepartureICAO;
        public string DepartureCity => _SwimFlightLeg?.DepartureCity;
        public string ArrivalICAO => _SwimFlightLeg?.ArrivalICAO;
        public string ArrivalCity => _SwimFlightLeg?.ArrivalCity;
        public DateTime? ATDLocal => _SwimFlightLeg?.ATDLocal;
        public DateTime? ATDZulu => _SwimFlightLeg?.ATD;
        public DateTime? ETALocal => _SwimFlightLeg?.ETALocal;
        public DateTime? ETAZulu => _SwimFlightLeg?.ETA;
        public TimeSpan? ETE => (_SwimFlightLeg?.ETA).HasValue ? ((_SwimFlightLeg?.ETA).GetValueOrDefault() - DateTime.UtcNow).Duration() : null;
        public double? ActualSpeed => (_AirportWatchLiveData?.GroundSpeedKts).HasValue ? _AirportWatchLiveData?.GroundSpeedKts : _SwimFlightLeg?.ActualSpeed;
        public double? Altitude => (_AirportWatchLiveData?.GpsAltitude).HasValue ? _AirportWatchLiveData?.GpsAltitude : _SwimFlightLeg?.Altitude;
        public double? Latitude => (_AirportWatchLiveData?.Latitude).HasValue ? _AirportWatchLiveData?.Latitude : _SwimFlightLeg?.Latitude;
        public double? Longitude => (_AirportWatchLiveData?.Longitude).HasValue ? _AirportWatchLiveData?.Longitude : _SwimFlightLeg?.Longitude;

        public bool? IsAircraftOnGround => (_AirportWatchLiveData?.IsAircraftOnGround).HasValue
            ? _AirportWatchLiveData?.IsAircraftOnGround
            : _SwimFlightLeg?.IsAircraftOnGround;

        public string ITPMarginTemplate => _CustomerAircraft?.PricingTemplateName;
        public int? PricingTemplateId => _CustomerAircraft?.PricingTemplateId;

        public FlightLegStatus? Status
        {
            get
            {
                if (!_Status.HasValue)
                    return _SwimFlightLeg?.Status;
                return _Status;
            }
            set
            {
                _Status = value;
                if (_SwimFlightLeg != null)
                {
                    _SwimFlightLeg.Status = value;
                    _DoesSWIMFlightLegNeedUpdate = true;
                }
            }
        }

        public string Phone => _CustomerAircraft?.Phone;
        public int? ID => _UpcomingFuelOrderCollection?.FirstOrDefault()?.Oid;
        public int? FuelOrderId => _UpcomingFuelOrderCollection?.FirstOrDefault()?.Oid;
        public int? FuelerlinxID => _UpcomingFuelOrderCollection?.FirstOrDefault()?.SourceId;
        public int? FuelerlinxFuelOrderId => _UpcomingFuelOrderCollection?.FirstOrDefault()?.SourceId;
        public string Vendor => _UpcomingFuelOrderCollection?.FirstOrDefault()?.Source;
        public string TransactionStatus => ID > 0 ? "LIVE" : "";
        public string ICAOAircraftCode => _CustomerAircraft?.ICAOAircraftCode;
        public bool IsInNetwork => (_CustomerAircraft?.IsInNetwork()).GetValueOrDefault();
        public bool IsOutOfNetwork => (_CustomerAircraft?.IsOutOfNetwork()).GetValueOrDefault();

        public bool IsActiveFuelRelease =>
            (_UpcomingFuelOrderCollection?.FirstOrDefault()?.IsActiveFuelRelease()).GetValueOrDefault();

        public bool IsFuelerLinxClient => (_CustomerAircraft?.IsFuelerLinxClient()).GetValueOrDefault();

        public string AircraftIdentification
        {
            get
            {
                if (!string.IsNullOrEmpty(TailNumber))
                    return TailNumber;
                if (!string.IsNullOrEmpty(AtcFlightNumber))
                    return AtcFlightNumber;
                return _AirportWatchLiveData?.AircraftHexCode;
            }
        }
        public DateTime? LastQuoted { get; set; }

        public SWIMFlightLegDTO GetSwimFlightLeg()
        {
            return _SwimFlightLeg;
        }

        public void SetAirportWatchHistoricalDataCollection(List<AirportWatchHistoricalDataDto> airportWatchHistoricalDataCollection)
        {
            _AirportWatchHistoricalDataCollection = airportWatchHistoricalDataCollection;
        }

        public List<AirportWatchHistoricalDataDto> GetAirportWatchHistoricalDataCollection()
        {
            return _AirportWatchHistoricalDataCollection?.OrderByDescending(x => x.AircraftPositionDateTimeUtc).ToList();
        }

        public void SetAirportPosition(AirportPosition airportPosition)
        {
            _AirportPosition = airportPosition;
        }

        public AirportPosition GetAirportPosition()
        {
            return _AirportPosition;
        }

        public void SetUpcomingFuelOrderCollection(List<FuelReqDto> upcomingFuelOrderCollection)
        {
            _UpcomingFuelOrderCollection = upcomingFuelOrderCollection;
        }

        public List<FuelReqDto> GetUpcomingFuelOrderCollection()
        {
            return _UpcomingFuelOrderCollection;
        }

        public void SetCustomerAircraft(CustomerAircraftsViewModel customerAircraft)
        {
            _CustomerAircraft = customerAircraft;
        }

        public CustomerAircraftsViewModel GetCustomerAircraft()
        {
            return _CustomerAircraft;
        }

        public void MarketSWIMFlightLegAsNeedingUpdate()
        {
            _DoesSWIMFlightLegNeedUpdate = true;
        }

        public bool DoesSWIMFlightLegNeedUpdate()
        {
            return _DoesSWIMFlightLegNeedUpdate;
        }

        public Geolocation.Coordinate GetCoordinates()
        {
            if (_AirportWatchLiveData != null && _AirportWatchLiveData.Latitude != 0 &&
                _AirportWatchLiveData.Longitude != 0)
                return new Coordinate(_AirportWatchLiveData.Latitude, _AirportWatchLiveData.Longitude);
            if (_SwimFlightLeg != null && _SwimFlightLeg.Latitude.GetValueOrDefault() != 0 &&
                _SwimFlightLeg.Longitude.GetValueOrDefault() != 0)
                return new Geolocation.Coordinate(_SwimFlightLeg.Latitude.GetValueOrDefault(),
                    _SwimFlightLeg.Longitude.GetValueOrDefault());
            return new Coordinate();
        }
    }
}
