using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.DataAccess.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly DatabaseContext _context;

    public PersonRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAsyncEnumerable<Person> GetAllPersons()
    {
        return _context.Persons
            .AsNoTracking()
            .AsAsyncEnumerable();
    }

    public async Task<Person?> GetPersonById(long id)
    {
        return await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPerson(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePerson(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }  

    public async Task UpdatePerson(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }
}
