﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels
{
    public class MissedQuotesLogViewModel
    {
        public int Oid { get; set; }
        public int? FboId { get; set; }
        public string CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public string TailNumber { get; set; }
        public bool Emailed { get; set; }
        public int MissedQuotesCount { get; set; }
        public int CustomerInfoByGroupId { get; set; }
        public string ItpMarginTemplate { get; set; }
        public string Eta { get; set; }
        public string Etd { get; set; }
        public double Volume { get; set; }
    }
}
