using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;

namespace FBOLinx.ServiceLayer.DTO
{
    public class RampFeesGridViewModel
    {
        private RampFeeCategories? _CategoryType = RampFeeCategories.Notset;
        private int? _CategoryMinValue;


        public int Oid { get; set; }
        public double? Price { get; set; }
        public AircraftSizes? Size { get; set; }
        public double? Waived { get; set; }
        [Column("FBOID")]
        public int? Fboid { get; set; }

        public RampFeeCategories? CategoryType
        {
            get
            {
                if ((!_CategoryType.HasValue || _CategoryType.Value == RampFeeCategories.Notset) && (Size.HasValue && Size.Value != AircraftSizes.NotSet))
                    return RampFeeCategories.AircraftSize;
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
                if (CategoryType.HasValue && CategoryType.Value == RampFeeCategories.AircraftSize && Size.HasValue)
                    return (int)Size.Value;
                return _CategoryMinValue;
            }
            set { _CategoryMinValue = value; }
        }

        public int? CategoryMaxValue { get; set; }
        public string CategoryStringValue { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string SizeDescription
        {
            get
            {
                if (CategoryType.GetValueOrDefault() == RampFeeCategories.AircraftSize)
                    return FBOLinx.Core.Utilities.Enum.GetDescription((AircraftSizes)CategoryMinValue);
                return "";
            }
        }

        public string CategoryDescription
        {
            get { return FBOLinx.Core.Utilities.Enum.GetDescription(CategoryType.GetValueOrDefault()); }
        }

        public string AircraftMake { get; set; }
        public string AircraftModel { get; set; }
        public List<AirCrafts> AppliesTo { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
