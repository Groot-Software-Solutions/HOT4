﻿using Hot4.DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hot4.DataModel.Data
{
    public partial class HotDbContext : DbContext
    {
        public HotDbContext(DbContextOptions<HotDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Access> Access { get; set; }
        public virtual DbSet<AccessWeb> AccessWeb { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Banks> Bank { get; set; }
        public virtual DbSet<BankTrx> BankTrx { get; set; }
        public virtual DbSet<BankTrxBatch> BankTrxBatch { get; set; }
        public virtual DbSet<BankTrxStates> BankTrxState { get; set; }
        public virtual DbSet<BankTrxTypes> BankTrxType { get; set; }
        public virtual DbSet<BankvPayment> BankvPayment { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Bundle> Bundle { get; set; }
        public virtual DbSet<Channels> Channel { get; set; }
        public virtual DbSet<Configs> Config { get; set; }
        public virtual DbSet<ConsoleAccess> ConsoleAccess { get; set; }
        public virtual DbSet<ConsoleAction> ConsoleAction { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<ErrorLogContact> ErrorLogContact { get; set; }
        public virtual DbSet<ErrorLogNetwork> ErrorLogNetwork { get; set; }
        public virtual DbSet<ErrorLogSetup> ErrorLogSetup { get; set; }
        public virtual DbSet<ErrorLogTestType> ErrorLogTestType { get; set; }
        public virtual DbSet<HotTypes> HotType { get; set; }
        public virtual DbSet<HotTypeCode> HotTypeCode { get; set; }
        public virtual DbSet<Limit> Limit { get; set; }
        public virtual DbSet<LimitType> LimitType { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Networks> Network { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PaymentSources> PaymentSource { get; set; }
        public virtual DbSet<PaymentTypes> PaymentType { get; set; }
        public virtual DbSet<Pins> Pin { get; set; }
        public virtual DbSet<PinBatches> PinBatch { get; set; }
        public virtual DbSet<PinBatchTypes> PinBatchType { get; set; }
        public virtual DbSet<PinStates> PinState { get; set; }
        public virtual DbSet<Priorities> Priority { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductField> ProductField { get; set; }
        public virtual DbSet<ProductMetaDataType> ProductMetaDataType { get; set; }
        public virtual DbSet<ProductMetaData> ProductMetaData { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ProfileDiscount> ProfileDiscount { get; set; }
        public virtual DbSet<RechargePin> RechargePin { get; set; }
        public virtual DbSet<SmsRecharge> SmsRecharge { get; set; }
        public virtual DbSet<Recharge> Recharge { get; set; }
        public virtual DbSet<RechargePrepaid> RechargePrepaid { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<ReservationLog> ReservationLog { get; set; }
        public virtual DbSet<ReservationStates> ReservationState { get; set; }
        public virtual DbSet<SelfTopUp> SelfTopUp { get; set; }
        public virtual DbSet<SelfTopUpState> SelfTopUpState { get; set; }
        public virtual DbSet<Sms> Sms { get; set; }
        public virtual DbSet<Smpp> Smpp { get; set; }
        public virtual DbSet<States> State { get; set; }
        public virtual DbSet<StockData> StockData { get; set; }
        public virtual DbSet<Subscriber> Subscriber { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<Transfer> Transfer { get; set; }
        public virtual DbSet<WalletType> WalletType { get; set; }
        public virtual DbSet<WebRequests> WebRequest { get; set; }
        public virtual DbSet<ProfileBackup> ProfileBackup { get; set; }

        public virtual DbSet<ZArchive> ZtblArchive { get; set; }
        public virtual DbSet<ZStat> ZtblStat { get; set; }

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

                entity.HasOne(d => d.Account).WithMany(p => p.Accesses)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_tblAccount");

                entity.HasOne(d => d.Channel).WithMany(p => p.Accesses)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccess_tblChannel");
            });

            modelBuilder.Entity<AccessWeb>(entity =>
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

            modelBuilder.Entity<Account>(entity =>
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

                entity.HasOne(d => d.Profile).WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAccount_tblProfile");
            });

            modelBuilder.Entity<Address>(entity =>
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

                entity.HasOne(d => d.Account).WithOne(p => p.Address)
                    .HasForeignKey<Address>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAddress_tblAccount1");
            });

            modelBuilder.Entity<Banks>(entity =>
            {
                entity.HasKey(e => e.BankId);

                entity.ToTable("tblBank");

                entity.Property(e => e.BankId).HasColumnName("BankID");
                entity.Property(e => e.Bank)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SageBankId).HasColumnName("SageBankID");
            });

            modelBuilder.Entity<BankTrx>(entity =>
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

                entity.HasOne(d => d.BankTrxBatch).WithMany(p => p.BankTrxes)
                    .HasForeignKey(d => d.BankTrxBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrx_tblBankTrxBatch");

                entity.HasOne(d => d.BankTrxState).WithMany(p => p.BankTrxes)
                    .HasForeignKey(d => d.BankTrxStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrx_tblBankTrxState");

                entity.HasOne(d => d.BankTrxType).WithMany(p => p.BankTrxes)
                    .HasForeignKey(d => d.BankTrxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrx_tblBankTrxType");

                entity.HasOne(d => d.Payment).WithMany(p => p.BankTrxes)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_tblBankTrx_tblPayment");
            });

            modelBuilder.Entity<BankTrxBatch>(entity =>
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

                entity.HasOne(d => d.Bank).WithMany(p => p.BankTrxBatches)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBankTrxBatch_tblBank");
            });

            modelBuilder.Entity<BankTrxStates>(entity =>
            {
                entity.HasKey(e => e.BankTrxStateId);

                entity.ToTable("tblBankTrxState");

                entity.Property(e => e.BankTrxStateId).HasColumnName("BankTrxStateID");
                entity.Property(e => e.BankTrxState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BankTrxTypes>(entity =>
            {
                entity.HasKey(e => e.BankTrxTypeId);

                entity.ToTable("tblBankTrxType");

                entity.Property(e => e.BankTrxTypeId).HasColumnName("BankTrxTypeID");
                entity.Property(e => e.BankTrxType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BankvPayment>(entity =>
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

            modelBuilder.Entity<Brand>(entity =>
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

                entity.HasOne(d => d.Network).WithMany(p => p.Brands)
                    .HasForeignKey(d => d.NetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrand_tblNetwork");
            });

            modelBuilder.Entity<Bundle>(entity =>
            {
                //entity
                //    .HasNoKey()
                //    .ToTable("tblBundles");
                entity.HasKey(e => e.BundleId);
                entity.ToTable("tblBundles");

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

                entity.HasOne(d => d.Brand).WithMany(p => p.Bundle)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblBundle_tblBrand");
            });

            modelBuilder.Entity<Channels>(entity =>
            {
                entity.HasKey(e => e.ChannelId);

                entity.ToTable("tblChannel");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
                entity.Property(e => e.Channel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Configs>(entity =>
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

            modelBuilder.Entity<ConsoleAccess>(entity =>
            {
                entity.HasKey(e => new { e.RoleName, e.ConsoleActionId });

                entity.ToTable("tblConsoleAccess");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.HasOne(d => d.ConsoleAction).WithMany(p => p.ConsoleAccesses)
                    .HasForeignKey(d => d.ConsoleActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConsoleAccess_tblConsoleAction");
            });

            modelBuilder.Entity<ConsoleAction>(entity =>
            {
                entity.ToTable("tblConsoleAction");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.ActionName).HasMaxLength(255);
            });

            modelBuilder.Entity<ErrorLog>(entity =>
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

            modelBuilder.Entity<ErrorLogContact>(entity =>
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

            modelBuilder.Entity<ErrorLogNetwork>(entity =>
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

            modelBuilder.Entity<ErrorLogSetup>(entity =>
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

            modelBuilder.Entity<ErrorLogTestType>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblErrorLogTestType");

                entity.Property(e => e.TestTypeId).HasColumnName("TestTypeID");
                entity.Property(e => e.TestTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HotTypes>(entity =>
            {
                entity.HasKey(e => e.HotTypeId);

                entity.ToTable("tblHotType");

                entity.Property(e => e.HotTypeId).HasColumnName("HotTypeID");
                entity.Property(e => e.HotType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HotTypeCode>(entity =>
            {
                entity.HasKey(e => e.HotTypeCodeId);

                entity.ToTable("tblHotTypeCode");

                entity.Property(e => e.HotTypeCodeId).HasColumnName("HotTypeCodeID");
                entity.Property(e => e.HotTypeId).HasColumnName("HotTypeID");
                entity.Property(e => e.TypeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.HotType).WithMany(p => p.HotTypeCodes)
                    .HasForeignKey(d => d.HotTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblHotTypeCode_tblHotType");
            });

            modelBuilder.Entity<Limit>(entity =>
            {
                entity.HasKey(e => e.LimitId);

                entity.ToTable("tblLimits");

                entity.HasIndex(e => new { e.AccountId, e.NetworkId }, "IX_Limits_Select");

                entity.Property(e => e.LimitTypeId).HasDefaultValue(1);
            });

            modelBuilder.Entity<LimitType>(entity =>
            {
                entity.HasKey(e => e.LimitTypeId);

                entity.ToTable("tblLimitTypes");

                entity.Property(e => e.LimitTypeName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Log>(entity =>
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

            modelBuilder.Entity<Networks>(entity =>
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

            modelBuilder.Entity<Payment>(entity =>
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

                entity.HasOne(d => d.Account).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblAccount");

                entity.HasOne(d => d.PaymentSource).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblPaymentSource");

                entity.HasOne(d => d.PaymentType).WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPayment_tblPaymentType");
            });

            modelBuilder.Entity<PaymentSources>(entity =>
            {
                entity.HasKey(e => e.PaymentSourceId);

                entity.ToTable("tblPaymentSource");

                entity.Property(e => e.PaymentSourceId).HasColumnName("PaymentSourceID");
                entity.Property(e => e.PaymentSource)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentTypes>(entity =>
            {
                entity.HasKey(e => e.PaymentTypeId);

                entity.ToTable("tblPaymentType");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pins>(entity =>
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

                entity.HasOne(d => d.Brand).WithMany(p => p.Pins)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPin_tblBrand");

                entity.HasOne(d => d.PinBatch).WithMany(p => p.Pins)
                    .HasForeignKey(d => d.PinBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPin_tblPinBatch");

                entity.HasOne(d => d.PinState).WithMany(p => p.Pins)
                    .HasForeignKey(d => d.PinStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPin_tblPinState");
            });

            modelBuilder.Entity<PinBatches>(entity =>
            {
                entity.HasKey(e => e.PinBatchId);

                entity.ToTable("tblPinBatch");

                entity.Property(e => e.PinBatchId).HasColumnName("PinBatchID");
                entity.Property(e => e.BatchDate).HasColumnType("datetime");
                entity.Property(e => e.PinBatch)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.PinBatchTypeId).HasColumnName("PinBatchTypeID");

                entity.HasOne(d => d.PinBatchType).WithMany(p => p.PinBatches)
                    .HasForeignKey(d => d.PinBatchTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPinBatch_tblPinBatchType");
            });

            modelBuilder.Entity<PinBatchTypes>(entity =>
            {
                entity.HasKey(e => e.PinBatchTypeId);

                entity.ToTable("tblPinBatchType");

                entity.Property(e => e.PinBatchTypeId).HasColumnName("PinBatchTypeID");
                entity.Property(e => e.PinBatchType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PinStates>(entity =>
            {
                entity.HasKey(e => e.PinStateId);

                entity.ToTable("tblPinState");

                entity.Property(e => e.PinStateId).HasColumnName("PinStateID");
                entity.Property(e => e.PinState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Priorities>(entity =>
            {
                entity.HasKey(e => e.PriorityId);

                entity.ToTable("tblPriority");

                entity.HasIndex(e => new { e.PriorityId, e.Priority }, "IX_Priority");

                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");
                entity.Property(e => e.Priority)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.ToTable("tblProduct");

                entity.Property(e => e.BrandId).HasDefaultValue((byte)1);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ProductStateId).HasDefaultValue(1);

                entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduct_tblBrand");

                entity.HasOne(d => d.WalletType).WithMany(p => p.Products)
                    .HasForeignKey(d => d.WalletTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProduct_tblWalletType");
            });

            modelBuilder.Entity<ProductField>(entity =>
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

                entity.HasOne(d => d.Product).WithMany(p => p.ProductFields)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrandField_tblBrand");
            });

            modelBuilder.Entity<ProductMetaDataType>(entity =>
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

            modelBuilder.Entity<ProductMetaData>(entity =>
            {
                entity.HasKey(e => e.ProductMetaId).HasName("PK_tblBrandMetaData");

                entity.ToTable("tblProductMetaData");

                entity.Property(e => e.Data)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product).WithMany(p => p.ProductMetaData)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrandMetaData_tblBrand");

                entity.HasOne(d => d.ProductMetaDataType).WithMany(p => p.ProductMetaDatas)
                    .HasForeignKey(d => d.ProductMetaDataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBrandMetaData_tblBrandMetaDataType");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);

                entity.ToTable("tblProfile");

                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProfileDiscount>(entity =>
            {
                entity.HasKey(e => e.ProfileDiscountId);

                entity.ToTable("tblProfileDiscount");

                entity.Property(e => e.ProfileDiscountId).HasColumnName("ProfileDiscountID");
                entity.Property(e => e.BrandId).HasColumnName("BrandID");
                entity.Property(e => e.Discount).HasColumnType("money");
                entity.Property(e => e.ProfileId).HasColumnName("ProfileID");

                entity.HasOne(d => d.Brand).WithMany(p => p.ProfileDiscounts)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProfileDiscount_tblBrand");

                entity.HasOne(d => d.Profile).WithMany(p => p.ProfileDiscounts)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblProfileDiscount_tblProfile");
            });


            modelBuilder.Entity<Recharge>(entity =>
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

            entity.HasOne(d => d.Access).WithMany(p => p.Recharges)
                .HasForeignKey(d => d.AccessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblRecharge_tblAccess");

            entity.HasOne(d => d.Brand).WithMany(p => p.Recharges)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblRecharge_tblBrand");

            entity.HasOne(d => d.State).WithMany(p => p.Recharges)
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

            modelBuilder.Entity<RechargePrepaid>(entity =>
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

                entity.HasOne(d => d.Recharge).WithOne(p => p.RechargePrepaid)
                    .HasForeignKey<RechargePrepaid>(d => d.RechargeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRechargePrepaid_tblRecharge");
            });

            modelBuilder.Entity<Reservation>(entity =>
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

                entity.HasOne(d => d.Access).WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.AccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblreservation_tblAccess");

                entity.HasOne(d => d.Brand).WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReservationToBrand");

                entity.HasOne(d => d.Recharge).WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.RechargeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblreservation_tblRecharge");

                entity.HasOne(d => d.State).WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReservation_tblReservationState");
            });

            modelBuilder.Entity<ReservationLog>(entity =>
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

            modelBuilder.Entity<ReservationStates>(entity =>
            {
                entity.HasKey(e => e.ReservationStateId);

                entity.ToTable("tblReservationState");

                entity.Property(e => e.ReservationState)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SelfTopUp>(entity =>
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

                entity.HasOne(d => d.Access).WithMany(p => p.SelfTopUps)
                    .HasForeignKey(d => d.AccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSelfTopUp_tblAccess");

                entity.HasOne(d => d.BankTrx).WithMany(p => p.SelfTopUps)
                    .HasForeignKey(d => d.BankTrxId)
                    .HasConstraintName("FK_tblSelfTopUp_tblBankTrx");

                entity.HasOne(d => d.Brand).WithMany(p => p.SelfTopUps)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblSelfTo__Brand__5CACADF9");

                entity.HasOne(d => d.Recharge).WithMany(p => p.SelfTopUps)
                    .HasForeignKey(d => d.RechargeId)
                    .HasConstraintName("FK_tblSelfTopUp_tblRecharge");
                entity.HasOne(d => d.SelfTopUpState).WithMany(p => p.SelfTopUps)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSelfTopUp_tblSelfTopUpState");
            });

            modelBuilder.Entity<SelfTopUpState>(entity =>
            {
                entity.HasKey(e => e.SelfTopUpStateId);

                entity.ToTable("tblSelfTopUpState");

                entity.Property(e => e.SelfTopUpStateId).HasColumnName("SelfTopUpStateID");
                entity.Property(e => e.SelfTopUpStateName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sms>(entity =>
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

                entity.HasOne(d => d.Priority).WithMany(p => p.Sms)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSMS_tblPriority");

                entity.HasOne(d => d.Smpp).WithMany(p => p.Sms)
                    .HasForeignKey(d => d.SmppId)
                    .HasConstraintName("FK_tblSMS_tblSmpp");

                entity.HasOne(d => d.SmsidInNavigation).WithMany(p => p.InverseSmsidInNavigation)
                    .HasForeignKey(d => d.SmsidIn)
                    .HasConstraintName("FK_tblSMS_tblSMS");

                entity.HasOne(d => d.State).WithMany(p => p.Sms)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSMS_tblState");
            });

            modelBuilder.Entity<Smpp>(entity =>
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

            modelBuilder.Entity<RechargePin>(entity =>
            {
                entity.ToTable("tblRechargePin");
                // entity.HasKey(e => new { e.PinId, e.RechargeId });
            });
            modelBuilder.Entity<SmsRecharge>(entity =>
            {
                entity.ToTable("tblSmsRecharge");
                //  entity.HasKey(e => new { e.SmsId, e.RechargeId });
            });
            modelBuilder.Entity<States>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.ToTable("tblState");

                entity.Property(e => e.StateId).HasColumnName("StateID");
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StockData>(entity =>
            {
                entity.HasKey(e => e.StockDataId);
                entity.Property(e => e.StockDataId).ValueGeneratedOnAdd();
                entity.ToTable("tblStockData");

                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastSold).HasColumnType("datetime");
                entity.Property(e => e.MonthSold).HasColumnType("money");
                entity.Property(e => e.PinValue).HasColumnType("money");
                entity.Property(e => e.Sold).HasColumnType("money");
                entity.Property(e => e.WeekSold).HasColumnType("money");
            });

            modelBuilder.Entity<Subscriber>(entity =>
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

                entity.HasOne(d => d.Account).WithMany(p => p.Subscribers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubscriber_tblAccount");

                entity.HasOne(d => d.Brand).WithMany(p => p.Subscribers)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubscriber_tblBrand");
            });

            modelBuilder.Entity<Template>(entity =>
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

            modelBuilder.Entity<Transfer>(entity =>
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

                entity.HasOne(d => d.Channel).WithMany(p => p.Transfers)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTransfer_tblChannel");

                entity.HasOne(d => d.PaymentIdFromNavigation).WithMany(p => p.TransferPaymentIdFromNavigations)
                    .HasForeignKey(d => d.PaymentIdFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTransfer_tblPayment");

                entity.HasOne(d => d.PaymentIdToNavigation).WithMany(p => p.TransferPaymentIdToNavigations)
                    .HasForeignKey(d => d.PaymentIdTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTransfer_tblPayment1");
            });

            modelBuilder.Entity<WalletType>(entity =>
            {
                entity.HasKey(e => e.WalletTypeId);

                entity.ToTable("tblWalletType");

                entity.Property(e => e.WalletTypeId).ValueGeneratedNever();
                entity.Property(e => e.WalletName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WebRequests>(entity =>
            {
                entity.HasKey(e => e.WebId);
                entity.ToTable("tblWebRequest");

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

            modelBuilder.Entity<ProfileBackup>(entity =>
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

            modelBuilder.Entity<ZArchive>(entity =>
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

            modelBuilder.Entity<ZStat>(entity =>
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


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    }

}
