namespace CdLibrary.Data;

using Microsoft.EntityFrameworkCore;
using CdLibrary.Models;

public class CdContext : DbContext
{
    public CdContext(DbContextOptions<CdContext> options) : base(options)
    {
    }

    public DbSet<Cd> Cd { get; set; }
    public DbSet<Genre> Genre { get; set; }
}   