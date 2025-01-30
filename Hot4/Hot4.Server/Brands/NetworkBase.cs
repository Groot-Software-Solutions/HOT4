//using Hot4.Core.Helper;
//using Hot4.DataModel.Models;
//using Hot4.Service.Abstract;
//using Hot4.ViewModel;
//using Microsoft.Data.SqlClient;

//namespace Hot4.Server.Brands
//{
//    public abstract class NetworkBase
//    {
//        protected string ApplicationName { get; set; }
//        protected SqlConnection SqlConn { get; set; }
//        protected string ServiceEndpoint { get; set; }
//        public bool IsTestMode { get; set; }
//        public bool WebServiceOrCorporate { get; set; }
//        public string ReferencePrefix { get; set; }

//        protected NetworkBase(SqlConnection sqlConn, string applicationName, string serviceEndpoint, bool isTestMode, bool webServiceOrCorporate, string referencePrefix = "999")
//        {
//            ApplicationName = applicationName;
//            SqlConn = sqlConn;
//            ServiceEndpoint = serviceEndpoint;
//            IsTestMode = isTestMode;
//            WebServiceOrCorporate = webServiceOrCorporate;
//            ReferencePrefix = referencePrefix;
//        }

//        public virtual string GetReference(string mobile)
//        {
//            return ReferencePrefix + "+" + mobile + "+" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
//        }

//        protected void ApplyDiscountRules(RechargeModel rechargeModel)
//        {
//            if (!WebServiceOrCorporate)
//            {
//                var limit = new LimitDiscountTo5Percent();
//                limit.Apply(rechargeModel);
//            }
//        }
//        public RechargePrepaid CreateRechargeObject(RechargeModel  rechargeModel)
//        {
//            var iRechargePrepaid = new RechargePrepaid
//            {
//                RechargeId = rechargeModel.RechargeId,
//                DebitCredit = rechargeModel.Amount >= 0,
//                Reference = GetReference(rechargeModel.Mobile),
//                FinalBalance = -1,
//                InitialBalance = -1,
//                Narrative = "", // Explicit ,ade empty
//                ReturnCode = "-1" // Explicit change into string 
//            };
//            iRechargePrepaid.Narrative = iRechargePrepaid.DebitCredit ? "Pending" : "Debit Pending";
//            return iRechargePrepaid;
//        }

//        public abstract ServiceRechargeResponse RechargePrepaid(Recharge recharge, int numberOfDays = 90);
//        public abstract INetworkClient GetNetworkClient(int timeout = 30000);
//    }
//}
