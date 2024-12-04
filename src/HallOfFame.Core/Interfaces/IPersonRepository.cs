namespace HallOfFame.Core.Interfaces;

public interface IPersonRepository
{
    IAsyncEnumerable<Person> GetAllPersons();

    Task<Person?> GetPersonById(long id);

    Task AddPerson(Person person);

    Task DeletePerson(Person person);

    Task UpdatePerson(Person person);
}
