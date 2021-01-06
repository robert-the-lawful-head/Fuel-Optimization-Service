using FBOLinx.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.DB.Context
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

        public virtual DbSet<CompanyFuelers> CompanyFuelers { get; set; }
        public virtual DbSet<FuelerList> FuelerList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CompanyFuelers>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => new { e.FuelerId, e.CompanyId, e.Active })
                    .HasName("INX_CompanyFuelers_CompanyIDActive");

                entity.HasIndex(e => new { e.Oid, e.CompanyId, e.FuelerId })
                    .HasName("INX_CompanyFuelers_FuelerID");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.AddDate).HasColumnType("datetime");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.FuelerId).HasColumnName("FuelerID");
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
