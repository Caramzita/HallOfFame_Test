namespace HallOfFame.Core;

public class Skill
{
    public string Name { get; private set; }

    public byte Level { get; private set; }

    public Skill(string name, byte level)
    {
        Name = name;
        Level = level;
    }

    public void UpdateLevel(byte level)
    {
        if (level >= 1 ||  level <= 10)
        {
            Level = level;
        }
    }
}
