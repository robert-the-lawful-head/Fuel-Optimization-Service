using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.DTO
{
    public class NgxChartDataType
    {
        public string Name { get; set; }
        public List<NgxChartItemType> Series { get; set; }
    }
}
