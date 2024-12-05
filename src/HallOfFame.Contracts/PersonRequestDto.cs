namespace HallOfFame.Contracts;

/// <summary>
/// Модель данных сотрудника.
/// Используется для передачи информации о сотруднике.
/// </summary
public class PersonRequestDto
{
    /// <summary>
    /// Имя сотрудника.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Отображаемое имя.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Список навыков.
    /// </summary>
    public List<PersonSkillDto> Skills { get; set; } = new List<PersonSkillDto>();
}
