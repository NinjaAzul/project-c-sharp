

using Project_C_Sharp.Infra.DataBase.UnitOfWork;
using Project_C_Sharp.Modules.Auth.Services.LoginUsers.Interfaces;
using Project_C_Sharp.Modules.Auth.Services.Token;
using Project_C_Sharp.Modules.Login.Dto.Response;
using Project_C_Sharp.Modules.LoginUsers.Dto.Request;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Auth.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;

namespace Project_C_Sharp.Modules.Auth.Services.LoginUsers;

public class LoginUsersService : ILoginUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUsersService(IUserRepository userRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponseDto> Login(LoginUsersRequestDto loginUsersRequestDto)
    {
        var user = await _userRepository.GetByEmail(loginUsersRequestDto.Email);

        if (user == null)
        {
            throw new BadRequestException(AuthResource.GetError(AuthErrorsKeys.Email_Or_Password_Invalid));
        }

        if (!BCrypt.Net.BCrypt.Verify(loginUsersRequestDto.Password, user.Password))
        {
            throw new BadRequestException(AuthResource.GetError(AuthErrorsKeys.Email_Or_Password_Invalid));
        }

        var token = _tokenService.GenerateToken(user);

        await _unitOfWork.CompleteAsync();

        return new LoginResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = token
        };

    }
}