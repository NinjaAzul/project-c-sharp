using System.Globalization;
using FluentAssertions;
using Moq;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindByIdUsers;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Xunit;

namespace Project_C_Sharp.Tests.Modules.Users.Services;

[Collection("Find By Id Users Service Tests")]
public class FindByIdUsersServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly FindByIdUsersService _service;

    public FindByIdUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new FindByIdUsersService(_userRepositoryMock.Object);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }

    [Fact(DisplayName = "Deve retornar um usuário quando o ID for válido")]
    public void FindById_ShouldReturnUserWhenIdIsValid()
    {
        // Arrange
        var user = new User(
            name: UserMocks.Valid.Name,
            email: UserMocks.Valid.Email,
            password: UserMocks.Valid.Password
        );

        _userRepositoryMock.Setup(x => x.GetById(user.Id)).Returns(user);

        // Act
        var act = _service.FindById(user.Id);

        // Assert
        act.Should().NotBeNull();
        act.Should().BeEquivalentTo(new UserBasicInfoResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });

        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Exception quando o ID for inválido")]
    public void FindById_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns((User)null);

        // Act
        var act = () => _service.FindById(userId);

        // Assert
        act.Should()
           .Throw<NotFoundException>()
           .WithMessage(UsersResource.GetError(UsersErrorsKeys.User_NotFound));

        _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
    }
}
