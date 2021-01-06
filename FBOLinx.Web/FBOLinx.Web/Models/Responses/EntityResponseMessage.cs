using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Models.Responses
{
    public class EntityResponseMessage<T> : UseCaseResponseMessage
    {
        public T Result { get; set; }

        public EntityResponseMessage(bool success = false, string message = null) : base(success, message)
        {

        }

        public EntityResponseMessage(T result) : base(true)
        {
            Result = result;
        }
    }
}
