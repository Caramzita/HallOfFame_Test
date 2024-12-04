using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using MediatR;
using System.Runtime.CompilerServices;

namespace HallOfFame.UseCases.Persons.Queries;

public record GetAllProfilesQuery : IStreamRequest<Person>;

public class GetAllProfilesQueryHandler : IStreamRequestHandler<GetAllProfilesQuery, Person>
{
    private readonly IPersonRepository _personRepository;

    public GetAllProfilesQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentException(nameof(personRepository));
    }

    public async IAsyncEnumerable<Person> Handle(GetAllProfilesQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var person in _personRepository.GetAllPersons())
        {
            yield return person;
        }
    }
}
