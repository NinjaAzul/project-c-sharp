using Project_C_Sharp.Modules.Users.Entities;

namespace Project_C_Sharp.Modules.Users.Repositories.Interfaces;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
    User? GetById(Guid id);
    User Add(User user);
    User Update(User user);
    User Delete(Guid id);

    User? GetByEmail(string email);
}

