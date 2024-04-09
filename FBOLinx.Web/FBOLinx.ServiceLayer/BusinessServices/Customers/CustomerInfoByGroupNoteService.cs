using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.BusinessServices.Common;
using FBOLinx.ServiceLayer.BusinessServices.User;
using FBOLinx.ServiceLayer.DTO;
using FBOLinx.ServiceLayer.EntityServices;

namespace FBOLinx.ServiceLayer.BusinessServices.Customers
{
    public interface ICustomerInfoByGroupNoteService : IBaseDTOService<CustomerInfoByGroupNoteDto, DB.Models.CustomerInfoByGroupNote>
    {
    }

    public class CustomerInfoByGroupNoteService : BaseDTOService<CustomerInfoByGroupNoteDto, DB.Models.CustomerInfoByGroupNote, FboLinxContext>, ICustomerInfoByGroupNoteService
    {
        public CustomerInfoByGroupNoteService(ICustomerInfoByGroupNoteEntityService entityService) : base(entityService)
        {
        }
    }
}
