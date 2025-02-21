using Project_C_Sharp.Modules.Users.Entities;

namespace Project_C_Sharp.Modules.Auth.Services.Token;

public interface ITokenService
{
    string GenerateToken(User user);
}