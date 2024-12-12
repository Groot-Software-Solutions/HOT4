using Hot4.DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hot4.DataModel.Data
{
    public partial class HotDbContext : DbContext
    {
        public HotDbContext(DbContextOptions<HotDbContext> options) : base(options)
        {

        }



        public virtual DbSet<Access> Access { get; set; }

        public virtual DbSet<TblAccessWeb> AccessWeb { get; set; }

        public virtual DbSet<TblAccount> Account { get; set; }

        public virtual DbSet<TblAddress> Address { get; set; }

        public virtual DbSet<TblBank> Bank { get; set; }

        public virtual DbSet<TblBankTrx> BankTrx { get; set; }

        public virtual DbSet<TblBankTrxBatch> BankTrxBatch { get; set; }

        public virtual DbSet<TblBankTrxState> BankTrxState { get; set; }

        public virtual DbSet<TblBankTrxType> BankTrxType { get; set; }

        public virtual DbSet<TblBankvPayment> BankvPayment { get; set; }

        public virtual DbSet<TblBrand> Brand { get; set; }

        public virtual DbSet<TblBundle> Bundle { get; set; }

        public virtual DbSet<TblChannel> Channel { get; set; }

        public virtual DbSet<TblConfig> Config { get; set; }

        public virtual DbSet<TblConsoleAccess> ConsoleAccess { get; set; }

        public virtual DbSet<TblConsoleAction> ConsoleAction { get; set; }

        public virtual DbSet<TblErrorLog> ErrorLog { get; set; }

        public virtual DbSet<TblErrorLogContact> ErrorLogContact { get; set; }

        public virtual DbSet<TblErrorLogNetwork> ErrorLogNetwork { get; set; }

        public virtual DbSet<TblErrorLogSetup> ErrorLogSetup { get; set; }

        public virtual DbSet<TblErrorLogTestType> ErrorLogTestType { get; set; }

        public virtual DbSet<TblHotType> HotType { get; set; }

        public virtual DbSet<TblHotTypeCode> HotTypeCode { get; set; }

        public virtual DbSet<TblLimit> Limit { get; set; }

        public virtual DbSet<TblLimitType> LimitType { get; set; }

        public virtual DbSet<TblLog> Log { get; set; }

        public virtual DbSet<TblNetwork> Network { get; set; }

        public virtual DbSet<TblPayment> Payment { get; set; }

        public virtual DbSet<TblPaymentSource> PaymentSource { get; set; }

        public virtual DbSet<TblPaymentType> PaymentType { get; set; }

        public virtual DbSet<TblPin> Pin { get; set; }

        public virtual DbSet<TblPinBatch> PinBatch { get; set; }

        public virtual DbSet<TblPinBatchType> PinBatchType { get; set; }

        public virtual DbSet<TblPinState> PinState { get; set; }

        public virtual DbSet<TblPriority> Priority { get; set; }

        public virtual DbSet<TblProduct> Product { get; set; }

        public virtual DbSet<TblProductField> ProductField { get; set; }

        public virtual DbSet<TblProductMetaDataType> ProductMetaDataType { get; set; }

        public virtual DbSet<TblProductMetaData> ProductMetaData { get; set; }

        public virtual DbSet<TblProfile> Profile { get; set; }

        public virtual DbSet<TblProfileDiscount> ProfileDiscount { get; set; }

        public virtual DbSet<TblRechargePin> RechargePin { get; set; }
        public virtual DbSet<TblSmsRecharge> SmsRecharge { get; set; }
        public virtual DbSet<TblRecharge> Recharge { get; set; }

        public virtual DbSet<TblRechargePrepaid> RechargePrepaid { get; set; }

        public virtual DbSet<TblReservation> Reservation { get; set; }

        public virtual DbSet<TblReservationLog> ReservationLog { get; set; }

        public virtual DbSet<TblReservationState> ReservationState { get; set; }

        public virtual DbSet<TblSelfTopUp> SelfTopUp { get; set; }

        public virtual DbSet<TblSelfTopUpState> SelfTopUpState { get; set; }

        public virtual DbSet<TblSms> Sms { get; set; }

        public virtual DbSet<TblSmpp> Smpp { get; set; }

        public virtual DbSet<TblState> State { get; set; }

        public virtual DbSet<TblStockData> StockData { get; set; }

        public virtual DbSet<TblSubscriber> Subscriber { get; set; }

        public virtual DbSet<TblTemplate> Template { get; set; }

        public virtual DbSet<TblTransfer> Transfer { get; set; }

        public virtual DbSet<TblWalletType> WalletType { get; set; }

        public virtual DbSet<TblWebRequest> WebRequest { get; set; }

        public virtual DbSet<TblprofileBackup> ProfileBackup { get; set; }

        public virtual DbSet<VwAccess> VwAccess { get; set; }

        public virtual DbSet<VwAccessWeb> VwAccessWeb { get; set; }

        public virtual DbSet<VwAccount> VwAccount { get; set; }

        public virtual DbSet<VwAccountDetail> VwAccountDetail { get; set; }

        public virtual DbSet<VwAccountOld> VwAccountOld { get; set; }

        public virtual DbSet<VwAccountWeb> VwAccountWeb { get; set; }

        public virtual DbSet<VwBalance> VwBalance { get; set; }

        public virtual DbSet<VwBankTrx> VwBankTrx { get; set; }

        public virtual DbSet<VwBankTrxBatch> VwBankTrxBatch { get; set; }

        public virtual DbSet<VwBankTrxDetail> VwBankTrxDetail { get; set; }

        public virtual DbSet<VwBrand> VwBrand { get; set; }

        public virtual DbSet<VwBundle> VwBundle { get; set; }

        public virtual DbSet<VwErrorDetail> VwErrorDetail { get; set; }

        public virtual DbSet<VwErrorDetailVp> VwErrorDetailVp { get; set; }

        public virtual DbSet<VwErrorStatus> VwErrorStatus { get; set; }

        public virtual DbSet<VwPayment> VwPayment { get; set; }

        public virtual DbSet<VwPaymentDetail> VwPaymentDetail { get; set; }

        public virtual DbSet<VwPin> VwPin { get; set; }

        public virtual DbSet<VwPinBatch> VwPinBatch { get; set; }

        public virtual DbSet<VwPlatformDetail> VwPlatformDetail { get; set; }

        public virtual DbSet<VwProfileDiscount> VwProfileDiscount { get; set; }

        public virtual DbSet<VwRecharge> VwRecharge { get; set; }

        public virtual DbSet<VwRechargeListDetail> VwRechargeListDetail { get; set; }

        public virtual DbSet<VwRechargeOld> VwRechargeOld { get; set; }

        public virtual DbSet<VwRechargePrepaid> VwRechargePrepaid { get; set; }

        public virtual DbSet<VwRegistration> VwRegistration { get; set; }

        public virtual DbSet<VwReservation> VwReservation { get; set; }

        public virtual DbSet<VwSelfTopUp> VwSelfTopUp { get; set; }

        public virtual DbSet<VwSm> VwSms { get; set; }

        public virtual DbSet<VwSubscriber> VwSubscriber { get; set; }

        public virtual DbSet<VwTempRechargeDb> VwTempRechargeDb { get; set; }

        public virtual DbSet<VwTransfer> VwTransfer { get; set; }

        public virtual DbSet<VwzPayment> VwzPayment { get; set; }

        public virtual DbSet<VwzPaymentTrf> VwzPaymentTrv { get; set; }

        public virtual DbSet<VwzRecharge> VwzRecharge { get; set; }

        public virtual DbSet<VwzSm> VwzSms { get; set; }

        public virtual DbSet<ZtblArchive> ZtblArchive { get; set; }

        public virtual DbSet<ZtblStat> ZtblStat { get; set; }

        public virtual DbSet<ZvwProductlistdetail> ZvwProductlistdetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.Entity<Access>(entity =>
            {
                entity.HasKey(e => e.AccessId);

                entity.ToTable("tblAccess", tb =>
                {
                    tb.HasTrigger("trgEnsurePasswordHash");
                    tb.HasTrigger("trgOnlyOneActiveAccess_OnInsert");
                    tb.HasTrigger("trgOnlyOneActiveAccess_OnUpdate");
                    tb.HasTrigger("trgStopBlank");
                });

                entity.HasIndex(e => e.AccessCode, "IX_AccessCode");

                entity.HasIndex(e => e.AccountId, "IX_AccountID_AccessID");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccessPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.InsertDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account).WithMany(p => p.TblAccesses)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_tblAccount");

                entity.HasOne(d => d.Channel).WithMany(p => p.TblAccesses)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_tblChannel");
            });

            modelBuilder.Entity<TblAccessWeb>(entity =>
            {
                entity.HasKey(e => e.AccessId);

                entity.ToTable("tblAccessWeb");

                entity.Property(e => e.AccessId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccessID");
                entity.Property(e => e.AccessName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.ResetToken)
                    .HasMaxLength(32)
                    .HasDefaultValue("");
                entity.Property(e => e.WebBackground)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
            });

            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("tblAccount", tb => tb.HasTrigger("AccountInsert_AddLimit"));

                entity.HasIndex(e => e.ReferredBy, "IX_AccountName_RefferedBy");

                entity.HasIndex(e => new { e.AccountName, e.Email }, "IX_Name_and_Email");

                entity.HasIndex(e => e.ProfileId, "IX_ProfileId");

                entity.HasIndex(e => new { e.AccountId, e.AccountName, e.NationalId, e.Email, e.ReferredBy }, "IX_Search");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.InsertDate).HasColumnType("smalldatetime");
                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ReferredBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Profile).WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccount_tblProfile");
            });

            modelBuilder.Entity<TblAddress>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("tblAddress");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");
                entity.Property(e => e.Address1)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ContactName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SageId).HasColumnName("SageID");
                entity.Property(e => e.SageIdusd).HasColumnName("SageIDUsd");
                entity.Property(e => e.VatNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account).WithOne(p => p.TblAddress)
                    .HasForeignKey<TblAddress>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAddress_tblAccount1");
            });

            modelBuilder.Entity<TblBank>(entity =>
            {
                entity.HasKey(e => e.BankId);

                entity.ToTable("tblBank");

                entity.Property(e => e.BankId).HasColumnName("BankID");
                entity.Property(e => e.Bank)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SageBankId).HasColumnName("SageBankID");
            });

            modelBuilder.Entity<TblBankTrx>(entity =>
            {
                entity.HasKey(e => e.BankTrxId);

                entity.ToTable("tblBankTrx");

                entity.HasIndex(e => new { e.Amount, e.Identifier, e.BankRef }, "IX_BankTrx_Search");

                entity.HasIndex(e => new { e.BankTrxBatchId, e.Identifier, e.TrxDate, e.BankRef, e.RefName, e.Amount, e.Balance }, "IX_tblBankTrx").IsUnique();

                entity.Property(e => e.BankTrxId).HasColumnName("BankTrxID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.BankRef)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.BankTrxBatchId).HasColumnName("BankTrxBatchID");
                entity.Property(e => e.BankTrxStateId).HasColumnName("BankTrxStateID");
                entity.Property(e => e.BankTrxTypeId).HasColumnName("BankTrxTypeID");
                entity.Property(e => e.Branch)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Identifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.RefName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.TrxDate).HasColumnType("datetime");

                entity.HasOne(d => d.BankTrxBatch).WithMany(p => p.TblBankTrxes)
                    .HasForeignKey(d => d.BankTrxBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrx_tblBankTrxBatch");

                entity.HasOne(d => d.BankTrxState).WithMany(p => p.TblBankTrxes)
                    .HasForeignKey(d => d.BankTrxStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrx_tblBankTrxState");

                entity.HasOne(d => d.BankTrxType).WithMany(p => p.TblBankTrxes)
                    .HasForeignKey(d => d.BankTrxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrx_tblBankTrxType");

                entity.HasOne(d => d.Payment).WithMany(p => p.TblBankTrxes)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_tblBankTrx_tblPayment");
            });

            modelBuilder.Entity<TblBankTrxBatch>(entity =>
            {
                entity.HasKey(e => e.BankTrxBatchId);

                entity.ToTable("tblBankTrxBatch");

                entity.Property(e => e.BankTrxBatchId).HasColumnName("BankTrxBatchID");
                entity.Property(e => e.BankId).HasColumnName("BankID");
                entity.Property(e => e.BatchDate).HasColumnType("datetime");
                entity.Property(e => e.BatchReference)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.LastUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Bank).WithMany(p => p.TblBankTrxBatches)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrxBatch_tblBank");
            });

            modelBuilder.Entity<TblBankTrxState>(entity =>
            {
                entity.HasKey(e => e.BankTrxStateId);

                entity.ToTable("tblBankTrxState");

                entity.Property(e => e.BankTrxStateId).HasColumnName("BankTrxStateID");
                entity.Property(e => e.BankTrxState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBankTrxType>(entity =>
            {
                entity.HasKey(e => e.BankTrxTypeId);

                entity.ToTable("tblBankTrxType");

                entity.Property(e => e.BankTrxTypeId).HasColumnName("BankTrxTypeID");
                entity.Property(e => e.BankTrxType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBankvPayment>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblBankvPayment");

                entity.Property(e => e.BankTrxId).HasColumnName("BankTrxID");
                entity.Property(e => e.CheckUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CheckURL");
                entity.Property(e => e.ErrorMsg)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.ProcessUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ProcessURL");
                entity.Property(e => e.VPaymentId).HasColumnName("vPaymentID");
                entity.Property(e => e.VPaymentRef)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vPaymentRef");
            });

            modelBuilder.Entity<TblBrand>(entity =>
            {
                entity.HasKey(e => e.BrandId);

                entity.ToTable("tblBrand");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");

                entity.HasOne(d => d.Network).WithMany(p => p.TblBrands)
                    .HasForeignKey(d => d.NetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrand_tblNetwork");
            });

            modelBuilder.Entity<TblBundle>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblBundles");

                entity.Property(e => e.Amount).HasComment("Bundle Value in cents");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BundleId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BundleID");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.Enabled).HasDefaultValue(true);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasDefaultValue("");
                entity.Property(e => e.ProductCode).HasMaxLength(10);
                entity.Property(e => e.ValidityPeriod).HasComment("Validity Period in Days");
            });

            modelBuilder.Entity<TblChannel>(entity =>
            {
                entity.HasKey(e => e.ChannelId);

                entity.ToTable("tblChannel");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.Channel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblConfig>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.ToTable("tblConfig");

                entity.Property(e => e.ConfigId).HasColumnName("ConfigID");
                entity.Property(e => e.MaxRecharge).HasColumnType("money");
                entity.Property(e => e.MinRecharge).HasColumnType("money");
                entity.Property(e => e.MinTransfer).HasColumnType("money");
                entity.Property(e => e.ProfileIdNewSmsdealer).HasColumnName("ProfileID_NewSMSDealer");
                entity.Property(e => e.ProfileIdNewWebDealer).HasColumnName("ProfileID_NewWebDealer");
            });

            modelBuilder.Entity<TblConsoleAccess>(entity =>
            {
                entity.HasKey(e => new { e.RoleName, e.ConsoleActionId });

                entity.ToTable("tblConsoleAccess");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.HasOne(d => d.ConsoleAction).WithMany(p => p.TblConsoleAccesses)
                    .HasForeignKey(d => d.ConsoleActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConsoleAccess_tblConsoleAction");
            });

            modelBuilder.Entity<TblConsoleAction>(entity =>
            {
                entity.ToTable("tblConsoleAction");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.ActionName).HasMaxLength(255);
            });

            modelBuilder.Entity<TblErrorLog>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblErrorLog");

                entity.HasIndex(e => new { e.LogDate, e.CheckId }, "Error LogDate and CheckID").IsDescending(true, false);

                entity.HasIndex(e => e.CheckId, "IX_CheckID_ErrorID");

                entity.Property(e => e.CheckId).HasColumnName("CheckID");
                entity.Property(e => e.ErrorCount).HasComment("0");
                entity.Property(e => e.ErrorData)
                    .HasDefaultValue("")
                    .HasColumnType("text");
                entity.Property(e => e.ErrorId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ErrorID");
                entity.Property(e => e.LogDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TblErrorLogContact>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblErrorLogContacts");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.ContactMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.ErrorLogtypeId).HasColumnName("ErrorLogtypeID");
            });

            modelBuilder.Entity<TblErrorLogNetwork>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblErrorLogNetworks");

                entity.Property(e => e.ErrorLogNetworkId).HasColumnName("ErrorLogNetworkID");
                entity.Property(e => e.HotBrandId).HasColumnName("HotBrandID");
                entity.Property(e => e.NetworkName)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblErrorLogSetup>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblErrorLogSetup");

                entity.Property(e => e.CheckInterval).HasDefaultValue(30000);
                entity.Property(e => e.CountThreshold).HasDefaultValue(2);
                entity.Property(e => e.Enabled).HasDefaultValue(true);
                entity.Property(e => e.ErrorInterval).HasDefaultValue(10000);
                entity.Property(e => e.ErrorLogCheckId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ErrorLogCheckID");
                entity.Property(e => e.ExpectedContent)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.HostAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Port)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Server)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<TblErrorLogTestType>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblErrorLogTestType");

                entity.Property(e => e.TestTypeId).HasColumnName("TestTypeID");
                entity.Property(e => e.TestTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblHotType>(entity =>
            {
                entity.HasKey(e => e.HotTypeId);

                entity.ToTable("tblHotType");

                entity.Property(e => e.HotTypeId).HasColumnName("HotTypeID");
                entity.Property(e => e.HotType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblHotTypeCode>(entity =>
            {
                entity.HasKey(e => e.HotTypeCodeId);

                entity.ToTable("tblHotTypeCode");

                entity.Property(e => e.HotTypeCodeId).HasColumnName("HotTypeCodeID");
                entity.Property(e => e.HotTypeId).HasColumnName("HotTypeID");
                entity.Property(e => e.TypeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.HotType).WithMany(p => p.TblHotTypeCodes)
                    .HasForeignKey(d => d.HotTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHotTypeCode_tblHotType");
            });

            modelBuilder.Entity<TblLimit>(entity =>
            {
                entity.HasKey(e => e.LimitId);

                entity.ToTable("tblLimits");

                entity.HasIndex(e => new { e.AccountId, e.NetworkId }, "IX_Limits_Select");

                entity.Property(e => e.LimitTypeId).HasDefaultValue(1);
            });

            modelBuilder.Entity<TblLimitType>(entity =>
            {
                entity.HasKey(e => e.LimitTypeId);

                entity.ToTable("tblLimitTypes");

                entity.Property(e => e.LimitTypeName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("tblLog");

                entity.HasIndex(e => new { e.LogDate, e.LogMethod }, "IX_Date_Method");

                entity.Property(e => e.LogId).HasColumnName("LogID");
                entity.Property(e => e.Idnumber).HasColumnName("IDNumber");
                entity.Property(e => e.Idtype)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("IDType");
                entity.Property(e => e.LogDate).HasColumnType("datetime");
                entity.Property(e => e.LogDescription).IsUnicode(false);
                entity.Property(e => e.LogMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LogModule)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LogObject)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblNetwork>(entity =>
            {
                entity.HasKey(e => e.NetworkId);

                entity.ToTable("tblNetwork");

                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPayment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.ToTable("tblPayment", tb => tb.HasTrigger("EmailPaymentInsert"));

                entity.HasIndex(e => new { e.AccountId, e.PaymentTypeId, e.PaymentDate }, "IX_Balances").IsDescending(false, false, true);

                entity.HasIndex(e => e.PaymentTypeId, "IX_Balances_Extra");

                entity.HasIndex(e => new { e.AccountId, e.PaymentId }, "IX_PaymentAccount");

                entity.HasIndex(e => e.PaymentDate, "IX_PaymentDate");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.LastUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentSourceId).HasColumnName("PaymentSourceID");
                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account).WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblAccount");

                entity.HasOne(d => d.PaymentSource).WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.PaymentSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblPaymentSource");

                entity.HasOne(d => d.PaymentType).WithMany(p => p.TblPayments)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblPaymentType");
            });

            modelBuilder.Entity<TblPaymentSource>(entity =>
            {
                entity.HasKey(e => e.PaymentSourceId);

                entity.ToTable("tblPaymentSource");

                entity.Property(e => e.PaymentSourceId).HasColumnName("PaymentSourceID");
                entity.Property(e => e.PaymentSource)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentTypeId);

                entity.ToTable("tblPaymentType");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPin>(entity =>
            {
                entity.HasKey(e => e.PinId);

                entity.ToTable("tblPin");

                entity.HasIndex(e => new { e.Pin, e.BrandId }, "IX_tblPin").IsUnique();

                entity.Property(e => e.PinId).HasColumnName("PinID");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.Pin)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchId).HasColumnName("PinBatchID");
                entity.Property(e => e.PinExpiry).HasColumnType("datetime");
                entity.Property(e => e.PinRef)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinStateId).HasColumnName("PinStateID");
                entity.Property(e => e.PinValue).HasColumnType("money");

                entity.HasOne(d => d.Brand).WithMany(p => p.TblPins)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPin_tblBrand");

                entity.HasOne(d => d.PinBatch).WithMany(p => p.TblPins)
                    .HasForeignKey(d => d.PinBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPin_tblPinBatch");

                entity.HasOne(d => d.PinState).WithMany(p => p.TblPins)
                    .HasForeignKey(d => d.PinStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPin_tblPinState");
            });

            modelBuilder.Entity<TblPinBatch>(entity =>
            {
                entity.HasKey(e => e.PinBatchId);

                entity.ToTable("tblPinBatch");

                entity.Property(e => e.PinBatchId).HasColumnName("PinBatchID");
                entity.Property(e => e.BatchDate).HasColumnType("datetime");
                entity.Property(e => e.PinBatch)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchTypeId).HasColumnName("PinBatchTypeID");

                entity.HasOne(d => d.PinBatchType).WithMany(p => p.TblPinBatches)
                    .HasForeignKey(d => d.PinBatchTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPinBatch_tblPinBatchType");
            });

            modelBuilder.Entity<TblPinBatchType>(entity =>
            {
                entity.HasKey(e => e.PinBatchTypeId);

                entity.ToTable("tblPinBatchType");

                entity.Property(e => e.PinBatchTypeId).HasColumnName("PinBatchTypeID");
                entity.Property(e => e.PinBatchType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPinState>(entity =>
            {
                entity.HasKey(e => e.PinStateId);

                entity.ToTable("tblPinState");

                entity.Property(e => e.PinStateId).HasColumnName("PinStateID");
                entity.Property(e => e.PinState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPriority>(entity =>
            {
                entity.HasKey(e => e.PriorityId);

                entity.ToTable("tblPriority");

                entity.HasIndex(e => new { e.PriorityId, e.Priority }, "IX_Priority");

                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");
                entity.Property(e => e.Priority)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("tblProduct");

                entity.Property(e => e.BrandId).HasDefaultValue((byte)1);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductStateId).HasDefaultValue(1);

                entity.HasOne(d => d.Brand).WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduct_tblBrand");

                entity.HasOne(d => d.WalletType).WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.WalletTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduct_tblWalletType");
            });

            modelBuilder.Entity<TblProductField>(entity =>
            {
                entity.HasKey(e => e.BrandFieldId).HasName("PK_tblBrandField");

                entity.ToTable("tblProductField");

                entity.Property(e => e.DataType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.FieldName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product).WithMany(p => p.TblProductFields)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrandField_tblBrand");
            });

            modelBuilder.Entity<TblProductMetaDataType>(entity =>
            {
                entity.HasKey(e => e.ProductMetaDataTypeId).HasName("PK_tblBrandMetaDataType");

                entity.ToTable("tblProductMetaDataType");

                entity.Property(e => e.ProductMetaDataTypeId).ValueGeneratedOnAdd();
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblProductMetaData>(entity =>
            {
                entity.HasKey(e => e.ProductMetaId).HasName("PK_tblBrandMetaData");

                entity.ToTable("tblProductMetaData");

                entity.Property(e => e.Data)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product).WithMany(p => p.TblProductMetaData)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrandMetaData_tblBrand");

                entity.HasOne(d => d.ProductMetaDataType).WithMany(p => p.TblProductMetaData)
                    .HasForeignKey(d => d.ProductMetaDataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrandMetaData_tblBrandMetaDataType");
            });

            modelBuilder.Entity<TblProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);

                entity.ToTable("tblProfile");

                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblProfileDiscount>(entity =>
            {
                entity.HasKey(e => e.ProfileDiscountId);

                entity.ToTable("tblProfileDiscount");

                entity.Property(e => e.ProfileDiscountId).HasColumnName("ProfileDiscountID");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

                entity.HasOne(d => d.Brand).WithMany(p => p.TblProfileDiscounts)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProfileDiscount_tblBrand");

                entity.HasOne(d => d.Profile).WithMany(p => p.TblProfileDiscounts)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProfileDiscount_tblProfile");
            });


            modelBuilder.Entity<TblRecharge>(entity =>
        {
            entity.HasKey(e => e.RechargeId);

            entity.ToTable("tblRecharge");

            entity.HasIndex(e => new { e.AccessId, e.StateId, e.BrandId, e.RechargeDate }, "IX_Balances").IsDescending(false, false, false, true);

            entity.HasIndex(e => new { e.StateId, e.BrandId }, "IX_Balances_Extra");

            entity.HasIndex(e => new { e.RechargeDate, e.Mobile }, "IX_Mobile").IsDescending(true, false);

            entity.HasIndex(e => e.AccessId, "IX_RechargeAccount");

            entity.HasIndex(e => e.RechargeDate, "IX_RechargeDate");

            entity.HasIndex(e => new { e.RechargeDate, e.StateId }, "IX_RechargeDateState").IsDescending(true, false);

            entity.HasIndex(e => new { e.RechargeId, e.StateId, e.BrandId }, "IX_Search").IsDescending(true, false, false);

            entity.HasIndex(e => new { e.StateId, e.AccessId }, "IX_StateID");

            entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
            entity.Property(e => e.AccessId).HasColumnName("AccessID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RechargeDate).HasColumnType("datetime");
            entity.Property(e => e.StateId).HasColumnName("StateID");

            entity.HasOne(d => d.Access).WithMany(p => p.TblRecharges)
                .HasForeignKey(d => d.AccessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblRecharge_tblAccess");

            entity.HasOne(d => d.Brand).WithMany(p => p.TblRecharges)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblRecharge_tblBrand");

            entity.HasOne(d => d.State).WithMany(p => p.TblRecharges)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblRecharge_tblState");



            //entity.HasMany(d => d.Pins).WithMany(p => p.Recharges)
            //    .UsingEntity<TblRechargePin>(

            //        r => r.HasOne<TblPin>().WithMany()
            //            .HasForeignKey("PinId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_tblRechargePin_tblPin"),
            //        l => l.HasOne<TblRecharge>().WithMany()
            //            .HasForeignKey("RechargeId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_tblRechargePin_tblRecharge"));

            //entity.HasMany(d => d.Sms).WithMany(p => p.Recharges)
            //    .UsingEntity<TblSmsRecharge>(

            //        r => r.HasOne<TblSm>().WithMany()
            //            .HasForeignKey("Smsid")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_tblSMSRecharge_tblSMS"),
            //        l => l.HasOne<TblRecharge>().WithMany()
            //            .HasForeignKey("RechargeId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_tblSMSRecharge_tblRecharge"));
        });

            modelBuilder.Entity<TblRechargePrepaid>(entity =>
            {
                entity.HasKey(e => e.RechargeId).HasName("PK_tblRechargePrepaid_1");

                entity.ToTable("tblRechargePrepaid");

                entity.Property(e => e.RechargeId)
                    .ValueGeneratedNever()
                    .HasColumnName("RechargeID");
                entity.Property(e => e.Data)
                    .HasMaxLength(10)
                    .IsFixedLength();
                entity.Property(e => e.FinalBalance).HasColumnType("money");
                entity.Property(e => e.FinalWallet).HasColumnType("money");
                entity.Property(e => e.InitialBalance).HasColumnType("money");
                entity.Property(e => e.InitialWallet).HasColumnType("money");
                entity.Property(e => e.Narrative)
                    .HasMaxLength(2500)
                    .IsUnicode(false);
                entity.Property(e => e.Reference)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ReturnCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Sms)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .HasColumnName("SMS");
                entity.Property(e => e.Window).HasColumnType("datetime");

                entity.HasOne(d => d.Recharge).WithOne(p => p.TblRechargePrepaid)
                    .HasForeignKey<TblRechargePrepaid>(d => d.RechargeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRechargePrepaid_tblRecharge");
            });

            modelBuilder.Entity<TblReservation>(entity =>
            {
                entity.HasKey(e => e.ReservationId).HasName("PK_tblreservation");

                entity.ToTable("tblReservation", tb =>
                {
                    tb.HasTrigger("LogNewReservation");
                    tb.HasTrigger("LogReservationUpdates");
                });

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.ConfirmationData)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Currency).HasDefaultValue(1);
                entity.Property(e => e.InsertDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.NotificationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.TargetNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Access).WithMany(p => p.TblReservations)
                    .HasForeignKey(d => d.AccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblreservation_tblAccess");

                entity.HasOne(d => d.Brand).WithMany(p => p.TblReservations)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReservationToBrand");

                entity.HasOne(d => d.Recharge).WithMany(p => p.TblReservations)
                    .HasForeignKey(d => d.RechargeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblreservation_tblRecharge");

                entity.HasOne(d => d.State).WithMany(p => p.TblReservations)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReservation_tblReservationState");
            });

            modelBuilder.Entity<TblReservationLog>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblReservationLog");

                entity.Property(e => e.LastUser)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(suser_sname())");
                entity.Property(e => e.LogDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ReservationLogId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.NewState).WithMany()
                    .HasForeignKey(d => d.NewStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReservationLog_tblReservationState1");

                entity.HasOne(d => d.OldState).WithMany()
                    .HasForeignKey(d => d.OldStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReservationLog_tblReservationState");

                entity.HasOne(d => d.Reservation).WithMany()
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation");
            });

            modelBuilder.Entity<TblReservationState>(entity =>
            {
                entity.HasKey(e => e.ReservationStateId);

                entity.ToTable("tblReservationState");

                entity.Property(e => e.ReservationState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblSelfTopUp>(entity =>
            {
                entity.HasKey(e => e.SelfTopUpId);

                entity.ToTable("tblSelfTopUp");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BankTrxId).HasColumnName("BankTrxID");
                entity.Property(e => e.BillerNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Currency).HasDefaultValue(1);
                entity.Property(e => e.InsertDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.NotificationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.TargetNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Access).WithMany(p => p.TblSelfTopUps)
                    .HasForeignKey(d => d.AccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSelfTopUp_tblAccess");

                entity.HasOne(d => d.BankTrx).WithMany(p => p.TblSelfTopUps)
                    .HasForeignKey(d => d.BankTrxId)
                    .HasConstraintName("FK_tblSelfTopUp_tblBankTrx");

                entity.HasOne(d => d.Brand).WithMany(p => p.TblSelfTopUps)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblSelfTo__Brand__5CACADF9");

                entity.HasOne(d => d.Recharge).WithMany(p => p.TblSelfTopUps)
                    .HasForeignKey(d => d.RechargeId)
                    .HasConstraintName("FK_tblSelfTopUp_tblRecharge");
            });

            modelBuilder.Entity<TblSelfTopUpState>(entity =>
            {
                entity.HasKey(e => e.SelfTopUpStateId);

                entity.ToTable("tblSelfTopUpState");

                entity.Property(e => e.SelfTopUpStateId).HasColumnName("SelfTopUpStateID");
                entity.Property(e => e.SelfTopUpStateName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblSms>(entity =>
            {
                entity.HasKey(e => e.Smsid);

                entity.ToTable("tblSMS");

                entity.HasIndex(e => e.Smsdate, "IX_SMSDate");

                entity.HasIndex(e => e.Mobile, "IX_SMSMobile");

                entity.HasIndex(e => new { e.StateId, e.Direction }, "IX_SMS_Inbox");

                entity.HasIndex(e => new { e.Smsdate, e.StateId, e.Mobile, e.Smstext, e.PriorityId, e.Smsid }, "IX_SMS_Search");

                entity.HasIndex(e => e.SmsidIn, "SmsOut");

                entity.Property(e => e.Smsid).HasColumnName("SMSID");
                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");
                entity.Property(e => e.SmppId).HasColumnName("SmppID");
                entity.Property(e => e.Smsdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SMSDate");
                entity.Property(e => e.SmsidIn).HasColumnName("SMSID_In");
                entity.Property(e => e.Smstext)
                    .HasMaxLength(640)
                    .IsUnicode(false)
                    .HasColumnName("SMSText");
                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.Priority).WithMany(p => p.TblSms)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSMS_tblPriority");

                entity.HasOne(d => d.Smpp).WithMany(p => p.TblSms)
                    .HasForeignKey(d => d.SmppId)
                    .HasConstraintName("FK_tblSMS_tblSmpp");

                entity.HasOne(d => d.SmsidInNavigation).WithMany(p => p.InverseSmsidInNavigation)
                    .HasForeignKey(d => d.SmsidIn)
                    .HasConstraintName("FK_tblSMS_tblSMS");

                entity.HasOne(d => d.State).WithMany(p => p.TblSms)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSMS_tblState");
            });

            modelBuilder.Entity<TblSmpp>(entity =>
            {
                entity.HasKey(e => e.SmppId);

                entity.ToTable("tblSmpp");

                entity.Property(e => e.SmppId).HasColumnName("SmppID");
                entity.Property(e => e.AddressRange)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.EconetPrefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetOnePrefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RemoteHost)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SmppName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SmppPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SourceAddress)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.SystemId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SystemID");
                entity.Property(e => e.SystemType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TelecelPrefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRechargePin>(entity =>
            {
                entity.ToTable("tblRechargePin");
                // entity.HasKey(e => new { e.PinId, e.RechargeId });
            });
            modelBuilder.Entity<TblSmsRecharge>(entity =>
            {
                entity.ToTable("tblSmsRecharge");
                //  entity.HasKey(e => new { e.SmsId, e.RechargeId });
            });
            modelBuilder.Entity<TblState>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.ToTable("tblState");

                entity.Property(e => e.StateId).HasColumnName("StateID");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblStockData>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblStockData");

                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastSold).HasColumnType("datetime");
                entity.Property(e => e.MonthSold).HasColumnType("money");
                entity.Property(e => e.PinValue).HasColumnType("money");
                entity.Property(e => e.Sold).HasColumnType("money");
                entity.Property(e => e.WeekSold).HasColumnType("money");
            });

            modelBuilder.Entity<TblSubscriber>(entity =>
            {
                entity.HasKey(e => e.SubscriberId);

                entity.ToTable("tblSubscriber");

                entity.HasIndex(e => e.SubscriberMobile, "IX_SubMobile");

                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.DefaultAmount).HasColumnType("decimal(18, 6)");
                entity.Property(e => e.DefaultProductId).HasColumnName("DefaultProductID");
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.NotifyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SubscriberGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.SubscriberMobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SubscriberName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account).WithMany(p => p.TblSubscribers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubscriber_tblAccount");

                entity.HasOne(d => d.Brand).WithMany(p => p.TblSubscribers)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubscriber_tblBrand");
            });

            modelBuilder.Entity<TblTemplate>(entity =>
            {
                entity.HasKey(e => e.TemplateId);

                entity.ToTable("tblTemplate");

                entity.Property(e => e.TemplateId)
                    .ValueGeneratedNever()
                    .HasColumnName("TemplateID");
                entity.Property(e => e.TemplateName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TemplateText)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTransfer>(entity =>
            {
                entity.HasKey(e => e.TransferId);

                entity.ToTable("tblTransfer");

                entity.Property(e => e.TransferId).HasColumnName("TransferID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.PaymentIdFrom).HasColumnName("PaymentID_From");
                entity.Property(e => e.PaymentIdTo).HasColumnName("PaymentID_To");
                entity.Property(e => e.Smsid).HasColumnName("SMSID");
                entity.Property(e => e.TransferDate).HasColumnType("datetime");

                entity.HasOne(d => d.Channel).WithMany(p => p.TblTransfers)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTransfer_tblChannel");

                entity.HasOne(d => d.PaymentIdFromNavigation).WithMany(p => p.TblTransferPaymentIdFromNavigations)
                    .HasForeignKey(d => d.PaymentIdFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTransfer_tblPayment");

                entity.HasOne(d => d.PaymentIdToNavigation).WithMany(p => p.TblTransferPaymentIdToNavigations)
                    .HasForeignKey(d => d.PaymentIdTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTransfer_tblPayment1");
            });

            modelBuilder.Entity<TblWalletType>(entity =>
            {
                entity.HasKey(e => e.WalletTypeId);

                entity.ToTable("tblWalletType");

                entity.Property(e => e.WalletTypeId).ValueGeneratedNever();
                entity.Property(e => e.WalletName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblWebRequest>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblWebRequest");

                entity.HasIndex(e => new { e.WebId, e.StateId }, "IX-StateID").IsUnique();

                entity.HasIndex(e => new { e.AccessId, e.AgentReference, e.WebId, e.RechargeId }, "IX_AccessID_AgentRef");

                entity.HasIndex(e => new { e.AgentReference, e.RechargeId }, "IX_Recharge_AgentRef").IsClustered();

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.AgentReference)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.Cost).HasColumnType("money");
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.HotTypeId).HasColumnName("HotTypeID");
                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.Reply).IsUnicode(false);
                entity.Property(e => e.ReplyDate).HasColumnType("datetime");
                entity.Property(e => e.StateId).HasColumnName("StateID");
                entity.Property(e => e.WalletBalance).HasColumnType("money");
                entity.Property(e => e.WebId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("WebID");
            });

            modelBuilder.Entity<TblprofileBackup>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblprofile_backup");

                entity.Property(e => e.ProfileId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ProfileID");
                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwAccess>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwAccess");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.AccessPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Channel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PasswordSalt)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwAccessWeb>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwAccess_Web");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.AccessName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccessPassword)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.WebBackground)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwAccount>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwAccount");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReferredBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.SaleValue).HasColumnType("money");
                entity.Property(e => e.Usdbalance)
                    .HasColumnType("money")
                    .HasColumnName("USDBalance");
                entity.Property(e => e.UsdutilityBalance)
                    .HasColumnType("money")
                    .HasColumnName("USDUtilityBalance");
                entity.Property(e => e.Zesabalance)
                    .HasColumnType("money")
                    .HasColumnName("ZESABalance");
            });

            modelBuilder.Entity<VwAccountDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwAccountDetail");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ReferredBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwAccountOld>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwAccount_Old");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ReferredBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.SaleValue).HasColumnType("money");
                entity.Property(e => e.Zesabalance)
                    .HasColumnType("money")
                    .HasColumnName("ZESABalance");
            });

            modelBuilder.Entity<VwAccountWeb>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwAccount_Web");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Address1)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ContactName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SaleValue).HasColumnType("money");
            });

            modelBuilder.Entity<VwBalance>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwBalances");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.SaleValue).HasColumnType("money");
                entity.Property(e => e.Usdbalance)
                    .HasColumnType("money")
                    .HasColumnName("USDBalance");
                entity.Property(e => e.UsdutilityBalance)
                    .HasColumnType("money")
                    .HasColumnName("USDUtilityBalance");
                entity.Property(e => e.Zesabalance)
                    .HasColumnType("money")
                    .HasColumnName("ZESABalance");
            });

            modelBuilder.Entity<VwBankTrx>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwBankTrx");

                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.BankRef)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BankTrxBatchId).HasColumnName("BankTrxBatchID");
                entity.Property(e => e.BankTrxId).HasColumnName("BankTrxID");
                entity.Property(e => e.BankTrxState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BankTrxStateId).HasColumnName("BankTrxStateID");
                entity.Property(e => e.BankTrxType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BankTrxTypeId).HasColumnName("BankTrxTypeID");
                entity.Property(e => e.Branch)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Identifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.RefName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TrxDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwBankTrxBatch>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwBankTrxBatch");

                entity.Property(e => e.Bank)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BankId).HasColumnName("BankID");
                entity.Property(e => e.BankTrxBatchId).HasColumnName("BankTrxBatchID");
                entity.Property(e => e.BatchDate).HasColumnType("datetime");
                entity.Property(e => e.BatchReference)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.LastUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwBankTrxDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwBankTrxDetail");

                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Balance).HasColumnType("money");
                entity.Property(e => e.Bank)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BankId).HasColumnName("BankID");
                entity.Property(e => e.BankRef)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BankTrxId).HasColumnName("BankTrxID");
                entity.Property(e => e.Identifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.RefName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TrxDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwBrand>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwBrand");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwBundle>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwBundles");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BundleId).HasColumnName("BundleID");
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductCode).HasMaxLength(10);
            });

            modelBuilder.Entity<VwErrorDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwErrorDetail");

                entity.Property(e => e.CheckId).HasColumnName("CheckID");
                entity.Property(e => e.ErrorData).HasColumnType("text");
                entity.Property(e => e.ErrorId).HasColumnName("ErrorID");
                entity.Property(e => e.ExpectedContent)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.HostAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LogDate).HasColumnType("datetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Port)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Server)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.TestTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<VwErrorDetailVp>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwErrorDetail_VPS");

                entity.Property(e => e.CheckId).HasColumnName("CheckID");
                entity.Property(e => e.ErrorData)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                    .HasColumnType("text");
                entity.Property(e => e.ErrorId).HasColumnName("ErrorID");
                entity.Property(e => e.ExpectedContent)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                entity.Property(e => e.HostAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                entity.Property(e => e.LogDate).HasColumnType("datetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                entity.Property(e => e.Port)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                entity.Property(e => e.Server)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                entity.Property(e => e.TestTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<VwErrorStatus>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwErrorStatus");

                entity.Property(e => e.CheckId).HasColumnName("CheckID");
                entity.Property(e => e.ErrorData).HasColumnType("text");
                entity.Property(e => e.ErrorId).HasColumnName("ErrorID");
                entity.Property(e => e.ErrorLogCheckId).HasColumnName("ErrorLogCheckID");
                entity.Property(e => e.LogDate).HasColumnType("datetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Server)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwPayment>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwPayment");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.LastUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.PaymentSource)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentSourceId).HasColumnName("PaymentSourceID");
                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwPaymentDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwPaymentDetail");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.PaymentSource)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Reference)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwPin>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwPin");

                entity.Property(e => e.BatchDate).HasColumnType("datetime");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Pin)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatch)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchId).HasColumnName("PinBatchID");
                entity.Property(e => e.PinBatchType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchTypeId).HasColumnName("PinBatchTypeID");
                entity.Property(e => e.PinExpiry).HasColumnType("datetime");
                entity.Property(e => e.PinId).HasColumnName("PinID");
                entity.Property(e => e.PinNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinRef)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinStateId).HasColumnName("PinStateID");
                entity.Property(e => e.PinValue).HasColumnType("money");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwPinBatch>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwPinBatch");

                entity.Property(e => e.BatchDate).HasColumnType("datetime");
                entity.Property(e => e.PinBatch)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchId).HasColumnName("PinBatchID");
                entity.Property(e => e.PinBatchType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchTypeId).HasColumnName("PinBatchTypeID");
            });

            modelBuilder.Entity<VwPlatformDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwPlatformDetail");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.FinalBalance).HasColumnType("money");
                entity.Property(e => e.InitialBalance).HasColumnType("money");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Narrative)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeDate).HasColumnType("datetime");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.ReturnCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwProfileDiscount>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwProfileDiscount");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProfileDiscountId).HasColumnName("ProfileDiscountID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            });

            modelBuilder.Entity<VwRecharge>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwRecharge");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeDate).HasColumnType("datetime");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwRechargeListDetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwRechargeListDetail");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.FinalBalance).HasColumnType("money");
                entity.Property(e => e.InitialBalance).HasColumnType("money");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Narrative)
                    .HasMaxLength(2500)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeDate).HasColumnType("datetime");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ReturnCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwRechargeOld>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwRechargeOld");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeDate).HasColumnType("datetime");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwRechargePrepaid>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwRechargePrepaid");

                entity.Property(e => e.Data)
                    .HasMaxLength(10)
                    .IsFixedLength();
                entity.Property(e => e.FinalBalance).HasColumnType("money");
                entity.Property(e => e.FinalWallet).HasColumnType("money");
                entity.Property(e => e.InitialBalance).HasColumnType("money");
                entity.Property(e => e.InitialWallet).HasColumnType("money");
                entity.Property(e => e.Narrative)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.ReturnCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Sms)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .HasColumnName("SMS");
                entity.Property(e => e.Window).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwRegistration>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwRegistration");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.InsertDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NationalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NationalID");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ReferredBy)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Smsdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SMSDate");
                entity.Property(e => e.Smstext)
                    .HasMaxLength(160)
                    .IsUnicode(false)
                    .HasColumnName("SMSText");
            });

            modelBuilder.Entity<VwReservation>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwReservation");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.ConfirmationData)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.NotificationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.ReservationState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TargetNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwSelfTopUp>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwSelfTopUp");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BankTrxId).HasColumnName("BankTrxID");
                entity.Property(e => e.BillerNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.NotificationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.SelfTopUpStateName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TargetNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwSm>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwSMS");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
                entity.Property(e => e.Priority)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");
                entity.Property(e => e.SmppId).HasColumnName("SmppID");
                entity.Property(e => e.Smsdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SMSDate");
                entity.Property(e => e.Smsid).HasColumnName("SMSID");
                entity.Property(e => e.SmsidIn).HasColumnName("SMSID_In");
                entity.Property(e => e.Smstext)
                    .HasMaxLength(160)
                    .IsUnicode(false)
                    .HasColumnName("SMSText");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwSubscriber>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwSubscriber");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SubscriberId).HasColumnName("SubscriberID");
                entity.Property(e => e.SubscriberMobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SubscriberName)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwTempRechargeDb>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwTempRechargeDB");

                entity.Property(e => e.NRecharge).HasColumnName("nRecharge");
                entity.Property(e => e.NStateId).HasColumnName("nStateID");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwTransfer>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwTransfer");

                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.Channel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.PaymentIdFrom).HasColumnName("PaymentID_From");
                entity.Property(e => e.PaymentIdTo).HasColumnName("PaymentID_To");
                entity.Property(e => e.Smsid).HasColumnName("SMSID");
                entity.Property(e => e.TransferDate).HasColumnType("datetime");
                entity.Property(e => e.TransferId).HasColumnName("TransferID");
            });

            modelBuilder.Entity<VwzPayment>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwzPayment");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.LastUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.PaymentSource)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentSourceId).HasColumnName("PaymentSourceID");
                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwzPaymentTrf>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwzPaymentTrf");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.AccountNameTo)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.From)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("From#");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
                entity.Property(e => e.Reference)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.To)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("To#");
            });

            modelBuilder.Entity<VwzRecharge>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwzRecharge");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");
                entity.Property(e => e.Amount).HasColumnType("money");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Network)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetworkId).HasColumnName("NetworkID");
                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RechargeDate).HasColumnType("datetime");
                entity.Property(e => e.RechargeId).HasColumnName("RechargeID");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<VwzSm>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("vwzSMS");

                entity.Property(e => e.Insertdate).HasColumnType("datetime");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Priority)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");
                entity.Property(e => e.SmppId).HasColumnName("SmppID");
                entity.Property(e => e.Smsdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SMSDate");
                entity.Property(e => e.Smsid).HasColumnName("SMSID");
                entity.Property(e => e.SmsidIn).HasColumnName("SMSID_In");
                entity.Property(e => e.Smstext)
                    .HasMaxLength(160)
                    .IsUnicode(false)
                    .HasColumnName("SMSText");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<ZtblArchive>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ztblArchive");

                entity.Property(e => e.ArchiveEffectiveDate).HasColumnType("datetime");
                entity.Property(e => e.ArchiveRunDate).HasColumnType("datetime");
                entity.Property(e => e.MaxPaymentId).HasColumnName("MaxPaymentID");
                entity.Property(e => e.MaxRechargeId).HasColumnName("MaxRechargeID");
                entity.Property(e => e.MaxSmsid).HasColumnName("MaxSMSID");
            });

            modelBuilder.Entity<ZtblStat>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ztblStats");

                entity.Property(e => e.Accountid).HasColumnName("accountid");
                entity.Property(e => e.Band)
                    .HasMaxLength(19)
                    .IsUnicode(false);
                entity.Property(e => e.Mcount).HasColumnName("MCount");
                entity.Property(e => e.Mvalue)
                    .HasColumnType("money")
                    .HasColumnName("MValue");
                entity.Property(e => e.Rmonth).HasColumnName("RMonth");
            });

            modelBuilder.Entity<ZvwProductlistdetail>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("zvwProductlistdetail");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Data)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.DataType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Expr1)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FieldName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WalletName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    }

}
