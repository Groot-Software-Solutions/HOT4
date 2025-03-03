namespace Hot.Application.Handlers.CustomerResponseHandlers;

public class LesothoZTEErrorCustomerResponseHandler : CustomerResponseHandler, ICustomerResponseHandler
{
    public void Handle(ref string data)
    {
        GenericNetworkProviderErrors(ref data);
        ChangeData(ref data, "[S-ETL-BILLING-00014] [Other errors for WsRecharge4ETL.]", "Network Provider Error");

    } 
}