using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Requests.Integrations
{
    public class SendOrderNotificationRequest
    {
        public string TailNumber { get; set; }
        public string Arrival { get; set; }
        public string Departure { get; set; }
        public double QuotedVolume { get; set; }
        public string CallSign { get; set; }
    }
}
