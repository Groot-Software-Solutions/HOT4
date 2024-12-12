using Hot4.Core.Enums;

namespace Hot4.Core.DataViewModels
{
    public class SecurityDBRole
    {
        public SecurityDBRoleTypes RoleID { get; set; }
        public string RoleName { get; set; }
        public int PrincipalID { get; set; }
        public List<SecurityDBRolePermission> Permissions { get; set; }
    }

    public class SecurityDBRolePermission
    {
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string PermissionName { get; set; }
        public string PermissionType { get; set; }
        public string StateDescription { get; set; }
        public bool Active { get; set; } = true;
    }

    public class User
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public List<SecurityDBRole> Roles { get; set; } = new List<SecurityDBRole>();
    }

    public static class SecurityDBRolePermissionAdapter
    {
        public static List<SecurityDBRolePermission> BaseConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xAccess_Admin_Select" },
        new SecurityDBRolePermission { ObjectName = "xAccess_List" },
        new SecurityDBRolePermission { ObjectName = "xAccess_ListDeleted" },
        new SecurityDBRolePermission { ObjectName = "xAccess_Select" },
        new SecurityDBRolePermission { ObjectName = "xAccess_SelectCode" },
        new SecurityDBRolePermission { ObjectName = "xAccess_SelectLogin" },
        new SecurityDBRolePermission { ObjectName = "xAccess_SelectLogin2" },
        new SecurityDBRolePermission { ObjectName = "xAccessWeb_Select" },
        new SecurityDBRolePermission { ObjectName = "xAccount_Search" },
        new SecurityDBRolePermission { ObjectName = "xAccount_Select" },
        new SecurityDBRolePermission { ObjectName = "xAddress_Select" },
        new SecurityDBRolePermission { ObjectName = "xBank_List" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_FindDuplicate" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_GetFromBankRef" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_GetFromTrxID" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_GetFromvPayment" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_Insert" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_List" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_ListPendingEcoCash" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_Pending_EcoCash" },
        new SecurityDBRolePermission { ObjectName = "xBankTrxBatch_GetCurrentBatch" },
        new SecurityDBRolePermission { ObjectName = "xBankTrxBatch_List" },
        new SecurityDBRolePermission { ObjectName = "xBankTrxState_List" },
        new SecurityDBRolePermission { ObjectName = "xBankTrxType_List" },
        new SecurityDBRolePermission { ObjectName = "xBankvPayment_Select" },
        new SecurityDBRolePermission { ObjectName = "xBranch_List_EasyLink" },
        new SecurityDBRolePermission { ObjectName = "xBrand_Identify" },
        new SecurityDBRolePermission { ObjectName = "xBrand_List" },
        new SecurityDBRolePermission { ObjectName = "xChannel_List" },
        new SecurityDBRolePermission { ObjectName = "xErrorChecks" },
        new SecurityDBRolePermission { ObjectName = "xErrorLog_List" },
        new SecurityDBRolePermission { ObjectName = "xHotType_Identify" },
        new SecurityDBRolePermission { ObjectName = "xNetwork_Identify" },
        new SecurityDBRolePermission { ObjectName = "xPayment_List" },
        new SecurityDBRolePermission { ObjectName = "xPaymentSource_List" },
        new SecurityDBRolePermission { ObjectName = "xPaymentType_List" },
        new SecurityDBRolePermission { ObjectName = "xPin_List" },
        new SecurityDBRolePermission { ObjectName = "xPin_Loaded" },
        new SecurityDBRolePermission { ObjectName = "xPin_Recharge" },
        new SecurityDBRolePermission { ObjectName = "xPin_Stock" },
        new SecurityDBRolePermission { ObjectName = "xPinBatch_List" },
        new SecurityDBRolePermission { ObjectName = "xPinBatchType_List" },
        new SecurityDBRolePermission { ObjectName = "xProcessState_List" },
        new SecurityDBRolePermission { ObjectName = "xProfile_List" },
        new SecurityDBRolePermission { ObjectName = "xProfileDiscount_Discount" },
        new SecurityDBRolePermission { ObjectName = "xProfileDiscount_List" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_AggregatorSelect" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Find" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Find2" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Pending" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Pending_Africom" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Pending_Econet" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Pending_NetOne" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Pending_Other" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Select" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Select2" },
        new SecurityDBRolePermission { ObjectName = "xRecharge_Web_Duplicate" },
        new SecurityDBRolePermission { ObjectName = "xRechargePrepaid_Select" },
        new SecurityDBRolePermission { ObjectName = "xRechargeSMS_Select" },
        new SecurityDBRolePermission { ObjectName = "xSMS_Inbox" },
        new SecurityDBRolePermission { ObjectName = "xSMS_List" },
        new SecurityDBRolePermission { ObjectName = "xSMS_ListOut" },
        new SecurityDBRolePermission { ObjectName = "xSMS_Outbox" },
        new SecurityDBRolePermission { ObjectName = "xSMS_Search" },
        new SecurityDBRolePermission { ObjectName = "xState_List" },
        new SecurityDBRolePermission { ObjectName = "xSubscriber_List" },
        new SecurityDBRolePermission { ObjectName = "xSubscriber_Select" },
        new SecurityDBRolePermission { ObjectName = "xTemplate_Select" },
        new SecurityDBRolePermission { ObjectName = "xTransactions_Date" },
        new SecurityDBRolePermission { ObjectName = "xTransactions_Date_Access" },
        new SecurityDBRolePermission { ObjectName = "xWebRequest_Select" }
    };

        public static List<SecurityDBRolePermission> LoadStatmentConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xBankTrx_Save" },
        new SecurityDBRolePermission { ObjectName = "xBankTrxBatch_Insert" }
    };

        public static List<SecurityDBRolePermission> ProcessStatmentConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xBankTrx_UpdatePaymentID" },
        new SecurityDBRolePermission { ObjectName = "xBankTrx_UpdateState" },
        new SecurityDBRolePermission { ObjectName = "xBankvPayment_Update" },
        new SecurityDBRolePermission { ObjectName = "xPayment_Save" },
        new SecurityDBRolePermission { ObjectName = "xTransfer_Save" }
    };

        public static List<SecurityDBRolePermission> MakePaymentConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xPayment_Save" }
    };

        public static List<SecurityDBRolePermission> BulkSendConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xSMS_BulkSend" },
        new SecurityDBRolePermission { ObjectName = "xSMS_BulkSmsSend" },
        new SecurityDBRolePermission { ObjectName = "xSMS_EmailAggregators" },
        new SecurityDBRolePermission { ObjectName = "xSMS_EmailCorporates" }
    };

        public static List<SecurityDBRolePermission> AdminConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xConfig_Select" },
        new SecurityDBRolePermission { ObjectName = "xErrorLog_Save" },
        new SecurityDBRolePermission { ObjectName = "xGraph_RechargeState" },
        new SecurityDBRolePermission { ObjectName = "xPWD" }
    };

        public static List<SecurityDBRolePermission> AddAccessUserConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xAccess_Save" },
        new SecurityDBRolePermission { ObjectName = "xAccess_Save2" }
    };

        public static List<SecurityDBRolePermission> ModifyAccessConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xAccess_Delete" },
        new SecurityDBRolePermission { ObjectName = "xAccess_PasswordChange" },
        new SecurityDBRolePermission { ObjectName = "xAccess_PasswordChange2" },
        new SecurityDBRolePermission { ObjectName = "xAccess_UnDelete" },
        new SecurityDBRolePermission { ObjectName = "xAccessWeb_Save" }
    };

        public static List<SecurityDBRolePermission> UpdateAccountConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xAccount_Save" },
        new SecurityDBRolePermission { ObjectName = "xAddress_Save" }
    };

        public static List<SecurityDBRolePermission> LoadPinsConfiguration = new()
        {
        new SecurityDBRolePermission { ObjectName = "xPin_Insert" },
        new SecurityDBRolePermission { ObjectName = "xPinBatch_Insert" }
    };
    }
}
