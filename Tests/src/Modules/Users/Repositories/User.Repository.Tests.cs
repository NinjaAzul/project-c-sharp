using Project_C_Sharp.Infra.DataBaseInMemory;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.Users.Repositories;
using Project_C_Sharp.Tests.Modules.Users.Mocks;
using Xunit;
using Moq;


namespace Tests.Modules.Users.Repositories;

public abstract class UserRepositoryTestBase
{
    protected readonly Mock<IDataSource> _dataSourceMock;
    protected readonly List<User> _usersList;
    protected readonly UserRepository _repository;

    protected UserRepositoryTestBase()
    {
        _usersList = new List<User>();
        _dataSourceMock = new Mock<IDataSource>();
        _dataSourceMock.Setup(x => x.Users).Returns(_usersList);
        _repository = new UserRepository(_dataSourceMock.Object);
    }

    // Métodos auxiliares que podem ser usados por todos os testes
    protected void AddUserToList(User user)
    {
        _usersList.Add(user);
    }

    protected void AddUsersToList(IEnumerable<User> users)
    {
        _usersList.AddRange(users);
    }
}

[Collection("User Repository Tests")]
public class UserRepositoryTests
{
    public class AddUserTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve adicionar um usuário com sucesso")]
        public void Should_Add_User_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();

            // Act
            var userAdded = _repository.Add(user);

            // Assert
            Assert.NotNull(userAdded);
            Assert.Equal(user.Name, userAdded.Name);
            Assert.Equal(user.Email, userAdded.Email);
            Assert.NotEqual(Guid.Empty, userAdded.Id);
            Assert.Equal(user.Password, userAdded.Password);
            Assert.Single(_usersList);
        }
    }

    public class GetByIdTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve retornar um usuário pelo id")]
        public void Should_Get_User_ById_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();
            AddUserToList(user);

            // Act
            var userById = _repository.GetById(user.Id);

            // Assert
            Assert.NotNull(userById);
            Assert.Equal(user.Name, userById.Name);
            Assert.Equal(user.Email, userById.Email);
        }

        [Fact(DisplayName = "Deve retornar null se o usuário não existir")]
        public void Should_Return_Null_If_User_Does_Not_Exist()
        {
            // Act
            var userById = _repository.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(userById);
        }
    }

    public class GetAllTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve retornar todos os usuários")]
        public void Should_Get_All_Users_Successfully()
        {
            // Arrange
            var users = UserMocks.Valid.GenerateUsers(10);
            AddUsersToList(users);

            // Act
            var usersResult = _repository.GetAll();

            // Assert
            Assert.NotNull(usersResult);
            Assert.NotEmpty(usersResult);
            Assert.Equal(users.Count(), usersResult.Count());
        }

        [Fact(DisplayName = "Deve retornar uma lista vazia se não houver usuários")]
        public void Should_Return_Empty_List_If_No_Users()
        {
            // Act
            var usersResult = _repository.GetAll();

            // Assert
            Assert.NotNull(usersResult);
            Assert.Empty(usersResult);
        }
    }

    public class UpdateTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve atualizar um usuário com sucesso")]
        public void Should_Update_User_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();
            AddUserToList(user);

            var expectedName = "Nome Atualizado";
            var originalName = user.Name;
            user.UpdateName(expectedName);

            // Act
            var userUpdated = _repository.Update(user);

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
        public void Should_Delete_User_Successfully()
        {
            // Arrange
            var user = UserMocks.Valid.GenerateUser();
            AddUserToList(user);

            // Act
            var userDeleted = _repository.Delete(user.Id);

            // Assert
            Assert.NotNull(userDeleted);
            Assert.Empty(_usersList);
            var searchResult = _repository.GetById(user.Id);
            Assert.Null(searchResult);
        }
    }

    public class GetByEmailTests : UserRepositoryTestBase
    {
        [Fact(DisplayName = "Deve retornar um usuário pelo email")]
        public void Should_Get_User_By_Email_Successfully()
        {
            // Arrange   
            var user = UserMocks.Valid.GenerateUser();
            AddUserToList(user);

            // Act
            var userByEmail = _repository.GetByEmail(user.Email);

            // Assert
            Assert.NotNull(userByEmail);
            Assert.Equal(user.Name, userByEmail.Name);
            Assert.Equal(user.Email, userByEmail.Email);
        }

        [Fact(DisplayName = "Deve retornar null se o email não fornecido for null")]
        public void Should_Return_Null_If_Email_Is_Null()
        {
            // Act
            var userByEmail = _repository.GetByEmail(string.Empty);

            // Assert
            Assert.Null(userByEmail);
        }
    }
}