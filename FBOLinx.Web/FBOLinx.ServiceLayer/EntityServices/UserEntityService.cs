using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.Service.Mapping.Dto;
using FBOLinx.ServiceLayer.BusinessServices.Fbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IUserEntityService : IRepository<User, FboLinxContext>
    {
    }
    public class UserEntityService : Repository<User, FboLinxContext>, IUserEntityService
    {
        public UserEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
