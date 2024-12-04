using HallOfFame.Core;

namespace HallOfFame.Contracts;

public class PersonRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public List<Skill> Skills { get; set; } = new List<Skill>();
}
