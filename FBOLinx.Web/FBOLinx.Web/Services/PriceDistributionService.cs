﻿using EFCore.BulkExtensions;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Mail;
using System.Net.Mail;
using FBOLinx.Web.Models.Responses;
using FBOLinx.ServiceLayer.BusinessServices.PricingTemplate;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.Core.Enums;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.Web.Services
{
    public interface IPriceDistributionService
    {
        Task DistributePricing(DistributePricingRequest request, bool isPreview);
        Task SendCustomerPriceEmail(int groupId, List<GroupCustomerAnalyticsResponse> info, List<string> emails);
    }

    public class PriceDistributionService : IPriceDistributionService
    {
        private DistributePricingRequest _DistributePricingRequest;
        private FboLinxContext _context;
        private bool _IsPreview = false;
        private int _DistributionLogID = 0;
        private string _CurrentPostedRetail = "";
        private string _ValidUntil = "";
        private string _DecimalStringFormat = "";
        private FbosDto _Fbo;
        private FboLinxImageFileData _Logo;
        private FbolinxPricingTemplateFileAttachmentDto _PricingTemplateFileAttachment;
        private IHttpContextAccessor _HttpContextAccessor;
        private IMailTemplateService _MailTemplateService;
        private IPriceFetchingService _PriceFetchingService;
        private IPricingTemplateService _PricingTemplateService;
        private FbolinxEmailContentFileAttachment _EmailContentAttachment;
        private IEmailContentService _EmailContentService;
        private readonly FilestorageContext _fileStorageContext;
        private IMailService _MailService;
        private EmailContent _EmailContent;
        private IPricingTemplateAttachmentService _pricingTemplateAttachmentService;
        private IFboService _fboService;
        private readonly IFboPricesService _fbopricesService;
        private readonly IDistributionErrorsEntityService _distributionErrorsEntityService;
        private readonly ICustomerContactsEntityService _customerContactsEntityService;
        private PriceBreakdownDisplayTypes _PriceBreakdownDisplayType;
        private IFboPreferencesService _fboPreferencesService;
        private List<PricingTemplate> _AllCustomersPricingTemplates;
        private List<ContactInfoByGroup> _Recipients;


        #region Constructors
        public PriceDistributionService(IMailService mailService, FboLinxContext context, IHttpContextAccessor httpContextAccessor, IMailTemplateService mailTemplateService, IPriceFetchingService priceFetchingService, 
            FilestorageContext fileStorageContext, IPricingTemplateService pricingTemplateService, IEmailContentService emailContentService, IPricingTemplateAttachmentService pricingTemplateAttachmentService,
            IFboService fboService, IFboPricesService fbopricesService, IDistributionErrorsEntityService distributionErrorsEntityService, ICustomerContactsEntityService customerContactsEntityService,
             IFboPreferencesService fboPreferencesService)
        {
            _PriceFetchingService = priceFetchingService;
            _MailTemplateService = mailTemplateService;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _fileStorageContext = fileStorageContext;
            _MailService = mailService;
            _PricingTemplateService = pricingTemplateService;
            _EmailContentService = emailContentService;
            _pricingTemplateAttachmentService = pricingTemplateAttachmentService;
            _fboService = fboService;
            _fbopricesService = fbopricesService;
            _distributionErrorsEntityService = distributionErrorsEntityService;
            _customerContactsEntityService = customerContactsEntityService;
            _fboPreferencesService = fboPreferencesService;
        }
        #endregion


        #region Public Methods
        public async Task DistributePricing(DistributePricingRequest request, bool isPreview)
        {
            _DistributePricingRequest = request;
            _IsPreview = isPreview;
            _DecimalStringFormat = await _fboPreferencesService.GetDecimalPrecisionStringFormat(request.FboId);

            var customers = new List<CustomerInfoByGroupDto>();
            customers = await GetCustomersForDistribution(request);

            if (customers == null)
                return;

            await PerformPreDistributionTasks(customers);

            //#tx9ztn - Removed the requirement for aircraft in the account to send a distribution email. A fleet is no longer requireed to receive pricing.
            if (_IsPreview)
            {
                await GenerateDistributionMailMessage(customers[0]);
            }
            else
            {
                var customerToSend = new CustomerInfoByGroupDto();

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
        private async Task<List<CustomerInfoByGroupDto>> GetCustomersForDistribution(DistributePricingRequest request)
        {
            List<CustomerInfoByGroupDto> customers;
            if (request.Customer == null || request.Customer.Oid == 0)
            {
                customers = await GetCustomersForDistribution();
            }
            else
            {
                customers = new List<CustomerInfoByGroupDto>();
                customers.Add(request.Customer);
            }

            if (_DistributePricingRequest.CustomerCompanyType == 0)
                return customers;
            return customers.Where(x => x.CustomerCompanyType == _DistributePricingRequest.CustomerCompanyType).ToList();
        }

        private async Task<List<CustomerInfoByGroupDto>> GetCustomersForDistribution()
        {
            var result = await (from cg in _context.Set<CustomerInfoByGroup>()
                                join c in _context.Set<Customers>() on cg.CustomerId equals c.Oid
                                join cct in _context.Set<CustomCustomerTypes>() on cg.CustomerId equals cct.CustomerId
                                join pt in _context.Set<PricingTemplate>() on cct.CustomerType equals pt.Oid
                                where cg.GroupId == _DistributePricingRequest.GroupId && pt.Oid == _DistributePricingRequest.PricingTemplate.Oid
                                && !(c.Suspended ?? false)
                                select new CustomerInfoByGroupDto
                                {
                                    Oid = cg.Oid,
                                    GroupId = cg.GroupId,
                                    CustomerId = cg.CustomerId,
                                    Company = cg.Company,
                                    Username = cg.Username,
                                    Password = cg.Password,
                                    Joined = cg.Joined,
                                    Active = cg.Active,
                                    Distribute = cg.Distribute,
                                    Network = cg.Network,
                                    MainPhone = cg.MainPhone,
                                    Address = cg.Address,
                                    City = cg.City,
                                    State = cg.State,
                                    ZipCode = cg.ZipCode,
                                    Country = cg.Country,
                                    Website = cg.Website,
                                    ShowJetA = cg.ShowJetA,
                                    Suspended = cg.Suspended,
                                    DefaultTemplate = cg.DefaultTemplate,
                                    CustomerType = cg.CustomerType,
                                    EmailSubscription = cg.EmailSubscription,
                                    Sfid = cg.Sfid,
                                    CertificateType = cg.CertificateType,
                                    CustomerCompanyType = cg.CustomerCompanyType,
                                    PricingTemplateRemoved = cg.PricingTemplateRemoved,
                                    MergeRejected = cg.MergeRejected
                                }).ToListAsync();
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

        private async Task GenerateDistributionMailMessage(CustomerInfoByGroupDto customer)
        {
            DistributionQueue distributionQueueRecord = new DistributionQueue();
            try
            {
                var validPricingTemplates = GetValidPricingTemplates(customer);
                if (validPricingTemplates == null || validPricingTemplates.Count == 0)
                {
                    return;
                }

                if (!_IsPreview)
                {
                    await StoreSingleCustomer(customer);

                    var distributionQueueRecords = await _context.Set<DistributionQueue>().Where((x =>
                        x.CustomerId == customer.CustomerId && x.Fboid == _DistributePricingRequest.FboId &&
                        x.GroupId == _DistributePricingRequest.GroupId && x.DistributionLogId == _DistributionLogID)).ToListAsync();

                    distributionQueueRecord = distributionQueueRecords.FirstOrDefault();
                }

                var recipients = _Recipients.Where(r => r.CustomerContact.CustomerId == customer.CustomerId).ToList();
                if (!_IsPreview && recipients.Count == 0)
                {
                    await MarkDistributionRecordAsComplete(distributionQueueRecord);
                    return;
                }

                //Add email content to MailMessage
                FBOLinxMailMessage mailMessage = new FBOLinxMailMessage();
                mailMessage.From = new MailAddress(MailService.GetFboLinxAddress(_Fbo.SenderAddress));
                if (_Fbo.ReplyTo != null && _Fbo.ReplyTo != "")
                    mailMessage.ReplyToList.Add(_Fbo.ReplyTo);
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

                //Get additional attachments
                if (_PricingTemplateFileAttachment != null && _PricingTemplateFileAttachment.Oid > 0)
                    mailMessage.AttachmentsCollection.Add(new Core.Utilities.Mail.FileAttachment { ContentType = _PricingTemplateFileAttachment.ContentType, FileName = _PricingTemplateFileAttachment.FileName, FileData = Convert.ToBase64String(_PricingTemplateFileAttachment.FileData, 0, _PricingTemplateFileAttachment.FileData.Length) });

                if (_EmailContentAttachment != null)
                    mailMessage.AttachmentsCollection.Add(new Core.Utilities.Mail.FileAttachment { ContentType = _EmailContentAttachment.ContentType, FileName = _EmailContentAttachment.FileName, FileData = Convert.ToBase64String(_EmailContentAttachment.FileData, 0, _EmailContentAttachment.FileData.Length) });

                if (_Logo != null && _Logo.Oid > 0)
                {
                    mailMessage.Logo = new LogoDetails();
                    mailMessage.Logo.Filename = "logo." + _Logo.ContentType.Split('/')[1];
                    mailMessage.Logo.ContentType = _Logo.ContentType;
                    mailMessage.Logo.Base64String = Convert.ToBase64String(_Logo.FileData);
                }

                //_DistributePricingRequest.PricingTemplate.Notes = Regex.Replace(_DistributePricingRequest.PricingTemplate.Notes, @"<[^>]*>", String.Empty);

                // Add all of the images for the different products
                //Add the price breakdown as an image to prevent parsing
                Dictionary<string, byte[]> priceBreakdownImages = await GetPriceBreakdownImage(customer, validPricingTemplates);
                if (priceBreakdownImages.Count == 0)
                    return;

                var pricesForProducts = new List<PricesForProducts>();
                foreach (var priceBreakdownImage in priceBreakdownImages)
                {
                    pricesForProducts.Add(new PricesForProducts() { cId = "cid:" + priceBreakdownImage.Key, imageBase64 = Convert.ToBase64String(priceBreakdownImage.Value) });
                }

                var pricingLeftRightPadding = 0;
                if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                {
                    if (pricesForProducts.Count > 1)
                    {
                        var pricesForProductsCount = 1;
                        foreach (var price in pricesForProducts)
                        {
                            if (pricesForProductsCount / pricesForProducts.Count == 1)
                            {
                                price.isLeftPosition = false;
                            }
                            pricesForProductsCount++;
                        }
                    }
                    else
                        pricingLeftRightPadding = 110;
                }
                else
                {
                    if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly || _PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                        pricingLeftRightPadding = 110;
                }

                var dynamicTemplateData = new ServiceLayer.DTO.UseCaseModels.Mail.SendGridDistributionTemplateData
                {
                    recipientCompanyName = _IsPreview ? _Fbo.Fbo : customer.Company,
                    templateEmailBodyMessage = HttpUtility.HtmlDecode(_EmailContent.EmailContentHtml ?? ""),
                    templateNotesMessage = HttpUtility.HtmlDecode(_DistributePricingRequest.PricingTemplate.Notes),
                    fboName = _Fbo.Fbo,
                    fboICAOCode = _Fbo.FboAirport.Icao,
                    fboAddress = _Fbo.Address,
                    fboCity = _Fbo.City,
                    fboState = _Fbo.State,
                    fboZip = _Fbo.ZipCode,
                    Subject = HttpUtility.HtmlDecode(_EmailContent.Subject) ?? "Distribution pricing",
                    expiration = _ValidUntil,
                    currentPostedRetail = _CurrentPostedRetail,
                    pricesForProducts = pricesForProducts,
                    isLogo = _Logo == null || _Logo.Oid == 0 ? false : true,
                    isPricingDisplayTypeSingle = _PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights ? true : false,
                    isPricingDisplayTypeDouble = _PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly || _PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly ? true : false,
                    isPricingDisplayTypeAllFour = _PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.FourColumnsAllRules ? true : false,
                    pricingLeftRightPadding = pricingLeftRightPadding == 0 ? 70 : pricingLeftRightPadding
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

                await _distributionErrorsEntityService.AddAsync(errorRecord);
            }
        }

        private async Task MarkDistributionRecordAsComplete(DistributionQueue distributionQueueRecord)
        {
            if (distributionQueueRecord == null || distributionQueueRecord.DistributionLogId == 0)
                return;
            distributionQueueRecord.DateSent = DateTime.Now.ToUniversalTime();
            distributionQueueRecord.IsCompleted = true;
            _context.Set<DistributionQueue>().Update(distributionQueueRecord);
            await _context.SaveChangesAsync();
        }

        private async Task StoreSingleCustomer(CustomerInfoByGroupDto customer)
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

        private List<PricingTemplate> GetValidPricingTemplates(CustomerInfoByGroupDto customer)
        {
            var result = _AllCustomersPricingTemplates.Where(a => a.CustomerId == customer.CustomerId && a.Oid == _DistributePricingRequest.PricingTemplate.Oid).ToList();

            //var result = await _PricingTemplateService.GetAllPricingTemplatesForCustomerAsync(customer, _DistributePricingRequest.FboId,
            //    _DistributePricingRequest.GroupId, _DistributePricingRequest.PricingTemplate.Oid);

            return result;
        }

        private async Task<Dictionary<string, byte[]>> GetPriceBreakdownImage(CustomerInfoByGroupDto customer, List<PricingTemplate> pricingTemplates)
        {
            Dictionary<string, byte[]> productImages = new Dictionary<string, byte[]>();
            foreach (var pricingTemplate in pricingTemplates)
            {
                Dictionary<string, string> productsHtml = await GetPriceBreakdownHTML(customer, pricingTemplate);
                foreach (var html in productsHtml)
                {
                    StringBuilder result = new StringBuilder();
                    result.Append(html.Value);
                    string priceBreakdownHTML = result.ToString();

                    int width = 190;
                    if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly || _PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                        width = 380;
                    else if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.FourColumnsAllRules)
                        width = 460;
                    productImages.Add(html.Key, FBOLinx.Core.Utilities.HTML.CreateImageFromHTML(priceBreakdownHTML, width));
                }
            }

            return productImages;
        }

        private async Task<Dictionary<string, string>> GetPriceBreakdownHTML(CustomerInfoByGroupDto customer, PricingTemplate pricingTemplate)
        {
            var productImageHtml = new Dictionary<string, string>();

            var commercialInternationalPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Commercial, FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly);
            var privateInternationalPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Private, FBOLinx.Core.Enums.ApplicableTaxFlights.InternationalOnly);
            var commercialDomesticPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Commercial, FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly);
            var privateDomesticPricingResults = await _PriceFetchingService.GetCustomerPricingAsync(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId, customer.Oid, new List<int> { pricingTemplate.Oid }, FBOLinx.Core.Enums.FlightTypeClassifications.Private, FBOLinx.Core.Enums.ApplicableTaxFlights.DomesticOnly);

            var products = privateDomesticPricingResults.GroupBy(p => p.Product).ToList();

            foreach (var product in products)
            {
                var privateDomesticPricingResultsByProduct = privateDomesticPricingResults.Where(p => p.Product == product.Key).OrderBy(s => s.MinGallons).ToList();
                var genericProduct = product.Key.Contains("(") ? product.Key.Substring(0, Math.Max(product.Key.IndexOf('('), 0)).Trim() : product.Key;

                string priceBreakdownTemplate = GetPriceBreakdownTemplate();
                string rowHTMLTemplate = GetPriceBreakdownRowTemplate();
                StringBuilder rowsHTML = new StringBuilder();
                int loopIndex = 0;

                foreach (CustomerWithPricing model in privateDomesticPricingResultsByProduct)
                {
                    string row = rowHTMLTemplate;

                    if ((loopIndex + 1) < privateDomesticPricingResultsByProduct.Count)
                    {
                        row = row.Replace("%MIN_GALLON%", model.MinGallons.GetValueOrDefault().ToString());
                        var next = privateDomesticPricingResultsByProduct[loopIndex + 1];
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
                        row = row.Replace("&nbsp;-&nbsp;", "");
                    }

                    row = row.Replace("%ALL_IN_PRICE%", String.Format(_DecimalStringFormat, (model.AllInPrice.GetValueOrDefault())));
                    row = row.Replace("%ALL_IN_PRICE_INT_COMM%", String.Format(_DecimalStringFormat, (commercialInternationalPricingResults.Where(s => s.Product.Contains(genericProduct) && s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                    row = row.Replace("%ALL_IN_PRICE_INT_PRIVATE%", String.Format(_DecimalStringFormat, (privateInternationalPricingResults.Where(s => s.Product.Contains(genericProduct) && s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                    row = row.Replace("%ALL_IN_PRICE_DOMESTIC_COMM%", String.Format(_DecimalStringFormat, (commercialDomesticPricingResults.Where(s => s.Product.Contains(genericProduct) && s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                    row = row.Replace("%ALL_IN_PRICE_DOMESTIC_PRIVATE%", String.Format(_DecimalStringFormat, (privateDomesticPricingResultsByProduct.Where(s => s.Product.Contains(genericProduct) && s.MinGallons == model.MinGallons).Select(s => s.AllInPrice.GetValueOrDefault())).FirstOrDefault()));
                    rowsHTML.Append(row);

                    loopIndex++;
                }

                productImageHtml.Add(genericProduct.Replace(" ", ""), priceBreakdownTemplate.Replace("%PRODUCT%", genericProduct.Replace(" ", "")).Replace("%PRICE_BREAKDOWN_ROWS%", rowsHTML.ToString()));
            }

            return productImageHtml;
        }

        private string GetPriceBreakdownTemplate()
        {
            if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownSingleColumnAllFlights.html");
            else if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownTwoColumnsApplicableFlightTypesOnly.html");
            else if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownTwoColumnsDepartureOnly.html");
            else
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownFourColumnsAllRules.html");
        }

        private string GetPriceBreakdownRowTemplate()
        {
            if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.SingleColumnAllFlights)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowSingleColumnAllFlights.html");
            else if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsApplicableFlightTypesOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowTwoColumnsApplicableFlightTypesOnly.html");
            else if (_PriceBreakdownDisplayType == PriceBreakdownDisplayTypes.TwoColumnsDomesticInternationalOnly)
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowTwoColumnsDepartureOnly.html");
            else
                return _MailTemplateService.GetTemplatesFileContent("PriceDistribution",
                    "PriceBreakdownRowFourColumnsAllRules.html");
        }

        public async Task SendCustomerPriceEmail(int groupId, List<GroupCustomerAnalyticsResponse> info, List<string> emails)
        {
            var emailContent = _context.EmailContent.Where(e => e.GroupId == groupId).FirstOrDefault();

            //Add the price breakdown as an image to prevent parsing
            string priceBreakdownCsvContent = await GetCustomerPriceBreakdownCSV(info);

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

            mailMessage.AttachmentsCollection.Add(new Core.Utilities.Mail.FileAttachment { FileData = Convert.ToBase64String(Encoding.UTF8.GetBytes(priceBreakdownCsvContent)) });

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
                recipientCompanyName = info.Count > 0 ? info[0].Company : "",
            };
            mailMessage.SendGridGroupCustomerPricingTemplateData = dynamicTemplateData;

            //Send email
            await _MailService.SendAsync(mailMessage);
        }

        private async Task<string> GetCustomerPriceBreakdownCSV(List<GroupCustomerAnalyticsResponse> info)
        {
            string priceBreakdownTemplate = "";
            string rowHTMLTemplate = "";
            StringBuilder rowsHTML = new StringBuilder();

            foreach (GroupCustomerAnalyticsResponse groupCustomerAnalyticsResponse in info)
            {
                var defaultGroupedFboPrices = groupCustomerAnalyticsResponse.GroupCustomerFbos.FirstOrDefault();
                var defaultPrice = defaultGroupedFboPrices.Prices.FirstOrDefault();

                if (defaultGroupedFboPrices == null || defaultPrice == null)
                {
                    priceBreakdownTemplate = GetCustomerPriceBreakdownTemplate(PriceBreakdownDisplayTypes.SingleColumnAllFlights);
                    rowsHTML.Append("");
                }
                else
                {
                    _DecimalStringFormat = await _fboPreferencesService.GetDecimalPrecisionStringFormat(groupCustomerAnalyticsResponse.FboId);
                    priceBreakdownTemplate = GetCustomerPriceBreakdownTemplate(defaultPrice.PriceBreakdownDisplayType);
                    rowHTMLTemplate = GetCustomerPriceBreakdownRowTemplate(defaultPrice.PriceBreakdownDisplayType);

                    foreach (var groupCustomerFbos in groupCustomerAnalyticsResponse.GroupCustomerFbos)
                    {
                        for (var i = 0; i < groupCustomerFbos.Prices.Count; i++)
                        {
                            var model = groupCustomerFbos.Prices[i];
                            string row = rowHTMLTemplate;

                            if (i == 0)
                            {
                                row = row.Replace("%FBO%", groupCustomerFbos.Icao);
                            }
                            else
                            {
                                row = row.Replace("\"%FBO%\"", "");
                            }


                            var next = i < groupCustomerFbos.Prices.Count - 1 ? groupCustomerFbos.Prices[i + 1] : null;

                            if (next != null && next.Product == groupCustomerFbos.Prices[i].Product)
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
                            }
                            else
                            {
                                string output = Convert.ToDouble(model.MinGallons).ToString("#,##", CultureInfo.InvariantCulture);

                                output = output + "+";
                                row = row.Replace("%MIN_GALLON%", output);
                                row = row.Replace("%MAX_GALLON%", "");
                                row = row.Replace("-", "");
                            }

                            row = row.Replace("%ALL_IN_PRICE%", String.Format(_DecimalStringFormat, model.DomPrivate.GetValueOrDefault()));
                            row = row.Replace("%ALL_IN_PRICE_INT_COMM%", String.Format(_DecimalStringFormat, model.IntComm.GetValueOrDefault()));
                            row = row.Replace("%ALL_IN_PRICE_INT_PRIVATE%", String.Format(_DecimalStringFormat, model.IntPrivate.GetValueOrDefault()));
                            row = row.Replace("%ALL_IN_PRICE_DOMESTIC_COMM%", String.Format(_DecimalStringFormat, model.DomComm.GetValueOrDefault()));
                            row = row.Replace("%ALL_IN_PRICE_DOMESTIC_PRIVATE%", String.Format(_DecimalStringFormat, model.DomPrivate.GetValueOrDefault()));
                            row = row.Replace("%PRODUCT%", model.Product);

                            row = row.Replace("%TAIL_NUMBERS%", groupCustomerAnalyticsResponse.TailNumbers);

                            rowsHTML.Append(row);
                        }
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
    
        private async Task PerformPreDistributionTasks(List<CustomerInfoByGroupDto> customers)
        {

            await GetEmailContent();
            if (!_IsPreview)
                await LogDistributionRecord();

            var result = await _fbopricesService.GetPrices(_DistributePricingRequest.FboId);
            //Get current posted retail
            var currentRetailResult = result.Where(f => f.Product == "JetA Retail" && (f.EffectiveFrom <= DateTime.UtcNow || f.EffectiveTo == null)).FirstOrDefault();
            _CurrentPostedRetail = "Current Posted Retail: " + String.Format(_DecimalStringFormat, currentRetailResult.Price);

            //Get expiration date
            var priceDate = new DateTime();
            if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("Retail"))
            {
                var date = result.Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo != null && fp.Fboid == _DistributePricingRequest.FboId && fp.Product == "JetA Retail").OrderByDescending(fp => fp.Oid).Select(fp => fp.EffectiveTo).FirstOrDefault();

                priceDate = date.GetValueOrDefault();
            }
            else if (_DistributePricingRequest.PricingTemplate.MarginTypeProduct.Equals("Cost"))
            {
                var date = result.Where(fp => fp.EffectiveFrom <= DateTime.UtcNow && fp.EffectiveTo != null && fp.Fboid == _DistributePricingRequest.FboId && fp.Product == "JetA Cost").OrderByDescending(fp => fp.Oid).Select(fp => fp.EffectiveTo).FirstOrDefault();

                priceDate = date.GetValueOrDefault();
            }

            if (priceDate != null)
            {
                var localDateTime = await _fboService.GetAirportLocalDateTimeByUtcFboId(priceDate, _DistributePricingRequest.FboId);
                var localTimeZone = await _fboService.GetAirportTimeZoneByFboId(_DistributePricingRequest.FboId);
                _ValidUntil = "Pricing valid until: " + localDateTime.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " " + localTimeZone;
            }

            _Fbo = await _fboService.GetFbo(_DistributePricingRequest.FboId, false);

            _Logo = await _fileStorageContext.FboLinxImageFileData.Where(f => f.FboId == _Fbo.Oid).FirstOrDefaultAsync();

            _PricingTemplateFileAttachment = await _pricingTemplateAttachmentService.GetFileAttachmentObject(_DistributePricingRequest.PricingTemplate.Oid);

            _EmailContentAttachment = await _EmailContentService.GetFileAttachmentObject(_EmailContent.Oid);

            _AllCustomersPricingTemplates = await _PricingTemplateService.GetAllPricingTemplatesForFbo(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId);

            _Recipients = await _customerContactsEntityService.GetRecipientsForGroupFbo(_DistributePricingRequest.FboId, _DistributePricingRequest.GroupId);

            _PriceBreakdownDisplayType =
               await _PriceFetchingService.GetPriceBreakdownDisplayType(_DistributePricingRequest.FboId);
        }

        private async Task GetEmailContent()
        {
            _EmailContent = await _context.Set<EmailContent>().Where(x => x.Oid == _DistributePricingRequest.PricingTemplate.EmailContentId).FirstOrDefaultAsync();

            if (_DistributePricingRequest.PricingTemplate.EmailContentId == null || _DistributePricingRequest.PricingTemplate.EmailContentId == 0 || _EmailContent == null)
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