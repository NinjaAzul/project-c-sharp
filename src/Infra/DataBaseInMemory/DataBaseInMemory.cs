using Microsoft.EntityFrameworkCore;
using Project_C_Sharp.Infra.DataBase.EntityFrameworkCore;

public abstract class DataBaseInMemory
{
    protected ApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usa um nome único para cada teste
            .Options;

        return new ApplicationDbContext(options);
    }
}