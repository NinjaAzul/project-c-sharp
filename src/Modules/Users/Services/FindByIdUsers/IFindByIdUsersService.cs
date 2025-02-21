using System;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;

namespace Project_C_Sharp.Modules.Users.Services.FindByIdUsers.Interfaces;

public interface IFindByIdUsersService
{
    UserBasicInfoResponseDto FindById(Guid id);
}