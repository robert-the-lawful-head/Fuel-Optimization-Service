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

        public void CastFromComplexType(Fuelerlinx.DB.Models.ComplexTypes.Transaction.TransactionFuelPriceResult item)
        {
            Oid =
            ActualPpg = 
            ActualVolume = 
            Archived = 
            Cancelled = 
            CustomerId = 
            DateCreated = 
            DispatchNotes = 
            Eta = 
            Etd = 
            Icao =
            Notes = 
            QuotedPpg = 
            QuotedVolume = 
            Source = 
            SourceId = 
            TimeStandard = 
            CustomerName = 
            TailNumber =
            FboName = 
            Email = 
            PhoneNumber = 
        }

        public static FuelReqsGridViewModel Cast(
            Fuelerlinx.DB.Models.ComplexTypes.Transaction.TransactionFuelPriceResult item)
        {
            TransactionFuelPriceResultDTO result = new TransactionFuelPriceResultDTO();
            result.CastFromComplexType(item);
            return result;
        }
    }
}
