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
            
            //IEnumerable<FlightLegDTO> historicalFlightLegs = await subject.GetHistoricalFlightLegs("KVNY", true, DateTime.Parse("2022-07-25 00:00:00"), DateTime.Parse("2022-07-26 23:30:00"), DateTime.Parse("2022-07-25 20:30:00"));
            IEnumerable<FlightLegDTO> historicalFlightLegs = await subject.GetArrivals(84, 225, "KBFI");

            Assert.IsNotEmpty(historicalFlightLegs);
        }

        [Test]
        public async Task SaveFlightLegData_RawXmlMessageProvided_SWIMFlightLegDataErrorCreated()
        {
            Arrange(services =>
            {
            });

            SWIMFlightLegDTO testFlightLeg = new SWIMFlightLegDTO();
            testFlightLeg.AircraftIdentification = "test";
            testFlightLeg.DepartureICAO = "test";
            testFlightLeg.ArrivalICAO = "test";
            testFlightLeg.ATD = DateTime.UtcNow;
            testFlightLeg.ETA = DateTime.UtcNow;
            testFlightLeg.DepartureCity = "test";
            testFlightLeg.ArrivalCity = "test";

            testFlightLeg.SWIMFlightLegDataMessages = new List<SWIMFlightLegDataDTO>()
            {
                new ()
                {
                    Altitude = 1,
                    Latitude = 2,
                    Longitude = 3,
                    ETA = DateTime.UtcNow,
                    MessageTimestamp = DateTime.UtcNow,
                    RawXmlMessage = "test",
                }
            };

            await subject.SaveFlightLegData(new List<SWIMFlightLegDTO>() { testFlightLeg });
        }
        
        [Test]
        public async Task SaveFlightLegData_RawXmlMessageForExistingLegProvided_SWIMFlightLegDataErrorCreated()
        {
            Arrange(services =>
            {
            });

            SWIMFlightLegDTO testFlightLeg = new SWIMFlightLegDTO();
            testFlightLeg.AircraftIdentification = "test";
            testFlightLeg.DepartureICAO = "KATL";
            testFlightLeg.ArrivalICAO = "CYYC";
            testFlightLeg.ATD = Convert.ToDateTime("2022-11-29 22:47:00.000");

            testFlightLeg.SWIMFlightLegDataMessages = new List<SWIMFlightLegDataDTO>()
            {
                new ()
                {
                    Altitude = 1,
                    Latitude = 2,
                    Longitude = 3,
                    ETA = DateTime.UtcNow,
                    MessageTimestamp = DateTime.UtcNow,
                    RawXmlMessage = "test2",
                }
            };

            await subject.SaveFlightLegData(new List<SWIMFlightLegDTO>() { testFlightLeg });
        }
    }
}
