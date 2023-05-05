using FBOLinx.Core.Enums;
using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class FboPreferencesDTO : FBOLinxBaseEntityModel<int>
    {
        public int Fboid { get; set; }
        public bool? CostCalculator { get; set; }
        public bool? OmitJetARetail { get; set; }
        public bool? OmitJetACost { get; set; }
        public bool? Omit100LLRetail { get; set; }
        public bool? Omit100LLCost { get; set; }
        public bool? EnableJetA { get; set; }
        public bool? EnableSaf { get; set; }
        public bool? OrderNotificationsEnabled { get; set; } = true;

    }
}
