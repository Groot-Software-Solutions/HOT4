using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext;
public static class DbContextFaker
{
    public static IDbContext Mock()
    {
        return Substitute.For<IDbContext>();
    }
    public static IDbContext GetDbContext()
    {
        return Substitute.For<IDbContext>();
    }
}


