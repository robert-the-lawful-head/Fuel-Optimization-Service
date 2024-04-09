using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServiceOrder
{
    public class ServiceOrderItemResponse : EntityResponseMessage<ServiceOrderItemDto>
    {
        public ServiceOrderItemResponse(bool success, string message) : base(success, message)
        {
        }

        public ServiceOrderItemResponse(ServiceOrderItemDto result) : base(result)
        {
        }
    }
}
