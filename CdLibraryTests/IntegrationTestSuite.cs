using CdLibrary.Controllers;
using CdLibrary.Data;
using CdLibrary.DTOs;
using CdLibrary.Models;
using CdLibrary.Services;
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
    public async Task GetAllCDsShouldReturnAllCds()
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        var expectedCd = new Cd { Artist = "Test Artist", Name = "Test Title", Description = "Test Description", Genre = new Genre { Name = "Test Genre" } };
        _dbContext.Cd.Add(expectedCd);
        await _dbContext.SaveChangesAsync();

        var cdService = new CdService(_dbContext);
        var controller = new CdsController(_dbContext, cdService);

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

        var cdService = new CdService(_dbContext);
        var controller = new CdsController(_dbContext, cdService);

        var result = await controller.PostCd(newCd);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdCd = Assert.IsType<Cd>(createdAtActionResult.Value);

        Assert.Equal(newCd.Name, createdCd.Name);

        var dbCd = await _dbContext.Cd.FindAsync(createdCd.Id);
        Assert.NotNull(dbCd);
        Assert.Equal(newCd.Name, dbCd?.Name);
    }

    [Fact]
    public async Task PutCdShouldUpdateExistingRow()
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        var existingCd = new Cd
        {
            Artist = "Existing Artist",
            Name = "Existing Title",
            Description = "Existing Description",
            Genre = new Genre { Name = "Existing Genre" }
        };

        _dbContext.Cd.Add(existingCd);
        await _dbContext.SaveChangesAsync();

        var updatedCd = new Cd
        {
            Id = existingCd.Id,
            Name = "Updated Title",
            Artist = "Updated Artist",
            Description = "Updated Description",
            Genre = new Genre { Name = "Updated Genre" }
        };

        var cdService = new CdService(_dbContext);
        var controller = new CdsController(_dbContext, cdService);

        var result = await controller.UpdateCd(existingCd.Id, updatedCd.Artist);

        Assert.IsType<NoContentResult>(result);

        var dbCd = await _dbContext.Cd.FindAsync(existingCd.Id);
        Assert.NotNull(dbCd);
        Assert.Equal(updatedCd.Artist, dbCd?.Artist);
    }

    [Fact]
    public async Task DeleteShouldRemoveRow()
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        var cd = new Cd
        {
            Artist = "To Be Deleted Artist",
            Name = "To Be Deleted Title",
            Description = "To Be Deleted Description",
            Genre = new Genre { Name = "To Be Deleted Genre" }
        };

        _dbContext.Cd.Add(cd);
        await _dbContext.SaveChangesAsync();

        var cdService = new CdService(_dbContext);
        var controller = new CdsController(_dbContext, cdService);

        var result = await controller.DeleteCd(cd.Id);

        Assert.IsType<NoContentResult>(result);

        var dbCd = await _dbContext.Cd.FindAsync(cd.Id);
        Assert.Null(dbCd);
    }

    [Fact]
    public async Task GetCdShouldReturnCd()
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("Database context is not initialized.");
        }

        var cd = new Cd
        {
            Artist = "Test Artist",
            Name = "Test Title",
            Description = "Test Description",
            Genre = new Genre { Name = "Test Genre" }
        };

        _dbContext.Cd.Add(cd);
        await _dbContext.SaveChangesAsync();

        var cdService = new CdService(_dbContext);
        var controller = new CdsController(_dbContext, cdService);

        var result = await controller.GetCd(cd.Id);

        var cdResponse = Assert.IsType<CdResponse>(result.Value);

        Assert.Equal(cd.Artist, cdResponse.Artist);
        Assert.Equal(cd.Name, cdResponse.Name);
        Assert.Equal(cd.Description, cdResponse.Description);
        Assert.Equal(cd.Genre?.Name, cdResponse.Genre);
    }
}
