using System;

namespace Project_C_Sharp.Modules.Users.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public User()
    {
        Id = Guid.NewGuid();
    }

    public User(Guid id)
    {
        Id = id;
    }
}
