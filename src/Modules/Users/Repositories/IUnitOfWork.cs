using Project_C_Sharp.Modules.Users.Repositories.Interfaces;

namespace Project_C_Sharp.Infra.DataBase.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> CompleteAsync();
    }
}