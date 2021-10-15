using EFCore.BulkExtensions;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using MailSettings = FBOLinx.Web.Configurations.MailSettings;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using FBOLinx.Web.Services.Interfaces;
using FBOLinx.Web.Models.Responses;

namespace FBOLinx.Web.Services
{
    public interface IPriceDistributionService
    {
        Task DistributePricing(DistributePricingRequest request, bool isPreview);
        Task SendCustomerPriceEmail(int groupId, GroupCustomerAnalyticsResponse info, List<string> emails);
    }

    public class PriceDistributionService : IPriceDistributionService
    {
        public enum PriceBreakdownDisplayTypes : short
        {
            SingleColumnAllFlights = 0,
            TwoColumnsDomesticInternationalOnly = 1,
            TwoColumnsApplicableFlightTypesOnly = 2,
            FourColumnsAllRules = 3
        }

        private DistributePricingRequest _DistributePricingRequest;
        private FboLinxContext _context;
        private bool _IsPreview = false;
        private int _DistributionLogID = 0;
        private IHttpContextAccessor _HttpContextAccessor;
        private IMailTemplateService _MailTemplateService;
        private IPriceFetchingService _PriceFetchingService;
        private readonly FilestorageContext _fileStorageContext;
        private IMailService _MailService;
        private EmailContent _EmailContent;

        #region Constructors
        public PriceDistributionService(IMailService mailService, FboLinxContext context, IHttpContextAccessor httpContextAccessor, IMailTemplateService mailTemplateService, IPriceFetchingService priceFetchingService, FilestorageContext fileStorageContext)
        {
            _PriceFetchingService = priceFetchingService;
            _MailTemplateService = mailTemplateService;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _fileStorageContext = fileStorageContext;
            _MailService = mailService;
        }
        #endregion


        #region Public Methods
        public async Task DistributePricing(DistributePricingRequest request, bool isPreview)
        {
            _DistributePricingRequest = request;
            _IsPreview = isPreview;

            var customers = new List<CustomerInfoByGroup>();
            customers = await GetCustomersForDistribution(request);

            await PerformPreDistributionTasks(customers);

            if (customers == null)
                    return;

            //#tx9ztn - Removed the requirement for aircraft in the account to send a distribution email. A fleet is no longer requireed to receive pricing.
            if (_IsPreview)
            {
                foreach (var customer in customers)
                {
                    await GenerateDistributionMailMessage(customer);
                    break;
                }
            }
            else
            {
                var customerToSend = new CustomerInfoByGroup();

                foreach (var customer in customers)
                {
                    if (customerToSend.Oid == 0)
                        customerToSend = customer;

                    await GenerateDistributionMailMessage(customer);
                }

                var systemContacts = new List<Contacts>();
                systemContacts = await GetSystemContactsForDistribution();

                if (systemContacts != null)
                {
                    foreach (var systemContact in systemContacts)
                    {
                        if (systemContact.CopyAlerts.GetValueOrDefault())
                        {
                            _IsPreview = true;
                            _DistributePricingRequest.PreviewEmail = systemContact.Email;
                            await GenerateDistributionMailMessage(customerToSend);
                        }
                    }
                }

                var users = new List<User>();
                users = await GetUsersForDistribution();

                if (users != null)
                {
                    foreach (var user in users)
                    {
                        if (user.CopyAlerts.GetValueOrDefault() && user.Username.Contains('@'))
                        {
                            _IsPreview = true;
                            _DistributePricingRequest.PreviewEmail = user.Username;
                            await GenerateDistributionMailMessage(customerToSend);
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private async Task<List<CustomerInfoByGroup>> GetCustomersForDistribution(DistributePricingRequest request)
        {
            List<CustomerInfoByGroup> customers;
            if (request.Customer == null || request.Customer.Oid == 0)
            {
                customers = await GetCustomersForDistribution();
            }
            else
            {
                customers = new List<CustomerInfoByGroup>();
                customers.Add(request.Customer);
            }

            if (_DistributePricingRequest.CustomerCompanyType == 0)
                return customers;
            return customers.Where((x => x.CustomerCompanyType == _DistributePricingRequest.CustomerCompanyType)).ToList();
        }

        private async Task<List<CustomerInfoByGroup>> GetCustomersForDistribution()
        {
            var result = await (from cg in _context.Set<CustomerInfoByGroup>()
                          join c in _context.Set<Customers>() on cg.CustomerId equals c.Oid
                          join cct in _context.Set<CustomCustomerTypes>() on cg.CustomerId equals cct.CustomerId
                          join pt in _context.Set<PricingTemplate>() on cct.CustomerType equals pt.Oid
                          where cg.GroupId == _DistributePricingRequest.GroupId && pt.Oid == _DistributePricingRequest.PricingTemplate.Oid
                          && !(c.Suspended ?? false)
                          select cg).ToListAsync();
            return result;
        }

        private async Task<List<Contacts>> GetSystemContactsForDistribution()
        {
            var result = await (from fc in _context.Set<Fbocontacts>()
                          join c in _context.Set<Contacts>() on fc.ContactId equals c.Oid
                          where fc.Fboid == _DistributePricingRequest.FboId && c.Email != null && c.Email != ""
                          select c).ToListAsync();
            return result;
        }

        private async Task<List<User>> GetUsersForDistribution()
        {
            var result = await (from u in _context.Set<User>()
                          where u.GroupId == _DistributePricingRequest.GroupId && (u.FboId == 0 || u.FboId == _DistributePricingRequest.FboId)
                          select u).ToListAsync();
            return result;
        }

        private async Task GenerateDistributionMailMessage(CustomerInfoByGroup customer)
        {
            DistributionQueue distributionQueueRecord = new DistributionQueue();
            try
            {
                var validPricingTemplates = await GetValidPricingTemplates(customer);
                if (validPricingTemplates == null || validPricingTemplates.Count == 0)
                {
                    return;
                }

                await StoreSingleCustomer(customer);

                var distributionQueueRecords = await _context.Set<DistributionQueue>().Where((x =>
                    x.CustomerId == customer.CustomerId && x.Fboid == _DistributePricingRequest.FboId &&
                    x.GroupId == _DistributePricingRequest.GroupId && x.DistributionLogId == _DistributionLogID)).ToListAsync();

                distributionQueueRecord = distributionQueueRecords.FirstOrDefault();

                var recipients = await GetRecipientsForCustomer(customer);
                if (!_IsPreview && recipients.Count == 0)
                {
                    await MarkDistributionRecordAsComplete(distributionQueueRecord);
                    return;
                }

                //Get current posted retail
                var postedRetail = await _PriceFetchingService.GetCurrentPostedRetail(_DistributePricingRequest.FboId);
                var currentPostedRetail = "Current Posted Retail: " + String.Format("{0:C}", postedRetail);

                //Get expiration date
                string validUntil = "";

                var priceDate = new DateTime();
                if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("JetA Retail"))
                {
                    var date = await _context.Set<Fboprices>().Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo != null && fp.Fboid == _DistributePricingRequest.FboId && fp.Product == "JetA Retail").OrderByDescending(fp => fp.Oid).Select(fp => fp.EffectiveTo).FirstOrDefaultAsync();

                    priceDate = date.GetValueOrDefault();
                }
                else if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("JetA Cost"))
                {
                    var date = await _context.Set<Fboprices>().Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo != null && fp.Fboid == _DistributePricingRequest.FboId && fp.Product == "JetA Cost").OrderByDescending(fp => fp.Oid).Select(fp => fp.EffectiveTo).FirstOrDefaultAsync();

                    priceDate = date.GetValueOrDefault();
                }

                if (priceDate != null)
                {
                    validUntil = "Pricing valid until: " + priceDate.AddMinutes(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                //Add the price breakdown as an image to prevent parsing
                byte[] priceBreakdownImage = await GetPriceBreakdownImage(customer, validPricingTemplates);

                var fbo = await _context.Set<Fbos>().Where(s => s.Oid == _DistributePricingRequest.FboId).SingleOrDefaultAsync();
                var fboIcao = await _context.Set<Fboairports>().Where(s => s.Fboid == fbo.Oid).SingleOrDefaultAsync();

                //Add email content to MailMessage
                FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
                mailMessage.From = new MailAddress(MailService.GetFboLinxAddress(fbo.SenderAddress));
                if (fbo.ReplyTo != null && fbo.ReplyTo != "")
                    mailMessage.ReplyToList.Add(fbo.ReplyTo);
                if (!_IsPreview)
                {
                    foreach (ContactInfoByGroup contactInfoByGroup in recipients)
                    {
                        if (_MailService.IsValidEmailRecipient(contactInfoByGroup.Email))
                            mailMessage.To.Add(contactInfoByGroup.Email);
                    }
                }
                else
                    mailMessage.To.Add(_DistributePricingRequest.PreviewEmail);

                mailMessage.AttachmentBase64String = Convert.ToBase64String(priceBreakdownImage);

                var logo = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == fbo.Oid).ToListAsync();
                if (logo.Count > 0)
                {
                    mailMessage.Logo = new LogoDetails();
                    mailMessage.Logo.Filename = "logo." + logo[0].ContentType.Split('/')[1];
                    mailMessage.Logo.ContentType = logo[0].ContentType;
                    mailMessage.Logo.Base64String = Convert.ToBase64String(logo[0].FileData);
                }

                _DistributePricingRequest.PricingTemplate.Notes = Regex.Replace(_DistributePricingRequest.PricingTemplate.Notes, @"<[^>]*>", String.Empty);
                var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridDistributionTemplateData
                {
                    recipientCompanyName = _IsPreview ? fbo.Fbo : customer.Company,
                    templateEmailBodyMessage = HttpUtility.HtmlDecode(_EmailContent.EmailContentHtml ?? ""),
                    templateNotesMessage = HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Notes),
                    fboName = fbo.Fbo,
                    fboICAOCode = fboIcao.Icao,
                    fboAddress = fbo.Address,
                    fboCity = fbo.City,
                    fboState = fbo.State,
                    fboZip = fbo.ZipCode,
                    Subject = HttpUtility.HtmlDecode(_EmailContent.Subject) ?? "Distribution pricing",
                    expiration = validUntil,
                    currentPostedRetail = currentPostedRetail
                };
                mailMessage.SendGridDistributionTemplateData = dynamicTemplateData;

                //Send email
                var result = _MailService.SendAsync(mailMessage).Result;
                if (result)
                    await MarkDistributionRecordAsComplete(distributionQueueRecord);
            }
            catch (System.Exception exception)
            {
                //Customer failed, will not be marked as complete
                if (distributionQueueRecord == null)
                    return;
                var errorRecord = new DistributionErrors()
                {
                    DistributionLogId = _DistributionLogID,
                    DistributionQueueId = distributionQueueRecord.Oid,
                    ErrorDateTime = DateTime.Now.ToUniversalTime()
                };
                if (exception.InnerException != null)
                    errorRecord.Error = exception.Message + "***" + exception.InnerException.StackTrace + "****" + exception.StackTrace;
                else
                    errorRecord.Error = exception.Message + "****" + exception.StackTrace;
                _context.Set<DistributionErrors>().Add(errorRecord);
                await _context.SaveChangesAsync();
            }
        }

        private async Task MarkDistributionRecordAsComplete(DistributionQueue distributionQueueRecord)
        {
            if (distributionQueueRecord == null)
                return;
            distributionQueueRecord.DateSent = DateTime.Now.ToUniversalTime();
            distributionQueueRecord.IsCompleted = true;
            await _context.Set<DistributionQueue>().BatchUpdateAsync(distributionQueueRecord);
            await _context.SaveChangesAsync();
        }

        private async Task StoreSingleCustomer(CustomerInfoByGroup customer)
        {
            DistributionQueue queue = new DistributionQueue()
            {
                CustomerId = customer.CustomerId,
                DistributionLogId = _DistributionLogID,
                Fboid = _DistributePricingRequest.FboId,
                GroupId = _DistributePricingRequest.GroupId
            };
            await _context.Set<DistributionQueue>().AddAsync(queue);
            await _context.SaveChangesAsync();
        }

        private async Task<List<PricingTemplate>> GetValidPricingTemplates(CustomerInfoByGroup customer)
        {

            var result = await _PriceFetchingService.GetAllPricingTemplatesForCustomerAsync(customer, _DistributePricingRequest.FboId,
                _DistributePricingRequest.GroupId, _DistributePricingRequest.PricingTemplate.Oid);
            
            return result;
        }

        private async Task<byte[]> GetPriceBreakdownImage(CustomerInfoByGroup customer, List<PricingTemplate> pricingTemplates)
        {
            StringBuilder result = new StringBuilder();
            foreach (var pricingTemplate in pricingTemplates)
            {
                string html = await GetPriceBreakdownHTML(customer, pricingTemplate);
                result.Append(html);
            }
            string priceBreakdownHTML = result.ToString();
            return FBOLinx.Core.Utilities.HTML.CreateImageFromHTML(priceBreakdownHTML);
        }

        private async Task<string> GetPriceBreakdownHTML(CustomerInfoByGroup customer, PricingTemplate pricingTemplate)
        {
            var commercialInternationalPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Commercial, FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly);
            var privateInternationalPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Private, FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly);
            var commercialDomesticPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Commercial, FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly);
            var privateDomesticPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Private, FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly);
            
            var priceBreakdownDisplayType =
                await _PriceFetchingService.GetPriceBreakdownDisplayType(_DistributePricingRequest.FboId);
            
            privateDomesticPricingResults = privateDomesticPricingResults.OrderBy(s => s.MinGallons).ToList();

            string priceBreakdownTemplate = GetPriceBreakdownTemplate(priceBreakdownDisplayType);
            string rowHTMLTemplate = GetPriceBreakdownRowTemplate(priceBreakdownDisplayType);
            StringBuilder rowsHTML = new StringBuilder();
            int loopIndex = 0;

            foreach (DTO.CustomerWithPricing model in privateDomesticPricingResults)
            {
                string row = rowHTMLTemplate;

                if ((loopIndex + 1) < commercialInternationalPricingResults.Count)
                {
                    row = row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());
                    var next = commercialInternationalPricingResults[loopIndex + 1];
                    Double maxValue = Convert.ToDouble(next.MinGallons) - 1;

                    if (maxValue > 999)
                    {
                        string output = maxValue.ToString("#,##", CultureInfo.InvariantCulture);
                        row = row.Replace("%MAX_GALLON%", output);
                    }
                    else
                    {
                        row = row.Replace("%MAX_GALLON%", maxValue.ToString());
                    }
                }
                else
                {
                    string output = Convert.ToDouble(model.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);

                    output = output + "+";
                    row = row.Replace("%MIN_GALLON%", output);
                    row = row.Replace("%MAX_GALLON%", "");
                    row = row.Replace("-", "");
                }
                   
                row = row.Replace("%ALL_IN_PRICE%", String.Format("{0:C}", (model.AllInPrice.GetValueOrDefault())));
                row = row.Replace("%ALL_IN_PRICE_INT_COMM%", String.Format("{0:C}", (commercialInternationalPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                row = row.Replace("%ALL_IN_PRICE_INT_PRIVATE%", String.Format("{0:C}", (privateInternationalPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                row = row.Replace("%ALL_IN_PRICE_DOMESTIC_COMM%", String.Format("{0:C}", (commercialDomesticPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                row = row.Replace("%ALL_IN_PRICE_DOMESTIC_PRIVATE%", String.Format("{0:C}", (privateDomesticPricingResults.Where(s => s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                rowsHTML.Append(row);
                loopIndex++;
            }

            return priceBreakdownTemplate.Replace("%PRICE_BREAKDOWN_ROWS%", rowsHTML.ToString());
        }

        private string GetPriceBreakdownTemplate(PriceBreakdownDisplayTypes priceBreakdownDisplayType)
        {
            if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownSingleColumnAllFlights.html");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownTwoColumnsApplicableFlightTypesOnly.html");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownTwoColumnsDepartureOnly.html");
            else
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownFourColumnsAllRules.html");
        }

        private string GetPriceBreakdownRowTemplate(PriceBreakdownDisplayTypes priceBreakdownDisplayType)
        {
            if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowSingleColumnAllFlights.html");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowTwoColumnsApplicableFlightTypesOnly.html");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowTwoColumnsDepartureOnly.html");
            else
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowFourColumnsAllRules.html");
        }

        public async Task SendCustomerPriceEmail(int groupId, GroupCustomerAnalyticsResponse info, List<string> emails)
        {
            var emailContent = _context.EmailContent.Where(e => e.GroupId == groupId).FirstOrDefault();

            //Add the price breakdown as an image to prevent parsing
            string priceBreakdownCsvContent = GetCustomerPriceBreakdownCSV(info);

            //Add email content to MailMessage
            FBOLinxMailMessage mailMessage = new FBOLinxMailMessage
            {
                From = new MailAddress(
                    MailService.GetFboLinxAddress(
                        emailContent == null || string.IsNullOrEmpty(emailContent.FromAddress)
                        ? "donotreply"
                        : emailContent.FromAddress
                    )
                )
            };

            if (emailContent != null && !string.IsNullOrEmpty(emailContent.ReplyTo))
            {
                mailMessage.ReplyToList.Add(emailContent.ReplyTo);
            }

            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }

            mailMessage.AttachmentBase64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(priceBreakdownCsvContent));

            var logo = await _fileStorageContext.FboLinxImageFileData.Where(f => f.GroupId == groupId).ToListAsync();
            if (logo.Count > 0)
            {
                mailMessage.Logo = new LogoDetails();
                mailMessage.Logo.Filename = "logo." + logo[0].ContentType.Split('/')[1];
                mailMessage.Logo.ContentType = logo[0].ContentType;
                mailMessage.Logo.Base64String = Convert.ToBase64String(logo[0].FileData);
            }

            var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridGroupCustomerPricingTemplateData
            {
                templateEmailBodyMessage = HttpUtility.HtmlDecode(emailContent.EmailContentHtml ?? ""),
                Subject = HttpUtility.HtmlDecode(emailContent.Subject) ?? "Customers Pricing",
            };
            mailMessage.SendGridGroupCustomerPricingTemplateData = dynamicTemplateData;

            //Send email
            await _MailService.SendAsync(mailMessage);
        }

        private string GetCustomerPriceBreakdownCSV(GroupCustomerAnalyticsResponse info)
        {
            var defaultGroupedFboPrices = info.GroupCustomerFbos.FirstOrDefault();
            var defaultPrice = defaultGroupedFboPrices.Prices.FirstOrDefault();

            string priceBreakdownTemplate, rowHTMLTemplate;
            StringBuilder rowsHTML = new StringBuilder();

            if (defaultGroupedFboPrices == null || defaultPrice == null)
            {
                priceBreakdownTemplate = GetCustomerPriceBreakdownTemplate(PriceBreakdownDisplayTypes.SingleColumnAllFlights);
                rowsHTML.Append("");
            }
            else
            {
                priceBreakdownTemplate = GetCustomerPriceBreakdownTemplate(defaultPrice.PriceBreakdownDisplayType);
                rowHTMLTemplate = GetCustomerPriceBreakdownRowTemplate(defaultPrice.PriceBreakdownDisplayType);

                foreach (var groupCustomerFbos in info.GroupCustomerFbos)
                {
                    for (var i = 0; i < groupCustomerFbos.Prices.Count; i++)
                    {
                        var model = groupCustomerFbos.Prices[i];
                        string row = rowHTMLTemplate;

                        if (i == 0)
                        {
                            row = row.Replace("%FBO%", groupCustomerFbos.Icao);
                        }
                        else {
                            row = row.Replace("%FBO%", "");
                        }


                        var next = i < groupCustomerFbos.Prices.Count - 1 ? groupCustomerFbos.Prices[i + 1] : null;

                        if (next != null)
                        {
                            row = row.Replace("%MIN_GALLON%", model.MinGallons.ToString());

                            var maxValue = Convert.ToDouble(next.MinGallons) - 1;

                            if (maxValue > 999)
                            {
                                string output = Convert.ToDouble(next.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);
                                row = row.Replace("%MAX_GALLON%", output);
                            }
                            else
                            {
                                row = row.Replace("%MAX_GALLON%", maxValue.ToString());
                            }
                        } else
                        {
                            string output = Convert.ToDouble(model.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);

                            output = output + "+";
                            row = row.Replace("%MIN_GALLON%", output);
                            row = row.Replace("%MAX_GALLON%", "");
                            row = row.Replace("-", "");
                        }

                        row = row.Replace("%ALL_IN_PRICE%", String.Format("{0:C}", model.DomPrivate.GetValueOrDefault()));
                        row = row.Replace("%ALL_IN_PRICE_INT_COMM%", String.Format("{0:C}", model.IntComm.GetValueOrDefault()));
                        row = row.Replace("%ALL_IN_PRICE_INT_PRIVATE%", String.Format("{0:C}", model.IntPrivate.GetValueOrDefault()));
                        row = row.Replace("%ALL_IN_PRICE_DOMESTIC_COMM%", String.Format("{0:C}", model.DomComm.GetValueOrDefault()));
                        row = row.Replace("%ALL_IN_PRICE_DOMESTIC_PRIVATE%", String.Format("{0:C}", model.DomPrivate.GetValueOrDefault()));

                        row = row.Replace("%TAIL_NUMBERS%", info.TailNumbers);

                        rowsHTML.Append(row);
                    }
                }
            }

            return priceBreakdownTemplate.Replace("%PRICE_BREAKDOWN_ROWS%", rowsHTML.ToString());
        }

        private string GetCustomerPriceBreakdownTemplate(PriceBreakdownDisplayTypes priceBreakdownDisplayType)
        {
            if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "SingleColumn.csv");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "TwoColumnsApplicableFlightTypesOnly.csv");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "TwoColumnsDepartureOnly.csv");
            else
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "FourColumns.csv");
        }

        private string GetCustomerPriceBreakdownRowTemplate(PriceBreakdownDisplayTypes priceBreakdownDisplayType)
        {
            if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "SingleColumnRow.csv");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "TwoColumnsApplicableFlightTypesOnlyRow.csv");
            else if (priceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "TwoColumnsDepartureOnlyRow.csv");
            else
                return _MailTemplateService.GetTemplatesFileContent("GroupCustomerPrice", "FourColumnsRow.csv");
        }

        private async Task<List<ContactInfoByGroup>> GetRecipientsForCustomer(CustomerInfoByGroup customer)
        {
            var result = await (from cc in _context.Set<CustomerContacts>()
                join c in _context.Set<Contacts>() on cc.ContactId equals c.Oid
                join cibg in _context.Set<ContactInfoByGroup>() on c.Oid equals cibg.ContactId
                                where cibg.GroupId == _DistributePricingRequest.GroupId
                                      && cc.CustomerId == customer.CustomerId
                                      && (cibg.CopyAlerts ?? false)
                                      && !string.IsNullOrEmpty(cibg.Email)
                                select cibg).ToListAsync();
            return result;
        }

        private async Task PerformPreDistributionTasks(List<CustomerInfoByGroup> customers)
        {
            await GetEmailContent();
            await LogDistributionRecord();
        }

        private async Task GetEmailContent()
        {
            if (_DistributePricingRequest.PricingTemplate.EmailContentId == null || _DistributePricingRequest.PricingTemplate.EmailContentId == 0)
            {
                List<PricingTemplate> pricingTemplates = await _context.Set<PricingTemplate>().Where(x => x.Oid == _DistributePricingRequest.PricingTemplate.Oid).ToListAsync();
                PricingTemplate pricingTemplate = pricingTemplates.FirstOrDefault();

                EmailContent newEmailContent = new EmailContent();
                newEmailContent.Subject = _DistributePricingRequest.PricingTemplate.Subject;
                newEmailContent.EmailContentHtml = _DistributePricingRequest.PricingTemplate.Email;
                newEmailContent.FboId = _DistributePricingRequest.FboId;
                newEmailContent.Name = _DistributePricingRequest.PricingTemplate.Name;
                await _context.Set<EmailContent>().AddAsync(newEmailContent);
                await _context.SaveChangesAsync();
                _DistributePricingRequest.PricingTemplate.EmailContentId = newEmailContent.Oid;

                pricingTemplate.EmailContentId = _DistributePricingRequest.PricingTemplate.EmailContentId;
            }

            var emailContent = await _context.Set<EmailContent>().Where(x => x.Oid == _DistributePricingRequest.PricingTemplate.EmailContentId).ToListAsync();
            _EmailContent = emailContent.FirstOrDefault();
        }
        private async Task LogDistributionRecord()
        {
            var distributionLog = new DistributionLog()
            {
                CustomerCompanyType = _DistributePricingRequest.CustomerCompanyType,
                CustomerId = _DistributePricingRequest.Customer?.CustomerId,
                DateSent = DateTime.Now.ToUniversalTime(),
                Fboid = _DistributePricingRequest.FboId,
                GroupId = _DistributePricingRequest.GroupId,
                PricingTemplateId = _DistributePricingRequest.PricingTemplate?.Oid,
                UserId = JwtManager.GetClaimedUserId(_HttpContextAccessor)
            };
            await _context.Set<DistributionLog>().AddAsync(distributionLog);
            await _context.SaveChangesAsync();
            _DistributionLogID = distributionLog.Oid;
        }
        #endregion

        #region Objects
        private class PriceDistributionCustomer
        {
            
        }
        
        #endregion
    }
}
