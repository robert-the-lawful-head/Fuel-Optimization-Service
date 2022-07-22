using System;
using System.Collections.Generic;
using System.Text;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.Web.Auth;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace FBOLinx.ServiceLayer.Test.Services.SWIM
{
    public class SWIMServiceTests : BaseTestFixture<ISWIMService>
    {
        [Test]
        public void GetFlightStatus()
        {
            Arrange(services =>
            {
            });
            

        }
    }
}
