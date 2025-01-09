using CdLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CdLibraryTests;

public class TestSuite1 : IAsyncLifetime
{

    private readonly CdContext? _dbContext;
    private MsSqlContainer? _sqlServerContainer;

    public async Task InitializeAsync()
    {
        _sqlServerContainer = new MsSqlBuilder()
            .WithPassword("Password_2_Change_4_Real_Cases_&")
            .Build();
        await _sqlServerContainer.StartAsync();

        var connectionString = _sqlServerContainer.GetConnectionString();
        var ServiceProvider = new ServiceCollection()
            .AddDbContext<CdContext>(options =>
            {
                options.UseSqlServer(connectionString);
            })
            .BuildServiceProvider();
    }

    public async Task DisposeAsync()
    {
        if (_sqlServerContainer != null)
        {
            await _sqlServerContainer.StopAsync();
        }
    }

    [Fact]
    public void GetAllCDsShouldReturnOk()
    {


    }
}