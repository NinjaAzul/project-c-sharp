using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.CreateUsers.Interfaces;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;

namespace Project_C_Sharp.Modules.Users.Services.CreateUsers;

public class CreateUsersService : ICreateUserService
{
    private readonly IUserRepository _userRepository;

    public CreateUsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public BasicResponseCrudDto Create(CreateUserRequestDto createUserRequestDto)
    {
        var userAlreadyExists = _userRepository.GetByEmail(createUserRequestDto.Email);

        if (userAlreadyExists != null)
        {
            throw new BadRequestException(UsersResource.GetError(UsersErrorsKeys.Email_Already_Exists));
        }


        var user = new User(id: Guid.NewGuid())
        {
            Email = createUserRequestDto.Email,
            Name = createUserRequestDto.Name,
            Password = BCrypt.Net.BCrypt.HashPassword(createUserRequestDto.Password)
        };

        var createdUser = _userRepository.Add(user);

        return new BasicResponseCrudDto { Message = UsersResource.GetMessage(UsersMessagesKeys.User_Created), Id = createdUser.Id };
    }
}