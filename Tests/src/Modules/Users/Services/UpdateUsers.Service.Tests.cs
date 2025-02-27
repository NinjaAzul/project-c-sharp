using System.Globalization;
using FluentAssertions;
using Moq;
using Project_C_Sharp.Infra.DataBase.UnitOfWork;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Modules.UpdateUser.Dto.Request;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Xunit;

namespace Project_C_Sharp.Tests.Modules.Users.Services;

[Collection("Update Users Service Tests")]
public class UpdateUsersServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateUsersService _service;

    public UpdateUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new UpdateUsersService(_userRepositoryMock.Object, _unitOfWorkMock.Object);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }


    [Fact(DisplayName = "Deve atualizar um usuário quando o ID for válido")]
    public async Task Update_WithValidId_ShouldUpdateUser()
    {
        // Arrange
        var user = User.Create(
            new CreateUserRequestDto
            {
                Name = UserMocks.Valid.Name,
                Email = UserMocks.Valid.Email,
                Password = UserMocks.Valid.Password
            },
            new CreateUserRequestValidator()
        );

        // Simula o cadastro do usuário
        var createdUser = _userRepositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Configura o GetById para retornar o usuário criado
        _userRepositoryMock
            .Setup(x => x.GetById(user.Id))
            .ReturnsAsync(user);

        // Configura o Update para retornar o usuário atualizado
        _userRepositoryMock
            .Setup(x => x.Update(user))
            .ReturnsAsync(user);

        // Cria o DTO de atualização
        var updateUserRequestDto = new UpdateUserRequestDto
        {
            Name = UserMocks.Valid.Name,
        };

        // Act
        var result = await _service.Update(user.Id, updateUserRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be(UsersResource.GetMessage(UsersMessagesKeys.User_Updated));
        result.Id.Should().Be(user.Id);

        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção quando o usuário não for encontrado")]
    public async Task Update_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var updateUserRequestDto = new UpdateUserRequestDto
        {
            Name = UserMocks.Valid.Name,
        };

        _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _service.Update(userId, updateUserRequestDto);

        // Assert
        await act.Should()
           .ThrowAsync<NotFoundException>()
           .WithMessage(UsersResource.GetError(UsersErrorsKeys.User_NotFound));

        _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact(DisplayName = "Deve atualizar o nome do usuário corretamente")]
    public async Task Update_ShouldUpdateUserName()
    {
        // Arrange
        var user = UserMocks.Valid.GenerateUser();

        var novoNome = "Nome Novo";

        User? updatedUser = null;

        _userRepositoryMock
            .Setup(x => x.GetById(user.Id))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.Update(It.IsAny<User>()))
            .Callback<User>(u => updatedUser = u)
            .ReturnsAsync((User u) => u);

        var updateUserRequestDto = new UpdateUserRequestDto
        {
            Name = novoNome
        };

        // Act
        var result = await _service.Update(user.Id, updateUserRequestDto);

        // Assert
        updatedUser.Should().NotBeNull();
        updatedUser!.Name.Should().Be(novoNome);
        updatedUser.Name.Should().NotBe("Nome Antigo");
        result.Message.Should().Be(UsersResource.GetMessage(UsersMessagesKeys.User_Updated));
        result.Id.Should().Be(user.Id);



        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
    }
}
