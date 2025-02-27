using Project_C_Sharp.Modules.UpdateUser.Dto.Request;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;

namespace Project_C_Sharp.Modules.Users.Services.UpdateUsers.Interfaces;

public interface IUpdateUsersService
{
    Task<BasicResponseCrudDto> Update(Guid id, UpdateUserRequestDto updateUserRequestDto);
}