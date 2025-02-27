using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Project_C_Sharp.Modules.Users.Repositories;
using System.Text.Json;

namespace Tests.Modules.Users.Repositories;

public abstract class UserRepositoryTestBase : DataBaseInMemory
{

    protected readonly Mock<DbSet<User>> _usersDbSetMock;
    protected readonly UserRepository _repository;

    protected UserRepositoryTestBase()
    {
        var context = CreateInMemoryDbContext();
        _usersDbSetMock = new Mock<DbSet<User>>();

        _repository = new UserRepository(context);
    }
}

[Collection("User Repository Tests")]
public class UserRepositoryTests : UserRepositoryTestBase
{
    public class AddUserTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve adicionar um usuário com sucesso")]
        public async Task Should_Add_User_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();


            // Act
            var userAdded = await _repository.Add(user);

            // Assert
            Assert.NotNull(userAdded);
            Assert.Equal(user.Name, userAdded.Name);
            Assert.Equal(user.Email, userAdded.Email);
            Assert.NotEqual(Guid.Empty, userAdded.Id);
            Assert.Equal(user.Password, userAdded.Password);
        }
    }

    public class GetByIdTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve retornar um usuário pelo id")]
        public async Task Should_Get_User_ById_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();

            await _repository.Add(user);

            // Act
            var userById = await _repository.GetById(user.Id);

            // Assert
            Assert.NotNull(userById);
            Assert.Equal(user.Name, userById.Name);
            Assert.Equal(user.Email, userById.Email);
        }

        [Fact(DisplayName = "Deve retornar null se o usuário não existir")]
        public async Task Should_Return_Null_If_User_Does_Not_Exist()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();

            await _repository.Add(user);

            // Act
            var userById = await _repository.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(userById);
        }
    }

    public class GetAllTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve retornar todos os usuários")]
        public async Task Should_Get_All_Users_Successfully()
        {
            // Arrange
            var users = UserMocks.Valid.GenerateUsers(10);

            foreach (var user in users)
            {
                await _repository.Add(user);
            }

            // Act
            var usersResult = await _repository.GetAll();

            // Assert
            Assert.NotNull(usersResult);
            Assert.NotEmpty(usersResult);
            Assert.Equal(users.Count(), usersResult.Count());
        }

        [Fact(DisplayName = "Deve retornar uma lista vazia se não houver usuários")]
        public async Task Should_Return_Empty_List_If_No_Users()
        {

            // Act 
            var usersResult = await _repository.GetAll();

            // Assert
            Assert.NotNull(usersResult);
            Assert.Empty(usersResult);
        }
    }

    public class UpdateTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve atualizar um usuário com sucesso")]
        public async Task Should_Update_User_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();

            var createdUser = await _repository.Add(user);

            var expectedName = "Nome Atualizado";
            var originalName = user.Name;
            createdUser.UpdateName(expectedName);

            // Act
            var userUpdated = await _repository.Update(createdUser);

            // Assert
            Assert.NotNull(userUpdated);
            Assert.NotEqual(originalName, userUpdated.Name);
            Assert.Equal(expectedName, userUpdated.Name);
            Assert.Equal(user.Email, userUpdated.Email);
            Assert.Equal(user.Password, userUpdated.Password);
            Assert.Equal(user.Id, userUpdated.Id);
        }
    }

    public class DeleteTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve deletar um usuário com sucesso")]
        public async Task Should_Delete_User_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();

            await _repository.Add(user);

            // Act
            var userDeleted = await _repository.Delete(user.Id);

            // Assert
            Assert.NotNull(userDeleted);
            var searchResult = await _repository.GetById(user.Id);
            Assert.Null(searchResult);
        }
    }

    public class GetByEmailTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve retornar um usuário pelo email")]
        public async Task Should_Get_User_By_Email_Successfully()
        {
            // Arrange   
            var user = UserMocks.Valid.GenerateUser();

            await _repository.Add(user);

            // Act
            var userByEmail = await _repository.GetByEmail(user.Email);

            // Assert
            Assert.NotNull(userByEmail);
            Assert.Equal(user.Name, userByEmail.Name);
            Assert.Equal(user.Email, userByEmail.Email);
        }

        [Fact(DisplayName = "Deve retornar null se o email não fornecido for null")]
        public async Task Should_Return_Null_If_Email_Is_Null()
        {
            // Act
            var userByEmail = await _repository.GetByEmail(string.Empty);

            // Assert
            Assert.Null(userByEmail);
        }
    }
}