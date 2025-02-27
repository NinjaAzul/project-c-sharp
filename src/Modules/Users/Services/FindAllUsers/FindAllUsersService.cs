using Project_C_Sharp.Infra.DataBase.UnitOfWork;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers.Interfaces;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;

namespace Project_C_Sharp.Modules.Users.Services.FindAllUsers;

public class FindAllUsersService : IFindAllUsersService
{
    private readonly IUserRepository _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public FindAllUsersService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserBasicInfoResponseDto>> FindAll()
    {
        var users = await _userRepository.GetAll();

        await _unitOfWork.CompleteAsync();

        return users.Select(user => new UserBasicInfoResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}