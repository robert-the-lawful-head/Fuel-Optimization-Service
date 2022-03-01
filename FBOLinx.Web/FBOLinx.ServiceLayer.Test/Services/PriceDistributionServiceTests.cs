using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.Web.Services;
using Moq;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;

namespace FBOLinx.ServiceLayer.Test.Services
{
    public class PriceDistributionServiceTests : BaseTestFixture<IPriceDistributionService>
    {
        private string fromEmail = "test";
        private string toEmail = "test2@fbolinx.com";
        private DistributePricingRequest distributePricingRequest;

        [Test]
        public async Task DistributePricing_PreviewRequestedAndCustomerExistsInTheRequest_DistributePricingEmailSent()
        {
            // Arrange
            distributePricingRequest = TestDataHelper.CreateDistributePreviewPricingRequest();
            distributePricingRequest.PreviewEmail = toEmail;
            Mock<IMailService> mailServiceMock = null;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                mockCommonServices(ref mailServiceMock, ref services, ref dbContextMock);
                
                IEnumerable<CustomerContacts> customerContacts = new List<CustomerContacts>()
                {
                    new CustomerContacts()
                    {
                        ContactId = distributePricingRequest.Customer.CustomerId,
                    }
                };
                Mock<DbSet<CustomerContacts>> customerContactsDbSetMock = customerContacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<CustomerContacts>())
                    .Returns(customerContactsDbSetMock.Object);

                IEnumerable<Contacts> contacts = new List<Contacts>()
                {
                    new Contacts()
                    {
                        Oid = distributePricingRequest.Customer.CustomerId,
                    }
                };
                Mock<DbSet<Contacts>> contactsDbSetMock = contacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<Contacts>())
                    .Returns(contactsDbSetMock.Object);
               
                services.AddSingleton(dbContextMock.Object);
            });

            // Act
            await subject.DistributePricing(distributePricingRequest, true);

            //Assert
            mailServiceMock.Verify(x => x.SendAsync(It.Is<FBOLinxMailMessage>(
                x => x.From.Address == MailService.GetFboLinxAddress(fromEmail) &&
                     x.To.Any(y => y.Address == toEmail))), Times.AtLeastOnce);
        }

        [Test]
        public async Task DistributePricing_TemplateSelected_DistributePricingEmailSent()
        {
            // Arrange
            distributePricingRequest = TestDataHelper.CreateDistributePricingRequest();
            Mock<IMailService> mailServiceMock = null;
            Arrange(services =>
            {
                var dbContextMock = new Mock<FboLinxContext>();
                mockCommonServices(ref mailServiceMock, ref services, ref dbContextMock);
  
                IEnumerable<CustomerInfoByGroup> customerInfoByGroup = new List<CustomerInfoByGroup>()
                {
                    new CustomerInfoByGroup()
                    {
                        GroupId = distributePricingRequest.GroupId,
                        CustomerId = distributePricingRequest.Customer.CustomerId
                    }
                };
                Mock<DbSet<CustomerInfoByGroup>> customerInfoByGroupDbSetMock = customerInfoByGroup.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<CustomerInfoByGroup>())
                    .Returns(customerInfoByGroupDbSetMock.Object);

                IEnumerable<Customers> customers = new List<Customers>()
                {
                    new Customers()
                    {
                        Oid = distributePricingRequest.Customer.CustomerId,
                        GroupId = distributePricingRequest.GroupId,
                        Suspended = false
                    }
                };
                Mock<DbSet<Customers>> customersDbSetMock = customers.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<Customers>())
                    .Returns(customersDbSetMock.Object);

                IEnumerable<CustomCustomerTypes> customCustomerTypes = new List<CustomCustomerTypes>()
                {
                    new CustomCustomerTypes()
                    {
                        CustomerId = distributePricingRequest.Customer.CustomerId,
                        CustomerType = 1
                    }
                };
                Mock<DbSet<CustomCustomerTypes>> customCustomerTypesDbSetMock = customCustomerTypes.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<CustomCustomerTypes>())
                    .Returns(customCustomerTypesDbSetMock.Object);

                IEnumerable<CustomerContacts> customerContacts = new List<CustomerContacts>()
                {
                    new CustomerContacts()
                    {
                        ContactId = 1,
                        CustomerId = distributePricingRequest.Customer.CustomerId
                    }
                };
                Mock<DbSet<CustomerContacts>> customerContactsDbSetMock = customerContacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<CustomerContacts>())
                    .Returns(customerContactsDbSetMock.Object);

                IEnumerable<Contacts> contacts = new List<Contacts>()
                {
                    new Contacts()
                    {
                        Oid = 1,
                    },
                    new Contacts()
                    {
                        CopyAlerts = true,
                        Email = "test@fuelerlinx.com"
                    }
                };
                Mock<DbSet<Contacts>> contactsDbSetMock = contacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<Contacts>())
                    .Returns(contactsDbSetMock.Object);

                IEnumerable<Fbocontacts> fboContacts = new List<Fbocontacts>()
                {
                    new Fbocontacts()
                    {
                        Fboid = distributePricingRequest.FboId,
                    }
                };
                Mock<DbSet<Fbocontacts>> fboContactsDbSetMock = fboContacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<Fbocontacts>())
                    .Returns(fboContactsDbSetMock.Object);

                IEnumerable<User> usersList = new List<User>()
                {
                    new User()
                    {
                        Username = toEmail,
                        CopyAlerts = true,
                        GroupId = distributePricingRequest.GroupId,
                        FboId = distributePricingRequest.FboId
                    }
                };
                Mock<DbSet<User>> userDbSetMock = usersList.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<User>())
                    .Returns(userDbSetMock.Object);

                IEnumerable<Fboprices> fboPrices = new List<Fboprices>()
                {
                    new Fboprices()
                    {
                        EffectiveFrom = DateTime.UtcNow,
                        EffectiveTo = DateTime.UtcNow.AddDays(1),
                        Fboid = distributePricingRequest.FboId,
                        Product = "JetA Cost"
                    }
                };
                Mock<DbSet<Fboprices>> fbopricesDbSetMock = fboPrices.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Set<Fboprices>())
                    .Returns(fbopricesDbSetMock.Object);

                services.AddSingleton(dbContextMock.Object);
            });

            // Act
            await subject.DistributePricing(distributePricingRequest, false);

            //Assert
            mailServiceMock.Verify(x => x.SendAsync(It.Is<FBOLinxMailMessage>(
                x => x.From.Address == MailService.GetFboLinxAddress(fromEmail) &&
                     x.To.Any(y => y.Address == toEmail))), Times.AtLeastOnce);
        }

        private void mockCommonServices(ref Mock<IMailService> mailServiceMock, ref ServiceCollection services, ref Mock<FboLinxContext> dbContextMock)
        {
            mailServiceMock = new Mock<IMailService>();
            mailServiceMock.Setup(x => x.SendAsync(It.IsAny<FBOLinxMailMessage>())).Returns(Task.FromResult(true));
            services.AddSingleton(mailServiceMock.Object);

            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
            services.AddSingleton(httpContextMock.Object);

            var fileStorageContextMock = new Mock<FilestorageContext>();
            Mock<DbSet<FboLinxImageFileData>> fboLinxImageFileDataMock = (new List<FboLinxImageFileData>()).AsQueryable().BuildMockDbSet();
            fileStorageContextMock.Setup(x => x.FboLinxImageFileData)
                .Returns(fboLinxImageFileDataMock.Object);

            services.AddSingleton(fileStorageContextMock.Object);

            IEnumerable<CustomerAircrafts> customerAircrafts = new List<CustomerAircrafts>()
                {
                    new CustomerAircrafts()
                    {
                        CustomerId = distributePricingRequest.Customer.CustomerId,
                        GroupId = distributePricingRequest.GroupId
                    }
                };
            Mock<DbSet<CustomerAircrafts>> customerAircraftsDbSetMock = customerAircrafts.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<CustomerAircrafts>())
                .Returns(customerAircraftsDbSetMock.Object);

            IEnumerable<ContactInfoByGroup> contactInfoByGroupList = new List<ContactInfoByGroup>()
                {
                    new ContactInfoByGroup()
                    {
                        ContactId = distributePricingRequest.Customer.CustomerId,
                        GroupId = distributePricingRequest.GroupId,
                        CopyAlerts = true,
                        Email = toEmail
                    }
                };
            Mock<DbSet<ContactInfoByGroup>> contactInfoByGroupDbSetMock = contactInfoByGroupList.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<ContactInfoByGroup>())
                .Returns(contactInfoByGroupDbSetMock.Object);

            IEnumerable<Fbos> fbos = new List<Fbos>()
                {
                    new Fbos()
                    {
                        Oid = distributePricingRequest.FboId,
                        SenderAddress = fromEmail,
                        ReplyTo = "test@fuelerlinx.com"
                    }
                };
            Mock<DbSet<Fbos>> fbosDbSetMock = fbos.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<Fbos>())
                .Returns(fbosDbSetMock.Object);

            IEnumerable<Fboairports> fboairports = new List<Fboairports>()
                {
                    new Fboairports()
                    {
                        Fboid = distributePricingRequest.FboId,
                    }
                };
            Mock<DbSet<Fboairports>> fboairportsDbSetMock = fboairports.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<Fboairports>())
                .Returns(fboairportsDbSetMock.Object);

            IEnumerable<EmailContent> emailContent = new List<EmailContent>()
                {
                    new EmailContent()
                    {
                        Oid = 1
                    }
                };
            Mock<DbSet<EmailContent>> emailContentDbSetMock = emailContent.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<EmailContent>())
                .Returns(emailContentDbSetMock.Object);

            IEnumerable<Fboprices> fboPrices = new List<Fboprices>()
                {
                    new Fboprices()
                    {
                        EffectiveFrom = DateTime.UtcNow,
                        EffectiveTo = DateTime.UtcNow.AddDays(1),
                        Fboid = distributePricingRequest.FboId,
                        Product = "JetA Cost"
                    }
                };
            Mock<DbSet<Fboprices>> fbopricesDbSetMock = fboPrices.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<Fboprices>())
                .Returns(fbopricesDbSetMock.Object);

            Mock<DbSet<DistributionLog>> distributionLogDbSetMock = (new List<DistributionLog>()).AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<DistributionLog>()).Returns(distributionLogDbSetMock.Object);

            Mock<DbSet<DistributionQueue>> distributionQueueDbSetMock = (new List<DistributionQueue>()).AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Set<DistributionQueue>()).Returns(distributionQueueDbSetMock.Object);

            var pricingTemplateServiceMock = new Mock<IPricingTemplateService>();
            pricingTemplateServiceMock.Setup(x => x.GetAllPricingTemplatesForCustomerAsync(
                It.IsAny<CustomerInfoByGroup>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new List<PricingTemplate>() { new PricingTemplate() }));
            services.AddSingleton(pricingTemplateServiceMock.Object);

            var priceFetchingServiceMock = new Mock<IPriceFetchingService>();
            priceFetchingServiceMock.Setup(x => x.GetCustomerPricingAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(),
                It.IsAny<FlightTypeClassifications>(),
                It.IsAny<ApplicableTaxFlights>(), It.IsAny<List<FboFeesAndTaxes>>()))
                .Returns(Task.FromResult(new List<CustomerWithPricing>()));
            services.AddSingleton(priceFetchingServiceMock.Object);

            var mailTemplateServiceMock = new Mock<IMailTemplateService>();
            mailTemplateServiceMock.Setup(x => x.GetTemplatesFileContent(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            services.AddSingleton(mailTemplateServiceMock.Object);
        }
    }
}
