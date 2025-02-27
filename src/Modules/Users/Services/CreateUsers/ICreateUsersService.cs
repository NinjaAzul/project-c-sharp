using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;

namespace Project_C_Sharp.Modules.Users.Services.CreateUsers.Interfaces;

public interface ICreateUserService
{
    Task<BasicResponseCrudDto> Create(CreateUserRequestDto createUserRequestDto);
}