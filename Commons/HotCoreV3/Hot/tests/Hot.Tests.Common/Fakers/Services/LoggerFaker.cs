using Microsoft.Extensions.Logging;
using NSubstitute;
using System;

namespace Hot.Tests.Common.Fakers.Services;
public static class LoggerFaker
{ 
    public static ILogger<T> GetLogger<T>()
    {
        return Substitute.For<ILogger<T>>();
    }
}

