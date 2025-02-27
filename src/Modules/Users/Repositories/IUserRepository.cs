using Project_C_Sharp.Modules.Users.Entities;

namespace Project_C_Sharp.Modules.Users.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(Guid id);
    Task<User> Add(User user);
    Task<User> Update(User user);
    Task<User> Delete(Guid id);

    Task<User?> GetByEmail(string email);
}

