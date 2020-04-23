using FBOLinx.Web.Models;
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

        public virtual DbSet<FuelerData> FuelerData { get; set; }
        public virtual DbSet<FuelerList> FuelerList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FuelerData>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.ToTable("fuelerData");

                entity.HasIndex(e => e.CustId)
                    .HasName("FD_Cust");

                entity.HasIndex(e => new { e.QbvendorName, e.FuelerId, e.CompanyId })
                    .HasName("FD_ID_CoID");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.AddDate)
                    .HasColumnName("addDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgDate)
                    .HasColumnName("chgDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CompanyId).HasColumnName("companyID");

                entity.Property(e => e.CustId).HasColumnName("custID");

                entity.Property(e => e.FuelerId).HasColumnName("fuelerID");

                entity.Property(e => e.OffDate)
                    .HasColumnName("offDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.QbvendorName)
                    .HasColumnName("QBVendorName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<FuelerList>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.ToTable("fuelerList");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.AddDate)
                    .HasColumnName("addDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ccemail)
                    .HasColumnName("CCEmail")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ChgDate)
                    .HasColumnName("chgDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ContactEmail)
                    .HasColumnName("contactEMail")
                    .HasMaxLength(255);

                entity.Property(e => e.EMail)
                    .HasColumnName("eMail")
                    .HasMaxLength(255);

                entity.Property(e => e.FbolinxId).HasColumnName("FBOLinxID");

                entity.Property(e => e.FuelerNm)
                    .HasColumnName("fuelerNm")
                    .HasMaxLength(255);

                entity.Property(e => e.FuelerPhone)
                    .HasColumnName("fuelerPhone")
                    .HasMaxLength(255);

                entity.Property(e => e.FuelerType)
                    .HasColumnName("fuelerType")
                    .HasMaxLength(50);

                entity.Property(e => e.ImagePath)
                    .HasColumnName("imagePath")
                    .HasMaxLength(255);

                entity.Property(e => e.InternationalEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OffDate)
                    .HasColumnName("offDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProcessNm)
                    .HasColumnName("processNm")
                    .HasMaxLength(255);

                entity.Property(e => e.PullFlag).HasColumnName("pullFlag");

                entity.Property(e => e.ServiceUrl)
                    .HasColumnName("ServiceURL")
                    .HasMaxLength(50);

                entity.Property(e => e.WebLink)
                    .HasColumnName("webLink")
                    .HasMaxLength(255);
            });
        }
    }
}
