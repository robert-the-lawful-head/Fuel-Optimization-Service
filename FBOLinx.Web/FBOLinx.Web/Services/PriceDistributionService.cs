using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Models.Requests;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using SendGrid.Helpers.Mail;
using Attachment = System.Net.Mail.Attachment;
using MailSettings = FBOLinx.Web.Configurations.MailSettings;

namespace FBOLinx.Web.Services
{
    public class PriceDistributionService
    {
        private MailSettings _MailSettings;
        private DistributePricingRequest _DistributePricingRequest;
        private FboLinxContext _context;
        private IFileProvider _FileProvider;
        private bool _IsPreview = false;
        private int _DistributionLogID = 0;
        private IHttpContextAccessor _HttpContextAccessor;

        #region Constructors
        public PriceDistributionService(MailSettings mailSettings, FboLinxContext context, IFileProvider fileProvider, IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
            _FileProvider = fileProvider;
            _context = context;
            _MailSettings = mailSettings;
        }
        #endregion

        #region Static Methods
        public static async Task BeginPriceDistribution(MailSettings mailSettings, FboLinxContext context,
            Models.Requests.DistributePricingRequest request, IFileProvider fileProvider, IHttpContextAccessor httpContextAccessor)
        {
            PriceDistributionService service = new PriceDistributionService(mailSettings, context, fileProvider, httpContextAccessor);
            await service.DistributePricing(request);
        }
        #endregion

        #region Public Methods
        public async Task DistributePricing(Models.Requests.DistributePricingRequest request)
        {
            _DistributePricingRequest = request;
            var customers = GetCustomersForDistribution(request);

            if (customers == null)
                return;

            PerformPreDistributionTasks(customers);

            foreach (var customer in customers)
            {
                await GenerateDistributionMailMessage(customer);
            }
        }

        public async Task<string> GeneratePreview(Models.Requests.DistributePricingRequest request)
        {
            _IsPreview = true;
            _DistributePricingRequest = request;
            var customers = GetCustomersForDistribution(request);

            if (customers == null || customers.Count == 0)
                return "";

            var validPricingTemplates = await GetValidPricingTemplates(customers[0]);

            return await GetMailBody(customers[0], validPricingTemplates);
        }
        #endregion

        #region Private Methods
        private List<CustomerInfoByGroup> GetCustomersForDistribution(DistributePricingRequest request)
        {
            List<Models.CustomerInfoByGroup> customers;
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

        private List<Models.CustomerInfoByGroup> GetCustomersForDistribution()
        {
            var result = (from cg in _context.CustomerInfoByGroup
                join c in _context.Customers on cg.CustomerId equals c.Oid
                where cg.GroupId == _DistributePricingRequest.GroupId
                && cg.Distribute.GetValueOrDefault()
                select cg).ToList();

            return result;
        }

        private async Task GenerateDistributionMailMessage(Models.CustomerInfoByGroup customer)
        {
            var distributionQueueRecord = _context.DistributionQueue.Where((x =>
                x.CustomerId == customer.CustomerId && x.Fboid == _DistributePricingRequest.FboId &&
                x.GroupId == _DistributePricingRequest.GroupId)).FirstOrDefault();
            try
            {
                var validPricingTemplates = await GetValidPricingTemplates(customer);
                if (validPricingTemplates == null || validPricingTemplates.Count == 0)
                {
                    MarkDistributionRecordAsComplete(distributionQueueRecord);
                    return;
                }

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_MailSettings.MailUserName);
                var recipients = GetRecipients(customer);
                foreach (ContactInfoByGroup contactInfoByGroup in recipients)
                {
                    if (_MailSettings.IsValidEmailRecipient(contactInfoByGroup.Email))
                        mailMessage.To.Add(new MailAddress(contactInfoByGroup.Email));
                }
                if (mailMessage.To.Count == 0)
                {
                    MarkDistributionRecordAsComplete(distributionQueueRecord);
                    return;
                }

                string body = await GetMailBody(customer, validPricingTemplates);

                //Add the price breakdown as an image to prevent parsing
                byte[] priceBreakdownImage = await GetPriceBreakdownImage(customer, validPricingTemplates);

                var imageStream = new MemoryStream(priceBreakdownImage);
                Attachment priceBreakdownAttachment = new Attachment(imageStream, new ContentType("image/jpeg"));
                priceBreakdownAttachment.ContentDisposition.Inline = true;
                priceBreakdownAttachment.Name = "PriceBreakdown.jpg";
                var linkedResource = new LinkedResource(imageStream, new ContentType("image/jpeg"));
                linkedResource.ContentId = Guid.NewGuid().ToString();
                priceBreakdownAttachment.ContentId = linkedResource.ContentId;
                body = body.Replace("%PRICE_BREAKDOWN_CONTENT_ID%", linkedResource.ContentId);
                mailMessage.Attachments.Add(priceBreakdownAttachment);

                body = body.Replace("%PRICE_BREAKDOWN_IMAGE_BASE64%", System.Convert.ToBase64String(priceBreakdownImage));

                mailMessage.From = new MailAddress("donotreply@fbolinx.com");
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = _DistributePricingRequest.EmailContentGreeting.Subject;

                //Convert to a SendGrid message and use their API to send it
                Services.MailService mailService = new MailService(_MailSettings);
                var sendGridMessage = mailMessage.GetSendGridMessage();
                sendGridMessage.Asm.GroupId = 10326;

                var result = await mailService.SendAsync(sendGridMessage);
                if (result.StatusCode == HttpStatusCode.OK)
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
                    ErrorDateTime = DateTime.Now
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
            distributionQueueRecord.DateSent = DateTime.Now;
            distributionQueueRecord.IsCompleted = true;
            _context.DistributionQueue.Update(distributionQueueRecord);
            _context.SaveChanges();
        }

        private async Task<List<Models.PricingTemplate>> GetValidPricingTemplates(Models.CustomerInfoByGroup customer)
        {
            PriceFetchingService priceFetchingService = new PriceFetchingService(_context);

            var result = await priceFetchingService.GetAllPricingTemplatesForCustomerAsync(customer, _DistributePricingRequest.FboId,
                _DistributePricingRequest.GroupId, _DistributePricingRequest.PricingTemplate.Oid);
            
            return result;
        }

        private async Task<string> GetMailBody(CustomerInfoByGroup customer, List<Models.PricingTemplate> pricingTemplates)
        {
            string body = "";
            var customerType = _context.CustomerCompanyTypes.Where((x => x.Oid == customer.CustomerCompanyType)).FirstOrDefault();
            if ((customerType?.Name.ToLower().Contains("contract fuel vendor")).GetValueOrDefault())
                body = GetContractFuelVendorMailMessageTemplate();
            else
                body = GetFlightDepartmentMailMessageTemplate();

            body = await PerformBodyReplacements(customer, body, pricingTemplates);
            return body;
        }
        
        private async Task<string> PerformBodyReplacements(Models.CustomerInfoByGroup customer, string body, List<Models.PricingTemplate> pricingTemplates)
        {
            body = body.Replace("%GREETING%", _DistributePricingRequest.EmailContentGreeting.EmailContentHtml);
            body = body.Replace("%SIGNATURE%", _DistributePricingRequest.EmailContentSignature.EmailContentHtml);
            if (!_IsPreview)
                return body;

            StringBuilder priceBreakdown = new StringBuilder();
            foreach (var pricingTemplate in pricingTemplates)
            {
                string html = await GetPriceBreakdownHTML(customer, pricingTemplate);
                priceBreakdown.Append(html);
            }
            body = body.Replace("%PRICE_BREAKDOWN%", priceBreakdown.ToString());

            return body;
        }

        private async Task<byte[]> GetPriceBreakdownImage(Models.CustomerInfoByGroup customer, List<Models.PricingTemplate> pricingTemplates)
        {
            StringBuilder result = new StringBuilder();
            foreach (var pricingTemplate in pricingTemplates)
            {
                string html = await GetPriceBreakdownHTML(customer, pricingTemplate);
                result.Append(html);
            }
            string priceBreakdownHTML = result.ToString();
            return Utilities.HTML.CreateImageFromHTML(priceBreakdownHTML);
        }

        private async Task<string> GetPriceBreakdownHTML(Models.CustomerInfoByGroup customer, Models.PricingTemplate pricingTemplate)
        {
            PriceFetchingService priceFetchingService = new PriceFetchingService(_context);
            var priceResults = await priceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, pricingTemplate.Oid);

            string priceBreakdownTemplate = GetPriceBreakdownTemplate().Replace("%PRICING_TEMPLATE_NAME%", pricingTemplate.Name).Replace("%PRICING_TEMPLATE_NOTES%", pricingTemplate.Notes);
            string rowHTMLTemplate = GetPriceBreakdownRowTemplate();
            StringBuilder rowsHTML = new StringBuilder();

            foreach (DTO.CustomerWithPricing model in priceResults)
            {
                string row = rowHTMLTemplate;
                row =  row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());
                row = row.Replace("%MAX_GALLON%", model.MaxGallons.GetValueOrDefault().ToString());
                row = row.Replace("%ITP%",
                    String.Format("{0:C}",
                        (model.CustomerMarginAmount.GetValueOrDefault() + model.FboFeeAmount.GetValueOrDefault())));
                row = row.Replace("%ALL_IN_PRICE%", String.Format("{0:C}", (model.AllInPrice)));
                rowsHTML.Append(row);
            }

            return priceBreakdownTemplate.Replace("%PRICE_BREAKDOWN_ROWS%", rowsHTML.ToString());
        }

        private string GetFlightDepartmentMailMessageTemplate()
        {
            return Utilities.FileLocater.GetTemplatesFileContent(_FileProvider, "PriceDistribution",
                "FlightDepartmentBody" + (_IsPreview ? "Preview" : "") + ".html");
        }

        private string GetContractFuelVendorMailMessageTemplate()
        {
            return Utilities.FileLocater.GetTemplatesFileContent(_FileProvider, "PriceDistribution",
                "ContractFuelVendorBody" + (_IsPreview ? "Preview" : "") + ".html");
        }

        private string GetPriceBreakdownTemplate()
        {
            return Utilities.FileLocater.GetTemplatesFileContent(_FileProvider, "PriceDistribution",
                "PriceBreakdown" + ".html");
        }

        private string GetPriceBreakdownRowTemplate()
        {
            return Utilities.FileLocater.GetTemplatesFileContent(_FileProvider, "PriceDistribution",
                "PriceBreakdownRow" + ".html");
        }

        private string GetPriceSummaryTemplate()
        {
            return Utilities.FileLocater.GetTemplatesFileContent(_FileProvider, "PriceDistribution",
                "PriceSummary" + ".html");
        }

        private List<Models.ContactInfoByGroup> GetRecipients(Models.CustomerInfoByGroup customer)
        {
            var result = (from cc in _context.CustomerContacts
                join c in _context.Contacts on cc.ContactId equals c.Oid
                join cibg in _context.ContactInfoByGroup on c.Oid equals cibg.ContactId
                where cibg.GroupId == _DistributePricingRequest.GroupId
                      && cc.CustomerId == customer.CustomerId
                      && cibg.CopyAlerts.GetValueOrDefault()
                      && !string.IsNullOrEmpty(cibg.Email)
                select cibg).ToList();
            return result;
        }

        private void PerformPreDistributionTasks(List<CustomerInfoByGroup> customers)
        {
            SaveEmailContent();
            LogDistributionRecord();
            StoreCustomerDistributionQueue(customers);
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
            catch (System.Exception exception)
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
                DateSent = DateTime.Now,
                Fboid = _DistributePricingRequest.FboId,
                GroupId = _DistributePricingRequest.GroupId,
                PricingTemplateId = _DistributePricingRequest.PricingTemplate?.Oid,
                UserId = UserService.GetClaimedUserId(_HttpContextAccessor)
            };
            _context.DistributionLog.Add(distributionLog);
            _context.SaveChanges();
            _DistributionLogID = distributionLog.Oid;
        }

        private void StoreCustomerDistributionQueue(List<CustomerInfoByGroup> customers)
        {
            var records = (from c in customers
                select new Models.DistributionQueue()
                {
                    CustomerId = c.CustomerId,
                    DistributionLogId = _DistributionLogID,
                    Fboid = _DistributePricingRequest.FboId,
                    GroupId = _DistributePricingRequest.GroupId
                }).ToList();
            _context.BulkInsert(records);
        }
        #endregion

        #region Objects
        private class PriceDistributionCustomer
        {
            
        }
        #endregion
    }
}
