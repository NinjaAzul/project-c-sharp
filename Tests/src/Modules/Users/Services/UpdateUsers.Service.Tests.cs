using System.Globalization;
using FluentAssertions;
using Moq;
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
    private readonly UpdateUsersService _service;

    public UpdateUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _service = new UpdateUsersService(_userRepositoryMock.Object);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }


    [Fact(DisplayName = "Deve atualizar um usuário quando o ID for válido")]
    public void Update_WithValidId_ShouldUpdateUser()
    {
        // Arrange
        var user = new User(
            name: UserMocks.Valid.Name,
            email: UserMocks.Valid.Email,
            password: UserMocks.Valid.Password
        );

        // Simula o cadastro do usuário
        var createdUser = _userRepositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .Returns(user);

        // Configura o GetById para retornar o usuário criado
        _userRepositoryMock
            .Setup(x => x.GetById(user.Id))
            .Returns(user);

        // Configura o Update para retornar o usuário atualizado
        _userRepositoryMock
            .Setup(x => x.Update(user))
            .Returns(user);

        // Cria o DTO de atualização
        var updateUserRequestDto = new UpdateUserRequestDto
        {
            Name = UserMocks.Valid.Name,
        };

        // Act
        var act = _service.Update(user.Id, updateUserRequestDto);

        // Assert
        act.Should().NotBeNull();
        act.Message.Should().Be(UsersResource.GetMessage(UsersMessagesKeys.User_Updated));
        act.Id.Should().Be(user.Id);

        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
        _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção quando o usuário não for encontrado")]
    public void Update_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var updateUserRequestDto = new UpdateUserRequestDto
        {
            Name = UserMocks.Valid.Name,
        };

        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns((User?)null);

        // Act
        var act = () => _service.Update(userId, updateUserRequestDto);

        // Assert
        act.Should().Throw<NotFoundException>()
           .WithMessage(UsersResource.GetError(UsersErrorsKeys.User_NotFound));

        _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact(DisplayName = "Deve atualizar o nome do usuário corretamente")]
    public void Update_ShouldUpdateUserName()
    {
        // Arrange
        var user = new User(
            name: "Nome Antigo",
            email: UserMocks.Valid.Email,
            password: UserMocks.Valid.Password
        );

        var novoNome = "Nome Novo";

        User? updatedUser = null;

        _userRepositoryMock
            .Setup(x => x.GetById(user.Id))
            .Returns(user);

        _userRepositoryMock
            .Setup(x => x.Update(It.IsAny<User>()))
            .Callback<User>(u => updatedUser = u)
            .Returns((User u) => u);

        var updateUserRequestDto = new UpdateUserRequestDto
        {
            Name = novoNome
        };

        // Act
        var act = _service.Update(user.Id, updateUserRequestDto);

        // Assert
        updatedUser.Should().NotBeNull();
        updatedUser!.Name.Should().Be(novoNome);
        updatedUser.Name.Should().NotBe("Nome Antigo");
        act.Message.Should().Be(UsersResource.GetMessage(UsersMessagesKeys.User_Updated));
        act.Id.Should().Be(user.Id);



        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
    }
}
