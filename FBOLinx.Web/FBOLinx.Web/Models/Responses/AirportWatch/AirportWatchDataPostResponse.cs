using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses.AirportWatch
{
    public class AirportWatchDataPostResponse : UseCaseResponseMessage
    {
        public AirportWatchDataPostResponse(bool success = false, string message = null) : base(success, message)
        {

        }
    }
}
