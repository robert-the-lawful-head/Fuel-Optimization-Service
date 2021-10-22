using System;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerTagsController : ControllerBase
    {
        private readonly FboLinxContext _context;
        public CustomerTagsController(FboLinxContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return all tags from a customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTags([FromQuery] CustomerTagRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tags = await _context.CustomerTag.Where(x => x.GroupId == request.GroupId && x.CustomerId == request.CustomerId).ToListAsync();

            var avaibleTags = await _context.CustomerTag
                                    .Where(x => (x.GroupId == request.GroupId || x.GroupId == 0))
                                    .Where(x => (!request.IsFuelerLinx && (x.Name != "FuelerLinx")) || request.IsFuelerLinx)
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();


            foreach (var tag in avaibleTags)
            {
                if (!tags.Any(x => x.Name == tag.Name))
                {
                    tags.Add(tag);
                }
            }

            return Ok(tags);
        }

        /// <summary>
        /// Return all tags from a group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{id}")]
        public async Task<IActionResult> GetTagsByGroup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tags = await _context.CustomerTag
                                    .Where(x => (x.GroupId == id || x.GroupId == 0))
                                                         .OrderBy(x=>x.Name)
                                                         .Select(x=>x.Name)
                                                         .Distinct()
                                                         .ToListAsync()
                                                         ;

            return Ok(tags);
        }

        /// <summary>
        /// Delete a tag from a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tag = await _context.CustomerTag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.CustomerTag.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(tag);
        }

        /// <summary>
        /// Add a tag to a customer
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertTag([FromBody] CustomerTag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.CustomerTag.Add(tag);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}