using Project_C_Sharp.Infra.DataBase.UnitOfWork;
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

    private readonly IUnitOfWork _unitOfWork;

    public FindByIdUsersService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserBasicInfoResponseDto> FindById(Guid id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            throw new NotFoundException(UsersResource.GetError(UsersErrorsKeys.User_NotFound));
        }

        await _unitOfWork.CompleteAsync();

        return new UserBasicInfoResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}