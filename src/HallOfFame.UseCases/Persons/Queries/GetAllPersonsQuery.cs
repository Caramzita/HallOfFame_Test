using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using MediatR;
using System.Runtime.CompilerServices;

namespace HallOfFame.UseCases.Persons.Queries;

/// <summary>
/// Запрос для получения всех сотрудников.
/// </summary>
public record GetAllPersonsQuery : IStreamRequest<Person>;

/// <summary>
/// Обработчик запроса <see cref="GetAllPersonsQuery"/>.
/// Извлекает всех сотрудников из репозитория.
/// </summary>
public class GetAllPersonsQueryHandler : IStreamRequestHandler<GetAllPersonsQuery, Person>
{
    private readonly IPersonRepository _personRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GetAllPersonsQueryHandler"/>.
    /// </summary>
    /// <param name="personRepository"> Репозиторий сотрудников. </param>
    /// <exception cref="ArgumentException"> Выбрасывается, 
    /// если <paramref name="personRepository"/> равен <c>null</c>. </exception>
    public GetAllPersonsQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentException(nameof(personRepository));
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Person> Handle(GetAllPersonsQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var person in _personRepository.GetAllPersons())
        {
            yield return person;
        }
    }
}
