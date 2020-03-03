using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
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

        public CustomersController(FboLinxContext context)
        {
            _context = context;
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

        //// GET: api/Customers/5
        //[HttpGet("group/{groupId}/fbo/{fboId}")]
        //public async Task<IActionResult> GetCustomersByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var customers = await GetAllCustomers().Include("CustomerInfoByGroup").Where(c => c.CustomerInfoByGroup?.GroupId == groupId).ToListAsync();

        //    //View model for customers grid
        //    var customerVM = customers.Select(c => new CustomersGridViewModel
        //    {
        //        CustomerId = c.Oid,
        //        CustomerInfoByGroupId = c.CustomerInfoByGroup?.Oid,
        //        CompanyByGroupId = c.CompanyByGroup?.Oid,
        //        Company = c.CustomerInfoByGroup?.Company,
        //        PricingTemplateId = 0,
        //        DefaultCustomerType = c.CustomerInfoByGroup?.CustomerType,
        //        Joined = c.CustomerInfoByGroup?.Joined,
        //        Price = 0,
        //        Suspended = c.CustomerInfoByGroup?.Suspended,
        //        FuelerLinxId = c.FuelerlinxId,
        //        Network = c.CustomerInfoByGroup?.Network,
        //        GroupId = c.CustomerInfoByGroup?.GroupId
        //    });

        //    return Ok(customerVM);
        //}

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomers([FromRoute] int id, [FromBody] Customers customers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customers.Oid)
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

            return CreatedAtAction("GetCustomers", new { id = customers.Oid }, customers);
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

            foreach(var customer in customers)
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

                if(newC.Oid != 0)
                {
                    customer.CompanyId = newC.Oid;
                    CustomerInfoByGroup cibg = new CustomerInfoByGroup();
                    cibg.CustomerId = newC.Oid;
                    cibg.GroupId = customer.groupid;
                    cibg.Company = newC.Company;
                    if(newC.Active == true)
                    {
                        cibg.Active = true;
                    }
                    _context.CustomerInfoByGroup.Add(cibg);
                    await _context.SaveChangesAsync();
                }
            }

            var custWithAircrafts = customers.Where(s => s.AircraftMake != null).ToList();

            if(custWithAircrafts.Count > 0)
            {
                foreach(var custPlane in custWithAircrafts)
                {
                    AirCrafts ac = new AirCrafts();
                    ac.Make = custPlane.AircraftMake;
                    ac.Model = custPlane.AircraftModel;

                    _context.Aircrafts.Add(ac);
                    await _context.SaveChangesAsync();

                    if(ac.AircraftId != 0)
                    {
                        CustomerAircrafts ca = new CustomerAircrafts();
                        ca.AircraftId = ac.AircraftId;
                        ca.TailNumber = "1ABB";
                        ca.GroupId = custPlane.groupid;
                        ca.CustomerId = custPlane.CompanyId;

                        _context.CustomerAircrafts.Add(ca);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var custWithContacts = customers.Where(s => s.FirstName != null).ToList();

            if(custWithContacts.Count > 0)
            {
                foreach(var custContact in custWithContacts)
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

                    if(ct.Oid != 0)
                    {
                        CustomerContacts cc = new CustomerContacts();
                        cc.ContactId = ct.Oid;
                        cc.CustomerId = custContact.CompanyId;

                        _context.CustomerContacts.Add(cc);
                        await _context.SaveChangesAsync();
                    }

                }
            }

            return Ok(customers);
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Oid == id);
        }

        private IQueryable<Customers> GetAllCustomers()
        {
            return _context.Customers.AsQueryable();
        }
    }
}