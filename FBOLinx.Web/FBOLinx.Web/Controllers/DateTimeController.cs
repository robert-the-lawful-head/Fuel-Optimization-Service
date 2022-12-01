using FBOLinx.DB.Context;
using FBOLinx.ServiceLayer.BusinessServices.DateAndTime;
using FBOLinx.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DateTimeController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly DateTimeService _dateTimeService;

        public DateTimeController(FboLinxContext context, DateTimeService dateTimeService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        // GET: api/datetime/next-tuesday/01-01-2022
        [HttpGet("next-tuesday/{date}")]
        public IActionResult GetNextTuesdayDate([FromRoute] string date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nextTuesdayDate = _dateTimeService.GetNextTuesdayDate(DateTime.Parse(date));

            return Ok(nextTuesdayDate.ToString());
        }
    }
}
