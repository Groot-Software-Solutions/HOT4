using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private HotDbContext _context;
        private IAccessRepository _accessRepository;
        private IAccountRepository _accountRepository;
        private IAccessWebRepository _accessWebRepository;

        public UnitOfWork(HotDbContext context)
        {
            _context = context;
        }
        public IAccessRepository AccessRepository
        {
            get
            {
                if (_accessRepository == null)
                {
                    _accessRepository = new AccessRepository(_context);
                }
                return _accessRepository;
            }
        }
        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accessRepository == null)
                {
                    _accountRepository = new AccountRepository(_context);
                }
                return _accountRepository;
            }
        }
        public IAccessWebRepository AccessWebRepository
        {
            get
            {
                if (_accessWebRepository == null)
                {
                    _accessWebRepository = new AccessWebRepository(_context);
                }
                return _accessWebRepository;
            }
        }




        //public async Task SaveAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
