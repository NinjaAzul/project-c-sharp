using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.CreateUsers.Interfaces;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Infra.DataBase.UnitOfWork;

namespace Project_C_Sharp.Modules.Users.Services.CreateUsers;

public class CreateUsersService : ICreateUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CreateUsersService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BasicResponseCrudDto> Create(CreateUserRequestDto createUserRequestDto)
    {
        var userAlreadyExists = await _userRepository.GetByEmail(createUserRequestDto.Email);

        if (userAlreadyExists != null)
        {
            throw new BadRequestException(UsersResource.GetError(UsersErrorsKeys.Email_Already_Exists));
        }


        var user = User.Create(
            createUserRequestDto,
            new CreateUserRequestValidator()
        );

        var createdUser = await _userRepository.Add(user);

        await _unitOfWork.CompleteAsync();

        return new BasicResponseCrudDto { Message = UsersResource.GetMessage(UsersMessagesKeys.User_Created), Id = createdUser.Id };
    }
}