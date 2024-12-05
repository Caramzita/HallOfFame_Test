using HallOfFame.Contracts;
using HallOfFame.Core;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.AddPerson;

/// <summary>
/// Команда добавления нового сотрудника.
/// </summary>
public class AddPersonCommand : IRequest<Person>, IPersonCommand
{
    /// <summary>
    /// Имя нового сотрудника.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Отображаемое имя нового сотрудника.
    /// </summary>
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>
    /// Список навыков нового сотрудника.
    /// </summary>
    public List<PersonSkillDto> Skills { get; private set; } = new List<PersonSkillDto>();

    /// <summary>
    /// Инициализирует новый экземпляр команды <see cref="AddPersonCommand"/>.
    /// </summary>
    /// <param name="name"> Имя нового сотрудника. </param>
    /// <param name="displayName"> Отображаемое имя нового сотрудника. </param>
    /// <param name="skills"> Список навыков нового сотрудника. </param>
    public AddPersonCommand(string name, string displayName, List<PersonSkillDto> skills)
    {
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }
}
