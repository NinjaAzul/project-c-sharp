
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;

namespace Project_C_Sharp.Modules.Users.Services.FindAllUsers.Interfaces;

public interface IFindAllUsersService
{
    Task<IEnumerable<UserBasicInfoResponseDto>> FindAll();
}