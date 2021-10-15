using FBOLinx.DB.Models;
using FBOLinx.Web;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.DB.Context
{
    public class FboLinxContext : DbContext
    {
        public FboLinxContext()
        {
        }

        public FboLinxContext(DbContextOptions<FboLinxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessTokens> AccessTokens { get; set; }
        //public virtual DbSet<AirCrafts> Aircrafts { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<CompanyPricingLog> CompanyPricingLog { get; set; }
        public virtual DbSet<CustomerAircrafts> CustomerAircrafts { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Fboairports> Fboairports { get; set; }
        public virtual DbSet<Fbos> Fbos { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Fbocontacts> Fbocontacts { get; set; }
        public virtual DbSet<CustomCustomerTypes> CustomCustomerTypes { get; set; }
        public virtual DbSet<CustomerMargins> CustomerMargins { get; set; }
        public virtual DbSet<CustomersViewedByFbo> CustomersViewedByFbo { get; set; }
        public virtual DbSet<Fboprices> Fboprices { get; set; }
        public virtual DbSet<PriceTiers> PriceTiers { get; set; }
        public virtual DbSet<PricingTemplate> PricingTemplate { get; set; }
        public virtual DbSet<CompaniesByGroup> CompaniesByGroup { get; set; }
        public virtual DbSet<ContactInfoByGroup> ContactInfoByGroup { get; set; }
        public virtual DbSet<CustomerContacts> CustomerContacts { get; set; }
        public virtual DbSet<AircraftPrices> AircraftPrices { get; set; }
        public virtual DbSet<RampFeeSettings> RampFeeSettings { get; set; }
        public virtual DbSet<RampFees> RampFees { get; set; }
        public virtual DbSet<FboaircraftSizes> FboaircraftSizes { get; set; }
        public virtual DbSet<Fbopreferences> Fbopreferences { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<DistributionLog> DistributionLog { get; set; }
        public virtual DbSet<CustomerInfoByGroup> CustomerInfoByGroup { get; set; }
        public virtual DbSet<EmailContent> EmailContent { get; set; }
        public virtual DbSet<CustomerCompanyTypes> CustomerCompanyTypes { get; set; }
        public virtual DbSet<DistributionQueue> DistributionQueue { get; set; }
        public virtual DbSet<DistributionErrors> DistributionErrors { get; set; }
        public virtual DbSet<TempAddOnMargin> TempAddOnMargin { get; set; }
        public virtual DbSet<MappingPrices> MappingPrices { get; set; }
        public virtual DbSet<IntegrationPartners> IntegrationPartners { get; set; }
        public virtual DbSet<VolumeScaleDiscount> VolumeScaleDiscount { get; set; }
        public virtual DbSet<CustomerDefaultTemplates> CustomerDefaultTemplates { get; set; }
        public virtual DbSet<AdminEmails> AdminEmails { get; set; }
        public virtual DbSet<CustomerAircraftViewedByGroup> CustomerAircraftViewedByGroup { get; set; }
        public virtual DbSet<CustomerNotes> CustomerNotes { get; set; }
        public virtual DbSet<CustomerSchedulingSoftwareByGroup> CustomerSchedulingSoftwareByGroup { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<NetworkNotes> NetworkNotes { get; set; }
        public virtual DbSet<DefaultDiscount> DefaultDiscount { get; set; }
        public virtual DbSet<FuelReq> FuelReq { get; set; }
        public virtual DbSet<FuelReqPricingTemplate> FuelReqPricingTemplate { get; set; }
        public virtual DbSet<ReminderEmailsToFbos> ReminderEmailsToFbos { get; set; }
        public virtual DbSet<ContractFuelRelationships> ContractFuelRelationships { get; set; }
        public virtual DbSet<DistributionEmailsBody> DistributionEmailsBody { get; set; }
        public virtual DbSet<FbocustomerPricing> FbocustomerPricing { get; set; }
        public virtual DbSet<Fbologos> Fbologos { get; set; }
        public virtual DbSet<FbosalesTax> FbosalesTax { get; set; }
        public virtual DbSet<PriceHistory> PriceHistory { get; set; }
        public virtual DbSet<RequestPricingTracker> RequestPricingTracker { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<FboFeesAndTaxes> FbofeesAndTaxes { get; set; }
        public virtual DbSet<FboFeeAndTaxOmitsByCustomer> FboFeeAndTaxOmitsByCustomer { get; set; }
        public virtual DbSet<FboFeeAndTaxOmitsByPricingTemplate> FboFeeAndTaxOmitsByPricingTemplate { get; set; }
        public virtual DbSet<AirportWatchHistoricalData> AirportWatchHistoricalData { get; set; }
        public virtual DbSet<AirportWatchAircraftTailNumber> AirportWatchAircraftTailNumber { get; set; }
        public virtual DbSet<AirportWatchLiveData> AirportWatchLiveData { get; set; }
        public virtual DbSet<AirportWatchChangeTracker> AirportWatchChangeTracker { get; set; }
        public virtual DbSet<CustomerTag> CustomerTag { get; set; }
        public virtual DbSet<CustomerInfoByGroupLog> CustomerInfoByGroupLog { get; set; }
        public virtual DbSet<CustomerInfoByGroupLogData> CustomerInfoByGroupLogData { get; set; }
        public virtual DbSet<AircraftLogData> AircraftLogData { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValue(true);
            });

            //modelBuilder.Entity<AirCrafts>(entity =>
            //{
            //    entity.Property(e => e.FuelType).IsUnicode(false);
            //});

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Extension).IsUnicode(false);

                entity.Property(e => e.Fax).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);
            });

            modelBuilder.Entity<CompanyPricingLog>(entity =>
            {
                entity.Property(e => e.ICAO).IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<CustomerAircrafts>(entity =>
            {
                entity.HasIndex(e => e.CustomerId)
                    .HasName("INX_CA_CustomerID");

                entity.HasIndex(e => e.TailNumber)
                    .HasName("INX_CustomerAircrafts_TailNumber");

                entity.HasIndex(e => new { e.Oid, e.CustomerId, e.AircraftId, e.TailNumber, e.Size, e.BasedPaglocation, e.NetworkCode, e.GroupId })
                    .HasName("INX_CA_ID");

                entity.HasIndex(e => new { e.Oid, e.CustomerId, e.AircraftId, e.TailNumber, e.Size, e.BasedPaglocation, e.NetworkCode, e.AddedFrom, e.GroupId })
                    .HasName("INX_GroupID");

                entity.Property(e => e.BasedPaglocation).IsUnicode(false);

                entity.Property(e => e.NetworkCode).IsUnicode(false);

                entity.Property(e => e.TailNumber).IsUnicode(false);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.CheckFuelPrice).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Company).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.Lastlogin).IsUnicode(false);

                entity.Property(e => e.MainPhone).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Pilot).IsUnicode(false);

                entity.Property(e => e.Show100Ll).HasDefaultValueSql("((0))");

                entity.Property(e => e.ShowJetA).HasDefaultValueSql("((1))");

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.Property(e => e.Website).IsUnicode(false);

                entity.Property(e => e.ZipCode).IsUnicode(false);
            });

            modelBuilder.Entity<Fboairports>(entity =>
            {
                entity.Property(e => e.Iata).IsUnicode(false);

                entity.Property(e => e.Icao).IsUnicode(false);
            });

            modelBuilder.Entity<Fbos>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.Extension).IsUnicode(false);

                entity.Property(e => e.Fbo).IsUnicode(false);

                entity.Property(e => e.FuelDeskEmail).IsUnicode(false);
                entity.Property(e => e.Website).IsUnicode(false);

                entity.Property(e => e.InitialSetupPhase).HasDefaultValueSql("((1))");

                entity.Property(e => e.MainPhone).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PriceUpdateReminderPrompt).HasDefaultValueSql("((1))");

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.Property(e => e.ZipCode).IsUnicode(false);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Fbos)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_FBOs_Groups");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Domain).IsUnicode(false);

                entity.Property(e => e.GroupName).IsUnicode(false);

                entity.Property(e => e.Isfbonetwork).HasDefaultValueSql("((0))");

                entity.Property(e => e.LoggedInHomePage).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<FuelReq>(entity =>
            {
                entity.Property(e => e.DispatchNotes).IsUnicode(false);

                entity.Property(e => e.Icao).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Source).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.TimeStandard).IsUnicode(false);
            });

            modelBuilder.Entity<FuelReqPricingTemplate>(entity =>
            {
                entity.Property(e => e.PricingTemplateName).IsUnicode(false);

                entity.Property(e => e.PricingTemplateRaw).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerInfoByGroup>(entity =>
            {
                entity.HasIndex(e => new { e.CustomerId, e.GroupId })
                    .HasName("INX_GroupID");

                entity.HasIndex(e => new { e.GroupId, e.CustomerId })
                    .HasName("inxCustomerInfoByGroupGroupID");

                entity.HasIndex(e => new { e.Company, e.Distribute, e.CustomerType, e.CustomerId })
                    .HasName("inxCustomerInfoByGroupCompanyDistributeCustomerType");

                entity.HasIndex(e => new { e.CustomerId, e.Company, e.Username, e.Password, e.Joined, e.Active, e.Distribute, e.Network, e.MainPhone, e.Address, e.City, e.State, e.ZipCode, e.Country, e.Website, e.ShowJetA, e.Show100Ll, e.Suspended, e.DefaultTemplate, e.CustomerType, e.GroupId })
                    .HasName("INX_CIBG_ID");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Company).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.EmailSubscription).HasDefaultValueSql("((1))");

                entity.Property(e => e.MainPhone).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Sfid).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.Property(e => e.Website).IsUnicode(false);

                entity.Property(e => e.ZipCode).IsUnicode(false);
            });

            modelBuilder.Entity<CustomCustomerTypes>(entity =>
            {
                entity.HasIndex(e => new { e.CustomerId, e.CustomerType, e.Fboid })
                    .HasName("indx_CustomCustomerTypes_FBOID");
            });

            modelBuilder.Entity<Fboprices>(entity =>
            {
                entity.HasIndex(e => new { e.Oid, e.Fboid, e.Price, e.EffectiveFrom, e.EffectiveTo, e.Timestamp, e.SalesTax, e.Currency, e.Product })
                    .HasName("indx_FBOPrices");

                entity.HasIndex(e => new { e.Oid, e.Price, e.EffectiveFrom, e.EffectiveTo, e.Timestamp, e.SalesTax, e.Currency, e.Fboid, e.Product })
                    .HasName("indx_FBOPrices_FBOID_Product");

                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.Product).IsUnicode(false);

                entity.Property(e => e.SalesTax).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<PricingTemplate>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);
            });

            modelBuilder.Entity<TempAddOnMargin>(entity =>
            {
                entity.Property(e => e.EffectiveFrom)
                    .HasColumnName("effectiveFrom")
                    .HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo)
                    .HasColumnName("effectiveTo")
                    .HasColumnType("datetime");

                entity.Property(e => e.FboId).HasColumnName("fboId");

                entity.Property(e => e.MarginAvgas)
                    .HasColumnName("marginAvgas");

                entity.Property(e => e.MarginJet)
                    .HasColumnName("marginJet");
            });

            modelBuilder.Entity<ContactInfoByGroup>(entity =>
            {
                entity.HasIndex(e => new { e.ContactId, e.Email, e.FirstName, e.LastName, e.Phone, e.Mobile, e.Address, e.City, e.State, e.Country, e.Fax, e.Title, e.Primary, e.CopyAlerts, e.GroupId })
                    .HasName("INX_CoIBG_ID");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Extension).IsUnicode(false);

                entity.Property(e => e.Fax).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Mobile).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);
            });

            modelBuilder.Entity<AircraftPrices>(entity =>
            {
                entity.HasIndex(e => new { e.CustomerAircraftId, e.PriceTemplateId })
                    .HasName("INX_AirPrice_Template");

                entity.HasIndex(e => new { e.PriceTemplateId, e.CustomerAircraftId })
                    .HasName("INX_Price_Aircraft");

                entity.HasIndex(e => new { e.Oid, e.PriceTemplateId, e.CustomTemplate, e.CustomerAircraftId })
                    .HasName("INX_AP_CustomerAircraftID");

                entity.Property(e => e.CustomTemplate).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CustomersViewedByFbo>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Customer__CB394B39DB271CC8");

                entity.HasIndex(e => new { e.CustomerId, e.Fboid })
                    .HasName("INX_FBOID");
            });

            modelBuilder.Entity<RampFeeSettings>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__RampFeeS__CB394B39B66F96B6");

                entity.Property(e => e.HasRampFees).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<FboaircraftSizes>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__FBOAircr__CB394B39404ED118");
            });

            modelBuilder.Entity<Fbopreferences>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__FBOPrefe__CB394B3926CE926B");
            });

            modelBuilder.Entity<CustomerCompanyTypes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<DistributionErrors>(entity =>
            {
                entity.Property(e => e.Error).IsUnicode(false);
            });

            modelBuilder.Entity<IntegrationPartners>(entity =>
            {
                entity.Property(e => e.PartnerName).IsUnicode(false);
            });

            modelBuilder.Entity<VolumeScaleDiscount>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => new { e.Margin, e.MarginType, e.Fboid })
                    .HasName("indx_VolumeScaleDiscount_FBOID");

                entity.HasIndex(e => new { e.CustomerId, e.Fboid, e.Margin, e.MarginType })
                    .HasName("INX_VolScaleDis_MarginType");

                entity.HasIndex(e => new { e.Fboid, e.Margin, e.CustomerId, e.MarginType })
                    .HasName("INX_VolScaleDis_CustIDMarginType");

                entity.HasIndex(e => new { e.CustomerId, e.Fboid, e.Margin, e.MarginType, e.DefaultSettings })
                    .HasName("INX_VolumeScaleDiscount_DefaultSettings");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DefaultSettings).HasDefaultValueSql("((1))");

                entity.Property(e => e.DefaultSettings100Ll)
                    .HasColumnName("DefaultSettings100LL")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.JetAvolumeDiscount).HasColumnName("JetAVolumeDiscount");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Margin100Ll).HasColumnName("Margin100LL");

                entity.Property(e => e.MarginType100Ll).HasColumnName("MarginType100LL");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            });

            modelBuilder.Entity<CustomerDefaultTemplates>(entity =>
            {
                entity.HasKey(e => e.Oid);
                entity.Property(e => e.Oid).HasColumnName("OID");
                entity.Property(e => e.CustomerID).IsRequired();
                entity.Property(e => e.PricingTemplateID).IsRequired();
                entity.Property(e => e.Fboid).HasColumnName("FBOID").IsRequired();
            });

            modelBuilder.Entity<DistributionQueue>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DistributionLogId).HasColumnName("DistributionLogID");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<AdminEmails>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__AdminEma__CB394B39D11BD235");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ContactTitle)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmailBody).IsUnicode(false);

                entity.Property(e => e.FromEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Page)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ToEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerAircraftViewedByGroup>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Customer__CB394B39B45E3BF6");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerAircraftId).HasColumnName("CustomerAircraftID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<CustomerNotes>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Customer__CB394B39D4BF72CF");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CustomerNotes1)
                    .HasColumnName("CustomerNotes")
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<CustomerSchedulingSoftwareByGroup>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Customer__CB394B39245A0032");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.SchedulingSoftware)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Jobs__CB394B39213A15E5");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<NetworkNotes>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__NetworkN__CB394B390007520A");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Notes).IsUnicode(false);
            });

            modelBuilder.Entity<DefaultDiscount>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Notes).IsUnicode(false);
            });

            modelBuilder.Entity<FuelReq>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__FuelReq__CB394B39DBB2377B");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.ActualPpg).HasColumnName("ActualPPG");

                entity.Property(e => e.CustomerAircraftId).HasColumnName("CustomerAircraftID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DispatchNotes).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Eta)
                    .HasColumnName("ETA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Etd)
                    .HasColumnName("ETD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.Icao)
                    .HasColumnName("ICAO")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.QuotedPpg).HasColumnName("QuotedPPG");

                entity.Property(e => e.Source)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.TimeStandard)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReminderEmailsToFbos>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Reminder__CB394B39831AD733");

                entity.ToTable("ReminderEmailsToFBOs");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.Message).IsUnicode(false);
            });

            modelBuilder.Entity<ContractFuelRelationships>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Contract__CB394B3901F1903A");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.FuelVendorId).HasColumnName("FuelVendorID");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            });

            modelBuilder.Entity<DistributionEmailsBody>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__Distribu__CB394B393F37F287");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.BodyOfEmail).IsUnicode(false);

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.UnsubscribeLink).IsUnicode(false);
            });

            modelBuilder.Entity<FbocustomerPricing>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__FBOCusto__CB394B39F8359FAF");

                entity.ToTable("FBOCustomerPricing");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Fbologos>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__FBOLogos__CB394B392D88E74A");

                entity.ToTable("FBOLogos");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.LogoFilename).IsUnicode(false);
            });

            modelBuilder.Entity<FbosalesTax>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__FBOSales__CB394B399B085A65");

                entity.ToTable("FBOSalesTax");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.SalesTax100Lldecimal).HasColumnName("SalesTax100LLDecimal");
            });

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__PriceHis__CB394B39F6AC8701");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Amount).HasMaxLength(10);

                entity.Property(e => e.Company)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Fbo)
                    .HasColumnName("FBO")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.Icao)
                    .HasColumnName("ICAO")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostedRetail100Ll).HasColumnName("PostedRetail100LL");

                entity.Property(e => e.TemplateId).HasColumnName("TemplateID");

                entity.Property(e => e.TotalCost100Ll).HasColumnName("TotalCost100LL");

                entity.Property(e => e.ValidUntil).HasColumnType("datetime");

                entity.Property(e => e._100llmargin).HasColumnName("100LLMargin");

                entity.Property(e => e._100llmarginType).HasColumnName("100LLMarginType");
            });

            modelBuilder.Entity<RequestPricingTracker>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK__RequestP__CB394B3912DFF6F5");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Fboid).HasColumnName("FBOID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.AccessTokenId).HasColumnName("AccessTokenID");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<AccessTokens>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Expired).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.AccessTokens)
                    .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<FboFeesAndTaxes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<FboFeeAndTaxOmitsByCustomer>(entity => { entity.HasKey(e => e.Oid); });

            modelBuilder.Entity<FboFeeAndTaxOmitsByPricingTemplate>(entity => { entity.HasKey(e => e.Oid); });

            modelBuilder.Entity<AirportWatchHistoricalData>(entity =>
            {
                entity.Property(e => e.AircraftHexCode).IsUnicode(false);

                entity.Property(e => e.AircraftTypeCode).IsUnicode(false);

                entity.Property(e => e.AtcFlightNumber).IsUnicode(false);

                entity.Property(e => e.BoxName).IsUnicode(false);
            });

            modelBuilder.Entity<AirportWatchAircraftTailNumber>(entity =>
            {
                entity.Property(e => e.AircraftHexCode).IsUnicode(false);

                entity.Property(e => e.AtcFlightNumber).IsUnicode(false);
            });

            modelBuilder.Entity<AirportWatchLiveData>(entity =>
            {
                entity.Property(e => e.AircraftHexCode).IsUnicode(false);

                entity.Property(e => e.AircraftTypeCode).IsUnicode(false);

                entity.Property(e => e.AtcFlightNumber).IsUnicode(false);

                entity.Property(e => e.BoxName).IsUnicode(false);
            });

            modelBuilder.Entity<AirportWatchChangeTracker>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.DateTimeAppliedUtc).HasColumnName("DateTimeAppliedUTC");
            });

            modelBuilder.Entity<CustomerTag>(entity =>
            {
                entity.HasKey(e => e.Oid);
                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Name).IsUnicode(false);

            });
        }
    }
}
