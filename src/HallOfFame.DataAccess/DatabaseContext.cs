using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HallOfFame.Core;
using Microsoft.Extensions.Logging;

namespace HallOfFame.DataAccess;

/// <summary>
/// Контекст базы данных для управления сущностями <see cref="Person"/>, <see cref="Skill"/> и <see cref="PersonSkill"/>.
/// Наследуется от <see cref="DbContext"/> и предоставляет интерфейс для взаимодействия с базой данных.
/// </summary>
public class DatabaseContext : DbContext
{
    private readonly ILogger<DatabaseContext> _logger;

    /// <summary>
    /// Набор данных для сотрудников.
    /// </summary>
    public DbSet<Person> Persons { get; set; }

    /// <summary>
    /// Набор данных для навыков.
    /// </summary>
    public DbSet<Skill> Skills { get; set; }

    /// <summary>
    /// Набор данных для ассоциаций между сотрудниками и навыками.
    /// </summary>
    public DbSet<PersonSkill> PersonSkills { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DatabaseContext"/> с заданными параметрами.
    /// </summary>
    /// <param name="options"> Опции базы данных. </param>
    /// <param name="logger"> Логгер для записи событий и ошибок. </param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options, ILogger<DatabaseContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    /// <summary>
    /// Конфигурирует модель сущностей при создании контекста.
    /// </summary>
    /// <param name="modelBuilder"> Строитель модели для настройки сущностей. </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Конфигурирует параметры контекста базы данных.
    /// </summary>
    /// <param name="optionsBuilder"> Строитель опций для настройки контекста. </param>
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