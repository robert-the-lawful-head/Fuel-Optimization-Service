using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.Services;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using static FBOLinx.DB.Models.AirCrafts;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAircraftsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly AircraftService _aircraftService;
        private CustomerAircraftService _CustomerAircraftService;

        public CustomerAircraftsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor, AircraftService aircraftService, CustomerAircraftService customerAircraftService)
        {
            _CustomerAircraftService = customerAircraftService;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _aircraftService = aircraftService;
        }

        [HttpGet]
        public IEnumerable<CustomerAircrafts> GetCustomerAircrafts()
        {
            return _context.CustomerAircrafts;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAircrafts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aircrafts = await _aircraftService.GetAllAircrafts();
            var customAircraft = await _context.CustomerAircrafts.Where(x => x.Oid == id).ToListAsync();

            if (customAircraft == null || customAircraft.Count == 0)
                return NotFound();
            
            var result = (from ca in customAircraft
                                join ac in aircrafts on ca.AircraftId equals ac.AircraftId into leftJoinAircrafts
                                           from ac in leftJoinAircrafts.DefaultIfEmpty()
                                           where ca.Oid == id
                                           select new
                                           {
                                               ca.CustomerId,
                                               ca.AircraftId,
                                               ca.Oid,
                                               Size = (ca.Size.HasValue && ca.Size.Value != AirCrafts.AircraftSizes.NotSet) || ac == null
                                                   ? ca.Size
                                                   : (AirCrafts.AircraftSizes)(ac.Size ?? 0),
                                               ca.AddedFrom,
                                               ca.NetworkCode,
                                               ca.BasedPaglocation,
                                               ca.TailNumber,
                                               ca.GroupId,
                                               ac.Make,
                                               ac.Model
                                           }).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndCustomerId([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _CustomerAircraftService.GetCustomerAircrafts(groupId, fboId);
            
            if (result == null)
                return Ok(result);

            return Ok(result.Where(x => x.CustomerId == customerId).ToList());
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroup([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (groupId != JwtManager.GetClaimedGroupId(_HttpContextAccessor))
                return BadRequest(ModelState);

            var result = await _CustomerAircraftService.GetCustomerAircrafts(groupId);

            return Ok(result);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var result = await _CustomerAircraftService.GetCustomerAircrafts(groupId, fboId);
            
            return Ok(result);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}/list")]
        public async Task<IActionResult> GetCustomerAircraftsListByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var result = await _CustomerAircraftService.GetAircraftsList(groupId, fboId);

            return Ok(result);
        }

        [HttpGet("group/{groupId}/count")]
        public async Task<IActionResult> GetCustomerAircraftsCountByGroupId([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _CustomerAircraftService.GetCustomerAircrafts(groupId);

            return Ok(result?.Count);
        }

        [HttpPut("{id}/{userId}")]
        public async Task<IActionResult> PutCustomerAircrafts([FromRoute] int id, [FromRoute] int userId, [FromBody] CustomerAircraftsViewModel customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerAircrafts.Oid)
            {
                return BadRequest();
            }


            try
            {
                CustomerAircrafts custAircraft = _context.CustomerAircrafts.FirstOrDefault(s => s.Oid == customerAircrafts.Oid);
                 
                if (custAircraft != null)
                {
                    EditAirCraftLog(custAircraft, customerId, userId);
                    custAircraft.TailNumber = customerAircrafts.TailNumber;
                    custAircraft.AircraftId = customerAircrafts.AircraftId;
                    custAircraft.Size = customerAircrafts.Size;
                    _context.CustomerAircrafts.Update(custAircraft);
                }
                await _context.SaveChangesAsync(userId);

                return Ok(custAircraft);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAircraftsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPut("fbo/{fboid}")]
        public async Task<IActionResult> PutCustomerAircraftsTemplate([FromRoute] int fboid, [FromBody] CustomerAircraftsViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                CustomerAircrafts custAircraft = _context.CustomerAircrafts.FirstOrDefault(s => s.Oid == request.Oid);

                AircraftPrices aircraftPrice = (from ca in _context.CustomerAircrafts
                                                join ap in _context.AircraftPrices on ca.Oid equals ap.CustomerAircraftId
                                                join pt in _context.PricingTemplate on ap.PriceTemplateId equals pt.Oid
                                                where ca.Oid == request.Oid
                                                    && pt.Fboid == fboid
                                                select ap).FirstOrDefault();

                PricingTemplate pricingTemplate = _context.PricingTemplate.FirstOrDefault(s => s.Oid == request.PricingTemplateId && s.Fboid == fboid);

                if (aircraftPrice != null)
                {
                    if (request.PricingTemplateId == null)
                    {
                        _context.AircraftPrices.Remove(aircraftPrice);
                    }
                    else
                    {
                        aircraftPrice.PriceTemplateId = pricingTemplate?.Oid;
                        _context.AircraftPrices.Update(aircraftPrice);
                    }
                }
                else
                {
                    AircraftPrices newAircraftPrice = new AircraftPrices
                    {
                        CustomerAircraftId = custAircraft.Oid,
                        PriceTemplateId = pricingTemplate.Oid
                    };
                    _context.AircraftPrices.Add(newAircraftPrice);
                }

                if (custAircraft != null)
                {
                    custAircraft.TailNumber = request.TailNumber;
                    custAircraft.AircraftId = request.AircraftId;
                    custAircraft.Size = request.Size;
                    _context.CustomerAircrafts.Update(custAircraft);
                }

                await _context.SaveChangesAsync();

                return Ok(custAircraft);
            }
            catch (Exception)
            {
                if (!CustomerAircraftsExists(request.Oid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> PostCustomerAircraft([FromRoute] int userId , [FromBody] CustomerAircrafts customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
            _context.CustomerAircrafts.Add(customerAircrafts);
            await _context.SaveChangesAsync(userId);

            return CreatedAtAction("GetCustomerAircrafts", new { id = customerAircrafts.Oid }, customerAircrafts);
        }

        [HttpPost("group/{groupId}/fbo/{fboId}/customer/{customerId}/multiple")]
        public async Task<IActionResult> PostCustomerAircraftsWithTemplates([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int customerId, [FromBody] List<CustomerAircraftsWithTemplateRequest> request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircrafts = request.Select(r => new CustomerAircrafts
            {
                GroupId = groupId,
                CustomerId = customerId,
                AircraftId = r.AircraftId,
                Size = r.Size,
                TailNumber = r.TailNumber,
            }).ToList();

            _context.CustomerAircrafts.AddRange(customerAircrafts);
            await _context.SaveChangesAsync();

            for(int i = 0; i< customerAircrafts.Count; i++)
            {
                if (request[i].PricingTemplateId > 0)
                {
                    AircraftPrices newAircraftPrice = new AircraftPrices
                    {
                        CustomerAircraftId = customerAircrafts[i].Oid,
                        PriceTemplateId = request[i].PricingTemplateId
                    };
                    _context.AircraftPrices.Add(newAircraftPrice);
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("create-with-customer")]
        public async Task<IActionResult> PostCustomerAircraftsWithCustomer([FromBody] CreateAircraftsWithCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var customer = new Customers
                {
                    Company = request.Customer
                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                var customerViewedByFbo = new CustomersViewedByFbo
                {
                    Fboid = request.FboId,
                    CustomerId = customer.Oid,
                    ViewDate = DateTime.Now,
                };
                _context.CustomersViewedByFbo.Add(customerViewedByFbo);

                var customerInfoByGroup = new CustomerInfoByGroup
                {
                    GroupId = request.GroupId,
                    Company = request.Customer,
                    CustomerId = customer.Oid,
                    Active = true,
                };
                _context.CustomerInfoByGroup.Add(customerInfoByGroup);


                var customerAircraft = new CustomerAircrafts
                {
                    GroupId = request.GroupId,
                    AircraftId = request.AircraftId,
                    TailNumber = request.TailNumber,
                    Size = request.Size,
                    CustomerId = customer.Oid,
                };
               
                _context.CustomerAircrafts.Add(customerAircraft);

                _context.SaveChanges();

                await transaction.CommitAsync();

                return Ok(customerInfoByGroup);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(ex);
            }
        }


        [HttpPost("import")]
        public async Task<IActionResult> ImportCustomerAircrafts([FromBody] List<AircraftImportVM> customerAircrafts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(var singleAircraft in customerAircrafts)
            {
                var aircraft = await _aircraftService.GetAllAircraftsAsQueryable().FirstOrDefaultAsync(s => s.Make == singleAircraft.AircraftMake && s.Model == singleAircraft.Model);

                if (aircraft != null)
                {
                    CustomerAircrafts newCa = new CustomerAircrafts();
                    newCa.GroupId = singleAircraft.GroupId;
                    newCa.CustomerId = singleAircraft.CustomerId;
                    newCa.AircraftId = aircraft.AircraftId;
                    newCa.TailNumber = singleAircraft.TailNumber;
                    if (singleAircraft.Size != null)
                    {
                        AircraftSizes acSize = (AircraftSizes)singleAircraft.Size;
                        newCa.Size = acSize;
                    }
                    singleAircraft.IsImported = true;
                    _context.CustomerAircrafts.Add(newCa);
                    _context.SaveChanges();

                    
                }
                else
                {
                    singleAircraft.IsImported = false;
                    var otherOptions = await _aircraftService.GetAllAircraftsAsQueryable().Where(s => s.Make == singleAircraft.AircraftMake).Select(s => s.Model).ToListAsync();

                    if(otherOptions != null)
                    {
                        List<string> listOfModels = new List<string>();
                        foreach(var model in otherOptions)
                        {
                            listOfModels.Add(model);    
                        }
                        listOfModels = listOfModels.OrderByDescending(s => s).ToList();
                        singleAircraft.OtherOptions = listOfModels;
                        singleAircraft.selectedModel = "";
                    }

                    
                }
            }

            var checkForErrors = customerAircrafts.FirstOrDefault(s => s.IsImported == false);

            if (checkForErrors !=null)
            {
                return Ok(customerAircrafts.Where(s => s.IsImported == false).ToList());
            }

            return Ok(customerAircrafts);
        }

        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> DeleteCustomerAircraft([FromRoute] int id , [FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAircrafts = await _context.CustomerAircrafts.FindAsync(id);
            if (customerAircrafts == null)
            {
                return NotFound();
            }

            DeleteCustomerAirCraftLog(customerAircrafts, customerId, userId);
            _context.CustomerAircrafts.Remove(customerAircrafts);
            await _context.SaveChangesAsync(userId);

            return Ok(customerAircrafts);
        }

        #region Integration Partner APIs
        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpPost("fuelerlinx-aircraft")]
        public async Task<IActionResult> FuelerLinxAircraft([FromBody] FuelerLinxAircraftRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                if (request.TailNumbers != null && request.TailNumbers.Split(',').Length == 1)
                    return await ChangeFuelerLinxCustomerAircraftsAddedFrom(request, false);
                else
                {
                    if (request.FuelerlinxCompanyID > 0)
                        return await ChangeFuelerLinxCustomerAircraftsAddedFrom(request, true);
                    else
                        return await ChangeFuelerLinxCustomerAircraftsAddedFrom(request, false);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        private  void AddCustomerAirCraftLog (CustomerAircrafts customer, int customerId, int userId = 0  )
        {
            var newCustomer = _context.CustomerAircrafts
                                      .Where(c=> c.CustomerId.Equals(customer.CustomerId)
                                             && c.GroupId.Equals(customer.GroupId)
                                             && c.TailNumber.Equals(customer.TailNumber)
                                             && c.AircraftId.Equals(customer.AircraftId))
                                      .FirstOrDefault();

            if(newCustomer != null)
            {
                try
                {
                    _context.CustomerAircraftLog.Add(new CustomerAircraftLog
                    {
                        Action = CustomerInfoByGroupLog.Actions.AircraftAdded ,
                        Location = CustomerInfoByGroupLog.Locations.CustomerAircarft ,
                        newcustomeraircraftId = newCustomer.Oid , 
                        Role = (CustomerInfoByGroupLog.UserRoles)_context.User.FirstOrDefault(u => u.Oid == userId).Role, 
                        userId = userId , 
                        Time = DateTime.Now , 
                        customerId = customerId
                    });

                    _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        private void EditAirCraftLog(CustomerAircrafts customer, int customerId , int userId= 0 ) 
        {
            _context.CustomerAircraftLogData.Add(new CustomerAircraftLogData
            {
                AddedFrom = customer.AddedFrom,
                AircraftId = customer.AircraftId,
                BasedPaglocation = customer.BasedPaglocation,
                CustomerId = customer.CustomerId,
                GroupId = customer.GroupId,
                NetworkCode = customer.NetworkCode,
                Size = customer.Size,
                TailNumber = customer.TailNumber
            });

            try
            {
                _context.SaveChanges();
                int oldaircraftId = _context.CustomerAircraftLogData.Where(a => a.GroupId == customer.GroupId && a.CustomerId == customer.CustomerId)
                                    .OrderByDescending(c => c.Oid).FirstOrDefault().Oid;

                _context.CustomerAircraftLog.Add(new CustomerAircraftLog
                {
                    Action = CustomerInfoByGroupLog.Actions.Edited,
                    Location = CustomerInfoByGroupLog.Locations.CustomerAircarft,
                    newcustomeraircraftId = customer.Oid,
                    oldcustomeraircraftId = oldaircraftId,
                    Role = (CustomerInfoByGroupLog.UserRoles) _context.User.FirstOrDefault(u=> u.Oid == userId).Role,
                    Time = DateTime.Now,
                    userId = userId , 
                    customerId = customerId
                });

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void DeleteCustomerAirCraftLog (CustomerAircrafts customer , int customerId, int userId =0 )
        {
            _context.CustomerAircraftLogData.Add(new CustomerAircraftLogData
            {
                AddedFrom = customer.AddedFrom,
                AircraftId = customer.AircraftId,
                BasedPaglocation = customer.BasedPaglocation,
                CustomerId = customer.CustomerId,
                GroupId = customer.GroupId,
                NetworkCode = customer.NetworkCode,
                Size = customer.Size,
                TailNumber = customer.TailNumber
            });

            try
            {
                _context.SaveChanges();
                int oldaircraftId = _context.CustomerAircraftLogData.Where(a => a.GroupId == customer.GroupId && a.CustomerId == customer.CustomerId)
                                    .OrderByDescending(c => c.Oid).FirstOrDefault().Oid;

                _context.CustomerAircraftLog.Add(new CustomerAircraftLog
                {
                    Action = CustomerInfoByGroupLog.Actions.AircraftDeleted,
                    Location = CustomerInfoByGroupLog.Locations.CustomerAircarft,
                    newcustomeraircraftId = customer.Oid,
                    oldcustomeraircraftId = oldaircraftId,
                    Role = (CustomerInfoByGroupLog.UserRoles)_context.User.FirstOrDefault(u => u.Oid == userId).Role,
                    Time = DateTime.Now,
                    userId = userId , 
                    customerId = customerId
                });

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        private bool CustomerAircraftsExists(int id)
        {
            return _context.CustomerAircrafts.Any(e => e.Oid == id);
        }

        private async Task<IActionResult> ChangeFuelerLinxCustomerAircraftsAddedFrom(FuelerLinxAircraftRequest request, bool isActivate)
        {
            var customerAircrafts = new List<FBOLinx.DB.Models.CustomerAircrafts>();
            var tailNumbers = request.TailNumbers.Split(',');
            if (tailNumbers.Length == 1)
                customerAircrafts = await (
                                    from ca in _context.CustomerAircrafts
                                    join c in _context.Customers on ca.CustomerId equals c.Oid
                                    where c.FuelerlinxId == request.FuelerlinxCompanyID && ca.TailNumber == tailNumbers[0]
                                    select ca
                                    ).ToListAsync();
            else
                customerAircrafts = await (
                                    from ca in _context.CustomerAircrafts
                                    join c in _context.Customers on ca.CustomerId equals c.Oid
                                    where c.FuelerlinxId == request.FuelerlinxCompanyID 
                                    select ca
                                    ).ToListAsync();

            customerAircrafts = customerAircrafts.Where(a => tailNumbers.Contains(a.TailNumber)).ToList();

            if (customerAircrafts.Count == 0)
            {
                return Ok(new { Message = "The tail numbers provided did not match any FBOlinx customer" });
            }

            customerAircrafts.ForEach(ca => ca.AddedFrom = isActivate ? 1 : 0);

            _context.CustomerAircrafts.UpdateRange(customerAircrafts);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully " + (isActivate ? "activated" : "deactivated") + " the FuelerLinx aircrafts!" });
        }
    }
}