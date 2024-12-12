namespace Hot4.Repository.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IAccessRepository AccessRepository { get; }
        IAccountRepository AccountRepository { get; }
        IAccessWebRepository AccessWebRepository { get; }
        // Task SaveAsync();
    }
}
