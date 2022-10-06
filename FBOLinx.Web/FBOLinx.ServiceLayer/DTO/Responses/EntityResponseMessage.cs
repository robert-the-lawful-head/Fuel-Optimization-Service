using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO.Responses
{
    public abstract class EntityResponseMessage<T> : UseCaseResponseMessage
    {
        public T Result { get; set; }

        protected EntityResponseMessage(bool success, string message) : base(success, message)
        {

        }

        protected EntityResponseMessage(T result) : base(true)
        {
            Result = result;
        }
    }
}
