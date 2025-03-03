namespace Hot.Application.Common.Extensions;
public static class ResponseExtensions
{
    public static string GetCustomerResponseFromRawResponse(this string response)
    {
        return SanitizeCustomerMessageString(response);
    }

    private static string SanitizeCustomerMessageString(string data)
    {
        var serviceType = typeof(ICustomerResponseHandler);
        var AssemblyMarker = typeof(ICustomerResponseHandler);

        var CustomerResponseHandlers = AssemblyMarker.Assembly.ExportedTypes
             .Where(x => serviceType.IsAssignableFrom(x)
                        && !x.IsInterface && !x.IsAbstract)
             .Select(y => { return Activator.CreateInstance(y); })
             .Cast<ICustomerResponseHandler>()
             .ToList();
        CustomerResponseHandlers.ForEach(x => x.Handle(ref data));
        return data;
    }

    

}


