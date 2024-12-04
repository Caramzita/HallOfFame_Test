namespace HallOfFame.Core;

public class Person
{
    public long Id { get; init; }

    public string Name { get; private set; }

    public string DisplayName { get; private set; }

    public List<Skill> Skills { get; private set; } = new List<Skill>();

    public Person(string name, string displayName, List<Skill> skills)
    {
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }

    public Person(long id ,string name, string displayName, List<Skill> skills)
    {
        Id = id;
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }

    public void UpdatePersonInfo(string name, string displayName, List<Skill> skills)
    {
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }
}
