using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.DTO.SWIM;

namespace FBOLinx.ServiceLayer.BusinessServices.SWIM
{
    public interface ISWIMService
    {
        Task SaveFlightLegData(IEnumerable<SWIMFlightLegDTO> flightLegs);
    }
}
