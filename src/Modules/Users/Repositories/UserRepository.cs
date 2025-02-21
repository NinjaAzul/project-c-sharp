
using System;
using System.Collections.Generic;
using System.Linq;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Shared.Exceptions;

namespace Project_C_Sharp.Modules.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> users;

    public UserRepository()
    {
        users = [];
    }

    public IEnumerable<User> GetAll()
    {
        return users;
    }

    public User? GetById(Guid id)
    {
        return users.FirstOrDefault(u => u.Id == id);
    }

    public User Add(User user)
    {
        users.Add(user);

        return user;
    }

    public User Update(User user)
    {
        var index = users.FindIndex(u => u.Id == user.Id);
        if (index != -1)
        {
            users[index] = user;
        }
        else
        {
            throw new BadRequestException("Usuário não encontrado");
        }

        return user;
    }

    public User Delete(Guid id)
    {
        var user = GetById(id)!;
        users.Remove(user);
        return user;
    }

    public User? GetByEmail(string email)
    {
        return users.FirstOrDefault(u => u.Email == email);
    }
}