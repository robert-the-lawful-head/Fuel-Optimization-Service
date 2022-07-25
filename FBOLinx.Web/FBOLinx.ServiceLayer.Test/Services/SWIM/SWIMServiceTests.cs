using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.SWIM;
using FBOLinx.ServiceLayer.DTO.SWIM;
using FBOLinx.Web.Auth;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace FBOLinx.ServiceLayer.Test.Services.SWIM
{
    public class SWIMServiceTests : BaseTestFixture<ISWIMService>
    {
        [Test]
        public async Task GetFlightStatus()
        {
            Arrange(services =>
            {
            });
            
            IEnumerable<FlightLegDTO> historicalFlightLegs = await subject.GetHistoricalFlightLegs("KVNY", true, DateTime.Parse("2022-07-25 00:00:00"), DateTime.Parse("2022-07-26 23:30:00"), DateTime.Parse("2022-07-25 20:30:00"));

            Assert.IsNotEmpty(historicalFlightLegs);
        }
    }
}
