using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServiceOrder
{
    public class ServiceOrderListResponse : EntityResponseMessage<List<ServiceOrderDto>>
    {
        public ServiceOrderListResponse(bool success, string message) : base(success, message)
        {
        }

        public ServiceOrderListResponse(List<ServiceOrderDto> result) : base(result)
        {
        }
    }
}
