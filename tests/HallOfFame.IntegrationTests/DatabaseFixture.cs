using HallOfFame.Core.Interfaces;
using HallOfFame.Core;
using HallOfFame.DataAccess.Repositories;
using HallOfFame.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DatabaseFixture : IDisposable
{
    public IPersonRepository PersonRepository { get; private set; }

    public ISkillRepository SkillRepository { get; private set; }

    private readonly DatabaseContext _dbContext;

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _dbContext = new DatabaseContext(options, loggerFactory.CreateLogger<DatabaseContext>());

        PersonRepository = new PersonRepository(_dbContext);
        SkillRepository = new SkillRepository(_dbContext);

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        if (_dbContext.Skills.Count() == 0)
        {
            _dbContext.Skills.AddRange(new List<Skill>
            {
                new Skill ("C#"),
                new Skill ("Java"),
                new Skill ("JavaScript"),
                new Skill ("Python"),
                new Skill ("SQL")
            });
            _dbContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}