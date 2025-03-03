using FluentAssertions;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using NSubstitute;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Assertions
{
    public static class RechargeAssertions
    {
        public static async Task AssertRechargeResult(bool expected, byte resultState, IDbContext dbcontext, Recharge recharge, OneOf<RechargeResult, RechargeServiceNotFoundException> result, string RawResponseData)
        {
            result.IsT0.Should().BeTrue();
            result.AsT0.Recharge.Should().Be(recharge);
            result.AsT0.Successful.Should().Be(expected);
            await dbcontext.Received().Recharges.UpdateAsync(Arg.Is<Recharge>(r => r.StateId == resultState)); 
            await dbcontext.Received().RechargePrepaids.UpdateAsync(Arg.Is<RechargePrepaid>(r => r.Narrative == RawResponseData));

        }
    }
}
