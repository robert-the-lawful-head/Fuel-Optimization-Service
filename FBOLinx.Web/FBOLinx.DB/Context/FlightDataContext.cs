using System;
using System.Linq;
using FBOLinx.Core.BaseModels.Entities;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.DB.Context
{
    public class FlightDataContext : DbContext
    {
        public FlightDataContext()
        {
        }

        public FlightDataContext(DbContextOptions<FlightDataContext> options) : base(options)
        {

        }
        public virtual DbSet<SWIMFlightLeg> SWIMFlightLegs { get; set; }
        public virtual DbSet<SWIMFlightLegData> SWIMFlightLegData { get; set; }
        public virtual DbSet<SWIMFlightLegDataError> SWIMFlightLegDataErrors { get; set; }
        public virtual DbSet<SWIMUnrecognizedFlightLeg> SWIMUnrecognizedFlightLegs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SWIMFlightLeg>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK_SWIMFlightLegs_OID");
                entity.HasIndex(x => new { x.AircraftIdentification, x.DepartureICAO, x.ArrivalICAO, x.ATD })
                    .HasName("IX_SWIMFlightLegs_TailNumber_DepartureICAO_ArrivalICAO_ATD");
            });

            modelBuilder.Entity<SWIMFlightLegData>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK_SWIMFlightLegData_OID");

                entity.HasOne(data => data.SWIMFlightLeg)
                    .WithMany(leg => leg.SWIMFlightLegDataMessages)
                    .HasForeignKey(data => data.SWIMFlightLegId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SWIMFlightLegData_SWIMFlightLegs");

                entity.Ignore(x => x.RawXmlMessage);
            });

            modelBuilder.Entity<SWIMFlightLegDataError>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK_SWIMFlightLegDataErrors_OID");

                entity.HasOne(data => data.SWIMFlightLegData)
                    .WithMany(leg => leg.SWIMFlightLegDataErrors)
                    .HasForeignKey(data => data.SWIMFlightLegDataId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SWIMFlightLegDataErrors_SWIMFlightLegData");
            });
        }
        [DbFunction("fn_Split")]
        public IQueryable<DatabaseStringSplitResult> SplitStringToTable(string inputString, string delimiter)
            => throw new NotSupportedException();
    }
}
