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
        if (_sqlServerContainer != null)
        {
            await _sqlServerContainer.StopAsync();
        }
    }

    [Fact]
    public async Task GetAllCDsShouldReturnOk()
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        var expectedCd = new Cd { Artist = "Test Artist", Name = "Test Title", Description = "Test Description", Genre = new Genre { Name = "Test Genre" } };
        _dbContext.Cd.Add(expectedCd);
        await _dbContext.SaveChangesAsync();

        var controller = new CdsController(_dbContext);

        var result = await controller.GetCds(null);

        var cds = Assert.IsAssignableFrom<IEnumerable<Cd>>(result.Value);

        Assert.Contains(cds, cd => cd.Artist == "Test Artist" && cd.Name == "Test Title");
    }

    [Fact]
    public async Task PostCdShouldCreateANewRow()
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        var newCd = new Cd
        {
            Artist = "New Artist",
            Name = "New Title",
            Description = "New Description",
            Genre = new Genre { Name = "New Genre" }
        };

        var controller = new CdsController(_dbContext);

        var result = await controller.PostCd(newCd);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdCd = Assert.IsType<Cd>(createdAtActionResult.Value);

        Assert.Equal(newCd.Name, createdCd.Name);

        var dbCd = await _dbContext.Cd.FindAsync(createdCd.Id);
        Assert.NotNull(dbCd);
        Assert.Equal(newCd.Name, dbCd?.Name);
    }
}
