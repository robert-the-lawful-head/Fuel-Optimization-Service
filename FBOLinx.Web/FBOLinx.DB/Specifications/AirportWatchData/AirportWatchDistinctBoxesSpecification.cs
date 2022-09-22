using System;
using FBOLinx.Core.BaseModels.Specifications;

namespace FBOLinx.DB.Specifications.AirportWatchDistinctBoxes
{
    public sealed class AirportWatchDistinctBoxesSpecification : Specification<DB.Models.AirportWatchDistinctBoxes>
    {
        public AirportWatchDistinctBoxesSpecification(int oid, bool isAllRecords)
            : base(x => (isAllRecords && x.Oid > oid) || (!isAllRecords && x.Oid == oid))
        {
        }

        public AirportWatchDistinctBoxesSpecification(string boxName)
            : base(x => x.BoxName == boxName)
        {
        }
    }
}