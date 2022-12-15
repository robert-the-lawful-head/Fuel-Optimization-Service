using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface ICustomersEntityService : IRepository<Customers, FboLinxContext>
    {
        
    }

    public class CustomersEntityService : Repository<Customers, FboLinxContext>, ICustomersEntityService
    {
        public CustomersEntityService(FboLinxContext context) : base(context)
        {
        }
    }
}
