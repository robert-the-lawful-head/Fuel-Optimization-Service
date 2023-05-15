using FBOLinx.Core.Enums;
using FBOLinx.Core.Utilities.Enums;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;
using FBOLinx.ServiceLayer.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.BusinessServices.Documents
{
    public interface IDocumentService
    {
        Task<UserAcceptedPolicyAndAgreements> AcceptPolicyAndAgreement(int userId, int documentId);
        Task<DocumentsToAcceptDto> DocumentsToAccept(int userId, int groupId);
        Task<List<GroupPolicyAndAgreementDocuments>> GetAllGroupDocuments(int groupId);
        Task AcceptPolicyAndAgreement(int userId, int[] documentIdList);
        Task ToogleDocumentExemption(int groupId, List<GroupPolicyAndAgreementDocuments> documents);
    }
    public class DocumentService : IDocumentService
    {
        private IRepository<UserAcceptedPolicyAndAgreements, FboLinxContext> _userAcceptedPolicyAndAgreementsRepo { get; set; } 
        private IRepository<PolicyAndAgreementDocuments, FboLinxContext> _policyAndAgreementDocumentsRepo { get; set; }
        private IRepository<PolicyAndAgreementGroupExemptions, FboLinxContext> _policyAndAgreementGroupExemptionsRepo { get; set; }
        public DocumentService( 
            IRepository<UserAcceptedPolicyAndAgreements, FboLinxContext> userAcceptedPolicyAndAgreementsRepo,
            IRepository<PolicyAndAgreementDocuments, FboLinxContext> policyAndAgreementDocumentsRepo,
            IRepository<PolicyAndAgreementGroupExemptions, FboLinxContext> policyAndAgreementGroupExemptionsRepo)
        {
            _userAcceptedPolicyAndAgreementsRepo = userAcceptedPolicyAndAgreementsRepo;
            _policyAndAgreementDocumentsRepo = policyAndAgreementDocumentsRepo;
            _policyAndAgreementGroupExemptionsRepo = policyAndAgreementGroupExemptionsRepo;
        }
        public async Task<UserAcceptedPolicyAndAgreements> AcceptPolicyAndAgreement(int userId, int documentId)
        {
            var newUserAcceptedPolicyAndAgreements = new UserAcceptedPolicyAndAgreements()
            {
                UserId = userId,
                DocumentId = documentId,
                AcceptedDateTime = DateTime.UtcNow
            };

            var insertedRecord = await _userAcceptedPolicyAndAgreementsRepo.AddAsync(newUserAcceptedPolicyAndAgreements);

            return insertedRecord;
        }
        public async Task<DocumentsToAcceptDto> DocumentsToAccept(int userId, int groupId)
        {
            var eulaDocument = await _policyAndAgreementDocumentsRepo.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.EULA)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync();
            var hasacceptedDocument = _userAcceptedPolicyAndAgreementsRepo.Where(x => x.DocumentId == eulaDocument.Oid && x.UserId == userId).Any();
            var isDocumentExempted = _policyAndAgreementGroupExemptionsRepo.Where(x => x.DocumentId == eulaDocument.Oid && x.GroupId == groupId).Any();

            var documentToAccept = new DocumentsToAcceptDto()
            {
                UserId = userId,
                DocumentToAccept = eulaDocument,
                hasPendingDocumentsToAccept = (eulaDocument == null || !eulaDocument.IsEnabled || isDocumentExempted) ? false : !hasacceptedDocument
            };
            return documentToAccept;
        }
        public async Task<List<GroupPolicyAndAgreementDocuments>> GetAllGroupDocuments(int groupId)
        {
            var documents = new List<PolicyAndAgreementDocuments>();
            documents.Add(await _policyAndAgreementDocumentsRepo.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.EULA)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync());
            documents.Add(await _policyAndAgreementDocumentsRepo.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.Cookie)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync());
            documents.Add(await _policyAndAgreementDocumentsRepo.Where(x => x.DocumentType == EnumHelper.GetDescription(DocumentTypeEnum.Privacy)).OrderByDescending(b => b.Oid).FirstOrDefaultAsync());
            documents.RemoveAll(item => item == null);

            var exemptions = await _policyAndAgreementGroupExemptionsRepo.Where(x => x.GroupId == groupId).ToListAsync();

            var groupdocs = documents.Map<List<GroupPolicyAndAgreementDocuments>>();

            foreach (var document in groupdocs)
            {

                document.IsExempted = exemptions.Any(x => x.DocumentId == document.Oid);
            }

            return groupdocs;
        }
        public async Task AcceptPolicyAndAgreement(int userId,int[] documentIdList)
        {
            var recordsToSubmit = new List<UserAcceptedPolicyAndAgreements>();

            foreach (var documentId in documentIdList)
            {
                var newUserAcceptedPolicyAndAgreements = new UserAcceptedPolicyAndAgreements()
                {
                    UserId = userId,
                    DocumentId = documentId,
                    AcceptedDateTime = DateTime.UtcNow
                };
                recordsToSubmit.Add(newUserAcceptedPolicyAndAgreements);
            }

            await _userAcceptedPolicyAndAgreementsRepo.AddRangeAsync(recordsToSubmit);
        }
        public async Task ToogleDocumentExemption(int groupId, List<GroupPolicyAndAgreementDocuments> documents)
        {
            foreach (var document in documents)
            {
                var excemption = await _policyAndAgreementGroupExemptionsRepo.Where(x => x.DocumentId == document.Oid).FirstOrDefaultAsync();

                if (document.IsExempted && excemption == null)
                {
                    var newExcemptionRecord = new PolicyAndAgreementGroupExemptions()
                    {
                        DocumentId = document.Oid,
                        DateTimeExempted = DateTime.UtcNow,
                        GroupId = groupId
                    };
                    await _policyAndAgreementGroupExemptionsRepo.AddAsync(newExcemptionRecord);
                }
                if (!document.IsExempted && excemption != null)
                {
                    if (excemption != null)
                    {
                        await _policyAndAgreementGroupExemptionsRepo.DeleteAsync(excemption);
                    }
                }
            }
        }
    }
}
