using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;

namespace Project_C_Sharp.Modules.Users.Services.DeleteUsers.Interfaces;

public interface IDeleteUsersService
{
    Task<BasicResponseCrudDto> Delete(Guid id);
}