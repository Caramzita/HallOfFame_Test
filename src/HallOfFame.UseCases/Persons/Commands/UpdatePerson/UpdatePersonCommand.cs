using HallOfFame.Contracts;
using HallOfFame.Core;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.UpdatePerson;

/// <summary>
/// Команда обновления нового сотрудника.
/// </summary>
public class UpdatePersonCommand : IRequest<Person>, IPersonCommand
{
    /// <summary>
    /// Идентификатор сотрудника.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя сотрудника.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Отображаемое имя сотрудника.
    /// </summary>
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>
    /// Список навыков сотрудника.
    /// </summary>
    public List<PersonSkillDto> Skills { get; private set; } = new List<PersonSkillDto>();

    /// <summary>
    /// Инициализирует новый экземпляр команды <see cref="UpdatePersonCommand"/>.
    /// </summary>
    /// <param name="name"> Имя сотрудника. </param>
    /// <param name="displayName"> Отображаемое имя сотрудника. </param>
    /// <param name="skills"> Список навыков сотрудника. </param>
    public UpdatePersonCommand(string name, string displayName, List<PersonSkillDto> skills)
    {
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }
}
