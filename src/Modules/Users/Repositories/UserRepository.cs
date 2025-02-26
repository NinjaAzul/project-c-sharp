using System;
using System.Collections.Generic;
using System.Linq;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Shared.Exceptions;

namespace Project_C_Sharp.Modules.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDataSource _dataSource;

    public UserRepository(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<User> GetAll()
    {
        return _dataSource.Users;
    }

    public User? GetById(Guid id)
    {
        return _dataSource.Users.FirstOrDefault(u => u.Id == id);
    }

    public User Add(User user)
    {
        _dataSource.Users.Add(user);

        return user;
    }

    public User Update(User user)
    {
        var index = _dataSource.Users.FindIndex(u => u.Id == user.Id);

        _dataSource.Users[index] = user;

        return user;
    }

    public User Delete(Guid id)
    {
        var user = GetById(id)!;
        _dataSource.Users.Remove(user);
        return user;
    }

    public User? GetByEmail(string email)
    {
        return _dataSource.Users.FirstOrDefault(u => u.Email == email);
    }
}