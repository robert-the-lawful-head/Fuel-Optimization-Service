using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.ViewModels
{
    public class FuelReqsTotalOrdersByMonthViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double AverageJetACost { get; set; }
        public double AverageJetARetail { get; set; }
        public int TotalOrders { get; set; }

        public string MonthName
        {
            get
            {
                return new DateTime(Year, Month, 1).ToString("MMM", CultureInfo.InvariantCulture);
            }
        }

        public string AverageJetACostFormatted
        {
            get { return AverageJetACost.ToString("C", CultureInfo.CurrentCulture); }
        }

        public string AverageJetARetailFormatted
        {
            get { return AverageJetARetail.ToString("C", CultureInfo.CurrentCulture); }
        }
    }
}
