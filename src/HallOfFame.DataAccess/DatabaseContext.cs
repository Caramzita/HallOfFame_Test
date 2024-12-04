using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HallOfFame.Core;
using Microsoft.Extensions.Logging;

namespace HallOfFame.DataAccess;

public class DatabaseContext : DbContext
{
    private readonly ILogger<DatabaseContext> _logger;

    public DbSet<Person> Persons { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options, ILogger<DatabaseContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _logger.LogWarning("DbContextOptionsBuilder is not configured.");
        }

        optionsBuilder.EnableSensitiveDataLogging()
                      .LogTo(log => _logger.LogInformation(log));
    }
}
