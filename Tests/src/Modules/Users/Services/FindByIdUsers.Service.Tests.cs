using System.Globalization;
using FluentAssertions;
using Moq;
using Project_C_Sharp.Infra.DataBase.UnitOfWork;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
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
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly FindByIdUsersService _service;

    public FindByIdUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new FindByIdUsersService(_userRepositoryMock.Object, _unitOfWorkMock.Object);

        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }

    [Fact(DisplayName = "Deve retornar um usuário quando o ID for válido")]
    public async Task FindById_ShouldReturnUserWhenIdIsValid()
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

        _userRepositoryMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _service.FindById(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new UserBasicInfoResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });

        _userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Exception quando o ID for inválido")]
    public async Task FindById_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.GetById(userId)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _service.FindById(userId);

        // Assert
        await act.Should()
           .ThrowAsync<NotFoundException>()
           .WithMessage(UsersResource.GetError(UsersErrorsKeys.User_NotFound));

        _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
    }
}
