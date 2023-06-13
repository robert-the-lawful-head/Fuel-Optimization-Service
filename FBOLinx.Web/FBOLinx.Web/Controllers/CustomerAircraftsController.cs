using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Aircraft;
using FBOLinx.ServiceLayer.BusinessServices.AirportWatch;
using FBOLinx.ServiceLayer.BusinessServices.Integrations;
using FBOLinx.ServiceLayer.DTO.Requests.Integrations.FuelerLinx;
using FBOLinx.ServiceLayer.DTO.UseCaseModels.Aircraft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FBOLinx.Web.Auth;
using FBOLinx.Web.Models.Requests;
using FBOLinx.ServiceLayer.Logging;
using Azure.Core;
using System.Security.Cryptography;
using FBOLinx.DB.Specifications.CustomerAircraftNote;
using FBOLinx.DB.Specifications.CustomerAircrafts;
using FBOLinx.ServiceLayer.BusinessServices.Groups;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.DB.Specifications.CustomerInfoByGroupNote;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAircraftsController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly AircraftService _aircraftService;
        private ICustomerAircraftService _CustomerAircraftService;
        private IFuelerLinxAircraftSyncingService _fuelerLinxAircraftSyncingService;
        private AirportWatchService _AirportWatchService;
        private readonly IGroupCustomersService _GroupCustomersService;
        private ICustomerAircraftNoteService _CustomerAircraftNoteService;

        public CustomerAircraftsController(FboLinxContext context, IHttpContextAccessor httpContextAccessor,
            AircraftService aircraftService, ICustomerAircraftService customerAircraftService,
            IFuelerLinxAircraftSyncingService fuelerLinxAircraftSyncingService, AirportWatchService airportWatchService,
            ILoggingService logger, IGroupCustomersService groupCustomersService,
            ICustomerAircraftNoteService customerAircraftNoteService) : base(logger)
        {
            _CustomerAircraftNoteService = customerAircraftNoteService;
            _fuelerLinxAircraftSyncingService = fuelerLinxAircraftSyncingService;
            _CustomerAircraftService = customerAircraftService;
            _HttpContextAccessor = httpContextAccessor;
            _context = context;
            _aircraftService = aircraftService;
            _AirportWatchService = airportWatchService;
            _GroupCustomersService = groupCustomersService;
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
            var customAircraft = await _CustomerAircraftService.GetSingleBySpec(new CustomerAircraftSpecification(id));


            if (customAircraft == null || customAircraft.Oid == 0)
                return NotFound();

            customAircraft.Aircraft = aircrafts.FirstOrDefault(x => x.AircraftId == customAircraft.AircraftId);

            return Ok(customAircraft);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}/customer/{customerId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndCustomerId([FromRoute] int groupId, [FromRoute] int fboId, [FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _CustomerAircraftService.GetCustomerAircraftsWithDetails(groupId, fboId, customerId);
            
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

            var result = await _CustomerAircraftService.GetCustomerAircraftsWithDetails(groupId);

            return Ok(result);
        }

        [HttpGet("group/{groupId}/fbo/{fboId}")]
        public async Task<IActionResult> GetCustomerAircraftsByGroupAndFbo([FromRoute] int groupId, [FromRoute] int fboId)
        {
            var result = await _CustomerAircraftService.GetCustomerAircraftsWithDetails(groupId, fboId);
            
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

            var result = await _CustomerAircraftService.GetCustomerAircraftsWithDetails(groupId);

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
                CustomerAircrafts custAircraft = await _context.CustomerAircrafts.FirstOrDefaultAsync(s => s.Oid == customerAircrafts.Oid);
                 
                if (custAircraft != null)
                {
                  
                    custAircraft.TailNumber = customerAircrafts.TailNumber;
                    custAircraft.AircraftId = customerAircrafts.AircraftId;
                    custAircraft.Size = customerAircrafts.Size;
                    _context.CustomerAircrafts.Update(custAircraft);
                }
                await _context.SaveChangesAsync(userId, customerAircrafts.CustomerId, customerAircrafts.GroupId.GetValueOrDefault());

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

                _CustomerAircraftService.ClearCache(request.GroupId.GetValueOrDefault(), fboid);

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
            await _context.SaveChangesAsync(userId, customerAircrafts.CustomerId, customerAircrafts.GroupId);
            _CustomerAircraftService.ClearCache(customerAircrafts.GroupId);

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

            _CustomerAircraftService.ClearCache(groupId, fboId);

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

                _CustomerAircraftService.ClearCache(request.GroupId, request.FboId);

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

           
            _context.CustomerAircrafts.Remove(customerAircrafts);
            await _context.SaveChangesAsync(userId, customerAircrafts.CustomerId, customerAircrafts.GroupId);

            return Ok(customerAircrafts);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpPost("re-sync-group-customer-aircrafts/{groupId}")]
        public async Task<IActionResult> ReSyncCustomerAircrafts([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _GroupCustomersService.StartAircraftTransfer(groupId, true);

            return Ok();
        }

        [HttpGet("notes/{customerAircraftId}")]
        public async Task<ActionResult<List<CustomerAircraftNoteDto>>> GetCustomerAircraftNotes([FromRoute] int customerAircraftId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customerAircraftNotes = await _context.CustomerAircraftNotes.Where(s => s.CustomerAircraftId == customerAircraftId).ToListAsync();
                return Ok(customerAircraftNotes);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                throw ex;
            }
        }

        [HttpPost("notes")]
        public async Task<ActionResult<CustomerAircraftNoteDto>> AddCustomerAircraftNotes(
            [FromBody] CustomerAircraftNoteDto customerAircraftNotes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingRecords = await _CustomerAircraftNoteService.GetListbySpec(
                    new CustomerAircraftNoteByCustomerAircraftIdSpecification(customerAircraftNotes
                        .CustomerAircraftId));
                var existingRecordForSameFbo =
                    existingRecords.FirstOrDefault(x => x.FboId == customerAircraftNotes.FboId);

                if (existingRecordForSameFbo != null)
                    customerAircraftNotes.Oid = existingRecordForSameFbo.Oid;

                customerAircraftNotes.LastUpdatedUtc = DateTime.UtcNow;

                if (customerAircraftNotes.Oid > 0)
                {
                    await _CustomerAircraftNoteService.UpdateAsync(customerAircraftNotes);
                }
                else
                {
                    await _CustomerAircraftNoteService.AddAsync(customerAircraftNotes);
                }
                
                return Ok(customerAircraftNotes);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                throw ex;
            }
        }

        [HttpPut("notes/{id}")]
        public async Task<IActionResult> UpdateCustomerAircraftNotes([FromRoute] int id,
            [FromBody] CustomerAircraftNoteDto customerAircraftNotes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerAircraftNotes.Oid)
            {
                return BadRequest();
            }

            try
            {
                customerAircraftNotes.LastUpdatedUtc = DateTime.UtcNow;
                await _CustomerAircraftNoteService.UpdateAsync(customerAircraftNotes);
                return Ok(customerAircraftNotes);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(ex);
                throw ex;
            }
        }

        [HttpDelete("notes/{id}")]
        public async Task<IActionResult> DeleteCustomerAircraftNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customerAircraftNotes = await _CustomerAircraftNoteService.FindAsync(id);
                if (customerAircraftNotes == null)
                {
                    return NotFound();
                }

                await _CustomerAircraftNoteService.DeleteAsync(customerAircraftNotes);

                return Ok(customerAircraftNotes);
            }
            catch (System.Exception exception)
            {
                HandleExceptionAsync(exception);
                throw exception;
            }
        }


        #region Integration Partner APIs
        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
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

        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpPost("sync-fuelerlinx-aircraft/for-company/{fuelerLinxCompanyId}/for-tailnumber/{tailNumber}")]
        public async Task<IActionResult> SyncCustomerAndAircraftFromFuelerLinx([FromRoute] int fuelerLinxCompanyId, [FromRoute] string tailNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fuelerLinxAircraftSyncingService.SyncFuelerlinxAircraft(fuelerLinxCompanyId, tailNumber);

            return Ok();
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartnerTypes.Internal)]
        [HttpGet("aircraft-locations/{fuelerlinxCustomerId}")]
        public async Task<IActionResult> GetAircraftLocations([FromRoute] int fuelerlinxCustomerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _AirportWatchService.GetAircraftLocations(fuelerlinxCustomerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
        
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