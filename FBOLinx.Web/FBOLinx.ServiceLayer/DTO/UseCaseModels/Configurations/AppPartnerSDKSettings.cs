using System;
using System.Collections.Generic;
using System.Text;

namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations
{
    public class AppPartnerSDKSettings
    {
        public FuelerlinxSDKSettings FuelerLinx { get; set; }
        public JetNetAPISettings JetNet { get; set; }

        #region Objects

        public class FuelerlinxSDKSettings
        {
            public string APIEndpoint { get; set; }
            public string APIKey { get; set; }
        }

        public class JetNetAPISettings
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        #endregion
    }
}
