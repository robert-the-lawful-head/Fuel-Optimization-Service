﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.DB.Specifications.Fbo;
using FBOLinx.DB.Specifications.FuelRequests;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.FuelPricing;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.Requests.FuelReq;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Requests.Integrations;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FBOLinx.ServiceLayer.BusinessServices.FuelRequests;
using FBOLinx.ServiceLayer.BusinessServices.Orders;
using FBOLinx.ServiceLayer.DTO.Requests.ServiceOrder;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using Azure.Core;
using FBOLinx.DB.Specifications.User;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.DB.Specifications.OrderDetails;
using FBOLinx.ServiceLayer.BusinessServices.Customers;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IntegrationPartnerController : FBOLinxControllerBase
    {
        private IIntegrationStatusService _IntegrationStatusService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtManager _jwtManager;
        private IAPIKeyManager _apiKeyManager;
        private readonly FboLinxContext _context;
        private IFboPricesService _fboPricesService;
        private readonly IFuelReqService _fuelReqService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IFboPreferencesService _fboPreferencesService;
        private readonly IFboService _fboService;
        private readonly ICustomerAircraftService _customerAircraftService;
        private readonly ICustomerService _customerService;

        public IntegrationPartnerController(IIntegrationStatusService integrationStatusService, IHttpContextAccessor httpContextAccessor, JwtManager jwtManager, IAPIKeyManager apiKeyManager, FboLinxContext context, IFboPricesService fbopricesService, ILoggingService logger,
                IFuelReqService fuelReqService, IOrderDetailsService orderDetailsService, IFboPreferencesService fboPreferencesService, IFboService fboService, ICustomerAircraftService customerAircraftService, ICustomerService customerService) : base(logger)
        {
            _IntegrationStatusService = integrationStatusService;
            _httpContextAccessor = httpContextAccessor;
            _jwtManager = jwtManager;
            _apiKeyManager = apiKeyManager;
            _context = context;
            _fboPricesService = fbopricesService;
            _fuelReqService = fuelReqService;
            _orderDetailsService = orderDetailsService;
            _fboPreferencesService = fboPreferencesService;
            _fboService = fboService;
            _customerAircraftService = customerAircraftService;
            _customerService = customerService;
        }


        // POST: api/integrationpartner/update-integration-status
        [HttpPost("update-integration-status")]
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdateIntegrationStatus(IntegrationStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _context.User.FindAsync(claimedId);
                var integrationPartner = await _apiKeyManager.GetIntegrationPartner();

                if (user.FboId > 0 && integrationPartner.Oid > 0)
                {
                    //Update status
                    await _IntegrationStatusService.UpdateIntegrationStatus(new Service.Mapping.Dto.IntegrationStatusDTO() { FboId = user.FboId, IntegrationPartnerId = integrationPartner.Oid, IsActive = request.IsActive });

                    //Expire any active pricing if deactivated
                    if (!request.IsActive)
                    {
                        await _fboPricesService.ExpirePricingForFbo(user.FboId);
                    }

                    return Ok(new { message = "Success" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid user" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [APIKey(Core.Enums.IntegrationPartnerTypes.Internal)]
        [HttpPost("handlerId/{handlerId}/create-service-order")]
        public async Task<ActionResult<List<FuelReqDto>>> CreateServiceOrderRequest([FromRoute] int handlerId, [FromBody] ServiceOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var fbo = await _fboService.GetSingleBySpec(new FboByAcukwikHandlerIdSpecification(handlerId));

            var orderDetails = new OrderDetailsDto();
            orderDetails.ConfirmationEmail = request.Email;
            orderDetails.FuelVendor = request.FuelVendor;
            orderDetails.FuelerLinxTransactionId = request.SourceId.GetValueOrDefault();
            orderDetails.PaymentMethod = request.PaymentMethod;
            orderDetails.Eta = request.Eta;
            orderDetails.FboHandlerId = handlerId;
            orderDetails.TimeStandard = request.TimeStandard;
            orderDetails.IsOkToEmail = request.IsOktoSendEmail.GetValueOrDefault();

            var customer = await _customerService.GetCustomerByFuelerLinxId(request.CompanyId.GetValueOrDefault());
            var customerAircrafts = await _customerAircraftService.GetAircraftsList(fbo.GroupId, fbo.Oid);
            var customerAircraft = customerAircrafts.Where(c => c.CustomerId == customer.Oid && c.TailNumber == request.TailNumber).FirstOrDefault();

            if (customerAircraft != null && orderDetails.CustomerAircraftId != customerAircraft.Oid)
                orderDetails.CustomerAircraftId = customerAircraft.Oid;

            if (request.Services.Count > 0)
                await _fuelReqService.AddServiceOrder(request, fbo);

            // Add order details if it doesn't exist yet
            var existingOrderDetails = await _orderDetailsService.GetSingleBySpec(new OrderDetailsByFuelerLinxTransactionIdFboHandlerIdSpecification(request.SourceId.GetValueOrDefault(), handlerId));
            if (existingOrderDetails == null)
                await _orderDetailsService.AddAsync(orderDetails);

            return Ok();
        }

        [AllowAnonymous]
        [APIKey(Core.Enums.IntegrationPartnerTypes.Internal)]
        [HttpPost("handlerId/{handlerId}/fuelerlinxTransactionId/{fuelerlinxTransactionId}/fuelerlinxCompanyId/{fuelerlinxCompanyId}/send-order-notification")]
        public async Task<IActionResult> SendOrderNotification([FromRoute] int handlerId, [FromRoute] int fuelerlinxTransactionId, [FromRoute] int fuelerlinxCompanyId, [FromBody] SendOrderNotificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _fuelReqService.SendFuelOrderNotificationEmail(handlerId, fuelerlinxTransactionId, fuelerlinxCompanyId, request);

            return Ok(new {success = result });
        }


        // POST: api/integrationpartner/update-tail-specific-all-inclusive
        [HttpPost("update-tail-specific-all-inclusive")]
        [APIKey(IntegrationPartnerTypes.OtherSoftware)]
        public async Task<IActionResult> UpdateTailSpecificAllInclusive(UpdateTailSpecificAllInclusiveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid body request!" });
            }

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var claimPrincipal = _jwtManager.GetPrincipal(token);
                var claimedId = Convert.ToInt32(claimPrincipal.Claims.First((c => c.Type == ClaimTypes.NameIdentifier)).Value);

                var user = await _context.User.FindAsync(claimedId);
                var integrationPartner = await _apiKeyManager.GetIntegrationPartner();

                if (user.FboId > 0 && integrationPartner.Oid > 0)
                {
                    
                    return Ok(new { message = "Success" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid user" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
