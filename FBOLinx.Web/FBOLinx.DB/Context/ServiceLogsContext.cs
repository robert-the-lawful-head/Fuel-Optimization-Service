using FBOLinx.DB.Models;
using FBOLinx.DB.Models.ServiceLogs;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.DB.Context
{
    public class ServiceLogsContext : DbContext
    {
        public ServiceLogsContext()
        {
        }

        public ServiceLogsContext(DbContextOptions<ServiceLogsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TableStorageLog> TableStorageLog { get; set; }
    }
}
