using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.DeletePerson;

/// <summary>
/// Команда удаления нового сотрудника.
/// </summary
public class DeletePersonCommand : IRequest
{
    /// <summary>
    /// Идентификатор сотрудника.
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// Инициализирует новый экземпляр команды <see cref="DeletePersonCommand"/>.
    /// </summary>
    /// <param name="id"> Идентификатор сотрудника. </param>
    public DeletePersonCommand(long id)
    {
        Id = id;
    }
}
