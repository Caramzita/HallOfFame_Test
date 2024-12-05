using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.DataAccess.Repositories;

/// <summary>
/// Реализация репозитория сотрудников <see cref="IPersonRepository"/>.
/// Обеспечивает доступ к данным сотрудников с использованием контекста базы данных.
/// </summary>
public class PersonRepository : IPersonRepository
{
    private readonly DatabaseContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PersonRepository"/> с заданным контекстом базы данных.
    /// </summary>
    /// <param name="context"> Контекст базы данных для работы с данными сотрудников. </param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="context"/> равен <c>null</c>.</exception>
    public PersonRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Person> GetAllPersons()
    {
        return _context.Persons
            .AsNoTracking()
            .Include(p => p.Skills)
            .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Person?> GetPersonById(long id)
    {
        return await _context.Persons
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc/>
    public async Task AddPerson(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeletePerson(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdatePerson(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }
}
