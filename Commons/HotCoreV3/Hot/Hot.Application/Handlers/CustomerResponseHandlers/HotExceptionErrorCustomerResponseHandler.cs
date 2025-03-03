namespace Hot.Application.Handlers.CustomerResponseHandlers;

public class HotExceptionErrorCustomerResponseHandler : CustomerResponseHandler, ICustomerResponseHandler
{
    public void Handle(ref string data)
    {  
        ReplaceData(ref data, "Not Allowed To Sell Exception ", ""); 
    }


}