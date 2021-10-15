using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class FuelReqsGridViewModel
    {
        public int Oid { get; set; }
        public int? CustomerId { get; set; }
        public string Icao { get; set; }
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
        public string CustomerName { get; set; }
        public string TailNumber { get; set; }
        public string FboName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PricingTemplateName { get; set; }

        public void CastFromFuelerLinxTransaction(Fuelerlinx.SDK.TransactionDTO item)
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
            Icao = item.Icao;
            Notes = "";
            QuotedPpg = 0;
            QuotedVolume = item.DispatchedVolume.Amount;
            Source = item.FuelVendor;
            SourceId = item.Id;
            TimeStandard = item.TimeStandard.GetValueOrDefault().ToString() == "0" ? "Z" : "L";
            CustomerName = item.CustomerName;
            TailNumber = item.TailNumber;
            FboName = item.Fbo;
            Email = "";
            PhoneNumber = "";
        }

        public static FuelReqsGridViewModel Cast(Fuelerlinx.SDK.TransactionDTO item)
        {
            FuelReqsGridViewModel result = new FuelReqsGridViewModel();
            result.CastFromFuelerLinxTransaction(item);
            return result;
        }
    }
}
