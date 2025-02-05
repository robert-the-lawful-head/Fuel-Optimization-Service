using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.DTO.UseCaseModels;
using FBOLinx.ServiceLayer.EntityServices;
using Microsoft.Extensions.Caching.Memory;

namespace FBOLinx.ServiceLayer.BusinessServices.Contacts
{
    public interface IContactInfoByFboService : IBaseDTOService<ContactInfoByFboDto, DB.Models.ContactInfoByFbo>
    {
        Task UpdateDistributionForAllCustomerContactsByCustomerId(int customerId, int fboId, bool isDistributionEnabled);
    }
    public class ContactInfoByFboService : BaseDTOService<ContactInfoByFboDto, DB.Models.ContactInfoByFbo, FboLinxContext>, IContactInfoByFboService
    {
        private IContactInfoByFboEntityService _contactInfoByFboEntityService;
        private IFboEntityService _fboEntityService;
        private IContactInfoByGroupService _contactInfoByGroupService;

        public ContactInfoByFboService(IContactInfoByFboEntityService contactInfoByFboEntityService, IFboEntityService fboEntityService, IContactInfoByGroupService contactInfoByGroupService) : base(contactInfoByFboEntityService)
        {
            _contactInfoByFboEntityService = contactInfoByFboEntityService;
            _fboEntityService = fboEntityService;
            _contactInfoByGroupService = contactInfoByGroupService;
        }

        public async Task UpdateDistributionForAllCustomerContactsByCustomerId(int customerId, int fboId, bool isDistributionEnabled)
        {
            var fbo = await _fboEntityService.GetFboModel(fboId);
            var allCustomerContacts = await _contactInfoByGroupService.GetAllCustomerContactsByGroupFbo(fbo.GroupId, fboId);
            var customerContacts = allCustomerContacts.Where(c => c.CustomerId == customerId).ToList();

            if (customerContacts.Count > 0)
            {
                List<ContactInfoByFbo> contactsToAdd = new List<ContactInfoByFbo>();
                List<ContactInfoByFbo> contactsToUpdate = new List<ContactInfoByFbo>();

                foreach (var contact in customerContacts)
                {
                    if (contact.ContactInfoByFboId > 0)
                    {
                        var contactInfoByFbo = new ContactInfoByFbo
                        {
                            ContactId = contact.ContactId,
                            FboId = fboId,
                            Oid = contact.ContactInfoByFboId.GetValueOrDefault(),
                            CopyAlerts = isDistributionEnabled
                        };

                        contactsToUpdate.Add(contactInfoByFbo);
                    }
                    else
                    {
                        var contactInfoByFbo = new ContactInfoByFbo
                        {
                            ContactId = contact.ContactId,
                            FboId = fboId,
                            CopyAlerts = isDistributionEnabled
                        };

                        contactsToAdd.Add(contactInfoByFbo);
                    }
                }

                await _contactInfoByFboEntityService.BulkInsert(contactsToAdd);
                await _contactInfoByFboEntityService.BulkUpdate(contactsToUpdate);
            }
        }
    }
}
