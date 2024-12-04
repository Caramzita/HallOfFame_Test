using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.DeletePerson;

public class DeletePersonCommand : IRequest
{
    public long Id { get; private set; }

    public DeletePersonCommand(long id)
    {
        Id = id;
    }
}
