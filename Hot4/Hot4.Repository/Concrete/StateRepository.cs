﻿using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class StateRepository : RepositoryBase<TblState>, IStateRepository
    {
        public StateRepository(HotDbContext context) : base(context) { }
        public async Task<TblState?> GetState(byte stateId)
        {
            return await GetById(stateId);
        }

        public async Task<List<TblState>> ListState()
        {
            return await GetAll().ToListAsync();
        }
    }
}
