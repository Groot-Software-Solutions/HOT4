using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace App
{
   public  class RechargeParsingService
    { 
        private readonly decimal maxRecharge;

        public RechargeParsingService(decimal maxRecharge)
        {
            this.maxRecharge = maxRecharge;
        }

        public List<BulkRecharge> ParseRecharges(string text)
        {
            var recharges = new List<BulkRecharge>();
            foreach (var item in text.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                string delimiter = IdentifyDelimiter(item);
                var details = item.Trim().Split(delimiter);
                if (details.Length > 1)
                {
                    if (IsValidPhoneNumber(details[0]) && IsValidAmountOrProductCode(details[1]))
                    {
                        var recharge = new BulkRecharge(GetNumber(details[0]), details[1], IsValidProductCode(details[1]));

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
                        recharges.Add(recharge);
                    }
                }
            }
            return recharges;
        }

        private string GetNumber(string number)
        {
            return number.StartsWith("7") ? $"0{number}" : number;
        }

        private bool IsValidProductCode(string ProductCode)
        {
            return new Regex(@"^(([F][D|W|M][B])|([W](([A](D|W|M)[B])|P(D(1|10|35)|M6|WB18)))|([D])(([W][B])|([D]([B]|([M][B]))))(([1|2][G])+|(\d+))|(S(DB(20|35|45)|M(B(220|450)|S(D(15|5(|0))|W(2(00|50)|500)))|WB(140|300|65)))|PWB(8|15|25|50)\b)").IsMatch(ProductCode);
        }

        private bool IsValidAmountOrProductCode(string amountOrProductCode)
        { 
            return new Regex(@"^\d*(\.\d\d?\b)*$|(([F][D|W|M][B])|([W](([A](D|W|M)[B])|P(D(1|10|35)|M6|WB18)))|([D])(([W][B])|([D]([B]|([M][B]))))(([1|2][G])+|(\d+))|(S(DB(20|35|45)|M(B(220|450)|S(D(15|5(|0))|W(2(00|50)|500)))|WB(140|300|65)))|PWB(8|15|25|50)\b)").IsMatch(amountOrProductCode);
        }

        private bool IsValidPhoneNumber(string number)
        {
            return new Regex(@"^(0|(\+|)263|)(7(7|8|1|3)\d|86(44|77))(\d\d\d\d\d\d)$").IsMatch(number);
        }

        private string IdentifyDelimiter(string item)
        {
            if (item.Contains(";")) return ";";
            if (item.Contains(" ")) return " ";
            if (item.Contains('\t')) return "\t";
            if (item.Contains("#")) return "#";
            return ",";
        }
    }
}
