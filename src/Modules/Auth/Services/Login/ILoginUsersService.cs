using Project_C_Sharp.Modules.Login.Dto.Response;
using Project_C_Sharp.Modules.LoginUsers.Dto.Request;

namespace Project_C_Sharp.Modules.Auth.Services.LoginUsers.Interfaces;

public interface ILoginUsersService
{
    Task<LoginResponseDto> Login(LoginUsersRequestDto loginUsersRequestDto);
}