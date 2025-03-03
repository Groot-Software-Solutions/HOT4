using Hot.Ecocash.Application.Common.Exceptions;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using System;
using Hot.Application.Common.Exceptions;

namespace Hot.Ecocash.Application.Common
{
    public class ServiceOptions
    {
        public string EcoCashURL { get; internal set; } = "";
        public string HotRechargeReturnURL { get; internal set; } = "";
        public string Username { get; internal set; } = "";
        public string Password { get; internal set; } = "";
        public string MechantCode { get; internal set; }= "";
        public string MerchantPin { get; internal set; } = "";
        public string MerchantNumber { get; internal set; } = "";

        public ServiceOptions() { }

        public ServiceOptions(string ecoCashURL, string hotRechargeReturnURL, string username, string password, string mechantCode, string merchantPIN, string merchantNumber)
        {
            EcoCashURL = ecoCashURL;
            HotRechargeReturnURL = hotRechargeReturnURL;
            Username = username;
            Password = password;
            MechantCode = mechantCode;
            MerchantPin = merchantPIN;
            MerchantNumber = merchantNumber;
        }

        public ServiceOptions(NameValueCollection collection)
        {
            if (collection.Count == 0) throw new AppException("Set Options", "Invalid Configuration");
            var listOfKeys = collection.AllKeys.ToList();
            listOfKeys.ForEach(key =>
            {
                SetOption(key ?? "", collection);
            });
        }

        public ServiceOptions(IConfigurationSection section)
        { 
            EcoCashURL = section.GetSection("EcoCashURL").Value; 
            HotRechargeReturnURL = section.GetSection("hotRechargeReturnURL").Value;
            Username = section.GetSection("Username").Value;
            Password = section.GetSection("Password").Value;
            MechantCode = section.GetSection("MerchantCode").Value;
            MerchantPin = section.GetSection("MerchantPin").Value;
            MerchantNumber = section.GetSection("MerchantNumber").Value;
        }

        private void SetOption(string key, NameValueCollection collection)
        {
            if (key == "EcoCashURL")
                EcoCashURL = collection["EcoCashURL"] ?? "";

            if (key == "HotRechargeReturnURL")
                HotRechargeReturnURL = collection["hotRechargeReturnURL"] ?? "";

            if (key == "Username")
                Username = collection["Username"] ?? "";

            if (key == "Password")
                Password = collection["Password"]??"";

            if (key == "MerchantCode")
                MechantCode = collection["MerchantCode"] ?? "";

            if (key == "MerchantPIN")
                MerchantPin = collection["MerchantPIN"] ?? "";

            if (key == "MerchantNumber")
                MerchantNumber = collection["MerchantNumber"] ?? "";

        }

    }
}
