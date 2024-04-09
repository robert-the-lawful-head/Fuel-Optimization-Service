using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.Job.Models
{
    public class UseCaseResponseMessage
    {
        public bool Success { get; }
        public string Message { get; }

        protected UseCaseResponseMessage()
        {
        }
    }
}
