using EFCore.BulkExtensions;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Mail;
using MailSettings = FBOLinx.Web.Configurations.MailSettings;
using Microsoft.Extensions.Options;

namespace FBOLinx.Web.Services
{
    public class PriceDistributionService
    {
        public enum PriceBreakdownDisplayTypes : short
        {
            SingleColumnAllFlights = 0,
            TwoColumnsDomesticInternationalOnly = 1,
            TwoColumnsApplicableFlightTypesOnly = 2,
            FourColumnsAllRules = 3
        }

        private MailSettings _MailSettings;
        private DistributePricingRequest _DistributePricingRequest;
        private FboLinxContext _context;
        private bool _IsPreview = false;
        private int _DistributionLogID = 0;
        private IHttpContextAccessor _HttpContextAccessor;
        private MailTemplateService _MailTemplateService;
        private PriceFetchingService _PriceFetchingService;

        #region Constructors
        public PriceDistributionService(IOptions<MailSettings> mailSettings, FboLinxContext context, IHttpContextAccessor httpContextAccessor, MailTemplateService mailTemplateService, PriceFetchingService priceFetchingService)
        {
            _PriceFetchingService = priceFetchingService;
            _MailTemplateService = mailTemplateService;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _MailSettings = mailSettings.Value;
        }
        #endregion


        #region Public Methods
        public async Task DistributePricing(DistributePricingRequest request, bool isPreview)
        {
            _DistributePricingRequest = request;
            _IsPreview = isPreview;

            var customers = new List<CustomerInfoByGroup>();
                customers = GetCustomersForDistribution(request);

            PerformPreDistributionTasks(customers);

            if (customers == null)
                    return;

            if (_IsPreview)
                await GenerateDistributionMailMessage(customers.FirstOrDefault());
            else
            {
                foreach (var customer in customers)
                {
                    await GenerateDistributionMailMessage(customer);
                }
            }
        }
        #endregion

        #region Private Methods
        private List<CustomerInfoByGroup> GetCustomersForDistribution(DistributePricingRequest request)
        {
            List<CustomerInfoByGroup> customers;
            if (request.Customer == null || request.Customer.Oid == 0)
            {
                customers = GetCustomersForDistribution();
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

        private List<CustomerInfoByGroup> GetCustomersForDistribution()
        {
            var result = (from cg in _context.CustomerInfoByGroup
                          join c in _context.Customers on cg.CustomerId equals c.Oid
                          join cct in _context.CustomCustomerTypes on cg.CustomerId equals cct.CustomerId
                          join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                          where cg.GroupId == _DistributePricingRequest.GroupId && pt.Oid == _DistributePricingRequest.PricingTemplate.Oid
                          && !(c.Suspended ?? false)
                          select cg).ToList();
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

                storeSingleCustomer(customer);

                distributionQueueRecord = _context.DistributionQueue.Where((x =>
                    x.CustomerId == customer.CustomerId && x.Fboid == _DistributePricingRequest.FboId &&
                    x.GroupId == _DistributePricingRequest.GroupId && x.DistributionLogId == _DistributionLogID)).FirstOrDefault();

                var recipients = await GetRecipientsForCustomer(customer);
                if (!_IsPreview && recipients.Count == 0)
                {
                    MarkDistributionRecordAsComplete(distributionQueueRecord);
                    return;
                }

                string validUntil = "";

                var priceDate = new DateTime();
                if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("JetA Retail"))
                {
                    var date = await _context.Fboprices.Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo != null && fp.Fboid == _DistributePricingRequest.FboId && fp.Product == "JetA Retail").OrderByDescending(fp => fp.Oid).Select(fp => fp.EffectiveTo).FirstOrDefaultAsync();

                    priceDate = date.GetValueOrDefault();
                }
                else if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("JetA Cost"))
                {
                    var date = await _context.Fboprices.Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo != null && fp.Fboid == _DistributePricingRequest.FboId && fp.Product == "JetA Cost").OrderByDescending(fp => fp.Oid).Select(fp => fp.EffectiveTo).FirstOrDefaultAsync();

                    priceDate = date.GetValueOrDefault();
                }

                if (priceDate != null)
                {
                    validUntil = "Pricing valid until: " + priceDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                //Add the price breakdown as an image to prevent parsing
                byte[] priceBreakdownImage = await GetPriceBreakdownImage(customer, validPricingTemplates);

                var fbo = _context.Fbos.FirstOrDefault(s => s.Oid == _DistributePricingRequest.FboId);
                var fboIcao = _context.Fboairports.FirstOrDefault(s => s.Fboid == fbo.Oid).Icao;

                //Convert to a SendGrid message and use their API to send it
                Services.MailService mailService = new MailService(_MailSettings);

                var sendGridMessageWithTemplate = new SendGridMessage();
                sendGridMessageWithTemplate.From = new EmailAddress(fbo.SenderAddress + "@fbolinx.com");
                if (fbo.ReplyTo != null && fbo.ReplyTo != "")
                    sendGridMessageWithTemplate.ReplyTo = new EmailAddress(fbo.ReplyTo);
                Personalization personalization = new Personalization();

                personalization.Tos = new System.Collections.Generic.List<EmailAddress>();

                if (!_IsPreview)
                {
                    foreach (ContactInfoByGroup contactInfoByGroup in recipients)
                    {
                        if (_MailSettings.IsValidEmailRecipient(contactInfoByGroup.Email))
                            personalization.Tos.Add(new EmailAddress(contactInfoByGroup.Email));
                    }
                }
                else
                    personalization.Tos.Add(new EmailAddress(_DistributePricingRequest.PreviewEmail));

                sendGridMessageWithTemplate.Personalizations = new List<Personalization>();
                sendGridMessageWithTemplate.Personalizations.Add(personalization);

                _DistributePricingRequest.PricingTemplate.Notes = Regex.Replace(_DistributePricingRequest.PricingTemplate.Notes, @"<[^>]*>", String.Empty);
                var dynamicTemplateData = new TemplateData
                {
                    recipientCompanyName = _IsPreview ? fbo.Fbo : customer.Company,
                    templateEmailBodyMessage = HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Email),
                    templateNotesMessage = HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Notes),
                    fboName = fbo.Fbo,
                    fboICAOCode = fboIcao,
                    fboAddress = fbo.Address,
                    fboCity = fbo.City,
                    fboState = fbo.State,
                    fboZip = fbo.ZipCode,
                    Subject = HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Subject) ?? "Distribution pricing",
                    expiration = validUntil
                };
                sendGridMessageWithTemplate.SetTemplateData(dynamicTemplateData);

                var newAttachment = new SendGrid.Helpers.Mail.Attachment();
                newAttachment.Disposition = "inline";
                newAttachment.Content = Convert.ToBase64String(priceBreakdownImage);
                newAttachment.Filename = "prices.png";
                newAttachment.Type = "image/png";
                newAttachment.ContentId = "Prices";

                sendGridMessageWithTemplate.AddAttachment(newAttachment);

                sendGridMessageWithTemplate.TemplateId = "d-537f958228a6490b977e372ad8389b71";
                var result = mailService.SendAsync(sendGridMessageWithTemplate).Result;

                if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Accepted)
                    MarkDistributionRecordAsComplete(distributionQueueRecord);
                else
                {
                    var mailError = await result.Body.ReadAsStringAsync();
                    throw new System.Exception(mailError);
                }

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
                _context.DistributionErrors.Add(errorRecord);
                await _context.SaveChangesAsync();
            }
        }

        private void MarkDistributionRecordAsComplete(DistributionQueue distributionQueueRecord)
        {
            if (distributionQueueRecord == null)
                return;
            distributionQueueRecord.DateSent = DateTime.Now.ToUniversalTime();
            distributionQueueRecord.IsCompleted = true;
            _context.DistributionQueue.Update(distributionQueueRecord);
            _context.SaveChanges();
        }

        private void storeSingleCustomer(CustomerInfoByGroup customer)
        {
            DistributionQueue queue = new DistributionQueue()
            {
                CustomerId = customer.CustomerId,
                DistributionLogId = _DistributionLogID,
                Fboid = _DistributePricingRequest.FboId,
                GroupId = _DistributePricingRequest.GroupId
            };
            _context.DistributionQueue.Add(queue);
            _context.SaveChanges();
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

            bool hasDepartureTypeRule = false;
            bool hasFlightTypeRule = false;
            var priceBreakdownDisplayType = PriceBreakdownDisplayTypes.SingleColumnAllFlights;

            var taxesAndFees = await _context.FbofeesAndTaxes.Where(x => x.Fboid == _DistributePricingRequest.FboId).ToListAsync();
            foreach(FboFeesAndTaxes fee in taxesAndFees)
            {
                if (fee.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly || fee.DepartureType == FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly)
                {
                    hasDepartureTypeRule = true;
                }
                if (fee.FlightTypeClassification == FBOLinx.Core.Enums.FlightTypeClassifications.Private || fee.FlightTypeClassification == FBOLinx.Core.Enums.FlightTypeClassifications.Commercial)
                {
                    hasFlightTypeRule = true;
                }
            }

            if (!hasDepartureTypeRule && !hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.SingleColumnAllFlights;
            }
            else if (!hasDepartureTypeRule && hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly;
            }
            else if (hasDepartureTypeRule && !hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly;
            }
            else if (hasDepartureTypeRule && hasFlightTypeRule)
            {
                priceBreakdownDisplayType = PriceBreakdownDisplayTypes.FourColumnsAllRules;
            }

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
                        string output = Convert.ToDouble(next.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);
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

        private async Task<List<ContactInfoByGroup>> GetRecipientsForCustomer(CustomerInfoByGroup customer)
        {
            var result = await (from cc in _context.CustomerContacts
                join c in _context.Contacts on cc.ContactId equals c.Oid
                join cibg in _context.ContactInfoByGroup on c.Oid equals cibg.ContactId
                where cibg.GroupId == _DistributePricingRequest.GroupId
                      && cc.CustomerId == customer.CustomerId
                      && (cibg.CopyAlerts ?? false)
                      && !string.IsNullOrEmpty(cibg.Email)
                select cibg).ToListAsync();
            return result;
        }

        private void PerformPreDistributionTasks(List<CustomerInfoByGroup> customers)
        {
            SaveEmailContent();
            LogDistributionRecord();
        }

        private void SaveEmailContent()
        {
            try
            {
                if (_DistributePricingRequest.EmailContentGreeting.Oid == 0)
                {
                    _DistributePricingRequest.EmailContentGreeting.FboId = _DistributePricingRequest.FboId;
                    _context.EmailContent.Add(_DistributePricingRequest.EmailContentGreeting);
                }
                else
                {
                    _context.Entry(_DistributePricingRequest.EmailContentGreeting).State = EntityState.Modified;
                }
                if (_DistributePricingRequest.EmailContentSignature.Oid == 0)
                {
                    _DistributePricingRequest.EmailContentSignature.FboId = _DistributePricingRequest.FboId;
                    _context.EmailContent.Add(_DistributePricingRequest.EmailContentSignature);
                }
                else
                {
                    _context.Entry(_DistributePricingRequest.EmailContentSignature).State = EntityState.Modified;
                }
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                //Do nothing
            }
        }

        private void LogDistributionRecord()
        {
            var distributionLog = new DistributionLog()
            {
                CustomerCompanyType = _DistributePricingRequest.CustomerCompanyType,
                CustomerId = _DistributePricingRequest.Customer?.CustomerId,
                DateSent = DateTime.Now.ToUniversalTime(),
                Fboid = _DistributePricingRequest.FboId,
                GroupId = _DistributePricingRequest.GroupId,
                PricingTemplateId = _DistributePricingRequest.PricingTemplate?.Oid,
                UserId = UserService.GetClaimedUserId(_HttpContextAccessor)
            };
            _context.DistributionLog.Add(distributionLog);
            _context.SaveChanges();
            _DistributionLogID = distributionLog.Oid;
        }
        #endregion

        #region Objects
        private class PriceDistributionCustomer
        {
            
        }

        private class TemplateData
        {
            [JsonProperty("recipientCompanyName")]
            public string recipientCompanyName { get; set; }

            [JsonProperty("templateEmailBodyMessage")]
            public string templateEmailBodyMessage { get; set; }

            [JsonProperty("fboName")]
            public string fboName { get; set; }

            [JsonProperty("fboICAOCode")]
            public string fboICAOCode { get; set; }

            [JsonProperty("fboAddress")]
            public string fboAddress { get; set; }

            [JsonProperty("fboCity")]
            public string fboCity { get; set; }

            [JsonProperty("fboState")]
            public string fboState { get; set; }

            [JsonProperty("fboZip")]
            public string fboZip { get; set; }

            [JsonProperty("templateNotesMessage")]
            public string templateNotesMessage { get; set; }

            [JsonProperty("Subject")]
            public string Subject { get; set; }
            [JsonProperty("expiration")]
            public string expiration { get; set; }
        }
        
        #endregion
    }
}
