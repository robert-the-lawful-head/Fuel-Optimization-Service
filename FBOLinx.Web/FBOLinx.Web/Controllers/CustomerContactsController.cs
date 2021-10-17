using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerContactsController : ControllerBase
    {
        private readonly FboLinxContext _context;

        public CustomerContactsController(FboLinxContext context)
        {
            _context = context;
        }

        // GET: api/CustomerContacts
        [HttpGet]
        public IEnumerable<CustomerContacts> GetCustomerContacts()
        {
            return _context.CustomerContacts;
        }

        // GET: api/CustomerContacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerContacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerContacts = await _context.CustomerContacts.FindAsync(id);

            if (customerContacts == null)
            {
                return NotFound();
            }

            return Ok(customerContacts);
        }

        // PUT: api/CustomerContacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerContacts([FromRoute] int id, [FromBody] CustomerContacts customerContacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerContacts.Oid)
            {
                return BadRequest();
            }

            _context.Entry(customerContacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerContactsExists(id))
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

        // POST: api/CustomerContacts
        [HttpPost]
        public async Task<IActionResult> PostCustomerContacts([FromBody] CustomerContacts customerContacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerContacts.Add(customerContacts);
            await _context.SaveChangesAsync();
            AddCustomerContactLog(customerContacts);

            return CreatedAtAction("GetCustomerContacts", new { id = customerContacts.Oid }, customerContacts);
        }

        // DELETE: api/CustomerContacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerContacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerContacts = await _context.CustomerContacts.FindAsync(id);
            if (customerContacts == null)
            {
                return NotFound();
            }

            DeleteCustomerContactLog(customerContacts);
            _context.CustomerContacts.Remove(customerContacts);
            await _context.SaveChangesAsync();

            return Ok(customerContacts);
        }

        // GET: api/CustomerContacts/group/5/fbo/6/pricingtemplate/7
        [HttpGet("group/{groupId}/fbo/{fboId}/pricingtemplate/{pricingTemplateId}")]
        public async Task<IActionResult> GetDistributionEmailsByTemplate([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int pricingTemplateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emails = await (from cg in _context.CustomerInfoByGroup.Where((x => x.GroupId == groupId))
                                join c in _context.Customers on cg.CustomerId equals c.Oid
                                join cc in _context.CustomCustomerTypes.Where(x => x.Fboid == fboId) on cg.CustomerId equals cc.CustomerId
                                join custc in _context.CustomerContacts on c.Oid equals custc.CustomerId
                                join co in _context.Contacts on custc.ContactId equals co.Oid
                                join cibg in _context.ContactInfoByGroup on co.Oid equals cibg.ContactId
                                where (cg.Active ?? false)
                                      && (cc.CustomerType == pricingTemplateId || pricingTemplateId == 0)
                                      && (cibg.CopyAlerts ?? false) == true
                                      && !string.IsNullOrEmpty(cibg.Email)
                                      && cibg.GroupId == groupId
                                      && (c.Suspended ?? false) == false
                                select cibg.Email
                                ).ToListAsync();

            return Ok(emails);
        }

        private  void AddCustomerContactLog (CustomerContacts contact , int userId = 0 , int Role = 0 )
        {
            var newCustomerContact = _context.CustomerContacts.FirstOrDefault(c => c.CustomerId.Equals(contact.CustomerId) && c.ContactId.Equals(contact.ContactId));

            if(newCustomerContact != null)
            {
                _context.CustomerContactLog.Add(new CustomerContactLog
                {
                    Action = CustomerContactLog.Actions.ContactAdded ,
                    Location = CustomerContactLog.Locations.EditCustomer,
                    Role = Role , 
                    userId = userId, 
                    Time = DateTime.Now , 
                    newcustomercontactId = newCustomerContact.Oid
                });

                _context.SaveChanges();
            }

        }
       
        private void DeleteCustomerContactLog (CustomerContacts contact, int userId = 0, int Role = 0)
        {
            _context.CustomerContactLogData.Add(new CustomerContactLogData
            {
                ContactId = contact.ContactId , 
                CustomerId = contact.CustomerId
            });

            try
            {
                _context.SaveChanges();

                var deletedContactID = _context.CustomerContactLogData.FirstOrDefault(c => c.CustomerId.Equals(contact.CustomerId) && c.ContactId.Equals(contact.ContactId)).Oid;

                if(deletedContactID.ToString() != "")
                {
                    _context.CustomerContactLog.Add(new CustomerContactLog
                    {
                        Action = CustomerContactLog.Actions.ContactDeleted , 
                        Location = CustomerContactLog.Locations.EditCustomer , 
                        Role = Role ,
                        userId =userId , 
                        Time = DateTime.Now , 
                        oldcustomercontactId = deletedContactID
                    });

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private bool CustomerContactsExists(int id)
        {
            return _context.CustomerContacts.Any(e => e.Oid == id);
        }
    }
}