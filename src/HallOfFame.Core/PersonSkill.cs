namespace HallOfFame.Core;

/// <summary>
/// Навык сотрудника.
/// </summary>
public class PersonSkill
{
    /// <summary>
    /// Идентификатор сотрудника, которому принадлежит этот навык.
    /// </summary>
    public long PersonId { get; private set; }

    /// <summary>
    /// Название навыка.
    /// </summary>
    public string SkillName { get; private set; }

    /// <summary>
    /// Уровень навыка, выраженный в байтах.
    /// Значение должно быть от 1 до 10.
    /// </summary>
    public byte Level { get; private set; }

    /// <summary>
    /// Создает новый экземпляр класса <see cref="PersonSkill"/> с заданным идентификатором сотрудника, названием навыка и уровнем.
    /// </summary>
    /// <param name="personId"> Идентификатор сотрудника, которому принадлежит этот навык. </param>
    /// <param name="skillName"> Название навыка. </param>
    /// <param name="level"> Уровень навыка (от 1 до 10). </param>
    public PersonSkill(long personId, string skillName, byte level)
    {
        PersonId = personId;
        SkillName = skillName;
        Level = level;
    }

    /// <summary>
    /// Устанавливает уровень навыка.
    /// </summary>
    /// <param name="level"> Новое значение уровня навыка (от 1 до 10). </param>
    /// <exception cref="ArgumentOutOfRangeException"> Выбрасывается, если уровень выходит за пределы от 1 до 10. </exception>
    public void SetLevel(byte level)
    {
        if (level < 0 || level > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 10.");
        }

        Level = level;
    }
}