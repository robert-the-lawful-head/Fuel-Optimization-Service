using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Services;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.Web.Models.Responses
{
    public class GroupCustomerAnalyticsResponse
    {
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public string TailNumbers { get; set; }

        public int FboId { get; set; }

        public List<GroupedFboPrices> GroupCustomerFbos { get; set; }

        public void AddGroupedFboPrices(FlightTypeClassifications flightTypeClassification,
            List<CustomerWithPricing> pricing)
        {
            pricing.ForEach(x =>
            {
                var groupCustomerFbo =
                    this.GroupCustomerFbos.FirstOrDefault(r => r.Icao == x.Icao);
                if (groupCustomerFbo == null)
                {
                    groupCustomerFbo = new GroupedFboPrices() {
                        FboId = x.FboId,
                        Icao = x.Icao, 
                        Prices = new List<Prices>()
                    };
                    this.GroupCustomerFbos.Add(groupCustomerFbo);
                }

                var existingPrice = groupCustomerFbo.Prices.FirstOrDefault(p =>
                    Math.Abs(x.MinGallons.GetValueOrDefault() - p.MinGallons) < 0.0001);
                if (existingPrice == null)
                {
                    existingPrice = new Prices() {MinGallons = x.MinGallons.GetValueOrDefault()};
                    if (x.MaxGallons.GetValueOrDefault() <= 0 || x.MaxGallons.GetValueOrDefault() >= 9999)
                    {
                        existingPrice.VolumeTier =
                            Convert.ToDouble(x.MinGallons).ToString("#,##", CultureInfo.InvariantCulture) + "+";
                    }
                    else
                    {
                        existingPrice.VolumeTier =
                            Convert.ToDouble(x.MinGallons).ToString("#,##", CultureInfo.InvariantCulture) + " - " +
                            Convert.ToDouble(x.MaxGallons).ToString("#,##", CultureInfo.InvariantCulture);
                    }
                    groupCustomerFbo.Prices.Add(existingPrice);
                }

                existingPrice.SetPrice(x.AllInPrice.GetValueOrDefault(), x.Product, flightTypeClassification);
                existingPrice.PriceBreakdownDisplayType = x.PriceBreakdownDisplayType.GetValueOrDefault();
            });

        }
    }

    public class GroupedFboPrices
    {
        public int FboId { get; set; }
        public string Icao { get; set; }
        public List<Prices> Prices { get; set; }
    }

    public class Prices
    {
        public string VolumeTier { get; set; }
        public double? IntComm { get; set; }
        public double? IntPrivate { get; set; }
        public double? DomComm { get; set; }
        public double? DomPrivate { get; set; }
        public double MinGallons { get; set; }
        public PriceBreakdownDisplayTypes PriceBreakdownDisplayType { get; set; }

        public void SetPrice(double price, string product, FlightTypeClassifications flightTypeClassification)
        {
            if (string.IsNullOrEmpty(product))
                product = "";
            bool isInternational = product.ToLower().Contains("international") || !product.ToLower().Contains("domestic");
            bool isDomestic = product.ToLower().Contains("domestic") || !product.ToLower().Contains("international");
            if (flightTypeClassification == FlightTypeClassifications.Private)
            {
                IntPrivate = (isInternational) ? price : IntPrivate;
                DomPrivate = (isDomestic) ? price : DomPrivate;
            }
            else
            {
                IntComm = (isInternational) ? price : IntComm;
                DomComm = (isDomestic) ? price : DomComm;
            }
        }
    }
}
