


using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;

namespace Project_C_Sharp.Modules.Auth.Services.Me.Interfaces;

public interface IUserDataService
{
    UserBasicInfoResponseDto Me(Guid id);
}
