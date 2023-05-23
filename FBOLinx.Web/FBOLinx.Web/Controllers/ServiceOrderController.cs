using System;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Specifications.ServiceOrder;
using FBOLinx.DB.Specifications.ServiceOrderItem;
using FBOLinx.ServiceLayer.BusinessServices.ServiceOrders;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.DTO.Responses.ServiceOrder;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceOrderController : FBOLinxControllerBase
    {
        private IServiceOrderService _ServiceOrderService;
        private IServiceOrderItemService _ServiceOrderItemService;

        public ServiceOrderController(ILoggingService logger, IServiceOrderService serviceOrderService, IServiceOrderItemService serviceOrderItemService) : base(logger)
        {
            _ServiceOrderItemService = serviceOrderItemService;
            _ServiceOrderService = serviceOrderService;
        }

        /// <summary>
        /// Fetches a list of service orders for a given fbo id
        /// </summary>
        /// <param name="fboId"></param>
        /// <param name="startDateTimeUtc"></param>
        /// <param name="endDateTimeUtc"></param>
        /// <returns></returns>
        [HttpGet("list/fbo/{fboId}")]
        public async Task<ActionResult<ServiceOrderListResponse>> GetServiceOrdersForFbo([FromRoute] int fboId, DateTime? startDateTimeUtc, DateTime? endDateTimeUtc)
        {
            try
            {
                var result = await _ServiceOrderService.GetListbySpec(new ServiceOrderByFboSpecification(fboId, startDateTimeUtc, endDateTimeUtc));
                return Ok(new ServiceOrderListResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderListResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Fetch a service order by a given service order id
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        [HttpGet("id/{serviceOrderId}")]
        public async Task<ActionResult<ServiceOrderResponse>> GetServiceOrder([FromRoute] int serviceOrderId)
        {
            try
            {
                var result = await _ServiceOrderService.GetSingleBySpec(new ServiceOrderByIdSpecification(serviceOrderId));
                return Ok(new ServiceOrderResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Post a new service order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ServiceOrderResponse>> PostServiceOrder([FromBody] ServiceOrderDto request)
        {
            try
            {
                var result = await _ServiceOrderService.AddAsync(request);
                return Ok(new ServiceOrderResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Update a passed in service order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ServiceOrderResponse>> PutServiceOrder([FromBody] ServiceOrderDto request)
        {
            try
            {
                await _ServiceOrderService.UpdateAsync(request);
                return Ok(new ServiceOrderResponse(request));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Delete a service order by a given service order id
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        [HttpDelete("{serviceOrderId}")]
        public async Task<ActionResult<ServiceOrderResponse>> DeleteServiceOrder([FromRoute] int serviceOrderId)
        {
            try
            {
                var serviceOrder = await _ServiceOrderService.GetSingleBySpec(new ServiceOrderByIdSpecification(serviceOrderId));
                if (serviceOrder == null)
                {
                    return Ok(new ServiceOrderResponse(false, "Service order not found"));
                }
                await _ServiceOrderService.DeleteAsync(serviceOrder);
                return Ok(new ServiceOrderResponse(true, null));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Fetch a list of service order items for a given service order id
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <returns></returns>
        [HttpGet("items/{serviceOrderId}")]
        public async Task<ActionResult<ServiceOrderItemListResponse>> GetServiceOrderItems(
            [FromRoute] int serviceOrderId)
        {
            try
            {
                var result =
                    await _ServiceOrderItemService.GetListbySpec(new ServiceOrderItemByOrderSpecification(serviceOrderId));
                return Ok(new ServiceOrderItemListResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderItemListResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Fetch a service order item by a given service order item id
        /// </summary>
        /// <param name="serviceOrderItemId"></param>
        /// <returns></returns>
        [HttpGet("item/id/{serviceOrderItemId}")]
        public async Task<ActionResult<ServiceOrderItemResponse>> GetServiceOrderItem(
            [FromRoute] int serviceOrderItemId)
        {
            try
            {
                var result =
                    await _ServiceOrderItemService.GetSingleBySpec(
                        new ServiceOrderItemByIdSpecification(serviceOrderItemId));
                return Ok(new ServiceOrderItemResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderItemResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Post a new service order item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("item")]
        public async Task<ActionResult<ServiceOrderItemResponse>> PostServiceOrderItem(
            [FromBody] ServiceOrderItemDto request)
        {
            try
            {
                var result = await _ServiceOrderItemService.AddAsync(request);
                return Ok(new ServiceOrderItemResponse(result));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderItemResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Update a passed in service order item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("item")]
        public async Task<ActionResult<ServiceOrderItemResponse>> PutServiceOrderItem(
            [FromBody] ServiceOrderItemDto request)
        {
            try
            {
                await _ServiceOrderItemService.UpdateAsync(request);
                return Ok(new ServiceOrderItemResponse(request));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderItemResponse(false, exception.Message));
            }
        }

        /// <summary>
        /// Delete a service order item by a given service order item id
        /// </summary>
        /// <param name="serviceOrderItemId"></param>
        /// <returns></returns>
        [HttpDelete("item/{serviceOrderItemId}")]
        public async Task<ActionResult<ServiceOrderItemResponse>> DeleteServiceOrderItem(
            [FromRoute] int serviceOrderItemId)
        {
            try
            {
                var serviceOrderItem =
                    await _ServiceOrderItemService.GetSingleBySpec(
                        new ServiceOrderItemByIdSpecification(serviceOrderItemId));
                if (serviceOrderItem == null)
                {
                    return Ok(new ServiceOrderItemResponse(false, "Service order item not found"));
                }

                await _ServiceOrderItemService.DeleteAsync(serviceOrderItem);
                return Ok(new ServiceOrderItemResponse(true, null));
            }
            catch (System.Exception exception)
            {
                return Ok(new ServiceOrderItemResponse(false, exception.Message));
            }
        }

        
    }
}
