using EFCore.BulkExtensions;
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
using MailSettings = FBOLinx.Web.Configurations.MailSettings;

namespace FBOLinx.Web.Services
{
    public class PriceDistributionService
    {
        private MailSettings _MailSettings;
        private DistributePricingRequest _DistributePricingRequest;
        private FboLinxContext _context;
        private FuelerLinxContext _fuelerLinxContext;
        private IFileProvider _FileProvider;
        private bool _IsPreview = false;
        private int _DistributionLogID = 0;
        private IHttpContextAccessor _HttpContextAccessor;

        #region Constructors
        public PriceDistributionService(MailSettings mailSettings, FboLinxContext context, FuelerLinxContext fuelerLinxContext, IFileProvider fileProvider, IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
            _FileProvider = fileProvider;
            _context = context;
            _fuelerLinxContext = fuelerLinxContext;
            _MailSettings = mailSettings;
        }
        #endregion

        #region Static Methods
        public static async Task BeginPriceDistribution(MailSettings mailSettings, FboLinxContext context, FuelerLinxContext fuelerLinxContext,
            Models.Requests.DistributePricingRequest request, IFileProvider fileProvider, IHttpContextAccessor httpContextAccessor)
        {
            PriceDistributionService service = new PriceDistributionService(mailSettings, context, fuelerLinxContext, fileProvider, httpContextAccessor);
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
            //var result = (from cg in _context.CustomerInfoByGroup
            //    join c in _context.Customers on cg.CustomerId equals c.Oid
            //    where cg.GroupId == _DistributePricingRequest.GroupId
            //    && cg.Distribute.GetValueOrDefault()
            //    select cg).ToList();

            var result = (from cg in _context.CustomerInfoByGroup
                          join c in _context.Customers on cg.CustomerId equals c.Oid
                          join cct in _context.CustomCustomerTypes on cg.CustomerId equals cct.CustomerId
                          join pt in _context.PricingTemplate on cct.CustomerType equals pt.Oid
                          where cg.GroupId == _DistributePricingRequest.GroupId && pt.Oid == _DistributePricingRequest.PricingTemplate.Oid
                          // && cg.Distribute.GetValueOrDefault()
                          select cg).ToList();

            return result;
        }

        private async Task GenerateDistributionMailMessage(Models.CustomerInfoByGroup customer)
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
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_MailSettings.MailUserName);
                var recipients = GetRecipientsForCustomer(customer);
                var fboRecipients = await GetRecipientsForFbo();
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
                /* if (fboRecipients != null && fboRecipients.Count > 0)
                 {
                     foreach (var fboRecipient in fboRecipients)
                     {
                         if (fboRecipient.Contact == null)
                             continue;
                         if (_MailSettings.IsValidEmailRecipient(fboRecipient.Contact.Email))
                             mailMessage.CC.Add(new MailAddress(fboRecipient.Contact.Email));
                     }
                 }*/

                string validUntil = "";
                
                if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("JetA Retail"))
                {
                    
                }
                else if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("JetA Cost"))
                {
                    var priceDate = _context.Fboprices.LastOrDefault(s => s.Fboid == _DistributePricingRequest.FboId).EffectiveTo;
                    if(priceDate != null)
                    {
                        DateTime dtEffectiveTo = Convert.ToDateTime(priceDate);
                        validUntil = "Pricing valid until: " + dtEffectiveTo.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }

                    string str = _DistributePricingRequest.PricingTemplate.MarginTypeProduct == "JetA Retail" ? "Retail -" : _DistributePricingRequest.PricingTemplate.MarginTypeProduct == "JetA Retail" ? "Cost +" : "Fleet Fee";
                string body = await getCustomBody(_DistributePricingRequest.PricingTemplate.Oid,str, _DistributePricingRequest.FboId);//_DistributePricingRequest.PricingTemplate.Email;//await GetMailBody(customer, validPricingTemplates);

                    //Add the price breakdown as an image to prevent parsing
                    byte[] priceBreakdownImage = await GetPriceBreakdownImage(customer, validPricingTemplates);

                    var imageStream = new MemoryStream(priceBreakdownImage);

                    //Attachment priceBreakdownAttachment = new Attachment(imageStream, new ContentType("image/jpeg"));
                    //priceBreakdownAttachment.ContentDisposition.Inline = true;
                    //priceBreakdownAttachment.Name = "PriceBreakdown.jpg";
                    //var linkedResource = new LinkedResource(imageStream, new ContentType("image/jpeg"));
                    //linkedResource.ContentId = Guid.NewGuid().ToString();
                    //priceBreakdownAttachment.ContentId = linkedResource.ContentId;
                    //body = body.Replace("%PRICE_BREAKDOWN_CONTENT_ID%", linkedResource.ContentId);
                    //mailMessage.Attachments.Add(priceBreakdownAttachment);

                    //body = body.Replace("%PRICE_BREAKDOWN_IMAGE_BASE64%", System.Convert.ToBase64String(priceBreakdownImage));

                    //mailMessage.From = new MailAddress("donotreply@fbolinx.com");
                    //mailMessage.Body = body;
                    //mailMessage.IsBodyHtml = true;
                    //mailMessage.Subject = _DistributePricingRequest.PricingTemplate.Subject ?? "Distribution pricing";

                    //Convert to a SendGrid message and use their API to send it
                    Services.MailService mailService = new MailService(_MailSettings);
                    //var sendGridMessage = mailMessage.GetSendGridMessage();
                    //sendGridMessage.Asm = new ASM() { GroupId = 10326 };

                    var sendGridMessageWithTemplate = new SendGridMessage();
                    sendGridMessageWithTemplate.From = new EmailAddress("donotreply@fbolinx.com");
                Personalization personalization = new Personalization();

                personalization.Tos = new System.Collections.Generic.List<EmailAddress>();

                foreach (ContactInfoByGroup contactInfoByGroup in recipients)
                {
                    if (_MailSettings.IsValidEmailRecipient(contactInfoByGroup.Email))
                        personalization.Tos.Add(new EmailAddress(contactInfoByGroup.Email));
                }

                //personalization.Tos.Add(new EmailAddress("angelarnaudov@gmail.com"));

                sendGridMessageWithTemplate.Personalizations = new List<Personalization>();
                sendGridMessageWithTemplate.Personalizations.Add(personalization);

                var fbo = _context.Fbos.FirstOrDefault(s => s.Oid == _DistributePricingRequest.FboId);
                var fboIcao = _context.Fboairports.FirstOrDefault(s => s.Fboid == fbo.Oid).Icao;

                _DistributePricingRequest.PricingTemplate.Notes = Regex.Replace(_DistributePricingRequest.PricingTemplate.Notes, @"<[^>]*>", String.Empty);
                var dynamicTemplateData = new ExampleTemplateData
                {
                    recipientCompanyName = customer.Company,
                    templateEmailBodyMessage = HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Email),
                    templateNotesMessage =  HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Notes),
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

                sendGridMessageWithTemplate.HtmlContent = body;
                
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
        public Expression<Func<dynamic, bool>> GetSelectXpr(string filter, string value)
        {
            return null;
        }

        private async Task<string> getCustomBody(int num,string id,int fboId)
        {
            var pom = await new Controllers.CustomerMarginsController(_context).GetCustomerMarginsByPricingTemplateId(num);
            var res = pom as OkObjectResult;
            var prom = await new Controllers.FbopricesController(_context, _fuelerLinxContext).GetFbopricesByFboIdCurrent(fboId);
            var resProm = prom as OkObjectResult;
         
            string body = "";
            //string title = id == 1 ? "Retail-" : id == 0 ? "Cost+" : "Flat Fee";
            body = "<div style=\"500 13px/25px Roboto, \"Helvetica Neue\", sans-serif;display:flex;flex-wrap:wrap;box-sizing:border-box;\"><div style=\"max-width: 20%;\"><div style=\"font-weight: bold;text-align:center !important;margin-top:1.5rem !important;min-width: 65%;max-width:75%\">" + id + "</div><div style=\"font-size:11px;min-width: 65%;max-width:75%\"><div style=\"max-width:66.66666667%;flex:0 0 66.66666667%;float:left !important\">Volume(gal.)</div><div style=\"max-width:33.33333333%;flex:0 0 33.33333333%;float:right !important\">Price</div></div><div>";
            body += "<table style=\"min-width: 65%;max-width:75%;border-spacing:0;margin-bottom:0;margin-top:0.5rem !important;border-collapse:collapse !important;display:table !important;\"><thead><tr style=\"display:table-row;vertical-align:middle;\"><th style=\"text-align:center !important;display:table-cell;font-weight:bold\">Min</th><th style=\"text-align:center !important;display:table-cell;font-weight:bold\">Max</th><th style=\"text-align:center !important;display:table-cell;font-weight:bold\">All-in</th></tr></thead>";
            body += "<tbody style=\"border:1px solid #000;display:table-row-group;vertical-align:middle;border-spacing:0\">";
            foreach(var pm in res.Value as IEnumerable<CustomerMarginsGridViewModel>)
            {
                IEnumerable<dynamic> tst = resProm.Value as IEnumerable<dynamic>;
                var pom1 = tst.GetType().GetProperties().Where(x => x.Name == "Product").Select(s => s).FirstOrDefault();
                double? bd = pm.Min == 0 ? pm.Min + 249 : pm.Max;
                if (pm.Max.ToString().Equals("99999"))
                {
                    body += "<tr class=\"mat-row mat-row-not-hover text-left\"><td cstyle=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">" + pm.Min + "+</td><td style=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">&nbsp;</td>";
                }
                else
                {
                    body += "<tr class=\"mat-row mat-row-not-hover text-left\"><td cstyle=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">" + pm.Min + "</td><td style=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">" + bd.ToString();
                }
                //body += "<tr class=\"mat-row mat-row-not-hover text-left\"><td cstyle=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">" + pm.Min + "</td><td style=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">" + bd.ToString();
                body += "</td><td style=\"padding:0 0.6875rem;padding-right:24px;text-align:center !important\">$" + pm.Amount + "</td></tr>";
            }
            body += "</tbody></table></div></div><div style=\"max-width: 20%;\">" + _DistributePricingRequest.PricingTemplate.Email + "</div></div>";

            return body;
        }

        //    private Task<int> updateCustomerMargin(CustomerMarginsGridViewModel pm, OkObjectResult prom)
        //    {
        //    var jetACost = 
        //    var jetARetail = this.currentPrice.filter(item => item.product == 'JetA Retail')[0].price;
        //    int allin = 0;
        //    if (this.data.marginType == 0)
        //    {
        //        if (pm.Min!=null && pm.itp)
        //        {
        //                allin = jetACost + pm;
        //        }

        //    }
        //    else if (pm.marginType == 1)
        //    {
        //            if (pm.Amount != null && pm.Min != null)
        //        {
        //            allin = jetARetail - pm.amount;
        //            if (margin.allin)
        //            {
        //                margin.itp = margin.allin - jetACost;
        //            }
        //        }
        //    }
        //    return allin;
        //}

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

            priceResults = priceResults.GroupBy(s => s.MinGallons).Select(s => s.Last()).ToList();

            //string priceBreakdownTemplate = GetPriceBreakdownTemplate().Replace("%PRICING_TEMPLATE_NAME%", pricingTemplate.Name).Replace("%PRICING_TEMPLATE_NOTES%", pricingTemplate.Notes);
            //string priceBreakdownTemplate = GetPriceBreakdownTemplate().Replace("%PRICING_TEMPLATE_NAME%", pricingTemplate.Name).Replace("%PRICING_TEMPLATE_NOTES%", "");
            string priceBreakdownTemplate = GetPriceBreakdownTemplate();
            string rowHTMLTemplate = GetPriceBreakdownRowTemplate();
            StringBuilder rowsHTML = new StringBuilder();
            int loopIndex = 0;
            foreach (DTO.CustomerWithPricing model in priceResults)
            {
                string row = rowHTMLTemplate;

                //if(model.MinGallons > 999)
                //{
                //    string output = Convert.ToDouble(model.MaxGallons).ToString("#,##", CultureInfo.InvariantCulture);
                //    row = row.Replace("%MIN_GALLON%", output);
                //}
                //else
                //{
                //    row = row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());
                //}
                //row =  row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());

                if (loopIndex + 1 < priceResults.Count)
                {
                    row = row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());
                    var next = priceResults[loopIndex + 1];
                    Double maxValue = Convert.ToDouble(next.MinGallons) - 1;

                    if(maxValue > 999)
                    {
                        string output = Convert.ToDouble(next.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);
                        row = row.Replace("%MAX_GALLON%", output);
                    }
                    else
                    {
                        row = row.Replace("%MAX_GALLON%", maxValue.ToString());
                    }

                    //row = row.Replace("%MAX_GALLON%", maxValue.ToString());
                }
                else
                {
                    //if(model.MaxGallons > 999)
                    //{
                    //    string output = Convert.ToDouble(model.MaxGallons).ToString("#,##", CultureInfo.InvariantCulture);
                    //    row = row.Replace("%MAX_GALLON%", output);
                    //}
                    //else
                    //{
                    //    row = row.Replace("%MAX_GALLON%", model.MaxGallons.GetValueOrDefault().ToString());
                    //}
                    string output = Convert.ToDouble(model.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);

                    output = output + "+";
                    //row = row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());
                    row = row.Replace("%MIN_GALLON%", output);
                    row = row.Replace("%MAX_GALLON%", "");
                    row = row.Replace("-", "");
                }
                    
                //row = row.Replace("%MAX_GALLON%", model.MaxGallons.GetValueOrDefault().ToString());
                //row = row.Replace("%ITP%",
                //    String.Format("{0:C}",
                //        (model.CustomerMarginAmount.GetValueOrDefault() + model.FboFeeAmount.GetValueOrDefault())));
                row = row.Replace("%ALL_IN_PRICE%", String.Format("{0:C}", (model.AllInPrice)));
                rowsHTML.Append(row);
                loopIndex++;
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

        private List<Models.ContactInfoByGroup> GetRecipientsForCustomer(Models.CustomerInfoByGroup customer)
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

        private async Task<List<Models.Fbocontacts>> GetRecipientsForFbo()
        {
            var result = await _context.Fbocontacts.Include("Contact")
                .Where((x => x.Fboid == _DistributePricingRequest.FboId)).ToListAsync();

            return result;
        }

        private void PerformPreDistributionTasks(List<CustomerInfoByGroup> customers)
        {
            SaveEmailContent();
            LogDistributionRecord();
           // StoreCustomerDistributionQueue(customers);
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

        private class ExampleTemplateData
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
