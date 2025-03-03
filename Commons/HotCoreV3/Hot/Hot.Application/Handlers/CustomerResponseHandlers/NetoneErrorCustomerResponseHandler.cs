namespace Hot.Application.Handlers.CustomerResponseHandlers;
public class NetoneErrorCustomerResponseHandler : CustomerResponseHandler, ICustomerResponseHandler
{
    public void Handle(ref string data)
    {
        GenericNetworkProviderErrors(ref data);
        ChangeData(ref data, "Recharge(CCA_E) Error# 5003", "Mobile is not activated on network");
        ChangeData(ref data, "Recharge Error# Object reference not set", "Network Provider Error");
        ChangeData(ref data, "NetoneAPI Error:", "Network Provider Error"); 
        ChangeData(ref data, "Invalid x-access-code or x-access-password invalid.", "Network Provider Error");
         
        ReplaceData(ref data, "e-top up", "");

    }

    
}
