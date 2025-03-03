using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IDbContext
    {
        public IDbContextTable<SMS> SMSs { get; set; } 

    }
}
