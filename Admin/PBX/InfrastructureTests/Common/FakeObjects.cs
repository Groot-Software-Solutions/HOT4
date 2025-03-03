using Application.Common.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureTests.Common
{
    public class FakeObjects
    {
         public static Mock<IDbContext> GetDbContext()
        {
            var context = new Mock<IDbContext>();
            return context;
        }
    }
}
