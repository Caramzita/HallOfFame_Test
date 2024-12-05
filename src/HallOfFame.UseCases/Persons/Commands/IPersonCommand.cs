using HallOfFame.Contracts;

namespace HallOfFame.UseCases.Persons.Commands;

/// <summary>
/// Интерфейс для команд, связанных с сущностью <see cref="Person"/>.
/// Определяет основные свойства, необходимые для команд добавления или обновления сотрудника.
/// </summary>
public interface IPersonCommand
{
    /// <summary>
    /// Имя сотрудника.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Отображаемое имя сотрудника.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Список навыков, связанных с сотрудником.
    /// </summary>
    List<PersonSkillDto> Skills { get; }
}
