using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Shared.Resources.Users;
using Moq;
using Xunit;
using FluentAssertions;
using System.Globalization;
using Project_C_Sharp.Modules.Users.Services.DeleteUsers;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Infra.DataBase.UnitOfWork;

namespace Project_C_Sharp.Tests.Modules.Users.Services;

[Collection("Delete Users Service Tests")]
public class DeleteUsersServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteUsersService _service;

    public DeleteUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new DeleteUsersService(_userRepositoryMock.Object, _unitOfWorkMock.Object);

        // Configurar a cultura para pt-BR nos testes
        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }

    [Fact(DisplayName = "Deve deletar um usuário quando o ID for válido")]
    public async Task Delete_WithValidId_ShouldDeleteUser()
    {
        // Arrange
        var user = UserMocks.Valid.GenerateUser();

        _userRepositoryMock
            .Setup(x => x.GetById(user.Id))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.Delete(user.Id))
            .ReturnsAsync(user);

        // Act
        var result = await _service.Delete(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be(UsersResource.GetMessage(UsersMessagesKeys.User_Deleted));
        result.Id.Should().Be(user.Id);

        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
        _userRepositoryMock.Verify(x => x.Delete(user.Id), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção quando o usuário não for encontrado")]
    public async Task Delete_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _service.Delete(userId);

        // Assert
        await act.Should()
           .ThrowAsync<NotFoundException>()
           .WithMessage(UsersResource.GetError(UsersErrorsKeys.User_NotFound));

        _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(x => x.Delete(userId), Times.Never);
    }
}