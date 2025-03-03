using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hot.Econet.Prepaid.Models;

public struct LoadAirtimeRequest
{
    public string MSISDN;
    public string ProviderCode;
    public int AccountType;
    public int Currency;
    public int Amount;
    public string Reference;

}




