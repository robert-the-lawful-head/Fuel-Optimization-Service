using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly AircraftService _aircraftService;
        private IFuelerLinxAccoutSyncingService _fuelerLinxAccoutSyncingService;

        public CustomersController(FboLinxContext context, AircraftService aircraftService, IFuelerLinxAccoutSyncingService fuelerLinxAccoutSyncingService)
        {
            _fuelerLinxAccoutSyncingService = fuelerLinxAccoutSyncingService;
            _context = context;
            _aircraftService = aircraftService;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customers = await _context.Customers.FindAsync(id);

            if (customers == null)
            {
                return NotFound();
            }

            return Ok(customers);
        }

        // GET: api/Customers/getbyfuelerlinxid/5
        [HttpGet("getbyfuelerlinxid/{fuelerlinxid}")]
        public async Task<IActionResult> GetCustomerByFuelerlinxId([FromRoute] int fuelerlinxid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.Where(x => x.FuelerlinxId == fuelerlinxid).FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpPost("sync-fuelerlinx-company/{fuelerLinxCompanyId}")]
        public async Task<IActionResult> SyncCustomerFromFuelerLinx([FromRoute] int fuelerLinxCompanyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fuelerLinxAccoutSyncingService.SyncFuelerLinxAccount(fuelerLinxCompanyId);

            return Ok();
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers([FromRoute] int id, [FromBody] Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customers.Id)
            {
                return BadRequest();
            }

            _context.Entry(customers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomers([FromBody] Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomers", new { id = customers.Id }, customers);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();

            return Ok(customers);
        }

        [HttpPost("importcustomers")]
        public async Task<IActionResult> ImportCustomers([FromBody] List<CustomerImportVM> customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distinctCust = customers.GroupBy(s => s.CompanyName).Select(s => s.First()).ToList();
            int custId = 0;
            foreach(var customer in distinctCust)
            {
                Customers newC = new Customers();
                newC.Company = customer.CompanyName;

                if(customer.Activate != null)
                {
                    if(customer.Activate.ToLower().Equals("active") || customer.Activate.ToLower().Equals("on"))
                    {
                        newC.Active = true;
                    }
                }

                _context.Customers.Add(newC);
                await _context.SaveChangesAsync();

                if(newC.Id != 0)
                {
                    custId = newC.Id;
                    customer.CompanyId = newC.Id;
                    CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                    cibg.CustomerId = newC.Id;
                    cibg.GroupId = customer.groupid;
                    cibg.Company = newC.Company;
                    if(newC.Active == true)
                    {
                        cibg.Active = true;
                    }
                    _context.CustomerInfoByGroup.Add(cibg);
                    await _context.SaveChangesAsync();
                }

                var custWithAircrafts = customers.Where(s => s.AircraftMake != null && s.CompanyName == customer.CompanyName).ToList();

                if (custWithAircrafts.Count > 0)
                {
                    var aircraftSizes = FBOLinx.Core.Utilities.Enum.GetDescriptions(typeof(AircraftSizes));

                    foreach (var custPlane in custWithAircrafts)
                    {
                        AirCrafts ac = new AirCrafts();
                        ac.Make = custPlane.AircraftMake;
                        ac.Model = custPlane.AircraftModel;
                        var aircraftSize = aircraftSizes.FirstOrDefault(s => s.Description == custPlane.AircraftSize);

                        if (aircraftSize != null)
                        {
                            AircraftSizes acSize = (AircraftSizes)aircraftSize.Value;
                            ac.Size = acSize;
                        }

                        await _aircraftService.AddAirCrafts(ac);

                        if (ac.AircraftId != 0)
                        {
                            CustomerAircrafts ca = new CustomerAircrafts();
                            ca.AircraftId = ac.AircraftId;
                            ca.TailNumber = custPlane.Tail;
                            ca.GroupId = custPlane.groupid;
                            ca.CustomerId = custId;

                            _context.CustomerAircrafts.Add(ca);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                var custWithContacts = customers.Where(s => s.FirstName != null && s.CompanyName == customer.CompanyName).ToList();

                if (custWithContacts.Count > 0)
                {
                    foreach (var custContact in custWithContacts)
                    {
                        Contacts ct = new Contacts();
                        ct.FirstName = custContact.FirstName;
                        ct.LastName = custContact.FirstName;
                        ct.Mobile = custContact.Mobile;
                        ct.Email = custContact.Email;
                        ct.Phone = custContact.Phone;
                        ct.Title = custContact.Title;

                        _context.Contacts.Add(ct);
                        await _context.SaveChangesAsync();

                        if (ct.Oid != 0)
                        {
                            CustomerContacts cc = new CustomerContacts();
                            cc.ContactId = ct.Oid;
                            cc.CustomerId = custContact.CompanyId;

                            _context.CustomerContacts.Add(cc);
                            await _context.SaveChangesAsync();

                            ContactInfoByGroup cibg = new ContactInfoByGroup();
                            cibg.GroupId = custContact.groupid;
                            cibg.ContactId = ct.Oid;
                            cibg.FirstName = custContact.FirstName;
                            cibg.LastName = custContact.LastName;
                            cibg.Mobile = custContact.Mobile;
                            cibg.Email = custContact.Email;
                            cibg.Phone = custContact.Phone;
                            cibg.Title = custContact.Title;

                            _context.ContactInfoByGroup.Add(cibg);
                            await _context.SaveChangesAsync();
                        }

                    }
                }
            }

            return Ok(customers);
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}