using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using FBOLinx.Web.Services;
using Moq;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using FBOLinx.Web;
using FBOLinx.Web.DTO;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace FBOLinx.ServiceLayer.Test.Services
{
    public class PriceDistributionServiceTests : BaseTestFixture<IPriceDistributionService>
    {
        [Test]
        public async Task DistributePricing_PreviewRequestedAndCustomerExistsInTheRequest_DistributePricingEmailSent()
        {
            // Arrange
            string fromEmail = "test";
            string toEmail = "test2@fbolinx.com";
            DistributePricingRequest distributePricingRequest = TestDataHelper.CreateDistributePreviewPricingRequest();
            distributePricingRequest.PreviewEmail = toEmail;
            Mock<IMailService> mailServiceMock = null;
            Arrange(services =>
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

                var dbContextMock = new Mock<FboLinxContext>();

                IEnumerable<CustomerAircrafts> customerAircrafts = new List<CustomerAircrafts>()
                {
                    new CustomerAircrafts()
                    {
                        CustomerId = distributePricingRequest.Customer.CustomerId,
                        GroupId = distributePricingRequest.GroupId
                    }
                };
                Mock<DbSet<CustomerAircrafts>> customerAircraftsDbSetMock = customerAircrafts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.CustomerAircrafts)
                    .Returns(customerAircraftsDbSetMock.Object);

                IEnumerable<CustomerContacts> customerContacts = new List<CustomerContacts>()
                {
                    new CustomerContacts()
                    {
                        ContactId = distributePricingRequest.Customer.CustomerId,
                    }
                };
                Mock<DbSet<CustomerContacts>> customerContactsDbSetMock = customerContacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.CustomerContacts)
                    .Returns(customerContactsDbSetMock.Object);

                IEnumerable<Contacts> contacts = new List<Contacts>()
                {
                    new Contacts()
                    {
                        Oid = distributePricingRequest.Customer.CustomerId,
                    }
                };
                Mock<DbSet<Contacts>> contactsDbSetMock = contacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Contacts)
                    .Returns(contactsDbSetMock.Object);
                
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
                dbContextMock.Setup(x => x.ContactInfoByGroup)
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
                dbContextMock.Setup(x => x.Fbos)
                    .Returns(fbosDbSetMock.Object);

                IEnumerable<Fboairports> fboairports = new List<Fboairports>()
                {
                    new Fboairports()
                    {
                        Fboid = distributePricingRequest.FboId,
                    }
                };
                Mock<DbSet<Fboairports>> fboairportsDbSetMock = fboairports.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Fboairports)
                    .Returns(fboairportsDbSetMock.Object);

                IEnumerable<EmailContent> emailContent = new List<EmailContent>()
                {
                    new EmailContent()
                    {
                        Oid = 1
                    }
                };
                Mock<DbSet<EmailContent>> emailContentDbSetMock = emailContent.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.EmailContent)
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
                dbContextMock.Setup(x => x.Fboprices)
                    .Returns(fbopricesDbSetMock.Object);

                Mock<DbSet<DistributionLog>> distributionLogDbSetMock = (new List<DistributionLog>()).AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.DistributionLog).Returns(distributionLogDbSetMock.Object);

                Mock<DbSet<DistributionQueue>> distributionQueueDbSetMock = (new List<DistributionQueue>()).AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.DistributionQueue).Returns(distributionQueueDbSetMock.Object);

                services.AddSingleton(dbContextMock.Object);

                var priceFetchingServiceMock = new Mock<IPriceFetchingService>();
                priceFetchingServiceMock.Setup(x => x.GetAllPricingTemplatesForCustomerAsync(
                    It.IsAny<CustomerInfoByGroup>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new List<PricingTemplate>() { new PricingTemplate() }));
                priceFetchingServiceMock.Setup(x => x.GetCustomerPricingAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(),
                    It.IsAny<FlightTypeClassifications>(),
                    It.IsAny<ApplicableTaxFlights>(), It.IsAny<List<FboFeesAndTaxes>>()))
                    .Returns(Task.FromResult(new List<CustomerWithPricing>()));
                services.AddSingleton(priceFetchingServiceMock.Object);

                var mailTemplateServiceMock = new Mock<IMailTemplateService>();
                mailTemplateServiceMock.Setup(x => x.GetTemplatesFileContent(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
                services.AddSingleton(mailTemplateServiceMock.Object);
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
            int customerId = 1;
            string fromEmail = "test";
            DistributePricingRequest distributePricingRequest = TestDataHelper.CreateDistributePricingRequest();
            Mock<IMailService> mailServiceMock = null;
            Arrange(services =>
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

                var dbContextMock = new Mock<FboLinxContext>();

                IEnumerable<CustomerInfoByGroup> customerInfoByGroup = new List<CustomerInfoByGroup>()
                {
                    new CustomerInfoByGroup()
                    {
                        GroupId = distributePricingRequest.GroupId,
                        CustomerId = customerId
                    }
                };
                Mock<DbSet<CustomerInfoByGroup>> customerInfoByGroupDbSetMock = customerInfoByGroup.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.CustomerInfoByGroup)
                    .Returns(customerInfoByGroupDbSetMock.Object);

                IEnumerable<Customers> customers = new List<Customers>()
                {
                    new Customers()
                    {
                        Oid = customerId,
                        GroupId = distributePricingRequest.GroupId,
                        Suspended = false
                    }
                };
                Mock<DbSet<Customers>> customersDbSetMock = customers.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Customers)
                    .Returns(customersDbSetMock.Object);

                IEnumerable<CustomerAircrafts> customerAircrafts = new List<CustomerAircrafts>()
                {
                    new CustomerAircrafts()
                    {
                        CustomerId = customerId,
                        GroupId = distributePricingRequest.GroupId
                    }
                };
                Mock<DbSet<CustomerAircrafts>> customerAircraftsDbSetMock = customerAircrafts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.CustomerAircrafts)
                    .Returns(customerAircraftsDbSetMock.Object);

                IEnumerable<CustomCustomerTypes> customCustomerTypes = new List<CustomCustomerTypes>()
                {
                    new CustomCustomerTypes()
                    {
                        CustomerId = customerId,
                        CustomerType = 1
                    }
                };
                Mock<DbSet<CustomCustomerTypes>> customCustomerTypesDbSetMock = customCustomerTypes.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.CustomCustomerTypes)
                    .Returns(customCustomerTypesDbSetMock.Object);

                IEnumerable<CustomerContacts> customerContacts = new List<CustomerContacts>()
                {
                    new CustomerContacts()
                    {
                        ContactId = 1,
                        CustomerId = customerId
                    }
                };
                Mock<DbSet<CustomerContacts>> customerContactsDbSetMock = customerContacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.CustomerContacts)
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
                dbContextMock.Setup(x => x.Contacts)
                    .Returns(contactsDbSetMock.Object);

                IEnumerable<ContactInfoByGroup> contactInfoByGroupList = new List<ContactInfoByGroup>()
                {
                    new ContactInfoByGroup()
                    {
                        ContactId = 1,
                        GroupId = distributePricingRequest.GroupId,
                        CopyAlerts = true,
                        Email = "chau@fuelerlinx.com"
                    }
                };
                Mock<DbSet<ContactInfoByGroup>> contactInfoByGroupDbSetMock = contactInfoByGroupList.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.ContactInfoByGroup)
                    .Returns(contactInfoByGroupDbSetMock.Object);

                IEnumerable<Fbocontacts> fboContacts = new List<Fbocontacts>()
                {
                    new Fbocontacts()
                    {
                        Fboid = distributePricingRequest.FboId,
                    }
                };
                Mock<DbSet<Fbocontacts>> fboContactsDbSetMock = fboContacts.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Fbocontacts)
                    .Returns(fboContactsDbSetMock.Object);

                IEnumerable<User> usersList = new List<User>()
                {
                    new User()
                    {
                        Username = "chau@fuelerlinx.com",
                        CopyAlerts = true,
                        GroupId = distributePricingRequest.GroupId,
                        FboId = distributePricingRequest.FboId
                    }
                };
                Mock<DbSet<User>> userDbSetMock = usersList.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.User)
                    .Returns(userDbSetMock.Object);

                IEnumerable<Fbos> fbos = new List<Fbos>()
                {
                    new Fbos()
                    {
                        Oid = distributePricingRequest.FboId,
                        SenderAddress = fromEmail
                    }
                };
                Mock<DbSet<Fbos>> fbosDbSetMock = fbos.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Fbos)
                    .Returns(fbosDbSetMock.Object);

                IEnumerable<Fboairports> fboairports = new List<Fboairports>()
                {
                    new Fboairports()
                    {
                        Fboid = distributePricingRequest.FboId,
                    }
                };
                Mock<DbSet<Fboairports>> fboairportsDbSetMock = fboairports.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.Fboairports)
                    .Returns(fboairportsDbSetMock.Object);

                IEnumerable<EmailContent> emailContent = new List<EmailContent>()
                {
                    new EmailContent()
                    {
                        Oid = 1
                    }
                };
                Mock<DbSet<EmailContent>> emailContentDbSetMock = emailContent.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.EmailContent)
                    .Returns(emailContentDbSetMock.Object);

                IEnumerable<PricingTemplate> pricingTemplate = new List<PricingTemplate>()
                {
                    new PricingTemplate()
                    {
                        Oid = 1
                    }
                };
                Mock<DbSet<PricingTemplate>> pricingTemplateDbSetMock = pricingTemplate.AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.PricingTemplate)
                    .Returns(pricingTemplateDbSetMock.Object);

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
                dbContextMock.Setup(x => x.Fboprices)
                    .Returns(fbopricesDbSetMock.Object);

                Mock<DbSet<DistributionLog>> distributionLogDbSetMock = (new List<DistributionLog>()).AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.DistributionLog).Returns(distributionLogDbSetMock.Object);

                Mock<DbSet<DistributionQueue>> distributionQueueDbSetMock = (new List<DistributionQueue>()).AsQueryable().BuildMockDbSet();
                dbContextMock.Setup(x => x.DistributionQueue).Returns(distributionQueueDbSetMock.Object);

                services.AddSingleton(dbContextMock.Object);

                var priceFetchingServiceMock = new Mock<IPriceFetchingService>();
                priceFetchingServiceMock.Setup(x => x.GetAllPricingTemplatesForCustomerAsync(
                    It.IsAny<CustomerInfoByGroup>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new List<PricingTemplate>() { new PricingTemplate() }));
                priceFetchingServiceMock.Setup(x => x.GetCustomerPricingAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(),
                    It.IsAny<FlightTypeClassifications>(),
                    It.IsAny<ApplicableTaxFlights>(), It.IsAny<List<FboFeesAndTaxes>>()))
                    .Returns(Task.FromResult(new List<CustomerWithPricing>()));
                services.AddSingleton(priceFetchingServiceMock.Object);

                var mailTemplateServiceMock = new Mock<IMailTemplateService>();
                mailTemplateServiceMock.Setup(x => x.GetTemplatesFileContent(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
                services.AddSingleton(mailTemplateServiceMock.Object);
            });

            // Act
            await subject.DistributePricing(distributePricingRequest, false);

            //Assert
            mailServiceMock.Verify(x => x.SendAsync(It.Is<FBOLinxMailMessage>(
                x => x.From.Address == MailService.GetFboLinxAddress(fromEmail) &&
                     x.To.Any(y => y.Address == "chau@fuelerlinx.com"))), Times.AtLeastOnce);
        }

        // private void MockDbSet<TEntity>(Mock<FboLinxContext> dbContextMock, IEnumerable<TEntity> entities) where TEntity: class
        // {
        //     dbContextMock.Object.Set<TEntity>()
        //     Mock<DbSet<TEntity>> dbSetMock = entities.AsQueryable().BuildMockDbSet();
        //     dbContextMock.Setup(x => x.DistributionLog).Returns(dbSetMock.Object);
        // }
    }
}
