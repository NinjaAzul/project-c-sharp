using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Modules.UpdateUser.Dto.Request;
using Project_C_Sharp.Modules.Users.Controllers;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.CreateUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.DeleteUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindByIdUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers.Interfaces;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.I18n.Modules.Users.Messages.Keys;
using Project_C_Sharp.Shared.Resources.Users;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Xunit;
using System.Globalization;
using System.Threading;

namespace Project_C_Sharp.Tests.Modules.Users.Controllers;

[Collection("Users Controller Tests")]
public class UsersControllerTests
{
    public class FindAllTests
    {
        private readonly Mock<IFindAllUsersService> _findAllUsersServiceMock;
        private readonly UsersController _controller;

        public FindAllTests()
        {
            // Configurar a cultura para pt-BR nos testes
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            _findAllUsersServiceMock = new Mock<IFindAllUsersService>();
            _controller = new UsersController();
        }

        [Fact(DisplayName = "Deve retornar Ok quando usuários são encontrados")]
        public async Task Should_Return_Ok_When_Users_Are_Found()
        {
            // Arrange
            var users = UserMocks.Valid.GenerateBasicResponseCrudDto(10);
            _findAllUsersServiceMock.Setup(x => x.FindAll())
                                  .ReturnsAsync(users);

            // Act
            var result = await _controller.FindAll(_findAllUsersServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserBasicInfoResponseDto>>(okResult.Value);
            Assert.Equal(users, returnedUsers);
            _findAllUsersServiceMock.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar Ok com lista vazia quando não houver usuários")]
        public async Task Should_Return_Ok_With_Empty_List_When_No_Users()
        {
            // Arrange
            var emptyList = new List<UserBasicInfoResponseDto>();
            _findAllUsersServiceMock.Setup(x => x.FindAll())
                                  .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.FindAll(_findAllUsersServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserBasicInfoResponseDto>>(okResult.Value);
            Assert.Empty(returnedUsers);
        }
    }

    public class FindByIdTests
    {
        private readonly Mock<IFindByIdUsersService> _findByIdUsersServiceMock;
        private readonly UsersController _controller;

        public FindByIdTests()
        {
            _findByIdUsersServiceMock = new Mock<IFindByIdUsersService>();
            _controller = new UsersController();

            // Configurar a cultura para pt-BR nos testes
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
        }

        [Fact(DisplayName = "Deve retornar Ok quando usuário for encontrado")]
        public async Task Should_Return_Ok_When_User_Is_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = UserMocks.Valid.GenerateBasicResponseCrudDto(1).First();
            _findByIdUsersServiceMock.Setup(x => x.FindById(userId))
                                   .ReturnsAsync(user);

            // Act
            var result = await _controller.FindById(_findByIdUsersServiceMock.Object, userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserBasicInfoResponseDto>(okResult.Value);
            Assert.Equal(user, returnedUser);
        }
    }

    public class CreateTests
    {
        private readonly Mock<ICreateUserService> _createUserServiceMock;
        private readonly UsersController _controller;

        public CreateTests()
        {
            _createUserServiceMock = new Mock<ICreateUserService>();
            _controller = new UsersController();

            // Configurar a cultura para pt-BR nos testes
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
        }

        [Fact(DisplayName = "Deve retornar id e mensagem quando usuário for criado com sucesso")]
        public async Task Should_Return_Id_And_Message_When_User_Is_Created()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateUserRequestDto()
            {
                Name = UserMocks.Valid.Name,
                Email = UserMocks.Valid.Email,
                Password = UserMocks.Valid.Password
            };

            var response = new BasicResponseCrudDto()
            {
                Message = UsersResource.GetMessage(UsersMessagesKeys.User_Created),
                Id = userId
            };

            _createUserServiceMock.Setup(x => x.Create(request))
                                  .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(_createUserServiceMock.Object, request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result); // Ajuste aqui
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            var returnedResponse = Assert.IsType<BasicResponseCrudDto>(createdResult.Value);
            Assert.Equal(response.Message, returnedResponse.Message);
            Assert.Equal(response.Id, returnedResponse.Id);
            _createUserServiceMock.Verify(x => x.Create(request), Times.Once);
        }

        public class UpdateTests
        {
            private readonly Mock<IUpdateUsersService> _updateUsersServiceMock;
            private readonly UsersController _controller;

            public UpdateTests()
            {
                _updateUsersServiceMock = new Mock<IUpdateUsersService>();
                _controller = new UsersController();

                // Configurar a cultura para pt-BR nos testes
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }

            [Fact(DisplayName = "Deve retornar Ok quando usuário for atualizado com sucesso")]
            public async Task Should_Return_Ok_When_User_Is_Updated()
            {
                // Arrange
                var userId = Guid.NewGuid();
                var request = new UpdateUserRequestDto()
                {
                    Name = UserMocks.Valid.Name
                };

                var response = new BasicResponseCrudDto()
                {
                    Message = UsersResource.GetMessage(UsersMessagesKeys.User_Updated),
                    Id = userId
                };

                _updateUsersServiceMock.Setup(x => x.Update(userId, request))
                                     .ReturnsAsync(response);

                // Act
                var result = await _controller.Update(_updateUsersServiceMock.Object, userId, request);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedUser = Assert.IsType<BasicResponseCrudDto>(okResult.Value);
                Assert.Equal(response.Message, UsersResource.GetMessage(UsersMessagesKeys.User_Updated));
                Assert.Equal(response.Id, userId);

                _updateUsersServiceMock.Verify(x => x.Update(userId, request), Times.Once);
            }
        }

        public class DeleteTests
        {
            private readonly Mock<IDeleteUsersService> _deleteUsersServiceMock;
            private readonly UsersController _controller;

            public DeleteTests()
            {
                _deleteUsersServiceMock = new Mock<IDeleteUsersService>();
                _controller = new UsersController();

                // Configurar a cultura para pt-BR nos testes
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }

            [Fact(DisplayName = "Deve retornar Ok quando usuário for deletado com sucesso")]
            public async Task Should_Return_Ok_When_User_Is_Deleted()
            {
                // Arrange
                var userId = Guid.NewGuid();
                var response = new BasicResponseCrudDto()
                {
                    Message = UsersResource.GetMessage(UsersMessagesKeys.User_Deleted),
                    Id = userId
                };

                _deleteUsersServiceMock.Setup(x => x.Delete(userId))
                                     .ReturnsAsync(response);

                // Act
                var result = await _controller.Delete(_deleteUsersServiceMock.Object, userId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedUser = Assert.IsType<BasicResponseCrudDto>(okResult.Value);
                Assert.Equal(response.Message, UsersResource.GetMessage(UsersMessagesKeys.User_Deleted));
                Assert.Equal(response.Id, userId);

                _deleteUsersServiceMock.Verify(x => x.Delete(userId), Times.Once);
            }
        }
    }
}