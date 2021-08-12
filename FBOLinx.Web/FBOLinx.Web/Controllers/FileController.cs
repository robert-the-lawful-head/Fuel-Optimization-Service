using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FboLinxContext _context;
        private readonly FilestorageContext _filestorageContext;

        public FileController(FboLinxContext context, FilestorageContext filestorageContext)
        {
            _context = context;
            _filestorageContext = filestorageContext;
        }

        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<IActionResult> SaveFile([FromForm] IFormFile UploadFiles)
        {
            using MemoryStream ms = new MemoryStream();
            UploadFiles.CopyTo(ms);
            var fileBytes = ms.ToArray();

            var fbolinxFile = new FbolinxFileData
            {
                FileName = UploadFiles.FileName,
                ContentType = UploadFiles.ContentType,
                FileData = fileBytes
            };
            _filestorageContext.Add(fbolinxFile);

            await _filestorageContext.SaveChangesAsync();

            var url = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}/api/file/{fbolinxFile.Oid}";

            return Ok(new
            {
                Url = url
            });
        }
    }
}
