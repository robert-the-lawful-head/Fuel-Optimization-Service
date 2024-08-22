using System;

namespace FBOLinx.Core.BaseModels.Api
{
    public interface IApiException
    {
        int ErrorCode { get; set; }
        Object ErrorContent { get; }
    }
}
