namespace HallOfFame.Core;

/// <summary>
/// Навык.
/// </summary>
public class Skill
{
    /// <summary>
    /// Название навыка.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Создает новый экземпляр класса <see cref="Skill"/> с указанным названием навыка.
    /// </summary>
    /// <param name="name"> Название навыка. </param>
    public Skill(string name)
    {
        Name = name;
    }  
}
