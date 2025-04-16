using System;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Utilities.DatesAndTimes;
using FBOLinx.ServiceLayer.BusinessServices.Airport;
using FBOLinx.ServiceLayer.DTO;
using Fuelerlinx.SDK;

namespace FBOLinx.Service.Mapping.Dto
{
    public partial class FuelReqDto
    {
        private string _TailNumber;
        private string _FboName;
        private string _PricingTemplateName;
        private DateTime? _ArrivalDateTimeLocal;
        private DateTime? _DepartureDateTimeLocal;

        public int Oid { get; set; }
        public int? CustomerId { get; set; }
        public string Icao { get; set; }
        public int? Fboid { get; set; }
        public int? CustomerAircraftID { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
        public string TimeStandard { get; set; }
        public bool? Cancelled { get; set; }
        public double? QuotedVolume { get; set; }
        public double? QuotedPpg { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public double? ActualVolume { get; set; }
        public double? ActualPpg { get; set; }
        public string Source { get; set; }
        public int? SourceId { get; set; }
        public string DispatchNotes { get; set; }
        public bool? Archived { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FuelOn { get; set; } = "Departure";
        public string CustomerName { get; set; }
        public string CustomerNotes { get; set; }
        public string PaymentMethod { get; set; }
        public string TimeZone { get; set; }
        public bool? IsConfirmed { get; set; }
        public string TailNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(_TailNumber))
                    return _TailNumber;
                return CustomerAircraft?.TailNumber;
            }
            set => _TailNumber = value;
        }
        public bool? ShowConfirmationButton { get; set; } = false;

        public string FboName
        {
            get
            {
                if (!string.IsNullOrEmpty(_FboName))
                    return _FboName;
                return Fbo?.Fbo;
            }
            set
            {
                _FboName = value;
            }
        }

        public string PricingTemplateName
        {
            get
            {
                if (!string.IsNullOrEmpty(_PricingTemplateName))
                    return _PricingTemplateName;
                return FuelReqPricingTemplate?.PricingTemplateName;
            }
            set
            {
                _PricingTemplateName = value;
            }
        }

        public CustomersDto Customer { get; set; }
        public CustomerAircraftsDto CustomerAircraft { get; set; }
        public FbosDto Fbo { get; set; }
        public FuelReqPricingTemplateDto FuelReqPricingTemplate { get; set; }
        public ServiceOrderDto ServiceOrder { get; set; }
        public DateTime? ArrivalDateTimeLocal => _ArrivalDateTimeLocal;
        public DateTime? DepartureDateTimeLocal => _DepartureDateTimeLocal;

        public void CastFromFuelerLinxTransaction(Fuelerlinx.SDK.TransactionDTO item, string companyName, bool isConfirmed)
        {
            Oid = 0;
            ActualPpg = 0;
            ActualVolume = item.InvoicedVolume.Amount;
            Archived = item.Archived;
            Cancelled = false;
            CustomerId = item.CompanyId;
            DateCreated = item.CreationDate;
            DispatchNotes = "";
            Eta = item.ArrivalDateTime;
            Etd = item.DepartureDateTime;
            Icao = item.Icao.Trim();
            Notes = "";
            QuotedPpg = 0;
            QuotedVolume = item.DispatchedVolume.Amount;
            Source = item.FuelVendor;
            SourceId = item.Id;
            TimeStandard = DateTimeHelper.GetTimeStandardOffset((Core.Enums.TimeFormats)item.TimeStandard);
            TailNumber = item.TailNumber;
            FboName = item.Fbo;
            Email = "";
            PhoneNumber = "";
            FuelOn = item.TransactionDetails.FuelOn;
            CustomerName = companyName;
            IsConfirmed = isConfirmed;
        }

        public static FuelReqDto Cast(TransactionDTO transaction, string companyName, Fuelerlinx.SDK.GeneralAirportInformation airport, bool isConfirmed = false)
        {
            try
            {
                FuelReqDto fuelRequest = new FuelReqDto();
                fuelRequest.CastFromFuelerLinxTransaction(transaction, companyName, isConfirmed);
                SetAirportLocalTimes(fuelRequest, airport);
                SetCustomerNotesAndPaymentMethod(transaction, fuelRequest);

                return fuelRequest;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Error casting FuelReqDto from Fuelerlinx Transaction", ex);
            }

            return new FuelReqDto();
        }
        private static DateTime GetUtcTimeFromFuelerLinxServerTime(DateTime pacificDateime)
        {
            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeToUtc(pacificDateime, pstZone);
        }
        private static void SetCustomerNotesAndPaymentMethod(TransactionDTO transaction, FuelReqDto fuelRequest)
        {
            if (transaction.TransactionDetails.CopyFbo.HasValue && transaction.TransactionDetails.CopyFbo.Value)
            {
                if (transaction.TransactionNotes != null)
                {
                    var fboNotes = transaction.TransactionNotes.Where(t => t.NoteType == TransactionNoteTypes.FboNote).FirstOrDefault();
                    if (fboNotes != null)
                        fuelRequest.CustomerNotes = fboNotes.Note;
                }

                if (transaction.TransactionDetails.SendPaymentToFbo.HasValue && transaction.TransactionDetails.SendPaymentToFbo.Value)
                    fuelRequest.PaymentMethod = transaction.TransactionDetails.PaymentMethod;
            }
        }
        public static void SetAirportLocalTimes(FuelReqDto fuelRequest, Fuelerlinx.SDK.GeneralAirportInformation airport)
        {

            if (fuelRequest.TimeStandard == "Z")
            {
                fuelRequest.Eta = GetAirportLocalTime(fuelRequest.Eta.GetValueOrDefault(), airport);
                fuelRequest.Etd = GetAirportLocalTime(fuelRequest.Etd.GetValueOrDefault(), airport);
            }
            else
            {
                fuelRequest.Eta = fuelRequest.Eta.GetValueOrDefault();
                fuelRequest.Etd = fuelRequest.Etd.GetValueOrDefault();
            }
            fuelRequest.TimeZone = DateTimeHelper.GetLocalTimeZone(airport?.IntlTimeZone, airport?.AirportCity);

        }
        public static DateTime GetAirportLocalTime(DateTime date, Fuelerlinx.SDK.GeneralAirportInformation airport)
        {
            if (airport == null)
                return date;
            return DateTimeHelper.GetLocalTime(
                date, airport.IntlTimeZone, airport.RespectDaylightSavings);
        }

        public async Task PopulateLocalTimes(IAirportTimeService airportTimeService)
        {
            _ArrivalDateTimeLocal = await airportTimeService.GetAirportLocalDateTime(Fboid.GetValueOrDefault(), Etd);
            if (Etd.HasValue)
                _DepartureDateTimeLocal = await airportTimeService.GetAirportLocalDateTime(Fboid.GetValueOrDefault(), Etd);
        }
    }
}