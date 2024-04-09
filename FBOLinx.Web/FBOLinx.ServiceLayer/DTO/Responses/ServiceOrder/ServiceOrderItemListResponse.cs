using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses.ServiceOrder
{
    public class ServiceOrderItemListResponse : EntityResponseMessage<List<ServiceOrderItemDto>>
    {
        public ServiceOrderItemListResponse(bool success, string message) : base(success, message)
        {
        }

        public ServiceOrderItemListResponse(List<ServiceOrderItemDto> result) : base(result)
        {
        }
    }
}
