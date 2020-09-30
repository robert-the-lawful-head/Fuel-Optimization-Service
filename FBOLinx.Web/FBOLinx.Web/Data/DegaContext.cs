using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Web.Data
{
    public class DegaContext : DbContext
    {
        public DegaContext()
        {
        }

        public DegaContext(DbContextOptions<DegaContext> options) : base(options)
        {

        }

        public virtual DbSet<AcukwikAirports> AcukwikAirports { get; set; }
        public virtual DbSet<AcukwikFbohandlerDetail> AcukwikFbohandlerDetail { get; set; }
        public virtual DbSet<AircraftSpecifications> AircraftSpecifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("SERVER=fl-db1.fuelerlinx.com; Initial Catalog=Dega; Failover Partner=DegaSQL1.fuelerlinx.com; user=sqlunderling; pwd=b3ing r3pr3ss3d FTL; Connection Timeout=180;Min Pool Size=30;Max Pool Size=500;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcukwikAirports>(entity =>
            {
                entity.HasIndex(e => new { e.AirportId, e.Iata, e.Icao })
                    .HasName("INX_ICAO_AirportID_IATA");

                entity.Property(e => e.AirportId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AcukwikFbohandlerDetail>(entity =>
            {
                entity.Property(e => e.HandlerId).ValueGeneratedNever();
            });
        }
    }
}
