using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.DTO;
using System.Collections.Generic;
using FBOLinx.ServiceLayer.BusinessServices.Documents;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : FBOLinxControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService, ILoggingService logger) : base(logger)
        {
            _documentService = documentService;
        }

        // POST: api/documents/20/user/10/accepted-policies-and-agreements
        [HttpPost("{documentId}/user/{userId}/accept-policies-and-agreements")]
        public async Task<ActionResult<UserAcceptedPolicyAndAgreements>> AcceptPolicyAndAgreement([FromRoute] int userId, [FromRoute] int documentId)
        {
            var insertedRecord = _documentService.AcceptPolicyAndAgreement(userId, documentId);

            return Ok(insertedRecord);
        }
        // GET: api/documents/group/30/user/10/documents-to-accept
        [HttpGet("group/{groupId}/user/{userId}/documents-to-accept")]
        public async Task<ActionResult<DocumentsToAcceptDto>> DocumentsToAccept([FromRoute] int userId, [FromRoute] int groupId)
        {
            var documentToAccept = await _documentService.DocumentsToAccept(userId, groupId);
            return Ok(documentToAccept);
        }
        // GET: api/documents/group/5
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<GroupPolicyAndAgreementDocuments>> GetAllGroupDocuments([FromRoute] int groupId)
        {
            var groupdocs = await _documentService.GetAllGroupDocuments(groupId);

            return Ok(groupdocs);
        }
        // POST: api/documents/user/10/accepted-policies-and-agreements
        [HttpPost("user/{userId}/accept-policies-and-agreements")]
        public async Task<ActionResult<List<UserAcceptedPolicyAndAgreements>>> AcceptPolicyAndAgreement([FromRoute] int userId, [FromBody] int[] documentIdList)
        {
            _documentService.AcceptPolicyAndAgreement(userId, documentIdList);

            return NoContent();
        }
        // GET: api/documents/group/20/toogle-document-exemption
        [HttpPost("group/{groupId}/toogle-document-exemption")]
        public async Task<IActionResult> ToogleDocumentExemption([FromRoute] int groupId, [FromBody] List<GroupPolicyAndAgreementDocuments> documents)
        {
            await _documentService.ToogleDocumentExemption(groupId, documents);

            return NoContent();
        }
    }
}
