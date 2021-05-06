using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Web.Services;
using Moq;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;

namespace FBOLinx.ServiceLayer.Test.Services
{
    public class PriceDistributionServiceTests : BaseTestFixture<IPriceDistributionService>
    {
        [Test]
        public async Task DistributePricing_Preview()
        {
            // Arrange
            

            // Act
            var mailServiceMock = new Mock<IMailService>();
            mailServiceMock.Setup(x => x.SendAsync(It.IsAny<FBOLinxMailMessage>())).Returns(Task.FromResult(true));

            //Assert
            
        }

       
    }
}
