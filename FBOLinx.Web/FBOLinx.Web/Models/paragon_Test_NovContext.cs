using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FBOLinx.Web.Models
{
    public partial class paragon_Test_NovContext : DbContext
    {
        public paragon_Test_NovContext()
        {
        }

        public paragon_Test_NovContext(DbContextOptions<paragon_Test_NovContext> options)
            : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("SERVER=fl-db1.fuelerlinx.com; Initial Catalog=paragon_Test_Nov; Failover Partner=DegaSQL1.fuelerlinx.com; user=sqlunderling; pwd=b3ing r3pr3ss3d FTL; Connection Timeout=180;Min Pool Size=30;Max Pool Size=500;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");
        }
    }
}
