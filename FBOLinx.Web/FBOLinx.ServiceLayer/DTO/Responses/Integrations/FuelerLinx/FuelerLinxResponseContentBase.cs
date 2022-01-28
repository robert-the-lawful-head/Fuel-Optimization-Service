using System;

namespace FBOLinx.ServiceLayer.DTO.Responses.Integrations.FuelerLinx
{
    public class FuelerLinxResponseContentBase
    {
        public Int16 Error { get; set; }
        public int ErrorID { get; set; }
        public string ErrorMessage { get; set; }
    }
}
