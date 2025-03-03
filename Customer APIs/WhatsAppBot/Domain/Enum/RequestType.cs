using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enum
{
    public enum RequestType
    {
         Unknown =0
        , Airtime=1
        , PinReset=2
        , Help=3
        , HelpEcocash = 4 
        , HelpBank = 5
        , HelpBalance = 6
        , HelpRegistration = 7   
        , Greeting = 8 
        , SigningOff = 9
        , HelpRecharge =10,
        HelpPinReset = 11
    }
}
