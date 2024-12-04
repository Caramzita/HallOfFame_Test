using HallOfFame.Core;
using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Queries;

public record GetPersonQuery(long Id) : IRequest<Person>;

public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, Person>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
    }

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
