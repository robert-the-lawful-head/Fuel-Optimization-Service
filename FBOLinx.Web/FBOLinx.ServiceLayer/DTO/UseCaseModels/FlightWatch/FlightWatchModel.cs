﻿using System;
using System.Collections.Generic;
using System.Linq;
using FBOLinx.Core.Constants;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Airport;
using FBOLinx.ServiceLayer.Extensions.Aircraft;
using Geolocation;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.FlightWatch
{
    public class FlightWatchModel
    {
        private double? _TrackingDegree;
        private List<AirportWatchHistoricalDataDto> _AirportWatchHistoricalDataCollection;
        private AirportPosition _AirportPosition;
        private List<FuelReqDto> _UpcomingFuelOrderCollection;
        private CustomerAircraftsViewModel _CustomerAircraft;
        private SWIMFlightLegDTO _SwimFlightLeg;
        private AirportWatchLiveDataDto _AirportWatchLiveData;
        private FlightLegStatus? _Status;
        private bool _DoesSWIMFlightLegNeedUpdate = false;
        private DateTime _ValidPositionDateTimeUtc = DateTime.UtcNow.AddMinutes(-15);
        private AircraftHexTailMappingDTO _HexTailMapping;
        public FlightWatchModel(){  }
        public FlightWatchModel(AirportWatchLiveDataDto airportWatchLiveData,SWIMFlightLegDTO swimFlightLeg)
        {
            _SwimFlightLeg = swimFlightLeg;
            _AirportWatchLiveData = airportWatchLiveData;
        }
        public FlightWatchModel(AirportWatchLiveDataDto airportWatchLiveData, 
            List<AirportWatchHistoricalDataDto> airportWatchHistoricalDataCollection, 
            SWIMFlightLegDTO swimFlightLeg,
            AircraftHexTailMappingDTO hexTailMapping)
        {
            _HexTailMapping = hexTailMapping;
            _SwimFlightLeg = swimFlightLeg;
            _AirportWatchHistoricalDataCollection = airportWatchHistoricalDataCollection;
            _AirportWatchLiveData = airportWatchLiveData;
        }
        public string FAARegisteredOwner => string.IsNullOrEmpty(_HexTailMapping?.FAARegisteredOwner) ? _SwimFlightLeg?.FAARegisteredOwner : _HexTailMapping.FAARegisteredOwner;
        public int? AirportWatchLiveDataId => _AirportWatchLiveData?.Oid;
        public long? SWIMFlightLegId => _SwimFlightLeg?.Oid;

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

        public double? TrackingDegree
        {
            get
            {
                if (_TrackingDegree.HasValue)
                    return _TrackingDegree.GetValueOrDefault();
                return _AirportWatchLiveData?.TrackingDegree;
            }
            
        }
        public int? VerticalSpeedKts => _AirportWatchLiveData?.VerticalSpeedKts;
        public int? TransponderCode => _AirportWatchLiveData?.TransponderCode;
        public string BoxName => _AirportWatchLiveData?.BoxName;
        public DateTime? AircraftPositionDateTimeUtc => _AirportWatchLiveData?.AircraftPositionDateTimeUtc;
        public DateTime? SwimLastUpdated => _SwimFlightLeg?.LastUpdated;
        public DateTime? DateCreated => _AirportWatchLiveData?.CreatedDateTime;
        public string AircraftTypeCode => _AirportWatchLiveData?.AircraftTypeCode;
        public int? GpsAltitude => _AirportWatchLiveData?.GpsAltitude;
        public string AircraftHexCode => _AirportWatchLiveData?.AircraftHexCode;
        public string TailNumber => string.IsNullOrEmpty(_AirportWatchLiveData?.TailNumber) ? _SwimFlightLeg?.AircraftIdentification : _AirportWatchLiveData.TailNumber;
        
        public string FlightDepartment => string.IsNullOrEmpty(_SwimFlightLeg?.FlightDepartment) ? _CustomerAircraft?.Company : _SwimFlightLeg?.FlightDepartment;

        //Use customer make first, then HexTailMapping, then SWIM
        public string Make => string.IsNullOrEmpty(_CustomerAircraft?.Make) ?
            FAAMake : 
            _CustomerAircraft?.Make;
        //Use customer model first, then HexTailMapping, then SWIM
        public string Model => string.IsNullOrEmpty(_CustomerAircraft?.Model) ?
            FAAModel : 
            _CustomerAircraft?.Model;
        public string FAAMake => string.IsNullOrEmpty(_HexTailMapping?.FaaAircraftMakeModelReference?.MFR) ? _SwimFlightLeg?.FAAMake : _HexTailMapping?.FaaAircraftMakeModelReference?.MFR;
        public string FAAModel => string.IsNullOrEmpty(_HexTailMapping?.FaaAircraftMakeModelReference?.MODEL) ? _SwimFlightLeg?.FAAModel : _HexTailMapping?.FaaAircraftMakeModelReference?.MODEL;
        public double? FuelCapacityGal => _CustomerAircraft?.FuelCapacityGal;

        public string Origin
        {
            get
            {
                if (string.IsNullOrEmpty(FocusedAirportICAO))
                    return string.Empty;
                return FocusedAirportICAO == DepartureICAO ? ArrivalICAO : DepartureICAO;
            }
        }

        public string City
        {
            get
            {
                if (string.IsNullOrEmpty(FocusedAirportICAO))
                    return string.Empty;
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
        public TimeSpan? ETE => ((_SwimFlightLeg?.ETA).HasValue && _SwimFlightLeg?.ETA.Value >= DateTime.UtcNow) ? ((_SwimFlightLeg?.ETA).GetValueOrDefault() - DateTime.UtcNow).Duration() : null;
        public double? ActualSpeed => (_AirportWatchLiveData?.GroundSpeedKts).HasValue ? _AirportWatchLiveData?.GroundSpeedKts : _SwimFlightLeg?.ActualSpeed;
        public double? Altitude => (_AirportWatchLiveData?.GpsAltitude).HasValue ? _AirportWatchLiveData?.GpsAltitude : _SwimFlightLeg?.Altitude;
        public double? Latitude
        {
            get
            {
                if (this.SourceOfCoordinates == FlightWatchConstants.CoordinatesSource.Antenna)
                    return _AirportWatchLiveData?.Latitude;
                if (this.SourceOfCoordinates == FlightWatchConstants.CoordinatesSource.Swim)
                    return _SwimFlightLeg?.Latitude;
                return null;
            }
        }

        public double? Longitude
        {
            get
            {
                if (this.SourceOfCoordinates == FlightWatchConstants.CoordinatesSource.Antenna)
                    return _AirportWatchLiveData?.Longitude;
                if (this.SourceOfCoordinates == FlightWatchConstants.CoordinatesSource.Swim)
                    return _SwimFlightLeg?.Longitude;
                return null;
            }
        }
        public string SourceOfCoordinates
        {
            get
            {
                var sourceOfCoordinates = FlightWatchConstants.CoordinatesSource.None;
                switch (this.PositionDateTimeSource)
                {
                    case FlightWatchConstants.PositionDateTimeSource.SwimLastUpdate:
                       if (_SwimFlightLeg?.LastUpdated.GetValueOrDefault() >= _ValidPositionDateTimeUtc &&
(_SwimFlightLeg?.Longitude).HasValue)
                            sourceOfCoordinates = FlightWatchConstants.CoordinatesSource.Swim;
                        break;
                    case FlightWatchConstants.PositionDateTimeSource.BoxTransmissionDateTimeUtc:
                       if (_AirportWatchLiveData?.BoxTransmissionDateTimeUtc >=
_ValidPositionDateTimeUtc && (_AirportWatchLiveData?.Longitude).HasValue)
                            sourceOfCoordinates = FlightWatchConstants.CoordinatesSource.Antenna;
                        break;
                    case FlightWatchConstants.PositionDateTimeSource.AircraftPositionDateTimeUtc:
                       if (_AirportWatchLiveData?.AircraftPositionDateTimeUtc >=
_ValidPositionDateTimeUtc && (_AirportWatchLiveData?.Longitude).HasValue)
                            sourceOfCoordinates = FlightWatchConstants.CoordinatesSource.Antenna;
                        break;
                    case FlightWatchConstants.PositionDateTimeSource.CreatedDateTime:
                       if (_AirportWatchLiveData?.CreatedDateTime.GetValueOrDefault() >=
_ValidPositionDateTimeUtc && (_AirportWatchLiveData?.Longitude).HasValue)
                            sourceOfCoordinates = FlightWatchConstants.CoordinatesSource.Antenna;
                        break;
                }
                return sourceOfCoordinates;
            }
        }
        public string PositionDateTimeSource
        {
            get
            {
                var gratestLiveDataDate = FlightWatchConstants.PositionDateTimeSource.CreatedDateTime;
                if (this.SwimLastUpdated.GetValueOrDefault() > this.DateCreated.GetValueOrDefault() && this.SwimLastUpdated.GetValueOrDefault() > this.AircraftPositionDateTimeUtc.GetValueOrDefault() && this.SwimLastUpdated.GetValueOrDefault() > this.BoxTransmissionDateTimeUtc.GetValueOrDefault())
                    gratestLiveDataDate = FlightWatchConstants.PositionDateTimeSource.SwimLastUpdate;
                else if (this.AircraftPositionDateTimeUtc.GetValueOrDefault() > this.DateCreated.GetValueOrDefault() && this.AircraftPositionDateTimeUtc.GetValueOrDefault() > this.BoxTransmissionDateTimeUtc.GetValueOrDefault())
                    gratestLiveDataDate = FlightWatchConstants.PositionDateTimeSource.AircraftPositionDateTimeUtc;
                else if (this.BoxTransmissionDateTimeUtc.GetValueOrDefault() > this.DateCreated.GetValueOrDefault() && this.BoxTransmissionDateTimeUtc.GetValueOrDefault() > this.AircraftPositionDateTimeUtc.GetValueOrDefault())
                    gratestLiveDataDate = FlightWatchConstants.PositionDateTimeSource.BoxTransmissionDateTimeUtc;

                return gratestLiveDataDate;
            }
        }

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
        public int? FuelerlinxCompanyId => _CustomerAircraft?.FuelerlinxCompanyId;
        public string Vendor => _UpcomingFuelOrderCollection?.FirstOrDefault()?.Source;
        public string TransactionStatus => ID > 0 ? FlightWatchConstants.TransactionStatus.Live : string.Empty;
        public string ICAOAircraftCode => _CustomerAircraft?.ICAOAircraftCode?.Trim() ?? _SwimFlightLeg?.ICAOAircraftCode?.Trim();
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

        public int? CustomerInfoByGroupId => _CustomerAircraft?.CustomerInfoByGroupId;
        public DateTime? LastQuoteDate { get; set; }

        public string LastQuote =>
            (!LastQuoteDate.HasValue ? string.Empty: LastQuoteDate.GetValueOrDefault().ToShortDateString());

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
            IsCustomerManagerAircraft = customerAircraft != null;
            FavoriteAircraft = customerAircraft?.FavoriteAircraft;
            CustomerAircraftId = customerAircraft?.Oid;
        }

        public CustomerAircraftsViewModel GetCustomerAircraft()
        {
            return _CustomerAircraft;
        }

        public void MarkSWIMFlightLegAsNeedingUpdate()
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

        public void SetTrackingDegree(double trackingDegree)
        {
            _TrackingDegree = trackingDegree;
        }
        public FboFavoriteAircraft FavoriteAircraft { get; set; }
        public bool IsCustomerManagerAircraft { get; set; } =  false;
        public int? CustomerAircraftId { get; set; }
    }
}
