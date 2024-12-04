using HallOfFame.Core;
using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Person>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
    }

    public async Task<Person> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetPersonById(request.Id);

        if (person == null)
        {
            throw new NotFoundException($"Person with ID {request.Id} was not found.");
        }

        try
        {
            person.UpdatePersonInfo(request.Name, request.DisplayName, request.Skills);

            await _personRepository.UpdatePerson(person);

            return person;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
