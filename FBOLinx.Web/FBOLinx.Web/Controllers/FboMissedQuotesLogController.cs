using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FBOLinx.ServiceLayer.BusinessServices.MissedQuoteLog;
using FBOLinx.ServiceLayer.BusinessServices.MissedOrderLog;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FboMissedQuotesLogController : Controller
    {
        private IMissedQuoteLogService _MissedQuoteService;
        private readonly IMissedOrderLogService _MissedOrderLogService;

        public FboMissedQuotesLogController(IMissedOrderLogService missedOrderLogService, IMissedQuoteLogService missedQuoteLogService)
        {
            _MissedQuoteService = missedQuoteLogService;
            _MissedOrderLogService = missedOrderLogService;
        }

        // GET: api/FboMissedQuotesLog/recent-missed-quotes/fbo/5
        [HttpGet("recent-missed-quotes/fbo/{fboId}")]
        public async Task<IActionResult> GetRecentMissedQuotes([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var missedQuotesLogList = await _MissedQuoteService.GetMissedQuotesList(fboId);

                return Ok(missedQuotesLogList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/FboMissedQuotesLog/recent-missed-orders/fbo/5
        [HttpGet("recent-missed-orders/fbo/{fboId}")]
        public async Task<IActionResult> GetRecentMissedOrders([FromRoute] int fboId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var missedOrdersLogList = await _MissedOrderLogService.GetRecentMissedOrders(fboId);

                return Ok(missedOrdersLogList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/FboMissedQuotesLog/get-missed-orders/fbo/5
        [HttpGet("missed-orders/fbo/{fboId}")]
        public async Task<IActionResult> GetMissedOrders([FromRoute] int fboId, [FromBody] FboMissedOrdersLogRequest fboMissedOrdersLogRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var missedOrdersLogList = await _MissedOrderLogService.GetRecentMissedOrders(fboId);

                return Ok(missedOrdersLogList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
