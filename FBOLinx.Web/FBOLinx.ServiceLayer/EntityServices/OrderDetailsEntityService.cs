using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.Extensions;
using FBOLinx.DB;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IOrderDetailsEntityService : IRepository<OrderDetails, FboLinxContext>
    {
        Task<List<OrderDetails>> GetOrderDetailsByIds(List<int> ids);
    }

    public class OrderDetailsEntityService : Repository<OrderDetails, FboLinxContext>, IOrderDetailsEntityService
    {
        private readonly FboLinxContext _context;

        public OrderDetailsEntityService(FboLinxContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderDetails>> GetOrderDetailsByIds(List<int> ids)
        {
            var orderDetailsList = await (from orderDetails in _context.OrderDetails
                                      join id in _context.AsTable(ids) on orderDetails.FuelerLinxTransactionId equals System.Convert.ToInt32(id.Value)
                                      select orderDetails).ToListAsync();
            return orderDetailsList;
        }
    }
}
