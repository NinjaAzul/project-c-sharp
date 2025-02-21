using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindByIdUsers.Interfaces;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;

namespace Project_C_Sharp.Modules.Users.Services.FindByIdUsers;

public class FindByIdUsersService : IFindByIdUsersService
{
    private readonly IUserRepository _userRepository;

    public FindByIdUsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public UserBasicInfoResponseDto FindById(Guid id)
    {
        var user = _userRepository.GetById(id);

        if (user is null)
        {
            throw new NotFoundException(UsersResource.GetError(UsersErrorsKeys.User_NotFound));
        }

        return new UserBasicInfoResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}