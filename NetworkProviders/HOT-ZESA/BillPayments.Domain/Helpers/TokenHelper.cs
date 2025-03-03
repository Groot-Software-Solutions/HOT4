using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Helpers
{
    public static class TokenHelper
    {
        public static bool IsValidInput(string Data,int FieldIndex =0,string Separator = "|" ,bool Numeric = true,int Length=0)
        {
            if (string.IsNullOrEmpty(Data)) return false;
            if (Data.Split("|").Count() < FieldIndex + 1) return false;
            if ((Data.Split(Separator)[FieldIndex]).IsNumeric() !=Numeric) return false;
            if (Length != 0 && Data.Length != Length) return false;
            return true;
        }
         
        public static decimal GetTaxAmount(string data)
        { 
            return IsValidInput(data,5) 
                ? Convert.ToDecimal(data.Split("|")[5]) / 100 
                : 0;
        }

        public static decimal GetNetAmount(string data)
        { 
            return IsValidInput(data, 4) 
                ? Convert.ToDecimal(data.Split("|")[4]) / 100 
                :0;
        }

        public static decimal GetLevy(string data)
        {
            return IsValidInput(data, 2) 
                ? Convert.ToDecimal(data.Split("|")[2]) / 100
                : 0;
        }

        public static decimal GetArrears(string data)
        { 
            return IsValidInput(data, 2) 
                ?  IsDoubleDebt(data)
                    ? GetArrears(data.Split("#")[0]) + GetArrears(data.Split("#")[1])
                    : Convert.ToDecimal(data.Split("|")[2]) / 100
                :0;
        }

        public static string GetToken(string data)
        {
            return IsValidInput(data, 0) 
                ? data.Split("|")[0]
                : "";
        }

        public static string GetFormattedToken(string data)
        {
            return FormattedToken(GetToken(data));
        }

        public static decimal GetUnits(string data)
        {
            return IsValidInput(data, 1) 
                ? Convert.ToDecimal(data.Split("|")[1])
                : 0;
        }

        public static bool IsDoubleToken(string data)
        {
            return data.Split("#").Count() > 1;
        }
         
        public static bool IsDoubleDebt(string data)
        {
            return data.Split("#").Count() > 1;
        }

        public static string GetZESAReference(string data)
        {
            return IsValidInput(data, 3, Numeric:false) 
                ? data.Split("|")[3]
                : "";
        }

        public static string FormattedToken(string data)
        {
            return IsValidInput(data, Length: 20)
                ? $"{ data.Substring(0,4)} { data.Substring(4, 4)} { data.Substring(8, 4)} { data.Substring(12, 4)} { data.Substring(16, 4)}"
                : data;
        }
    }
}
