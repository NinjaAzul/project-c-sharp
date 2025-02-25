using System.Globalization;
using FluentAssertions;
using Moq;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Xunit;

namespace Project_C_Sharp.Tests.Modules.Users.Services;

public class FindAllUsersServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly FindAllUsersService _service;

    public FindAllUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new FindAllUsersService(_userRepositoryMock.Object);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }

    [Fact(DisplayName = "Deve retornar todos os usuários")]
    public void FindAll_ShouldReturnAllUsers()
    {
        // Arrange
        var users = UserMocks.Valid.GenerateUsers(10); // Gera 10 usuários diferentes automaticamente

        _userRepositoryMock.Setup(x => x.GetAll()).Returns(users);

        // Act
        var result = _service.FindAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo(users.Select(u => new UserBasicInfoResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }));

        _userRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar uma lista vazia quando não houver usuários")]
    public void FindAll_ShouldReturnEmptyListWhenNoUsers()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetAll()).Returns(new List<User>());

        // Act
        var act = _service.FindAll();

        // Assert
        act.Should().NotBeNull();
        act.Should().BeEmpty();

        _userRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }
}
