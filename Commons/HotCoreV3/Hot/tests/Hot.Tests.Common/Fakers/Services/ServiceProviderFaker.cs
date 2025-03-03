
using NSubstitute;
using System;

namespace Hot.Tests.Common.Fakers.Services;

public class ServiceProviderFaker
{
    public static IServiceProvider Mock()
    {
        return Substitute.For<IServiceProvider>();
    }
} 
public static class ServiceProviderFakerExtensions
{ 
    public static IServiceProvider Gives<T>(this IServiceProvider provider,T service)
    {
        provider.GetService(typeof(T)).Returns(service);
        return provider;
    }
}
