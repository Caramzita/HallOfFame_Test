using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.AddPerson;

public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, Person>
{
    private readonly IPersonRepository _personRepository;

    public AddPersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
    }

    public async Task<Person> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newPerson = new Person(request.Name, request.DisplayName, request.Skills);

            await _personRepository.AddPerson(newPerson);

            return newPerson;
        }   
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
