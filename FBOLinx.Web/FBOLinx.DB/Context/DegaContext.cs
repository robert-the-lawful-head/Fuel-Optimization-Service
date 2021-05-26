using FBOLinx.DB.Models;
using FBOLinx.Web;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.DB.Context
{
    public class DegaContext : DbContext
    {
        public DegaContext()
        {
        }

        public DegaContext(DbContextOptions<DegaContext> options) : base(options)
        {

        }

        public virtual DbSet<AirCrafts> AirCrafts { get; set; }
        public virtual DbSet<AcukwikAirports> AcukwikAirports { get; set; }
        public virtual DbSet<AcukwikFbohandlerDetail> AcukwikFbohandlerDetail { get; set; }
        public virtual DbSet<AFSAircraft> AFSAircraft { get; set; }
        public virtual DbSet<AircraftSpecifications> AircraftSpecifications { get; set; }
        public virtual DbSet<ImportedFboEmails> ImportedFboEmails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

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
