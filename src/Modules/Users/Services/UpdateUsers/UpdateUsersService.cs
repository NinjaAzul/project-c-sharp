using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers.Interfaces;
using Project_C_Sharp.Modules.UpdateUser.Dto.Request;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;

namespace Project_C_Sharp.Modules.Users.Services.UpdateUsers;

public class UpdateUsersService : IUpdateUsersService
{
    private readonly IUserRepository _userRepository;

    public UpdateUsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public BasicResponseCrudDto Update(Guid id, UpdateUserRequestDto updateUserRequestDto)
    {
        var existingUser = _userRepository.GetById(id);

        if (existingUser is null)
        {
            throw new NotFoundException(UsersResource.GetError(UsersErrorsKeys.User_NotFound));
        }

        existingUser.UpdateName(updateUserRequestDto.Name);

        var updatedUser = _userRepository.Update(existingUser);

        return new BasicResponseCrudDto { Message = UsersResource.GetMessage(UsersMessagesKeys.User_Updated), Id = updatedUser.Id };
    }
}