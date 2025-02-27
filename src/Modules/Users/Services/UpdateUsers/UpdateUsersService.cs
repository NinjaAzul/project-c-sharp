using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers.Interfaces;
using Project_C_Sharp.Modules.UpdateUser.Dto.Request;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Infra.DataBase.UnitOfWork;

namespace Project_C_Sharp.Modules.Users.Services.UpdateUsers;

public class UpdateUsersService : IUpdateUsersService
{
    private readonly IUserRepository _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public UpdateUsersService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BasicResponseCrudDto> Update(Guid id, UpdateUserRequestDto updateUserRequestDto)
    {
        var existingUser = await _userRepository.GetById(id);

        if (existingUser is null)
        {
            throw new NotFoundException(UsersResource.GetError(UsersErrorsKeys.User_NotFound));
        }

        existingUser.UpdateName(updateUserRequestDto.Name);

        var updatedUser = await _userRepository.Update(existingUser);

        await _unitOfWork.CompleteAsync();

        return new BasicResponseCrudDto { Message = UsersResource.GetMessage(UsersMessagesKeys.User_Updated), Id = updatedUser.Id };
    }
}