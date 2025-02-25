using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers.Interfaces;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;

namespace Project_C_Sharp.Modules.Users.Services.FindAllUsers;

public class FindAllUsersService : IFindAllUsersService
{
    private readonly IUserRepository _userRepository;

    public FindAllUsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<UserBasicInfoResponseDto> FindAll()
    {
        var users = _userRepository.GetAll();

        return users.Select(user => new UserBasicInfoResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}