namespace MyCarearApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    int Save();
}