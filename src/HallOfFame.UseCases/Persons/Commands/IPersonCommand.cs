using HallOfFame.Core;

namespace HallOfFame.UseCases.Persons.Commands;

public interface IPersonCommand
{
    string Name { get; }

    string DisplayName { get; }

    List<Skill> Skills { get; }
}
