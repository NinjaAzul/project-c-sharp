using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.DeleteUsers.Interfaces;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Infra.DataBase.UnitOfWork;

namespace Project_C_Sharp.Modules.Users.Services.DeleteUsers;

public class DeleteUsersService : IDeleteUsersService
{
    private readonly IUserRepository _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public DeleteUsersService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BasicResponseCrudDto> Delete(Guid id)
    {
        var existingUser = await _userRepository.GetById(id);

        if (existingUser is null)
        {
            throw new NotFoundException(UsersResource.GetError(UsersErrorsKeys.User_NotFound));
        }

        var deletedUser = await _userRepository.Delete(id);

        await _unitOfWork.CompleteAsync();

        return new BasicResponseCrudDto { Message = UsersResource.GetMessage(UsersMessagesKeys.User_Deleted), Id = deletedUser.Id };
    }
}