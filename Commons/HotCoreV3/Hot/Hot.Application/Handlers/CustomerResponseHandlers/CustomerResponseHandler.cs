namespace Hot.Application.Handlers.CustomerResponseHandlers;


public abstract class CustomerResponseHandler
{
    public static void InsufficientFundsErrors(ref string data)
    {
        ChangeData(ref data, "insufficient funds", "Provider Error Code 36180");
        ChangeData(ref data, "insufficient balance", "Provider Error Code 36180");
    }
    public static void GenericNetworkProviderErrors(ref string data)
    { 
      
        ChangeData(ref data, "An error has occurred", "Network Provider Error");
        ChangeData(ref data, "Internal Server Error", "Network Provider Error");
        ChangeData(ref data, "error occurred while sending the request", "Network Provider Error");
        
    }
    public static void ChangeData(ref string data, string HasData, string newData)
    {
        if (data.ToLower().Contains(HasData.ToLower())) data = newData;
    }
    public static void ReplaceData(ref string data, string HasData, string newData)
    {
        if (data.ToLower().Contains(HasData.ToLower())) data = data.Replace(HasData, newData, StringComparison.CurrentCultureIgnoreCase);
    }
}
