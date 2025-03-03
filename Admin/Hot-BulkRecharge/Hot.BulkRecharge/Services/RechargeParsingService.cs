using Hot.API.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Hot.BulkRecharge
{
    public class RechargeParsingService
    {
        private readonly decimal maxRecharge;
        private readonly string dataBundleRegex = @"(([F][D|W|M][B])|(UA(05|1|2|5|10|20|50)\b)|(OF(((2|5|8|15)GB)|(Y(5|7|10)D)))|(DAD(D1D|W5D))|(WWU1|WMU[3|5])|(ECR[1|5|10])|([W](([A](D|W|M)[B])|P(D(1|10|35)|M6|WB18)))|([D])(([W][B])|([D]([B]|([M][B]))))(([1|2][G])+|(\\d+))|(S(DB(20|35|45)|M(B(220|450)|S(D(15|5(|0))|W(2(00|50)|500)))|WB(140|300|65)))|(PWB((8|15|25|50)|U(10|14|S7|S20|S28)))\b)";
        private readonly string mobileRegex = @"(0|(\+|)263|)(7(7|8|1|3)\d|86(44|77))(\d\d\d\d\d\d)$";
        private readonly string amountRegex = @"^\d*(\.\d\d?\d?\b)*$";
        public RechargeParsingService(decimal maxRecharge,string? dataBundleRegex=null,string? mobileRegex = null)
        {
            this.maxRecharge = maxRecharge;
            if (dataBundleRegex is not null )this.dataBundleRegex =dataBundleRegex;
            if (mobileRegex is not null) this.mobileRegex = mobileRegex;

        }

        public List<BulkRecharge> ParseRecharges(string text, bool uSDRecharges)
        {
            var recharges = new List<BulkRecharge>();
            foreach (var item in text.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                string delimiter = IdentifyDelimiter(item);
                var details = item.Trim().Split(delimiter);
                if (details.Length <= 1) goto ErrorInData;
                details[0] = SanitizeData(details[0]);
                details[1] = SanitizeData(details[1]);

                if (!(IsValidPhoneNumber(details[0]) && IsValidAmountOrProductCode(details[1].Trim()))) goto ErrorInData;

                var recharge = new BulkRecharge(GetNumber(details[0]), GetAmount(details[1]), IsValidProductCode(details[1]),true,item);

                if (!IsValidProductCode(details[1]) && recharge.Amount > maxRecharge)
                {
                    var currentmax = maxRecharge;
                    while (recharge.Amount > currentmax)
                    {
                        var temprecharge = new BulkRecharge(recharge.Mobile, currentmax.ToString(), false);
                        recharges.Add(temprecharge);
                        recharge.Amount -= currentmax;
                        currentmax -= 10;
                    }
                }
                recharge.IsUSD = uSDRecharges;
                recharges.Add(recharge);

                continue;
                ErrorInData:
                recharges.Add(new("", "", false, false, item));

            }
            return recharges;
        }

        private string SanitizeData(string data)
        {
            return data.Replace(" ", "").Replace(",","");
        }

        private string GetAmount(string Amount)
        {
            var regex = new Regex(@$"^\d*(\.\d\d\d?\b)*$|{dataBundleRegex}");
            var match = regex.Match(Amount);
            var result = match.Success ? match.Value : Amount;
            return result;
        }

        public string GetNumber(string number)
        {
            var regex = new Regex(@$"{mobileRegex}");
            var match = regex.Match(number);
            var result = match.Success ? match.Value : number;
            return result.ToMobileNumber();
        }

        public  bool IsValidProductCode(string ProductCode)
        {
            return new Regex(@$"^{dataBundleRegex}").IsMatch(ProductCode);
        }

        public  bool IsValidAmountOrProductCode(string amountOrProductCode)
        {
            return new Regex(@$"{amountRegex}|{dataBundleRegex}").IsMatch(amountOrProductCode);
        }

        public bool IsValidPhoneNumber(string number)
        {
            return new Regex(@$"{mobileRegex}").IsMatch(number);
        }

        public static string IdentifyDelimiter(string item)
        { 
            if (item.Contains(";")) return ";";
            if (item.Contains(",")) return ",";
            if (item.Contains('\t')) return "\t";
            if (item.Contains("#")) return "#";
            return " ";
        }
    }
}
