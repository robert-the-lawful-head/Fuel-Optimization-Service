﻿using FBOLinx.Core.BaseModels.Entities;
using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using FBOLinx.DB.Models.Dega;

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
        public virtual DbSet<AcukwikAirport> AcukwikAirports { get; set; }
        public virtual DbSet<AcukwikFbohandlerDetail> AcukwikFbohandlerDetail { get; set; }
        public virtual DbSet<AFSAircraft> AFSAircraft { get; set; }
        public virtual DbSet<AircraftSpecifications> AircraftSpecifications { get; set; }
        public virtual DbSet<ImportedFboEmails> ImportedFboEmails { get; set; }
        public virtual DbSet<AircraftHexTailMapping> AircraftHexTailMapping { get; set; }
        public virtual DbSet<FAAAircraftMakeModelReference> FAAAircraftMakeModelReference { get; set; }
        public virtual DbSet<SWIMUnrecognizedFlightLeg> SWIMUnrecognizedFlightLegs { get; set; }
        public virtual DbSet<AcukwikServicesOffered> AcukwikServicesOffered { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcukwikAirport>(entity =>
            {
                entity.HasIndex(e => new { AirportId = e.Oid, e.Iata, e.Icao })
                    .HasName("INX_ICAO_AirportID_IATA");

                entity.Property(e => e.Oid).ValueGeneratedNever();
            });

            modelBuilder.Entity<AcukwikFbohandlerDetail>(entity =>
            {
                entity.Property(e => e.HandlerId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AircraftHexTailMapping>(entity =>
            {
                entity.Property(e => e.AircraftHexCode).IsUnicode(false);

                entity.Property(e => e.TailNumber).IsUnicode(false);
            });

            modelBuilder.Entity<DatabaseStringSplitResult>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<AcukwikServicesOffered>()
           .HasKey(e => new { e.HandlerId, e.ServiceOfferedId });
        }

        [DbFunction("fn_Split")]
        public IQueryable<DatabaseStringSplitResult> SplitStringToTable(string inputString, string delimiter)
            => throw new NotSupportedException();
    }
}
