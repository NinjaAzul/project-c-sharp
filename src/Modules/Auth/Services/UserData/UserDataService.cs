
using Project_C_Sharp.Modules.Auth.Services.Me.Interfaces;

using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;


namespace Project_C_Sharp.Modules.Auth.Services.UserData;

public class UserDataService : IUserDataService
{
    private readonly IUserRepository _userRepository;

    public UserDataService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public UserBasicInfoResponseDto Me(Guid id)
    {
        var user = _userRepository.GetById(id);

        if (user == null)
        {
            throw new NotFoundException(UsersResource.GetError(UsersErrorsKeys.User_NotFound));
        }

        return new UserBasicInfoResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,

        };
    }
}
