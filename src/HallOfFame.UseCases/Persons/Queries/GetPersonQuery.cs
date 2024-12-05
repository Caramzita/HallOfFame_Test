using HallOfFame.Core;
using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Queries;

/// <summary>
/// Запрос для получения конкретного сотрудника по его идентификатору.
/// </summary>
/// <param name="Id"> Идентификатор сотрудника, который нужно получить. </param>
public record GetPersonQuery(long Id) : IRequest<Person>;

/// <summary>
/// Обработчик запроса <see cref="GetPersonQuery"/>.
/// </summary>
public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, Person>
{
    private readonly IPersonRepository _personRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GetPersonQueryHandler"/>.
    /// </summary>
    /// <param name="personRepository"> Репозиторий для работы с данными сотрудников. </param>
    /// <exception cref="ArgumentNullException"> Выбрасывается, 
    /// если <paramref name="personRepository"/> равен <c>null</c>. </exception>
    public GetPersonQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
    }

    /// <inheritdoc/>
    public async Task<Person> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetPersonById(request.Id);

        if (person == null)
        {
            throw new NotFoundException($"Person with ID {request.Id} was not found.");
        }

        return person;
    }
}
