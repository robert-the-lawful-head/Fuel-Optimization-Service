using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FBOLinx.Core.BaseModels.Entities;
using FBOLinx.Core.Enums;

namespace FBOLinx.DB.Models
{
    [Table("SWIMFlightLegDataErrors")]
    public class SWIMFlightLegDataError : FBOLinxBaseEntityModel<int>
    {
        public string XmlMessage { get; set; }

        public int SWIMFlightLegDataId { get; set; }
        public virtual SWIMFlightLegData SWIMFlightLegData { get; set; }
    }
}
