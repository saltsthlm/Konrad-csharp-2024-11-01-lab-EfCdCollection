using Testcontainers.MsSql;

namespace CdLibraryTests;

public class TestSuite1 : IAsyncLifetime
{
    private MsSqlContainer? _sqlServerContainer;

    public async Task InitializeAsync()
    {
        _sqlServerContainer = new MsSqlBuilder()
            .WithPassword("Password_2_Change_4_Real_Cases_&")
            .Build();
        await _sqlServerContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_sqlServerContainer != null)
        {
            await _sqlServerContainer.StopAsync();
        }
    }

    [Fact]
    public void Test1()
    {

    }
}