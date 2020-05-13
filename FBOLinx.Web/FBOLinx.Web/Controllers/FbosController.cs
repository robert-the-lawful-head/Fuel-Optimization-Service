using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBOLinx.Web.Data;
using FBOLinx.Web.Models;
using FBOLinx.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using FBOLinx.Web.Models.Requests;
using Microsoft.Extensions.DependencyInjection;
using FBOLinx.Web.Services;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FbosController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private IServiceScopeFactory _serviceScopeFactory;

        public FbosController(FboLinxContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: api/Fbos/group/5
        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetFboByGroupID([FromRoute] int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fbos = await GetAllFbos().Include("fboAirport").Where((x => x.GroupId == groupId)).ToListAsync();

            //FbosViewModel used to display FBO info in the grid
            var fbosVM = fbos.Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport == null ? "" : f.fboAirport.Icao,
                Iata = f.fboAirport == null ? "" : f.fboAirport.Iata,
                Oid = f.Oid
            }).Distinct().ToList();
            return Ok(fbosVM);
        }

        // GET: api/Fbos
        [HttpGet]
        public async Task<IActionResult> GetFbos()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fbos = await GetAllFbos().Include("fboAirport").ToListAsync();
            if (fbos == null)
            {
                return NotFound();
            }

            //FbosViewModel used to display FBO info in the grid
            var fbosVM = fbos.Select(f => new FbosGridViewModel
            {
                Active = f.Active,
                Fbo = f.Fbo,
                Icao = f.fboAirport?.Icao,
                Oid = f.Oid
            }).ToList();
            return Ok(fbosVM);
        }

        // GET: api/Fbos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFbo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await _context.Fbos.FindAsync(id);

            if (fbos == null)
            {
                return NotFound();
            }

            return Ok(fbos);
        }

        // PUT: api/Fbos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbos([FromRoute] int id, [FromBody] Fbos fbos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fbos.Oid)
            {
                return BadRequest();
            }

            bool activeStatus = await _context.Fbos.Where(f => f.Oid.Equals(fbos.Oid))
                                                   .Select(f => f.Active ?? false)
                                                   .FirstAsync();
            if (activeStatus != fbos.Active)
            {
                List<User> users = _context.User.Where(u => u.FboId.Equals(id))
                                            .ToList();
                foreach (var user in users)
                {
                    user.Active = fbos.Active ?? false;
                }

                _context.User.UpdateRange(users);

                if (!fbos.Active.GetValueOrDefault())
                {
                    // Expire All Prices from the de-activated FBOLinx accounts
                    var fboPrices = _context.Fboprices.Where(fp => fp.Fboid.Equals(id));

                    foreach (var fboPrice in fboPrices)
                    {
                        fboPrice.Expired = true;
                    }

                    _context.Fboprices.UpdateRange(fboPrices);
                }
            }

            _context.Fbos.Update(fbos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbosExists(id))
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

        // POST: api/Fbos
        [HttpPost]
        public async Task<IActionResult> PostFbos([FromBody] SingleFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Fbos fbo = new Fbos
            {
                Fbo = request.Fbo,
                GroupId = request.GroupId,
                AcukwikFBOHandlerId = request.AcukwikFboHandlerId,
                Active = true,
                DateActivated = DateTime.Now
            };

            _context.Fbos.Add(fbo);
            await _context.SaveChangesAsync();

            Fboairports fboairport = new Fboairports
            {
                Icao = request.Icao,
                Iata = request.Iata,
                Fboid = fbo.Oid
            };
            _context.Fboairports.Add(fboairport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFbo", new { id = fbo.Oid }, new
            {
                request.Icao,
                request.Iata,
                Active = true,
                request.Fbo,
                fbo.Oid
            });
        }

        [HttpPost("single")]
        public async Task<IActionResult> CreateSingleFbo([FromBody] SingleFboRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Group group = new Group
            {
                GroupName = request.Group,
                Active = true
            };
            _context.Group.Add(group);
            await _context.SaveChangesAsync();

            Fbos fbo = new Fbos
            {
                Fbo = request.Fbo,
                GroupId = group.Oid,
                AcukwikFBOHandlerId = request.AcukwikFboHandlerId,
                Active = true,
                DateActivated = DateTime.Now
            };

            _context.Fbos.Add(fbo);
            await _context.SaveChangesAsync();

            Fboairports fboairport = new Fboairports
            {
                Icao = request.Icao,
                Iata = request.Iata,
                Fboid = fbo.Oid
            };
            _context.Fboairports.Add(fboairport);
            await _context.SaveChangesAsync();

            var task = Task.Run(async () =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<FboLinxContext>();
                    await GroupCustomersService.BeginCustomerAircraftsImport(db, group.Oid);
                }

            });

            return CreatedAtAction("GetFbo", new { id = fbo.Oid }, new
            {
                request.Icao,
                request.Iata,
                Active = true,
                request.Fbo,
                fbo.Oid
            });
        }

        // DELETE: api/Fbos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFbos([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbos = await _context.Fbos.FindAsync(id);
            if (fbos == null)
            {
                return NotFound();
            }

            _context.Fbos.Remove(fbos);
            await _context.SaveChangesAsync();

            return Ok(fbos);
        }

        [AllowAnonymous]
        [APIKey(IntegrationPartners.IntegrationPartnerTypes.Internal)]
        [HttpGet("by-akukwik-record/{handlerId}")]
        public async Task<ActionResult<Fbos>> GetFboByAcukwikRecord([FromRoute] int handlerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fbo = await _context.Fbos.Where(x => x.AcukwikFBOHandlerId == handlerId).Include(x => x.Group).Include(x => x.fboAirport).Include(x => x.Contacts).ThenInclude(c => c.Contact).FirstOrDefaultAsync();

            return Ok(fbo);
        }

        private bool FbosExists(int id)
        {
            return _context.Fbos.Any(e => e.Oid == id);
        }

        private IQueryable<Fbos> GetAllFbos()
        {
            return _context.Fbos.AsQueryable();
        }
    }
}