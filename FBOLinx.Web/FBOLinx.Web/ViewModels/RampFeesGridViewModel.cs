using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models;

namespace FBOLinx.Web.ViewModels
{
    public class RampFeesGridViewModel
    {
        private RampFees.RampFeeCategories? _CategoryType = RampFees.RampFeeCategories.Notset;
        private int? _CategoryMinValue;


        public int Oid { get; set; }
        public double? Price { get; set; }
        public AirCrafts.AircraftSizes? Size { get; set; }
        public double? Waived { get; set; }
        [Column("FBOID")]
        public int? Fboid { get; set; }

        public RampFees.RampFeeCategories? CategoryType
        {
            get
            {
                if ((!_CategoryType.HasValue || _CategoryType.Value == RampFees.RampFeeCategories.Notset) && (Size.HasValue && Size.Value != AirCrafts.AircraftSizes.NotSet))
                    return RampFees.RampFeeCategories.AircraftSize;
                return _CategoryType;
            }
            set { _CategoryType = value; }
        }

        public int? CategoryMinValue
        {
            get
            {
                if (_CategoryMinValue.HasValue && _CategoryMinValue.Value > 0)
                    return _CategoryMinValue;
                if (CategoryType.HasValue && CategoryType.Value == RampFees.RampFeeCategories.AircraftSize && Size.HasValue)
                    return (int)Size.Value;
                return _CategoryMinValue;
            }
            set { _CategoryMinValue = value; }
        }

        public int? CategoryMaxValue { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string SizeDescription
        {
            get
            {
                if (CategoryType.GetValueOrDefault() == RampFees.RampFeeCategories.AircraftSize)
                    return FBOLinx.Web.Utilities.Enum.GetDescription((AirCrafts.AircraftSizes)CategoryMinValue);
                return "";
            }
        }

        public string CategoryDescription
        {
            get { return FBOLinx.Web.Utilities.Enum.GetDescription(CategoryType.GetValueOrDefault()); }
        }

        public string AircraftMake { get; set; }
        public string AircraftModel { get; set; }
        public List<Models.AirCrafts> AppliesTo { get; set; }
    }
}
