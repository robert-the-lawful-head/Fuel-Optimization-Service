using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.Logging;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.CodeAnalysis;
using FBOLinx.ServiceLayer.Mapping;
using System.Collections.Generic;
using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Enums;

namespace FBOLinx.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : FBOLinxControllerBase
    {
        private readonly FboLinxContext _context;

        public DocumentsController( FboLinxContext context, ILoggingService logger) : base(logger)
        {
            _context = context;
        }

        // POST: api/documents/20/user/10/accepted-policies-and-agreements
        [HttpPost("{documentId}/user/{userId}/accept-policies-and-agreements")]
        public async Task<ActionResult<UserAcceptedPolicyAndAgreements>> AcceptPolicyAndAgreement([FromRoute] int userId, [FromRoute] int documentId)
        {
            var newUserAcceptedPolicyAndAgreements = new UserAcceptedPolicyAndAgreements()
            {
                UserId = userId,
                DocumentId = documentId,
                AcceptedDateTime = DateTime.UtcNow
            };

            var insertedRecord = await _context.UserAcceptedPolicyAndAgreements.AddAsync(newUserAcceptedPolicyAndAgreements);
            await _context.SaveChangesAsync();

            return Ok(insertedRecord.Entity);
        }
        // GET: api/documents/user/10/documents-to-accept
        [HttpGet("user/{userId}/documents-to-accept")]
        public async Task<ActionResult<DocumentsToAcceptDto>> documentsToAccept([FromRoute] int userId)
        {
            var eulaDocument = await _context.PolicyAndAgreementDocuments.Where(x => x.DocumentType == "EULA").OrderByDescending(b => b.Oid).FirstOrDefaultAsync();
            var hasacceptedDocument = _context.UserAcceptedPolicyAndAgreements.Where(x => x.DocumentId == eulaDocument.Oid && x.UserId == userId).Any();

            var documentToAccept = new DocumentsToAcceptDto()
            {
                UserId = userId,
                DocumentToAccept = eulaDocument,
                hasPendingDocumentsToAccept = (eulaDocument == null) ? false : !hasacceptedDocument
            };
            return Ok(documentToAccept);
        }
        // GET: api/documents/group/5
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<GroupPolicyAndAgreementDocuments>> GetAllGroupDocuments([FromRoute] int groupId)
        {
            var documents =  new List<PolicyAndAgreementDocuments>();
            documents.Add(await _context.PolicyAndAgreementDocuments.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.EULA)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync());
            documents.Add(await _context.PolicyAndAgreementDocuments.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.Cookie)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync());
            documents.Add(await _context.PolicyAndAgreementDocuments.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.Privacy)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync());

            var exemptions = await _context.PolicyAndAgreementGroupExemptions.Where(x => x.GroupId == groupId).ToListAsync();

            var groupdocs = documents.Map<List<GroupPolicyAndAgreementDocuments>>();

            foreach (var document in groupdocs)
            {

                document.IsExempted = exemptions.Any(x => x.DocumentId == document.Oid);
            }

            return Ok(groupdocs);
        }
        // POST: api/documents/user/10/accepted-policies-and-agreements
        [HttpPost("user/{userId}/accept-policies-and-agreements")]
        public async Task<ActionResult<List<UserAcceptedPolicyAndAgreements>>> AcceptPolicyAndAgreement([FromRoute] int userId, [FromBody] int[] documentIdList)
        {
            var recordsToSubmit = new List<UserAcceptedPolicyAndAgreements>();

            foreach(var documentId in documentIdList)
            {
                var newUserAcceptedPolicyAndAgreements = new UserAcceptedPolicyAndAgreements()
                {
                    UserId = userId,
                    DocumentId = documentId,
                    AcceptedDateTime = DateTime.UtcNow
                };
                recordsToSubmit.Add(newUserAcceptedPolicyAndAgreements);
            }

            await _context.UserAcceptedPolicyAndAgreements.AddRangeAsync(recordsToSubmit);
            await _context.SaveChangesAsync();

            return Ok();
        }
        // GET: api/documents/group/20/toogle-document-exemption
        [HttpPost("group/{groupId}/toogle-document-exemption")]
        public async Task<IActionResult> ToogleDocumentExemption([FromRoute] int groupId, [FromBody] List<GroupPolicyAndAgreementDocuments> documents)
        {
            foreach (var document in documents)
            {
                var excemption = await _context.PolicyAndAgreementGroupExemptions.Where(x => x.DocumentId == document.Oid).FirstOrDefaultAsync();

                if (document.IsExempted && excemption == null)
                {
                    var newExcemptionRecord = new PolicyAndAgreementGroupExemptions()
                    {
                        DocumentId = document.Oid,
                        DateTimeExempted = DateTime.UtcNow,
                        GroupId = groupId
                    };
                    await _context.PolicyAndAgreementGroupExemptions.AddAsync(newExcemptionRecord);
                    await _context.SaveChangesAsync();
                }
                if (!document.IsExempted && excemption != null)
                {
                    if (excemption != null)
                    {
                        _context.PolicyAndAgreementGroupExemptions.Remove(excemption);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return NoContent();
        }
    }
}
