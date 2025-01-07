using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hot4.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.CreateTable(
            //        name: "tblAccessWeb",
            //        columns: table => new
            //        {
            //            AccessID = table.Column<long>(type: "bigint", nullable: false),
            //            AccessName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
            //            WebBackground = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: ""),
            //            SalesPassword = table.Column<bool>(type: "bit", nullable: false),
            //            ResetToken = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true, defaultValue: "")
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblAccessWeb", x => x.AccessID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBank",
            //        columns: table => new
            //        {
            //            BankID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Bank = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            SageBankID = table.Column<int>(type: "int", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBank", x => x.BankID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBankTrxState",
            //        columns: table => new
            //        {
            //            BankTrxStateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            BankTrxState = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBankTrxState", x => x.BankTrxStateID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBankTrxType",
            //        columns: table => new
            //        {
            //            BankTrxTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            BankTrxType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBankTrxType", x => x.BankTrxTypeID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBankvPayment",
            //        columns: table => new
            //        {
            //            BankTrxID = table.Column<long>(type: "bigint", nullable: true),
            //            vPaymentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //            CheckURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
            //            ProcessURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
            //            ErrorMsg = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
            //            vPaymentRef = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBundles",
            //        columns: table => new
            //        {
            //            BundleID = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            BrandID = table.Column<int>(type: "int", nullable: false),
            //            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
            //            Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //            Amount = table.Column<int>(type: "int", nullable: false, comment: "Bundle Value in cents"),
            //            ProductCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //            ValidityPeriod = table.Column<int>(type: "int", nullable: true, comment: "Validity Period in Days"),
            //            Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblChannel",
            //        columns: table => new
            //        {
            //            ChannelID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Channel = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblChannel", x => x.ChannelID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblConfig",
            //        columns: table => new
            //        {
            //            ConfigID = table.Column<byte>(type: "tinyint", nullable: false),
            //            ProfileID_NewSMSDealer = table.Column<int>(type: "int", nullable: false),
            //            ProfileID_NewWebDealer = table.Column<int>(type: "int", nullable: false),
            //            MinRecharge = table.Column<decimal>(type: "money", nullable: false),
            //            MaxRecharge = table.Column<decimal>(type: "money", nullable: false),
            //            PrepaidEnabled = table.Column<bool>(type: "bit", nullable: false),
            //            MinTransfer = table.Column<decimal>(type: "money", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblConfig", x => x.ConfigID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblConsoleAction",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false),
            //            ActionName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblConsoleAction", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblErrorLog",
            //        columns: table => new
            //        {
            //            ErrorID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            LogDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())", comment: ""),
            //            CheckID = table.Column<int>(type: "int", nullable: false),
            //            ErrorCount = table.Column<int>(type: "int", nullable: false, comment: "0"),
            //            ErrorData = table.Column<string>(type: "text", nullable: false, defaultValue: "")
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblErrorLogContacts",
            //        columns: table => new
            //        {
            //            ErrorLogtypeID = table.Column<int>(type: "int", nullable: false),
            //            TestType = table.Column<int>(type: "int", nullable: false),
            //            ContactEmail = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
            //            ContactMobile = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblErrorLogNetworks",
            //        columns: table => new
            //        {
            //            ErrorLogNetworkID = table.Column<int>(type: "int", nullable: false),
            //            NetworkName = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
            //            HotBrandID = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblErrorLogSetup",
            //        columns: table => new
            //        {
            //            ErrorLogCheckID = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            Server = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, defaultValue: ""),
            //            Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            Network = table.Column<int>(type: "int", nullable: false),
            //            TestType = table.Column<int>(type: "int", nullable: false),
            //            HostAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            URL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
            //            Port = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
            //            Latency = table.Column<int>(type: "int", nullable: true),
            //            ExpectedContent = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
            //            CountThreshold = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
            //            ErrorInterval = table.Column<int>(type: "int", nullable: false, defaultValue: 10000),
            //            CheckInterval = table.Column<int>(type: "int", nullable: false, defaultValue: 30000),
            //            Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblErrorLogTestType",
            //        columns: table => new
            //        {
            //            TestTypeID = table.Column<int>(type: "int", nullable: false),
            //            TestTypeName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblHotType",
            //        columns: table => new
            //        {
            //            HotTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            HotType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            SplitCount = table.Column<byte>(type: "tinyint", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblHotType", x => x.HotTypeID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblLimits",
            //        columns: table => new
            //        {
            //            LimitId = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            NetworkId = table.Column<byte>(type: "tinyint", nullable: false),
            //            AccountId = table.Column<long>(type: "bigint", nullable: false),
            //            LimitTypeId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
            //            DailyLimit = table.Column<double>(type: "float", nullable: false),
            //            MonthlyLimit = table.Column<double>(type: "float", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblLimits", x => x.LimitId);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblLimitTypes",
            //        columns: table => new
            //        {
            //            LimitTypeId = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            LimitTypeName = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblLimitTypes", x => x.LimitTypeId);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblLog",
            //        columns: table => new
            //        {
            //            LogID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            LogDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            LogModule = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            LogObject = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            LogMethod = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            LogDescription = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
            //            IDType = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
            //            IDNumber = table.Column<long>(type: "bigint", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblLog", x => x.LogID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblNetwork",
            //        columns: table => new
            //        {
            //            NetworkID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Network = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            Prefix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblNetwork", x => x.NetworkID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPaymentSource",
            //        columns: table => new
            //        {
            //            PaymentSourceID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PaymentSource = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            WalletTypeId = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPaymentSource", x => x.PaymentSourceID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPaymentType",
            //        columns: table => new
            //        {
            //            PaymentTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PaymentType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPaymentType", x => x.PaymentTypeID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPinBatchType",
            //        columns: table => new
            //        {
            //            PinBatchTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PinBatchType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPinBatchType", x => x.PinBatchTypeID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPinState",
            //        columns: table => new
            //        {
            //            PinStateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PinState = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPinState", x => x.PinStateID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPriority",
            //        columns: table => new
            //        {
            //            PriorityID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Priority = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPriority", x => x.PriorityID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblProductMetaDataType",
            //        columns: table => new
            //        {
            //            ProductMetaDataTypeId = table.Column<byte>(type: "tinyint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBrandMetaDataType", x => x.ProductMetaDataTypeId);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblProfile",
            //        columns: table => new
            //        {
            //            ProfileID = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProfileName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblProfile", x => x.ProfileID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblprofile_backup",
            //        columns: table => new
            //        {
            //            ProfileID = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProfileName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblReservationState",
            //        columns: table => new
            //        {
            //            ReservationStateId = table.Column<byte>(type: "tinyint", nullable: false),
            //            ReservationState = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblReservationState", x => x.ReservationStateId);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblSelfTopUpState",
            //        columns: table => new
            //        {
            //            SelfTopUpStateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            SelfTopUpStateName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblSelfTopUpState", x => x.SelfTopUpStateID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblSmpp",
            //        columns: table => new
            //        {
            //            SmppID = table.Column<byte>(type: "tinyint", nullable: false),
            //            SmppName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            AllowSend = table.Column<bool>(type: "bit", nullable: false),
            //            AllowReceive = table.Column<bool>(type: "bit", nullable: false),
            //            SmppEnabled = table.Column<bool>(type: "bit", nullable: false),
            //            DestinationAddressNpi = table.Column<int>(type: "int", nullable: false),
            //            DestinationAddressTon = table.Column<int>(type: "int", nullable: false),
            //            SourceAddress = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
            //            SourceAddressNpi = table.Column<int>(type: "int", nullable: false),
            //            SourceAddressTon = table.Column<int>(type: "int", nullable: false),
            //            SmppTimeout = table.Column<int>(type: "int", nullable: false),
            //            RemoteHost = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            RemotePort = table.Column<int>(type: "int", nullable: false),
            //            SystemID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            SmppPassword = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            AddressRange = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            InterfaceVersion = table.Column<int>(type: "int", nullable: false),
            //            SystemType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            EconetPrefix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            NetOnePrefix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            TelecelPrefix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblSmpp", x => x.SmppID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblState",
            //        columns: table => new
            //        {
            //            StateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            State = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblState", x => x.StateID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblStockData",
            //        columns: table => new
            //        {
            //            BrandName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            PinValue = table.Column<decimal>(type: "money", nullable: true),
            //            Available = table.Column<int>(type: "int", nullable: true),
            //            LastSold = table.Column<DateTime>(type: "datetime", nullable: true),
            //            Sold = table.Column<decimal>(type: "money", nullable: true),
            //            WeekSold = table.Column<decimal>(type: "money", nullable: true),
            //            MonthSold = table.Column<decimal>(type: "money", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblTemplate",
            //        columns: table => new
            //        {
            //            TemplateID = table.Column<int>(type: "int", nullable: false),
            //            TemplateName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            TemplateText = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblTemplate", x => x.TemplateID);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblWalletType",
            //        columns: table => new
            //        {
            //            WalletTypeId = table.Column<int>(type: "int", nullable: false),
            //            WalletName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblWalletType", x => x.WalletTypeId);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblWebRequest",
            //        columns: table => new
            //        {
            //            WebID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            HotTypeID = table.Column<byte>(type: "tinyint", nullable: true),
            //            AccessID = table.Column<long>(type: "bigint", nullable: true),
            //            StateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            InsertDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            ReplyDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //            AgentReference = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            ReturnCode = table.Column<int>(type: "int", nullable: true),
            //            Reply = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
            //            ChannelID = table.Column<byte>(type: "tinyint", nullable: false),
            //            RechargeID = table.Column<long>(type: "bigint", nullable: true),
            //            WalletBalance = table.Column<decimal>(type: "money", nullable: true),
            //            Cost = table.Column<decimal>(type: "money", nullable: true),
            //            Discount = table.Column<decimal>(type: "money", nullable: true),
            //            Amount = table.Column<decimal>(type: "money", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "ztblArchive",
            //        columns: table => new
            //        {
            //            ArchiveRunDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            ArchiveEffectiveDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            MaxRechargeID = table.Column<long>(type: "bigint", nullable: false),
            //            MaxSMSID = table.Column<long>(type: "bigint", nullable: false),
            //            MaxPaymentID = table.Column<long>(type: "bigint", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "ztblStats",
            //        columns: table => new
            //        {
            //            accountid = table.Column<long>(type: "bigint", nullable: false),
            //            RMonth = table.Column<int>(type: "int", nullable: true),
            //            MValue = table.Column<decimal>(type: "money", nullable: true),
            //            MCount = table.Column<int>(type: "int", nullable: true),
            //            Band = table.Column<string>(type: "varchar(19)", unicode: false, maxLength: 19, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBankTrxBatch",
            //        columns: table => new
            //        {
            //            BankTrxBatchID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            BankID = table.Column<byte>(type: "tinyint", nullable: false),
            //            BatchDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            BatchReference = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
            //            LastUser = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBankTrxBatch", x => x.BankTrxBatchID);
            //            table.ForeignKey(
            //                name: "FK_tblBankTrxBatch_tblBank",
            //                column: x => x.BankID,
            //                principalTable: "tblBank",
            //                principalColumn: "BankID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblConsoleAccess",
            //        columns: table => new
            //        {
            //            RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //            ConsoleActionId = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblConsoleAccess", x => new { x.RoleName, x.ConsoleActionId });
            //            table.ForeignKey(
            //                name: "FK_tblConsoleAccess_tblConsoleAction",
            //                column: x => x.ConsoleActionId,
            //                principalTable: "tblConsoleAction",
            //                principalColumn: "Id");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblHotTypeCode",
            //        columns: table => new
            //        {
            //            HotTypeCodeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            HotTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            TypeCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblHotTypeCode", x => x.HotTypeCodeID);
            //            table.ForeignKey(
            //                name: "FK_tblHotTypeCode_tblHotType",
            //                column: x => x.HotTypeID,
            //                principalTable: "tblHotType",
            //                principalColumn: "HotTypeID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBrand",
            //        columns: table => new
            //        {
            //            BrandID = table.Column<byte>(type: "tinyint", nullable: false),
            //            NetworkID = table.Column<byte>(type: "tinyint", nullable: false),
            //            BrandName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            BrandSuffix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            WalletTypeId = table.Column<int>(type: "int", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBrand", x => x.BrandID);
            //            table.ForeignKey(
            //                name: "FK_tblBrand_tblNetwork",
            //                column: x => x.NetworkID,
            //                principalTable: "tblNetwork",
            //                principalColumn: "NetworkID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPinBatch",
            //        columns: table => new
            //        {
            //            PinBatchID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            PinBatch = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
            //            BatchDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            PinBatchTypeID = table.Column<byte>(type: "tinyint", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPinBatch", x => x.PinBatchID);
            //            table.ForeignKey(
            //                name: "FK_tblPinBatch_tblPinBatchType",
            //                column: x => x.PinBatchTypeID,
            //                principalTable: "tblPinBatchType",
            //                principalColumn: "PinBatchTypeID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblAccount",
            //        columns: table => new
            //        {
            //            AccountID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProfileID = table.Column<int>(type: "int", nullable: false),
            //            AccountName = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
            //            NationalID = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
            //            ReferredBy = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
            //            InsertDate = table.Column<DateTime>(type: "smalldatetime", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblAccount", x => x.AccountID);
            //            table.ForeignKey(
            //                name: "FK_tblAccount_tblProfile",
            //                column: x => x.ProfileID,
            //                principalTable: "tblProfile",
            //                principalColumn: "ProfileID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblSMS",
            //        columns: table => new
            //        {
            //            SMSID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            SmppID = table.Column<byte>(type: "tinyint", nullable: true),
            //            StateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PriorityID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Direction = table.Column<bool>(type: "bit", nullable: false),
            //            Mobile = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            SMSText = table.Column<string>(type: "varchar(640)", unicode: false, maxLength: 640, nullable: false),
            //            SMSDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            SMSID_In = table.Column<long>(type: "bigint", nullable: true),
            //            InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblSMS", x => x.SMSID);
            //            table.ForeignKey(
            //                name: "FK_tblSMS_tblPriority",
            //                column: x => x.PriorityID,
            //                principalTable: "tblPriority",
            //                principalColumn: "PriorityID");
            //            table.ForeignKey(
            //                name: "FK_tblSMS_tblSMS",
            //                column: x => x.SMSID_In,
            //                principalTable: "tblSMS",
            //                principalColumn: "SMSID");
            //            table.ForeignKey(
            //                name: "FK_tblSMS_tblSmpp",
            //                column: x => x.SmppID,
            //                principalTable: "tblSmpp",
            //                principalColumn: "SmppID");
            //            table.ForeignKey(
            //                name: "FK_tblSMS_tblState",
            //                column: x => x.StateID,
            //                principalTable: "tblState",
            //                principalColumn: "StateID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblProduct",
            //        columns: table => new
            //        {
            //            ProductId = table.Column<byte>(type: "tinyint", nullable: false),
            //            BrandId = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
            //            Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            WalletTypeId = table.Column<int>(type: "int", nullable: false),
            //            ProductStateId = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblProduct", x => x.ProductId);
            //            table.ForeignKey(
            //                name: "FK_tblProduct_tblBrand",
            //                column: x => x.BrandId,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //            table.ForeignKey(
            //                name: "FK_tblProduct_tblWalletType",
            //                column: x => x.WalletTypeId,
            //                principalTable: "tblWalletType",
            //                principalColumn: "WalletTypeId");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblProfileDiscount",
            //        columns: table => new
            //        {
            //            ProfileDiscountID = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProfileID = table.Column<int>(type: "int", nullable: false),
            //            BrandID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Discount = table.Column<decimal>(type: "money", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblProfileDiscount", x => x.ProfileDiscountID);
            //            table.ForeignKey(
            //                name: "FK_tblProfileDiscount_tblBrand",
            //                column: x => x.BrandID,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //            table.ForeignKey(
            //                name: "FK_tblProfileDiscount_tblProfile",
            //                column: x => x.ProfileID,
            //                principalTable: "tblProfile",
            //                principalColumn: "ProfileID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPin",
            //        columns: table => new
            //        {
            //            PinID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            PinBatchID = table.Column<long>(type: "bigint", nullable: false),
            //            PinStateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            BrandID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Pin = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            PinRef = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            PinValue = table.Column<decimal>(type: "money", nullable: false),
            //            PinExpiry = table.Column<DateTime>(type: "datetime", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPin", x => x.PinID);
            //            table.ForeignKey(
            //                name: "FK_tblPin_tblBrand",
            //                column: x => x.BrandID,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //            table.ForeignKey(
            //                name: "FK_tblPin_tblPinBatch",
            //                column: x => x.PinBatchID,
            //                principalTable: "tblPinBatch",
            //                principalColumn: "PinBatchID");
            //            table.ForeignKey(
            //                name: "FK_tblPin_tblPinState",
            //                column: x => x.PinStateID,
            //                principalTable: "tblPinState",
            //                principalColumn: "PinStateID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblAccess",
            //        columns: table => new
            //        {
            //            AccessID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            AccountID = table.Column<long>(type: "bigint", nullable: false),
            //            ChannelID = table.Column<byte>(type: "tinyint", nullable: false),
            //            AccessCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            AccessPassword = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            Deleted = table.Column<bool>(type: "bit", nullable: true),
            //            PasswordHash = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            PasswordSalt = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            InsertDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblAccess", x => x.AccessID);
            //            table.ForeignKey(
            //                name: "FK_tblAccess_tblAccount",
            //                column: x => x.AccountID,
            //                principalTable: "tblAccount",
            //                principalColumn: "AccountID");
            //            table.ForeignKey(
            //                name: "FK_tblAccess_tblChannel",
            //                column: x => x.ChannelID,
            //                principalTable: "tblChannel",
            //                principalColumn: "ChannelID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblAddress",
            //        columns: table => new
            //        {
            //            AccountID = table.Column<long>(type: "bigint", nullable: false),
            //            Address1 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //            Address2 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //            City = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            ContactName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            ContactNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            VatNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
            //            Latitude = table.Column<double>(type: "float", nullable: true),
            //            Longitude = table.Column<double>(type: "float", nullable: true),
            //            SageID = table.Column<long>(type: "bigint", nullable: true),
            //            InvoiceFreq = table.Column<byte>(type: "tinyint", nullable: true),
            //            SageIDUsd = table.Column<long>(type: "bigint", nullable: true),
            //            Confirmed = table.Column<bool>(type: "bit", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblAddress", x => x.AccountID);
            //            table.ForeignKey(
            //                name: "FK_tblAddress_tblAccount1",
            //                column: x => x.AccountID,
            //                principalTable: "tblAccount",
            //                principalColumn: "AccountID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblPayment",
            //        columns: table => new
            //        {
            //            PaymentID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            AccountID = table.Column<long>(type: "bigint", nullable: false),
            //            PaymentTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PaymentSourceID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Amount = table.Column<decimal>(type: "money", nullable: false),
            //            PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            Reference = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
            //            LastUser = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblPayment", x => x.PaymentID);
            //            table.ForeignKey(
            //                name: "FK_tblPayment_tblAccount",
            //                column: x => x.AccountID,
            //                principalTable: "tblAccount",
            //                principalColumn: "AccountID");
            //            table.ForeignKey(
            //                name: "FK_tblPayment_tblPaymentSource",
            //                column: x => x.PaymentSourceID,
            //                principalTable: "tblPaymentSource",
            //                principalColumn: "PaymentSourceID");
            //            table.ForeignKey(
            //                name: "FK_tblPayment_tblPaymentType",
            //                column: x => x.PaymentTypeID,
            //                principalTable: "tblPaymentType",
            //                principalColumn: "PaymentTypeID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblSubscriber",
            //        columns: table => new
            //        {
            //            SubscriberID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            AccountID = table.Column<long>(type: "bigint", nullable: false),
            //            SubscriberName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
            //            SubscriberMobile = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            BrandID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Active = table.Column<bool>(type: "bit", nullable: false),
            //            NotifyNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            SubscriberGroup = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
            //            DefaultProductID = table.Column<int>(type: "int", nullable: true),
            //            DefaultAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
            //            NetworkID = table.Column<byte>(type: "tinyint", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblSubscriber", x => x.SubscriberID);
            //            table.ForeignKey(
            //                name: "FK_tblSubscriber_tblAccount",
            //                column: x => x.AccountID,
            //                principalTable: "tblAccount",
            //                principalColumn: "AccountID");
            //            table.ForeignKey(
            //                name: "FK_tblSubscriber_tblBrand",
            //                column: x => x.BrandID,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblProductField",
            //        columns: table => new
            //        {
            //            BrandFieldId = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProductId = table.Column<byte>(type: "tinyint", nullable: false),
            //            FieldName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            DataType = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
            //            Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBrandField", x => x.BrandFieldId);
            //            table.ForeignKey(
            //                name: "FK_tblBrandField_tblBrand",
            //                column: x => x.ProductId,
            //                principalTable: "tblProduct",
            //                principalColumn: "ProductId");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblProductMetaData",
            //        columns: table => new
            //        {
            //            ProductMetaId = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProductId = table.Column<byte>(type: "tinyint", nullable: false),
            //            ProductMetaDataTypeId = table.Column<byte>(type: "tinyint", nullable: false),
            //            Data = table.Column<string>(type: "varchar(5000)", unicode: false, maxLength: 5000, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBrandMetaData", x => x.ProductMetaId);
            //            table.ForeignKey(
            //                name: "FK_tblBrandMetaData_tblBrand",
            //                column: x => x.ProductId,
            //                principalTable: "tblProduct",
            //                principalColumn: "ProductId");
            //            table.ForeignKey(
            //                name: "FK_tblBrandMetaData_tblBrandMetaDataType",
            //                column: x => x.ProductMetaDataTypeId,
            //                principalTable: "tblProductMetaDataType",
            //                principalColumn: "ProductMetaDataTypeId");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblRecharge",
            //        columns: table => new
            //        {
            //            RechargeID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            StateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            AccessID = table.Column<long>(type: "bigint", nullable: false),
            //            Amount = table.Column<decimal>(type: "money", nullable: false),
            //            Discount = table.Column<decimal>(type: "money", nullable: false),
            //            Mobile = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            BrandID = table.Column<byte>(type: "tinyint", nullable: false),
            //            RechargeDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //            TblPinPinId = table.Column<long>(type: "bigint", nullable: true),
            //            TblSmsSmsid = table.Column<long>(type: "bigint", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblRecharge", x => x.RechargeID);
            //            table.ForeignKey(
            //                name: "FK_tblRecharge_tblAccess",
            //                column: x => x.AccessID,
            //                principalTable: "tblAccess",
            //                principalColumn: "AccessID");
            //            table.ForeignKey(
            //                name: "FK_tblRecharge_tblBrand",
            //                column: x => x.BrandID,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //            table.ForeignKey(
            //                name: "FK_tblRecharge_tblPin_TblPinPinId",
            //                column: x => x.TblPinPinId,
            //                principalTable: "tblPin",
            //                principalColumn: "PinID");
            //            table.ForeignKey(
            //                name: "FK_tblRecharge_tblSMS_TblSmsSmsid",
            //                column: x => x.TblSmsSmsid,
            //                principalTable: "tblSMS",
            //                principalColumn: "SMSID");
            //            table.ForeignKey(
            //                name: "FK_tblRecharge_tblState",
            //                column: x => x.StateID,
            //                principalTable: "tblState",
            //                principalColumn: "StateID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblBankTrx",
            //        columns: table => new
            //        {
            //            BankTrxID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            BankTrxBatchID = table.Column<long>(type: "bigint", nullable: false),
            //            BankTrxTypeID = table.Column<byte>(type: "tinyint", nullable: false),
            //            BankTrxStateID = table.Column<byte>(type: "tinyint", nullable: false),
            //            Amount = table.Column<decimal>(type: "money", nullable: false),
            //            TrxDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            Identifier = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            RefName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
            //            Branch = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            BankRef = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
            //            Balance = table.Column<decimal>(type: "money", nullable: false),
            //            PaymentID = table.Column<long>(type: "bigint", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblBankTrx", x => x.BankTrxID);
            //            table.ForeignKey(
            //                name: "FK_tblBankTrx_tblBankTrxBatch",
            //                column: x => x.BankTrxBatchID,
            //                principalTable: "tblBankTrxBatch",
            //                principalColumn: "BankTrxBatchID");
            //            table.ForeignKey(
            //                name: "FK_tblBankTrx_tblBankTrxState",
            //                column: x => x.BankTrxStateID,
            //                principalTable: "tblBankTrxState",
            //                principalColumn: "BankTrxStateID");
            //            table.ForeignKey(
            //                name: "FK_tblBankTrx_tblBankTrxType",
            //                column: x => x.BankTrxTypeID,
            //                principalTable: "tblBankTrxType",
            //                principalColumn: "BankTrxTypeID");
            //            table.ForeignKey(
            //                name: "FK_tblBankTrx_tblPayment",
            //                column: x => x.PaymentID,
            //                principalTable: "tblPayment",
            //                principalColumn: "PaymentID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblTransfer",
            //        columns: table => new
            //        {
            //            TransferID = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ChannelID = table.Column<byte>(type: "tinyint", nullable: false),
            //            PaymentID_From = table.Column<long>(type: "bigint", nullable: false),
            //            PaymentID_To = table.Column<long>(type: "bigint", nullable: false),
            //            Amount = table.Column<decimal>(type: "money", nullable: false),
            //            TransferDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //            SMSID = table.Column<long>(type: "bigint", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblTransfer", x => x.TransferID);
            //            table.ForeignKey(
            //                name: "FK_tblTransfer_tblChannel",
            //                column: x => x.ChannelID,
            //                principalTable: "tblChannel",
            //                principalColumn: "ChannelID");
            //            table.ForeignKey(
            //                name: "FK_tblTransfer_tblPayment",
            //                column: x => x.PaymentID_From,
            //                principalTable: "tblPayment",
            //                principalColumn: "PaymentID");
            //            table.ForeignKey(
            //                name: "FK_tblTransfer_tblPayment1",
            //                column: x => x.PaymentID_To,
            //                principalTable: "tblPayment",
            //                principalColumn: "PaymentID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblRechargePin",
            //        columns: table => new
            //        {
            //            RechargeId = table.Column<long>(type: "bigint", nullable: false),
            //            PinId = table.Column<long>(type: "bigint", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblRechargePin", x => new { x.RechargeId, x.PinId });
            //            table.ForeignKey(
            //                name: "FK_tblRechargePin_tblPin_PinId",
            //                column: x => x.PinId,
            //                principalTable: "tblPin",
            //                principalColumn: "PinID",
            //                onDelete: ReferentialAction.Cascade);
            //            table.ForeignKey(
            //                name: "FK_tblRechargePin_tblRecharge_RechargeId",
            //                column: x => x.RechargeId,
            //                principalTable: "tblRecharge",
            //                principalColumn: "RechargeID",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblRechargePrepaid",
            //        columns: table => new
            //        {
            //            RechargeID = table.Column<long>(type: "bigint", nullable: false),
            //            DebitCredit = table.Column<bool>(type: "bit", nullable: false),
            //            ReturnCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
            //            Narrative = table.Column<string>(type: "varchar(2500)", unicode: false, maxLength: 2500, nullable: false),
            //            InitialBalance = table.Column<decimal>(type: "money", nullable: false),
            //            FinalBalance = table.Column<decimal>(type: "money", nullable: false),
            //            Reference = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
            //            InitialWallet = table.Column<decimal>(type: "money", nullable: true),
            //            FinalWallet = table.Column<decimal>(type: "money", nullable: true),
            //            Window = table.Column<DateTime>(type: "datetime", nullable: true),
            //            Data = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
            //            SMS = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblRechargePrepaid_1", x => x.RechargeID);
            //            table.ForeignKey(
            //                name: "FK_tblRechargePrepaid_tblRecharge",
            //                column: x => x.RechargeID,
            //                principalTable: "tblRecharge",
            //                principalColumn: "RechargeID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblReservation",
            //        columns: table => new
            //        {
            //            ReservationId = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            AccessID = table.Column<long>(type: "bigint", nullable: false),
            //            RechargeID = table.Column<long>(type: "bigint", nullable: false),
            //            BrandId = table.Column<byte>(type: "tinyint", nullable: false),
            //            ProductCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            NotificationNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            TargetNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //            StateId = table.Column<byte>(type: "tinyint", nullable: false),
            //            Amount = table.Column<decimal>(type: "money", nullable: false),
            //            Currency = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
            //            ConfirmationData = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblreservation", x => x.ReservationId);
            //            table.ForeignKey(
            //                name: "FK_tblReservationToBrand",
            //                column: x => x.BrandId,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //            table.ForeignKey(
            //                name: "FK_tblReservation_tblReservationState",
            //                column: x => x.StateId,
            //                principalTable: "tblReservationState",
            //                principalColumn: "ReservationStateId");
            //            table.ForeignKey(
            //                name: "FK_tblreservation_tblAccess",
            //                column: x => x.AccessID,
            //                principalTable: "tblAccess",
            //                principalColumn: "AccessID");
            //            table.ForeignKey(
            //                name: "FK_tblreservation_tblRecharge",
            //                column: x => x.RechargeID,
            //                principalTable: "tblRecharge",
            //                principalColumn: "RechargeID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblSmsRecharge",
            //        columns: table => new
            //        {
            //            RechargeId = table.Column<long>(type: "bigint", nullable: false),
            //            SmsId = table.Column<long>(type: "bigint", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblSmsRecharge", x => new { x.RechargeId, x.SmsId });
            //            table.ForeignKey(
            //                name: "FK_tblSmsRecharge_tblRecharge_RechargeId",
            //                column: x => x.RechargeId,
            //                principalTable: "tblRecharge",
            //                principalColumn: "RechargeID",
            //                onDelete: ReferentialAction.Cascade);
            //            table.ForeignKey(
            //                name: "FK_tblSmsRecharge_tblSMS_SmsId",
            //                column: x => x.SmsId,
            //                principalTable: "tblSMS",
            //                principalColumn: "SMSID",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblSelfTopUp",
            //        columns: table => new
            //        {
            //            SelfTopUpId = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            AccessID = table.Column<long>(type: "bigint", nullable: false),
            //            BankTrxID = table.Column<long>(type: "bigint", nullable: true),
            //            RechargeID = table.Column<long>(type: "bigint", nullable: true),
            //            BrandId = table.Column<byte>(type: "tinyint", nullable: false),
            //            ProductCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            NotificationNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //            BillerNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            TargetNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
            //            InsertDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //            StateId = table.Column<byte>(type: "tinyint", nullable: false),
            //            Amount = table.Column<decimal>(type: "money", nullable: false),
            //            Currency = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_tblSelfTopUp", x => x.SelfTopUpId);
            //            table.ForeignKey(
            //                name: "FK__tblSelfTo__Brand__5CACADF9",
            //                column: x => x.BrandId,
            //                principalTable: "tblBrand",
            //                principalColumn: "BrandID");
            //            table.ForeignKey(
            //                name: "FK_tblSelfTopUp_tblAccess",
            //                column: x => x.AccessID,
            //                principalTable: "tblAccess",
            //                principalColumn: "AccessID");
            //            table.ForeignKey(
            //                name: "FK_tblSelfTopUp_tblBankTrx",
            //                column: x => x.BankTrxID,
            //                principalTable: "tblBankTrx",
            //                principalColumn: "BankTrxID");
            //            table.ForeignKey(
            //                name: "FK_tblSelfTopUp_tblRecharge",
            //                column: x => x.RechargeID,
            //                principalTable: "tblRecharge",
            //                principalColumn: "RechargeID");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "tblReservationLog",
            //        columns: table => new
            //        {
            //            ReservationLogId = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ReservationId = table.Column<long>(type: "bigint", nullable: false),
            //            LogDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
            //            OldStateId = table.Column<byte>(type: "tinyint", nullable: false),
            //            NewStateId = table.Column<byte>(type: "tinyint", nullable: false),
            //            LastUser = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false, defaultValueSql: "(suser_sname())")
            //        },
            //        constraints: table =>
            //        {
            //            table.ForeignKey(
            //                name: "FK_Reservation",
            //                column: x => x.ReservationId,
            //                principalTable: "tblReservation",
            //                principalColumn: "ReservationId");
            //            table.ForeignKey(
            //                name: "FK_tblReservationLog_tblReservationState",
            //                column: x => x.OldStateId,
            //                principalTable: "tblReservationState",
            //                principalColumn: "ReservationStateId");
            //            table.ForeignKey(
            //                name: "FK_tblReservationLog_tblReservationState1",
            //                column: x => x.NewStateId,
            //                principalTable: "tblReservationState",
            //                principalColumn: "ReservationStateId");
            //        });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AccessCode",
            //        table: "tblAccess",
            //        column: "AccessCode");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AccountID_AccessID",
            //        table: "tblAccess",
            //        column: "AccountID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblAccess_ChannelID",
            //        table: "tblAccess",
            //        column: "ChannelID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AccountName_RefferedBy",
            //        table: "tblAccount",
            //        column: "ReferredBy");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Name_and_Email",
            //        table: "tblAccount",
            //        columns: new[] { "AccountName", "Email" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ProfileId",
            //        table: "tblAccount",
            //        column: "ProfileID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Search",
            //        table: "tblAccount",
            //        columns: new[] { "AccountID", "AccountName", "NationalID", "Email", "ReferredBy" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_BankTrx_Search",
            //        table: "tblBankTrx",
            //        columns: new[] { "Amount", "Identifier", "BankRef" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblBankTrx",
            //        table: "tblBankTrx",
            //        columns: new[] { "BankTrxBatchID", "Identifier", "TrxDate", "BankRef", "RefName", "Amount", "Balance" },
            //        unique: true);

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblBankTrx_BankTrxStateID",
            //        table: "tblBankTrx",
            //        column: "BankTrxStateID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblBankTrx_BankTrxTypeID",
            //        table: "tblBankTrx",
            //        column: "BankTrxTypeID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblBankTrx_PaymentID",
            //        table: "tblBankTrx",
            //        column: "PaymentID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblBankTrxBatch_BankID",
            //        table: "tblBankTrxBatch",
            //        column: "BankID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblBrand_NetworkID",
            //        table: "tblBrand",
            //        column: "NetworkID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblConsoleAccess_ConsoleActionId",
            //        table: "tblConsoleAccess",
            //        column: "ConsoleActionId");

            //    migrationBuilder.CreateIndex(
            //        name: "Error LogDate and CheckID",
            //        table: "tblErrorLog",
            //        columns: new[] { "LogDate", "CheckID" },
            //        descending: new[] { true, false });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_CheckID_ErrorID",
            //        table: "tblErrorLog",
            //        column: "CheckID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblHotTypeCode_HotTypeID",
            //        table: "tblHotTypeCode",
            //        column: "HotTypeID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Limits_Select",
            //        table: "tblLimits",
            //        columns: new[] { "AccountId", "NetworkId" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Date_Method",
            //        table: "tblLog",
            //        columns: new[] { "LogDate", "LogMethod" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Balances",
            //        table: "tblPayment",
            //        columns: new[] { "AccountID", "PaymentTypeID", "PaymentDate" },
            //        descending: new[] { false, false, true });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Balances_Extra",
            //        table: "tblPayment",
            //        column: "PaymentTypeID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_PaymentAccount",
            //        table: "tblPayment",
            //        columns: new[] { "AccountID", "PaymentID" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_PaymentDate",
            //        table: "tblPayment",
            //        column: "PaymentDate");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblPayment_PaymentSourceID",
            //        table: "tblPayment",
            //        column: "PaymentSourceID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblPin",
            //        table: "tblPin",
            //        columns: new[] { "Pin", "BrandID" },
            //        unique: true);

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblPin_BrandID",
            //        table: "tblPin",
            //        column: "BrandID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblPin_PinBatchID",
            //        table: "tblPin",
            //        column: "PinBatchID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblPin_PinStateID",
            //        table: "tblPin",
            //        column: "PinStateID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblPinBatch_PinBatchTypeID",
            //        table: "tblPinBatch",
            //        column: "PinBatchTypeID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Priority",
            //        table: "tblPriority",
            //        columns: new[] { "PriorityID", "Priority" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProduct_BrandId",
            //        table: "tblProduct",
            //        column: "BrandId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProduct_WalletTypeId",
            //        table: "tblProduct",
            //        column: "WalletTypeId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProductField_ProductId",
            //        table: "tblProductField",
            //        column: "ProductId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProductMetaData_ProductId",
            //        table: "tblProductMetaData",
            //        column: "ProductId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProductMetaData_ProductMetaDataTypeId",
            //        table: "tblProductMetaData",
            //        column: "ProductMetaDataTypeId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProfileDiscount_BrandID",
            //        table: "tblProfileDiscount",
            //        column: "BrandID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblProfileDiscount_ProfileID",
            //        table: "tblProfileDiscount",
            //        column: "ProfileID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Balances",
            //        table: "tblRecharge",
            //        columns: new[] { "AccessID", "StateID", "BrandID", "RechargeDate" },
            //        descending: new[] { false, false, false, true });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Balances_Extra",
            //        table: "tblRecharge",
            //        columns: new[] { "StateID", "BrandID" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Mobile",
            //        table: "tblRecharge",
            //        columns: new[] { "RechargeDate", "Mobile" },
            //        descending: new[] { true, false });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_RechargeAccount",
            //        table: "tblRecharge",
            //        column: "AccessID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_RechargeDate",
            //        table: "tblRecharge",
            //        column: "RechargeDate");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_RechargeDateState",
            //        table: "tblRecharge",
            //        columns: new[] { "RechargeDate", "StateID" },
            //        descending: new[] { true, false });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Search",
            //        table: "tblRecharge",
            //        columns: new[] { "RechargeID", "StateID", "BrandID" },
            //        descending: new[] { true, false, false });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_StateID",
            //        table: "tblRecharge",
            //        columns: new[] { "StateID", "AccessID" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblRecharge_BrandID",
            //        table: "tblRecharge",
            //        column: "BrandID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblRecharge_TblPinPinId",
            //        table: "tblRecharge",
            //        column: "TblPinPinId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblRecharge_TblSmsSmsid",
            //        table: "tblRecharge",
            //        column: "TblSmsSmsid");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblRechargePin_PinId",
            //        table: "tblRechargePin",
            //        column: "PinId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservation_AccessID",
            //        table: "tblReservation",
            //        column: "AccessID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservation_BrandId",
            //        table: "tblReservation",
            //        column: "BrandId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservation_RechargeID",
            //        table: "tblReservation",
            //        column: "RechargeID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservation_StateId",
            //        table: "tblReservation",
            //        column: "StateId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservationLog_NewStateId",
            //        table: "tblReservationLog",
            //        column: "NewStateId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservationLog_OldStateId",
            //        table: "tblReservationLog",
            //        column: "OldStateId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblReservationLog_ReservationId",
            //        table: "tblReservationLog",
            //        column: "ReservationId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSelfTopUp_AccessID",
            //        table: "tblSelfTopUp",
            //        column: "AccessID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSelfTopUp_BankTrxID",
            //        table: "tblSelfTopUp",
            //        column: "BankTrxID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSelfTopUp_BrandId",
            //        table: "tblSelfTopUp",
            //        column: "BrandId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSelfTopUp_RechargeID",
            //        table: "tblSelfTopUp",
            //        column: "RechargeID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_SMS_Inbox",
            //        table: "tblSMS",
            //        columns: new[] { "StateID", "Direction" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_SMS_Search",
            //        table: "tblSMS",
            //        columns: new[] { "SMSDate", "StateID", "Mobile", "SMSText", "PriorityID", "SMSID" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_SMSDate",
            //        table: "tblSMS",
            //        column: "SMSDate");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_SMSMobile",
            //        table: "tblSMS",
            //        column: "Mobile");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSMS_PriorityID",
            //        table: "tblSMS",
            //        column: "PriorityID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSMS_SmppID",
            //        table: "tblSMS",
            //        column: "SmppID");

            //    migrationBuilder.CreateIndex(
            //        name: "SmsOut",
            //        table: "tblSMS",
            //        column: "SMSID_In");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSmsRecharge_SmsId",
            //        table: "tblSmsRecharge",
            //        column: "SmsId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_SubMobile",
            //        table: "tblSubscriber",
            //        column: "SubscriberMobile");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSubscriber_AccountID",
            //        table: "tblSubscriber",
            //        column: "AccountID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblSubscriber_BrandID",
            //        table: "tblSubscriber",
            //        column: "BrandID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblTransfer_ChannelID",
            //        table: "tblTransfer",
            //        column: "ChannelID");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblTransfer_PaymentID_From",
            //        table: "tblTransfer",
            //        column: "PaymentID_From");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_tblTransfer_PaymentID_To",
            //        table: "tblTransfer",
            //        column: "PaymentID_To");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AccessID_AgentRef",
            //        table: "tblWebRequest",
            //        columns: new[] { "AccessID", "AgentReference", "WebID", "RechargeID" });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Recharge_AgentRef",
            //        table: "tblWebRequest",
            //        columns: new[] { "AgentReference", "RechargeID" })
            //        .Annotation("SqlServer:Clustered", true);

            //    migrationBuilder.CreateIndex(
            //        name: "IX-StateID",
            //        table: "tblWebRequest",
            //        columns: new[] { "WebID", "StateID" },
            //        unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAccessWeb");

            migrationBuilder.DropTable(
                name: "tblAddress");

            migrationBuilder.DropTable(
                name: "tblBankvPayment");

            migrationBuilder.DropTable(
                name: "tblBundles");

            migrationBuilder.DropTable(
                name: "tblConfig");

            migrationBuilder.DropTable(
                name: "tblConsoleAccess");

            migrationBuilder.DropTable(
                name: "tblErrorLog");

            migrationBuilder.DropTable(
                name: "tblErrorLogContacts");

            migrationBuilder.DropTable(
                name: "tblErrorLogNetworks");

            migrationBuilder.DropTable(
                name: "tblErrorLogSetup");

            migrationBuilder.DropTable(
                name: "tblErrorLogTestType");

            migrationBuilder.DropTable(
                name: "tblHotTypeCode");

            migrationBuilder.DropTable(
                name: "tblLimits");

            migrationBuilder.DropTable(
                name: "tblLimitTypes");

            migrationBuilder.DropTable(
                name: "tblLog");

            migrationBuilder.DropTable(
                name: "tblProductField");

            migrationBuilder.DropTable(
                name: "tblProductMetaData");

            migrationBuilder.DropTable(
                name: "tblprofile_backup");

            migrationBuilder.DropTable(
                name: "tblProfileDiscount");

            migrationBuilder.DropTable(
                name: "tblRechargePin");

            migrationBuilder.DropTable(
                name: "tblRechargePrepaid");

            migrationBuilder.DropTable(
                name: "tblReservationLog");

            migrationBuilder.DropTable(
                name: "tblSelfTopUp");

            migrationBuilder.DropTable(
                name: "tblSelfTopUpState");

            migrationBuilder.DropTable(
                name: "tblSmsRecharge");

            migrationBuilder.DropTable(
                name: "tblStockData");

            migrationBuilder.DropTable(
                name: "tblSubscriber");

            migrationBuilder.DropTable(
                name: "tblTemplate");

            migrationBuilder.DropTable(
                name: "tblTransfer");

            migrationBuilder.DropTable(
                name: "tblWebRequest");

            migrationBuilder.DropTable(
                name: "ztblArchive");

            migrationBuilder.DropTable(
                name: "ztblStats");

            migrationBuilder.DropTable(
                name: "tblConsoleAction");

            migrationBuilder.DropTable(
                name: "tblHotType");

            migrationBuilder.DropTable(
                name: "tblProduct");

            migrationBuilder.DropTable(
                name: "tblProductMetaDataType");

            migrationBuilder.DropTable(
                name: "tblReservation");

            migrationBuilder.DropTable(
                name: "tblBankTrx");

            migrationBuilder.DropTable(
                name: "tblWalletType");

            migrationBuilder.DropTable(
                name: "tblReservationState");

            migrationBuilder.DropTable(
                name: "tblRecharge");

            migrationBuilder.DropTable(
                name: "tblBankTrxBatch");

            migrationBuilder.DropTable(
                name: "tblBankTrxState");

            migrationBuilder.DropTable(
                name: "tblBankTrxType");

            migrationBuilder.DropTable(
                name: "tblPayment");

            migrationBuilder.DropTable(
                name: "tblAccess");

            migrationBuilder.DropTable(
                name: "tblPin");

            migrationBuilder.DropTable(
                name: "tblSMS");

            migrationBuilder.DropTable(
                name: "tblBank");

            migrationBuilder.DropTable(
                name: "tblPaymentSource");

            migrationBuilder.DropTable(
                name: "tblPaymentType");

            migrationBuilder.DropTable(
                name: "tblAccount");

            migrationBuilder.DropTable(
                name: "tblChannel");

            migrationBuilder.DropTable(
                name: "tblBrand");

            migrationBuilder.DropTable(
                name: "tblPinBatch");

            migrationBuilder.DropTable(
                name: "tblPinState");

            migrationBuilder.DropTable(
                name: "tblPriority");

            migrationBuilder.DropTable(
                name: "tblSmpp");

            migrationBuilder.DropTable(
                name: "tblState");

            migrationBuilder.DropTable(
                name: "tblProfile");

            migrationBuilder.DropTable(
                name: "tblNetwork");

            migrationBuilder.DropTable(
                name: "tblPinBatchType");
        }
    }
}
