using HallOfFame.Core;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonCommand : IRequest<Person>, IPersonCommand
{
    public long Id { get; set; }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public List<Skill> Skills { get; private set; } = new List<Skill>();


    public UpdatePersonCommand(string name, string displayName, List<Skill> skills)
    {
        Name = name;
        DisplayName = displayName;
        Skills = skills;
    }
}
