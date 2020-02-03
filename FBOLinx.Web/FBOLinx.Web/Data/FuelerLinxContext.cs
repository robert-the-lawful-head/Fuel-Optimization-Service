using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Data
{
    public class FuelerLinxContext : DbContext
    {
        public FuelerLinxContext()
        {
        }

        public FuelerLinxContext(DbContextOptions<FuelerLinxContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
    }
}
