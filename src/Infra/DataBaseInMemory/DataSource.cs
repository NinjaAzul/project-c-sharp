
using Project_C_Sharp.Modules.Users.Entities;

public interface IDataSource
{
    List<User> Users { get; }
}

namespace Project_C_Sharp.Infra.DataBaseInMemory
{
    public class DataSource : IDataSource
    {
        public List<User> Users { get; } = new List<User>();
    }
}

