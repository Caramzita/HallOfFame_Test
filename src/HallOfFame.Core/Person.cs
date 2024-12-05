namespace HallOfFame.Core;

/// <summary>
/// Модель данных сотрудника.
/// </summary>
public class Person
{
    /// <summary>
    /// Уникальный идентификатор сотрудника.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Имя сотрудника.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Отображаемое имя сотрудника.
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// Список навыков сотрудника.
    /// </summary>
    public List<PersonSkill> Skills { get; private set; } = new List<PersonSkill>();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Person"/> с указанным именем и отображаемым именем.
    /// </summary>
    /// <param name="name"> Имя сотрудника. </param>
    /// <param name="displayName"> Отображаемое имя сотрудника. </param>
    public Person(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }

    /// <summary>
    /// Конструктор для инициализации модели уже существующего сотрудника.
    /// </summary>
    /// <param name="id"> Уникальный идентификатор сотрудника. </param>
    /// <param name="name"> Имя сотрудника. </param>
    /// <param name="displayName"> Отображаемое имя сотрудника. </param>
    /// <param name="skills"> Список навыков сотрудника. </param>
    public Person(long id ,string name, string displayName, List<PersonSkill> skills)
    {
        Id = id;
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }

    /// <summary>
    /// Обновляет информацию о сотруднике, включая имя, отображаемое имя и навыки.
    /// </summary>
    /// <param name="name"> Новое имя (опционально). </param>
    /// <param name="displayName"> Новое отображаемое имя (опционально). </param>
    /// <param name="skills"> Новый список навыков (опционально). </param>
    public void UpdatePersonInfo(string? name = null, string? displayName = null, List<PersonSkill>? skills = null)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Name = name;
        }

        if (!string.IsNullOrEmpty(displayName))
        {
            DisplayName = displayName;
        }

        if (skills != null && skills.Count > 0)
        {
            foreach (var skill in skills)
            {
                var existingSkill = Skills.FirstOrDefault(s => s.SkillName == skill.SkillName);

                if (existingSkill != null)
                {
                    existingSkill.SetLevel(skill.Level);
                }
                else
                {
                    Skills.Add(skill);
                }
            }
        }
    }
}
