using HallOfFame.Core;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.AddPerson;

public class AddPersonCommand : IRequest<Person>, IPersonCommand
{
    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public List<Skill> Skills { get; private set; } = new List<Skill>();

    public AddPersonCommand(string name, string displayName, List<Skill> skills)
    {
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }
}
