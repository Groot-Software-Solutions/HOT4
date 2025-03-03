using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hot.Web.Provider.Interfaces
{
    public enum ReturnCode : int
    {
        Pending = -1,
        Unknown = 0,
        Success = 1,
        Failed = 2,
        InsufficientFunds = 3,
        InvalidRequest = 4,
        IncorrectPin = 5,
        InvalidNumber = 6,
        InvalidAmount = 7,
        USSDError = 8,
        DongleError = 9,
        SignalError = 10,
        InvalidUSSDMessage = 11,
        RechargeError = 12,
        Timeout = 13,
        Duplicate = 14,
        FailedAppKey = 98,
        FailureApplication = 99
    };

    [Serializable]
    public class ReturnObject
    {
        
        public int ReturnCode;
        public string ReturnMsg;
        public decimal ReturnValue;


    }
}