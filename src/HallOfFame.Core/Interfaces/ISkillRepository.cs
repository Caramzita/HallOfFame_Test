namespace HallOfFame.Core.Interfaces;

/// <summary>
/// Интерфейс для управления операциями с данными навыков.
/// </summary>
public interface ISkillRepository
{
    /// <summary>
    /// Получает все навыки в виде асинхронного перечисления.
    /// </summary>
    /// <returns>Асинхронное перечисление, содержащее список навыков.</returns>
    IAsyncEnumerable<Skill> GetSkills();

    /// <summary>
    /// Получает навык по его названию.
    /// </summary>
    /// <param name="name"> Название навыка. </param>
    Task<Skill?> GetSkillByName(string name);

    /// <summary>
    /// Проверяет, являются ли указанные навыки допустимыми.
    /// </summary>
    /// <param name="skillNames"> Список имен навыков для проверки. </param>
    Task<bool> AreSkillsValid(IEnumerable<string> skillNames);

    /// <summary>
    /// Добавляет новый навык в репозиторий.
    /// </summary>
    /// <param name="skill"> Навык, который будет добавлен. </param>
    Task AddSkill(Skill skill);

    /// <summary>
    /// Обновляет данные навыка в репозитории.
    /// </summary>
    /// <param name="skill"> Навык с обновленными данными. </param>
    Task UpdateSkill(Skill skill);

    /// <summary>
    /// Удаляет навык из репозитория.
    /// </summary>
    /// <param name="skill"> Навык, который будет удален. </param>
    Task DeleteSkill(Skill skill);
}
