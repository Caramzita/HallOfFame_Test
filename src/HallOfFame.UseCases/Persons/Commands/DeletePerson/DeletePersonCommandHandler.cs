using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.DeletePerson;

/// <summary>
/// Обработчик команды <see cref="DeletePersonCommand"/>.
/// </summary>
public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand>
{
    private readonly IPersonRepository _personRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DeletePersonCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository"> Репозиторий сотрудников. </param>
    /// <exception cref="ArgumentNullException"> Выбрасывается, если <paramref name="personRepository"/> 
    /// или <paramref name="skillRepository"/> равен <c>null</c>. </exception>
    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
    }

    /// <inheritdoc/>
    public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetPersonById(request.Id);

        if (person == null)
        {
            throw new NotFoundException($"Person with ID {request.Id} was not found.");
        }

        await _personRepository.DeletePerson(person);
    }
}
