using FBOLinx.ServiceLayer.BusinessServices.ServicesAndFees;
using FBOLinx.ServiceLayer.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FboCustomServiceTypesController : FBOLinxControllerBase
    {
        private readonly IFboServicesAndFeesService _fboServicesAndFeesService;

        public FboCustomServiceTypesController(IFboServicesAndFeesService fboServicesAndFeesService, ILoggingService logger) : base(logger)
        {
            _fboServicesAndFeesService = fboServicesAndFeesService;
        }

        // GET: api/<FboCustomServiceTypesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FboCustomServiceTypesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FboCustomServiceTypesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FboCustomServiceTypesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FboCustomServiceTypesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
