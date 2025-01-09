using CdLibrary.Controllers;
using CdLibrary.Data;
using CdLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CdLibraryTests;

public class TestSuite1 : IAsyncLifetime
{
    private CdContext? _dbContext;
    private MsSqlContainer? _sqlServerContainer;

    public async Task InitializeAsync()
    {
        _sqlServerContainer = new MsSqlBuilder()
            .WithPassword("Password_2_Change_4_Real_Cases_&")
            .Build();
        await _sqlServerContainer.StartAsync();

        var connectionString = _sqlServerContainer.GetConnectionString();

        var serviceProvider = new ServiceCollection()
            .AddDbContext<CdContext>(options =>
            {
                options.UseSqlServer(connectionString);
            })
            .BuildServiceProvider();

        _dbContext = serviceProvider.GetRequiredService<CdContext>();

        await _dbContext.Database.MigrateAsync();
        
    }
    public async Task DisposeAsync()
    {
        // Stop the SQL Server Testcontainer
        if (_sqlServerContainer != null)
        {
            await _sqlServerContainer.StopAsync();
        }
    }

    [Fact]
    public async Task GetAllCDsShouldReturnOk()
    {
    // Arrange
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

    // Seed the database with a specific CD if needed (you can skip this if you already have data)
        var expectedCd = new Cd { Artist = "Test Artist", Name = "Test Title", Description = "Test Description", Genre = new Genre { Name = "Test Genre" } };
        _dbContext.Cd.Add(expectedCd);
        await _dbContext.SaveChangesAsync();

    // Create a controller instance
        var controller = new CdsController(_dbContext);

    // Act
        var result = await controller.GetCds(null);

    // Assert
        var cds = Assert.IsAssignableFrom<IEnumerable<Cd>>(result.Value);

    // Check if the list contains the expected CD
        Assert.Contains(cds, cd => cd.Artist == "Test Artist" && cd.Name == "Test Title");
    }


    
}
