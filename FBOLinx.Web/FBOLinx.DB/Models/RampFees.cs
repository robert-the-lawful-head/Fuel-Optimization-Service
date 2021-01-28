using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public partial class RampFees
    {
        private RampFeeCategories? _CategoryType = RampFeeCategories.Notset;
        private int? _CategoryMinValue;
        private string _CategoryDescription = "";

        public enum RampFeeCategories: short
        {
            [Description("Not Set")]
            Notset = 0,
            [Description("Aircraft Size")]
            AircraftSize = 1,
            [Description("Aircraft Type")]
            AircraftType = 2,
            [Description("Weight Range")]
            WeightRange = 3,
            [Description("Wingspan")]
            Wingspan = 4,
            [Description("Tailnumber")]
            TailNumber = 5
        }

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public double? Price { get; set; }
        public AirCrafts.AircraftSizes? Size { get; set; }
        public double? Waived { get; set; }
        [Column("FBOID")]
        public int? Fboid { get; set; }

        public RampFeeCategories? CategoryType
        {
            get
            {
                if ((!_CategoryType.HasValue || _CategoryType.Value == RampFeeCategories.Notset) && (Size.HasValue && Size.Value != AirCrafts.AircraftSizes.NotSet))
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
                    return (int) Size.Value;
                return _CategoryMinValue;
            }
            set { _CategoryMinValue = value; }
        }

        public int? CategoryMaxValue { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CategoryStringValue { get; set; }
        public DateTime? LastUpdated { get; set; }

        public string SizeDescription
        {
            get
            {
                if (CategoryType.GetValueOrDefault() == RampFeeCategories.AircraftSize)
                    return FBOLinx.Core.Utilities.Enum.GetDescription((AirCrafts.AircraftSizes) CategoryMinValue);
                return "";
            }
        }

        [NotMapped]
        public string CategoryDescription
        {
            get
            {
                if (string.IsNullOrEmpty(_CategoryDescription))
                    return FBOLinx.Core.Utilities.Enum.GetDescription(CategoryType.GetValueOrDefault());
                return _CategoryDescription;
            }
            set
            {
                _CategoryDescription = value;
            }
        }

        public string CategorizationDescription
        {
            get
            {
                switch (CategoryType)
                {
                    case RampFeeCategories.AircraftType:
                        return "Applies to all  " + CategoryDescription + ".";
                    case RampFeeCategories.AircraftSize:
                        return "Applies to all aircraft with the size: " + FBOLinx.Core.Utilities.Enum.GetDescription((AirCrafts.AircraftSizes)(System.Convert.ToInt16(CategoryMinValue))) + ".";
                    case RampFeeCategories.TailNumber:
                        return "Applies to the following tails: " + CategoryStringValue;
                    case RampFeeCategories.WeightRange:
                        return "Applies to aircraft weighing between " + CategoryMinValue + " lbs. and " +
                               CategoryMaxValue + " lbs.";
                    case RampFeeCategories.Wingspan:
                        return "Applies to aircraft with a wingspan between " + CategoryMinValue + " ft. and " + CategoryMaxValue + " ft.";
                    default:
                        return "Applies to all aircraft.";
                }
            }
        }

        public int GetCategoryPriority()
        {
            switch (CategoryType)
            {
                case RampFeeCategories.TailNumber:
                    return 1;
                case RampFeeCategories.AircraftType:
                    return 2;
                case RampFeeCategories.AircraftSize:
                    return 3;
                case RampFeeCategories.Wingspan:
                    return 4;
                case RampFeeCategories.WeightRange:
                    return 5;
                default:
                    return 6;
            }
        }
    }
}
