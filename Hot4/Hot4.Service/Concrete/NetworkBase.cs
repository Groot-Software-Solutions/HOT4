using Hot4.Core.Helper;
using Hot4.DataModel.Models;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Concrete
{
    public abstract class NetworkBase<T>
    {

        protected string ApplicationName;
        protected string ServiceEndpoint;
        
        public bool IsTestMode { get; set; }
        public bool WebServiceOrCorporate { get; set; }
        public string ReferencePrefix { get; set; }

        public NetworkBase(string serviceEndpoint , bool isTestMode , string applicationName)
        {
            ServiceEndpoint = serviceEndpoint;
            IsTestMode = isTestMode;
            ApplicationName = applicationName;
        }
        public RechargePrepaidModel CreateRechargeObject(RechargeModel rechargeModel)
        {
            RechargePrepaidModel rechargePrepaidModel = new RechargePrepaidModel();
            rechargePrepaidModel.RechargeId = rechargeModel.RechargeId;
            rechargePrepaidModel.DebitCredit = rechargeModel.Amount >= 0;
            rechargePrepaidModel.Reference = GetReference(rechargeModel.Mobile);
            rechargePrepaidModel.FinalBalance = -1;
            rechargePrepaidModel.InitialBalance = -1;
            rechargePrepaidModel.Narrative = rechargePrepaidModel.DebitCredit ? "Pending" : "Debit Pending";
            rechargePrepaidModel.ReturnCode = "-1";
            return rechargePrepaidModel;
        }


        public virtual string GetReference(string mobile)
        {
            return ReferencePrefix + "+" + mobile + "+" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
        }

        protected void ApplyDiscountRules(RechargeModel rechargeModel)
        {
            bool WebServiceOrCorporate = false;

            if (!WebServiceOrCorporate)
            {
                var limit = new LimitDiscountTo5Percent();
                limit.Apply(rechargeModel);
            }
        }
        public abstract INetworkClient<T> GetNetworkClient(int timeout = 30000);

        public abstract ServiceRechargeResponse RechargePrepaid(RechargeModel rechargeModel, int numberOfDays = 90);


    }


}

