using Project_C_Sharp.Modules.Users.Services.CreateUsers;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Shared.I18n.Modules.Users.Errors.Keys;
using Moq;
using Xunit;
using FluentAssertions;
using System.Globalization;
using Project_C_Sharp.Shared.Resources.Validation;
using Project_C_Sharp.Shared.I18n.Validation.Keys;
using Project_C_Sharp.Tests.Modules.Users.Mocks;

namespace Project_C_Sharp.Tests.Modules.Users.Services;

public class CreateUsersServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUsersService _service;

    public CreateUsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new CreateUsersService(_userRepositoryMock.Object);

        // Configurar a cultura para pt-BR nos testes
        Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
    }

    [Fact(DisplayName = "Deve criar um usuário quando os dados forem válidos")]
    public void Create_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var createUserDto = UserMocks.Valid.GenerateCreateDto();
        var expectedUser = UserMocks.Valid.GenerateUser();

        _userRepositoryMock
            .Setup(x => x.GetByEmail(createUserDto.Email))
            .Returns((User?)null);

        _userRepositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .Returns(expectedUser);

        // Act
        var result = _service.Create(createUserDto);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().NotBeNullOrEmpty();

        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetByEmail(createUserDto.Email), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção quando email já existir")]
    public void Create_WithExistingEmail_ShouldThrowException()
    {
        // Arrange
        var createUserDto = UserMocks.Valid.GenerateCreateDto();
        var existingUser = UserMocks.Valid.GenerateUser();

        _userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).Returns(existingUser);

        _userRepositoryMock
            .Setup(x => x.GetByEmail(createUserDto.Email))
            .Returns(existingUser);

        // Act
        var act = () => _service.Create(createUserDto);

        // Assert
        act.Should()
           .Throw<BadRequestException>()
           .WithMessage(UsersResource.GetError(UsersErrorsKeys.Email_Already_Exists));
    }

    [Fact(DisplayName = "Deve lançar exceção quando os dados do usuário não forem válidos")]
    public void Create_WithInvalidUserData_ShouldThrowException()
    {
        // Arrange
        var createUserDto = new CreateUserRequestDto
        {
            Name = UserMocks.Invalid.Name.Empty,
            Email = UserMocks.Invalid.Email.Empty,
            Password = UserMocks.Invalid.Password.Empty
        };

        // Act & Assert
        var exception = Assert.Throws<BadRequestException>(() => _service.Create(createUserDto));

        var errors = exception.Errors;

        // foreach (var error in errors)
        // {
        //     Console.WriteLine($"Erro: {error.ErrorMessage}");
        // }

        Assert.NotNull(errors);

        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Nome"));
        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Nome").Replace("{1}", "3").Replace("{2}", "100"));
        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Invalid_Email));
        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Senha"));
        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Senha").Replace("{1}", "6").Replace("{2}", "20"));
        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Password_RequireUpperCase));
        Assert.Contains(errors, e => e.ErrorMessage == ResourceValidation.GetString(ValidationKeys.Password_RequireNumber));
    }

    [Fact(DisplayName = "Deve gerar hash da senha quando os dados do usuário forem válidos")]
    public void Create_WithValidData_ShouldGeneratePasswordHash()
    {
        // Arrange
        var createUserDto = UserMocks.Valid.GenerateCreateDto();
        User? savedUser = null;

        _userRepositoryMock
            .Setup(x => x.GetByEmail(createUserDto.Email))
            .Returns((User?)null);

        _userRepositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .Callback<User>(user => savedUser = user)
            .Returns((User u) => u);

        // Act
        var result = _service.Create(createUserDto);

        // Assert
        savedUser.Should().NotBeNull();
        savedUser!.Password.Should().NotBe(createUserDto.Password);
        savedUser.Password.Should().NotBeNullOrEmpty();
        BCrypt.Net.BCrypt.Verify(createUserDto.Password, savedUser.Password).Should().BeTrue();

        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetByEmail(createUserDto.Email), Times.Once);
    }

}


