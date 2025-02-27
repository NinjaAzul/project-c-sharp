using Project_C_Sharp.Infra.DataBase.EntityFrameworkCore;
using Project_C_Sharp.Modules.Users.Repositories;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;

namespace Project_C_Sharp.Infra.DataBase.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public IUserRepository Users { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
