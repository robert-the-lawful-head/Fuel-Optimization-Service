using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServiceOrder
{
    public class ServiceOrderResponse : EntityResponseMessage<ServiceOrderDto>
    {
        public ServiceOrderResponse(bool success, string message) : base(success, message)
        {
        }

        public ServiceOrderResponse(ServiceOrderDto result) : base(result)
        {
        }
    }
}
